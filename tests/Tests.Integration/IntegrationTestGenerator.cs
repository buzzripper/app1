using System.Text;

namespace Dyvenix.App1.Tests.Integration;

public sealed class IntegrationTestGenerator
{
	private static readonly Dictionary<string, string> DataSetPropertyMap = new(StringComparer.OrdinalIgnoreCase)
	{
		["Patient"] = "PatientList",
		["Invoice"] = "InvoiceList",
		["AppUser"] = "AppUserList"
	};

	public void GenerateForInterface(string interfaceFilePath)
	{
		if (string.IsNullOrWhiteSpace(interfaceFilePath))
			throw new ArgumentException("Interface file path is required.", nameof(interfaceFilePath));

		var fullPath = Path.GetFullPath(interfaceFilePath);
		if (!File.Exists(fullPath))
			throw new FileNotFoundException("Interface file was not found.", fullPath);

		var interfaceInfo = ParseInterface(fullPath);
		var module = ResolveModule(fullPath);
		var repoRoot = FindRepoRoot(fullPath);
		var testsRoot = Path.Combine(repoRoot, "tests", "Tests.Integration");
		var outputDir = Path.Combine(testsRoot, module);

		Directory.CreateDirectory(outputDir);

		var serviceBaseName = GetServiceBaseName(interfaceInfo.InterfaceName);
		var readFilePath = Path.Combine(outputDir, $"{serviceBaseName}ReadTests.cs");
		var writeFilePath = Path.Combine(outputDir, $"{serviceBaseName}WriteTests.cs");

		var readFileContents = GenerateReadTests(interfaceInfo, module, serviceBaseName, repoRoot);
		File.WriteAllText(readFilePath, readFileContents);

		var writeFileContents = GenerateWriteTests(interfaceInfo, module, serviceBaseName, repoRoot);
		File.WriteAllText(writeFilePath, writeFileContents);
	}

	private static InterfaceInfo ParseInterface(string interfaceFilePath)
	{
		var lines = File.ReadAllLines(interfaceFilePath);
		var interfaceName = string.Empty;
		var namespaceName = string.Empty;
		var methods = new List<MethodInfo>();

		foreach (var line in lines)
		{
			var trimmed = line.Trim();
			if (trimmed.StartsWith("namespace ", StringComparison.Ordinal))
			{
				namespaceName = trimmed.Replace("namespace ", string.Empty, StringComparison.Ordinal).TrimEnd(';');
				continue;
			}

			if (trimmed.StartsWith("public interface ", StringComparison.Ordinal))
			{
				var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				interfaceName = parts[^1].Trim();
				continue;
			}

			if (!trimmed.StartsWith("Task", StringComparison.Ordinal))
				continue;

			var method = ParseMethod(trimmed.TrimEnd(';'));
			if (method != null)
				methods.Add(method);
		}

		if (string.IsNullOrWhiteSpace(interfaceName))
			throw new InvalidOperationException($"Unable to find interface name in {interfaceFilePath}.");

		return new InterfaceInfo(interfaceName, namespaceName, methods);
	}

	private static MethodInfo? ParseMethod(string line)
	{
		var signature = line;
		if (!signature.StartsWith("Task", StringComparison.Ordinal))
			return null;

		var returnType = "void";
		var signatureWithoutTask = signature[4..].Trim();
		if (signatureWithoutTask.StartsWith("<", StringComparison.Ordinal))
		{
			var endGeneric = FindMatchingGenericEnd(signatureWithoutTask, 0);
			if (endGeneric < 0)
				return null;
			returnType = signatureWithoutTask[1..endGeneric].Trim();
			signatureWithoutTask = signatureWithoutTask[(endGeneric + 1)..].Trim();
		}

		var openParen = signatureWithoutTask.IndexOf('(');
		var closeParen = signatureWithoutTask.LastIndexOf(')');
		if (openParen < 0 || closeParen < 0)
			return null;

		var methodName = signatureWithoutTask[..openParen].Trim();
		var paramList = signatureWithoutTask[(openParen + 1)..closeParen];
		var parameters = ParseParameters(paramList);
		return new MethodInfo(methodName, returnType, parameters);
	}

	private static int FindMatchingGenericEnd(string text, int startIndex)
	{
		var depth = 0;
		for (var i = startIndex; i < text.Length; i++)
		{
			if (text[i] == '<')
				depth++;
			else if (text[i] == '>')
			{
				depth--;
				if (depth == 0)
					return i;
			}
		}

		return -1;
	}

	private static List<ParameterInfo> ParseParameters(string paramList)
	{
		var parameters = new List<ParameterInfo>();
		if (string.IsNullOrWhiteSpace(paramList))
			return parameters;

		var parts = paramList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		foreach (var part in parts)
		{
			var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (tokens.Length < 2)
				continue;

			var name = tokens[^1].Trim();
			var type = string.Join(' ', tokens[..^1]);
			parameters.Add(new ParameterInfo(type, name));
		}

		return parameters;
	}

	private static string ResolveModule(string interfaceFilePath)
	{
		if (interfaceFilePath.Contains(Path.Combine("App", "App.Shared"), StringComparison.OrdinalIgnoreCase))
			return "App";
		if (interfaceFilePath.Contains(Path.Combine("Auth", "Auth.Shared"), StringComparison.OrdinalIgnoreCase))
			return "Auth";

		throw new InvalidOperationException("Unable to determine module from interface path.");
	}

	private static string FindRepoRoot(string interfaceFilePath)
	{
		var directory = new DirectoryInfo(Path.GetDirectoryName(interfaceFilePath));
		while (directory != null)
		{
			var hasTests = Directory.Exists(Path.Combine(directory.FullName, "tests"));
			var hasSrc = Directory.Exists(Path.Combine(directory.FullName, "src"));
			if (hasTests && hasSrc)
				return directory.FullName;

			directory = directory.Parent;
		}

		throw new DirectoryNotFoundException("Unable to locate repository root from interface path.");
	}

	private static string GetServiceBaseName(string interfaceName)
	{
		var trimmed = interfaceName.StartsWith('I') ? interfaceName[1..] : interfaceName;
		return trimmed.EndsWith("Service", StringComparison.Ordinal) ? trimmed[..^"Service".Length] : trimmed;
	}

	private static string GenerateReadTests(InterfaceInfo interfaceInfo, string module, string serviceBaseName, string repoRoot)
	{
		var entityType = serviceBaseName;
		var dataSetProperty = DataSetPropertyMap.GetValueOrDefault(entityType);
		if (string.IsNullOrWhiteSpace(dataSetProperty))
			throw new InvalidOperationException($"No dataset mapping found for entity '{entityType}'.");

		var namespaceName = module == "App"
			? "Dyvenix.App1.Tests.Integration.App"
			: "Dyvenix.App1.Tests.Integration.Auth";

		var contractNamespace = module == "App"
			? "Dyvenix.App1.App.Shared.Contracts.v1"
			: "Dyvenix.App1.Auth.Shared.Contracts.v1";
		var requestNamespace = module == "App"
			? "Dyvenix.App1.App.Shared.Requests.v1"
			: "Dyvenix.App1.Auth.Shared.Requests.v1";

		var serviceFieldName = ToCamel(serviceBaseName) + "Service";
		var fixtureName = serviceBaseName + "ReadTestFixture";
		var className = serviceBaseName + "ReadTests";
		var interfaceName = interfaceInfo.InterfaceName;

		var builder = new StringBuilder();
		builder.AppendLine($"using {contractNamespace};");
		builder.AppendLine($"using {requestNamespace};");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.Data;");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.DataSets;");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.Fixtures;");
		builder.AppendLine();
		builder.AppendLine($"namespace {namespaceName};");
		builder.AppendLine();
		builder.AppendLine($"public sealed class {fixtureName}(GlobalTestFixture globalFixture) : IAsyncLifetime");
		builder.AppendLine("{");
		builder.AppendLine("\tpublic GlobalTestFixture GlobalFixture { get; } = globalFixture;");
		builder.AppendLine("\tpublic TestDataSet DataSet { get; private set; } = default!;");
		builder.AppendLine();
		builder.AppendLine("\tpublic async ValueTask InitializeAsync()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tvar dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();");
		builder.AppendLine("\t\tDataSet = await dataManager.Reset(DataSetType.Main.ToString());");
		builder.AppendLine("\t}");
		builder.AppendLine();
		builder.AppendLine("\tpublic ValueTask DisposeAsync() => default;");
		builder.AppendLine("}");
		builder.AppendLine();
		builder.AppendLine("[Collection(nameof(GlobalTestCollection))]");
		builder.AppendLine($"public class {className} : TestBase, IClassFixture<{fixtureName}>");
		builder.AppendLine("{");
		builder.AppendLine($"\tprivate readonly {fixtureName} _fixture;");
		builder.AppendLine($"\tprivate {interfaceName} _{serviceFieldName} = default!;");
		builder.AppendLine();
		builder.AppendLine($"\tpublic {className}(GlobalTestFixture globalFixture, {fixtureName} fixture)");
		builder.AppendLine("\t\t: base(globalFixture)");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\t_fixture = fixture;");
		builder.AppendLine("\t}");
		builder.AppendLine();
		builder.AppendLine("\tpublic override async ValueTask InitializeAsync()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tawait base.InitializeAsync();");
		builder.AppendLine($"\t\t_{serviceFieldName} = _scope.ServiceProvider.GetRequiredService<{interfaceName}>();");
		builder.AppendLine("\t}");

		foreach (var method in interfaceInfo.Methods.Where(m => IsReadMethod(m)))
		{
			var testMethod = GenerateReadTestMethod(method, serviceFieldName, dataSetProperty, entityType, module, repoRoot);
			if (string.IsNullOrWhiteSpace(testMethod))
				continue;

			builder.AppendLine();
			builder.Append(testMethod);
		}

		builder.AppendLine("}");
		return builder.ToString();
	}

	private static string GenerateWriteTests(InterfaceInfo interfaceInfo, string module, string serviceBaseName, string repoRoot)
	{
		var entityType = serviceBaseName;
		var dataSetProperty = DataSetPropertyMap.GetValueOrDefault(entityType);
		if (string.IsNullOrWhiteSpace(dataSetProperty))
			throw new InvalidOperationException($"No dataset mapping found for entity '{entityType}'.");

		var namespaceName = module == "App"
			? "Dyvenix.App1.Tests.Integration.App"
			: "Dyvenix.App1.Tests.Integration.Auth";

		var contractNamespace = module == "App"
			? "Dyvenix.App1.App.Shared.Contracts.v1"
			: "Dyvenix.App1.Auth.Shared.Contracts.v1";
		var requestNamespace = module == "App"
			? "Dyvenix.App1.App.Shared.Requests.v1"
			: "Dyvenix.App1.Auth.Shared.Requests.v1";

		var entityNamespace = "Dyvenix.App1.Common.Data.Shared.Entities";
		var serviceFieldName = ToCamel(serviceBaseName) + "Service";
		var fixtureName = serviceBaseName + "WriteTestFixture";
		var className = serviceBaseName + "WriteTests";
		var interfaceName = interfaceInfo.InterfaceName;

		var builder = new StringBuilder();
		builder.AppendLine($"using {contractNamespace};");
		builder.AppendLine($"using {requestNamespace};");
		builder.AppendLine($"using {entityNamespace};");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.Data;");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.DataSets;");
		builder.AppendLine("using Dyvenix.App1.Tests.Integration.Fixtures;");
		builder.AppendLine();
		builder.AppendLine($"namespace {namespaceName};");
		builder.AppendLine();
		builder.AppendLine($"public sealed class {fixtureName}(GlobalTestFixture globalFixture) : IAsyncLifetime");
		builder.AppendLine("{");
		builder.AppendLine("\tpublic GlobalTestFixture GlobalFixture { get; } = globalFixture;");
		builder.AppendLine("\tpublic TestDataSet DataSet { get; private set; } = default!;");
		builder.AppendLine();
		builder.AppendLine("\tpublic async ValueTask InitializeAsync()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tvar dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();");
		builder.AppendLine("\t\tDataSet = await dataManager.Reset(DataSetType.Main.ToString());");
		builder.AppendLine("\t}");
		builder.AppendLine();
		builder.AppendLine("\tpublic ValueTask DisposeAsync() => default;");
		builder.AppendLine("}");
		builder.AppendLine();
		builder.AppendLine("[Collection(nameof(GlobalTestCollection))]");
		builder.AppendLine($"public class {className} : TestBase, IClassFixture<{fixtureName}>");
		builder.AppendLine("{");
		builder.AppendLine($"\tprivate readonly {fixtureName} _fixture;");
		builder.AppendLine($"\tprivate {interfaceName} _{serviceFieldName} = default!;");
		builder.AppendLine();
		builder.AppendLine($"\tpublic {className}(GlobalTestFixture globalFixture, {fixtureName} fixture)");
		builder.AppendLine("\t\t: base(globalFixture)");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\t_fixture = fixture;");
		builder.AppendLine("\t}");
		builder.AppendLine();
		builder.AppendLine("\tpublic override async ValueTask InitializeAsync()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tawait base.InitializeAsync();");
		builder.AppendLine($"\t\t_{serviceFieldName} = _scope.ServiceProvider.GetRequiredService<{interfaceName}>();");
		builder.AppendLine("\t}");

		foreach (var method in interfaceInfo.Methods.Where(m => !IsReadMethod(m)))
		{
			var testMethod = GenerateWriteTestMethod(method, serviceFieldName, dataSetProperty, entityType, module, repoRoot);
			if (string.IsNullOrWhiteSpace(testMethod))
				continue;

			builder.AppendLine();
			builder.Append(testMethod);
		}

		builder.AppendLine("}");
		return builder.ToString();
	}

	private static string GenerateReadTestMethod(MethodInfo method, string serviceFieldName, string dataSetProperty, string entityType, string module, string repoRoot)
	{
		var returnInfo = ExtractReturnInfo(method.ReturnType);
		if (returnInfo == null)
			return string.Empty;

		var sampleVar = "sample";
		var builder = new StringBuilder();
		builder.AppendLine("\t[Fact]");
		builder.AppendLine($"\tpublic async Task {method.Name}_Success()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tif (_db == null)");
		builder.AppendLine("\t\t\tthrow new InvalidOperationException(\"App1Db is not available from the test fixture.\");");
		builder.AppendLine($"\t\tvar {sampleVar} = _fixture.DataSet.{dataSetProperty}.First();");

		if (returnInfo.IsPaged)
		{
			builder.AppendLine($"\t\tvar totalCount = _fixture.DataSet.{dataSetProperty}.Count;");
			builder.AppendLine("\t\tif (totalCount < 6)");
			builder.AppendLine("\t\t\tthrow new InvalidOperationException($\"Test data should contain at least 6 items for this test. Current count: {totalCount}\");");
		}

		var requestAssignment = BuildRequestAssignment(method, module, repoRoot, sampleVar, includeIdentity: false, out var requestTypeName, out var requestIsPaged, out var requestHasSort);
		if (!string.IsNullOrWhiteSpace(requestTypeName))
		{
			builder.AppendLine($"\t\tvar request = new {requestTypeName}();");
			foreach (var assignment in requestAssignment)
				builder.AppendLine($"\t\t{assignment}");
		}

		var paramExpression = BuildParameterExpression(method, sampleVar, requestTypeName);
		if (returnInfo.IsPaged)
		{
			builder.AppendLine();
			builder.AppendLine("\t\tvar lastPgOffset = totalCount / request.PageSize;");
			builder.AppendLine("\t\tif (totalCount % request.PageSize == 0)");
			builder.AppendLine("\t\t\tlastPgOffset -= 1;");
			builder.AppendLine("\t\tvar lastPgSize = totalCount - (lastPgOffset * request.PageSize);");
			builder.AppendLine();
			builder.AppendLine("\t\trequest.PageOffset = 0;");
			builder.AppendLine($"\t\tvar firstPgList = await _{serviceFieldName}.{method.Name}({paramExpression});");
			builder.AppendLine("\t\trequest.PageOffset = lastPgOffset;");
			builder.AppendLine($"\t\tvar lastPgList = await _{serviceFieldName}.{method.Name}({paramExpression});");
			builder.AppendLine();
			builder.AppendLine("\t\tAssert.True(totalCount == firstPgList.TotalRowCount, $\"First total count s/b {totalCount} but was {firstPgList.TotalRowCount}\");");
			builder.AppendLine("\t\tAssert.True(request.PageSize == firstPgList.Items.Count, $\"First page size s/b {request.PageSize} but was {firstPgList.Items.Count}\");");
			builder.AppendLine("\t\tAssert.True(totalCount == lastPgList.TotalRowCount, $\"Last total count s/b {totalCount} but was {lastPgList.TotalRowCount}\");");
			builder.AppendLine("\t\tAssert.True(lastPgSize == lastPgList.Items.Count, $\"Last page size s/b {lastPgSize} but was {lastPgList.Items.Count}\");");
		}
		else if (returnInfo.IsList)
		{
			builder.AppendLine();
			builder.AppendLine($"\t\tvar results = await _{serviceFieldName}.{method.Name}({paramExpression});");
			builder.AppendLine($"\t\tAssert.Contains(results, item => item.Id == {sampleVar}.Id);");
		}
		else
		{
			builder.AppendLine();
			builder.AppendLine($"\t\tvar result = await _{serviceFieldName}.{method.Name}({paramExpression});");
			builder.AppendLine($"\t\tAssert.Equal({sampleVar}.Id, result.Id);");
		}

		builder.AppendLine("\t}");
		return builder.ToString();
	}

	private static string GenerateWriteTestMethod(MethodInfo method, string serviceFieldName, string dataSetProperty, string entityType, string module, string repoRoot)
	{
		if (method.Parameters.Count == 0)
			return string.Empty;

		var sampleVar = "sample";
		var builder = new StringBuilder();
		builder.AppendLine("\t[Fact]");
		builder.AppendLine($"\tpublic async Task {method.Name}_Success()");
		builder.AppendLine("\t{");
		builder.AppendLine("\t\tif (_db == null)");
		builder.AppendLine("\t\t\tthrow new InvalidOperationException(\"App1Db is not available from the test fixture.\");");
		builder.AppendLine($"\t\tvar {sampleVar} = _fixture.DataSet.{dataSetProperty}.First();");

		var parameter = method.Parameters[0];
		if (IsDeleteMethod(method))
		{
			builder.AppendLine($"\t\tvar expectedCount = _fixture.DataSet.{dataSetProperty}.Count - 1;");
			builder.AppendLine();
			builder.AppendLine($"\t\tawait _{serviceFieldName}.{method.Name}({sampleVar}.Id);");
			builder.AppendLine();
			builder.AppendLine($"\t\tAssert.Equal(expectedCount, _db.{entityType}.Count());");
			builder.AppendLine($"\t\tAssert.Null(_db.{entityType}.FirstOrDefault(entity => entity.Id == {sampleVar}.Id));");
			builder.AppendLine("\t}");
			return builder.ToString();
		}

		if (IsCreateMethod(method))
		{
			var entityProperties = ParseEntityProperties(entityType, repoRoot);
			var initializerLines = BuildEntityInitializer(entityType, entityProperties, sampleVar, isCreate: true, updatedPropertyName: null, updatedValueExpression: null);
			builder.AppendLine($"\t\tvar expectedCount = _fixture.DataSet.{dataSetProperty}.Count + 1;");
			builder.AppendLine($"\t\tvar newEntity = new {entityType}");
			builder.AppendLine("\t\t{");
			foreach (var line in initializerLines)
				builder.AppendLine($"\t\t\t{line}");
			builder.AppendLine("\t\t};");
			builder.AppendLine();
			builder.AppendLine($"\t\tawait _{serviceFieldName}.{method.Name}(newEntity);");
			builder.AppendLine();
			builder.AppendLine($"\t\tAssert.Equal(expectedCount, _db.{entityType}.Count());");
			builder.AppendLine($"\t\tAssert.NotNull(_db.{entityType}.FirstOrDefault(entity => entity.Id == newEntity.Id));");
			builder.AppendLine("\t}");
			return builder.ToString();
		}

		var requestInfo = TryGetRequestInfo(parameter.Type, module, repoRoot);
		if (requestInfo != null)
		{
			var updateAssignments = BuildRequestAssignment(method, module, repoRoot, sampleVar, includeIdentity: true, out _, out _, out _);
			builder.AppendLine($"\t\tvar request = new {parameter.Type}();");
			foreach (var assignment in updateAssignments)
				builder.AppendLine($"\t\t{assignment}");
			builder.AppendLine();
			if (method.ReturnType == "byte[]")
			{
				builder.AppendLine($"\t\tvar rowVersion = await _{serviceFieldName}.{method.Name}(request);");
				builder.AppendLine("\t\tAssert.NotNull(rowVersion);");
			}
			else
			{
				builder.AppendLine($"\t\tawait _{serviceFieldName}.{method.Name}(request);");
			}

			var updateProperty = requestInfo.Properties.FirstOrDefault(prop => !IsRequestInfrastructureProperty(prop.Name, includeIdentity: false));
			if (updateProperty != null)
			{
				builder.AppendLine();
				builder.AppendLine($"\t\tvar updated = _db.{entityType}.First(entity => entity.Id == {sampleVar}.Id);");
				builder.AppendLine($"\t\tAssert.Equal(request.{updateProperty.Name}, updated.{updateProperty.Name});");
			}

			builder.AppendLine("\t}");
			return builder.ToString();
		}

		var entityPropertiesForUpdate = ParseEntityProperties(entityType, repoRoot);
		var updatedProperty = entityPropertiesForUpdate.FirstOrDefault(prop => prop.Type == "string" && !IsEntityInfrastructureProperty(prop.Name));
		var updatedValueExpression = updatedProperty != null ? $"$\"{{{sampleVar}.{updatedProperty.Name}}}_Updated\"" : null;
		var initializer = BuildEntityInitializer(entityType, entityPropertiesForUpdate, sampleVar, isCreate: false, updatedProperty?.Name, updatedValueExpression);
		builder.AppendLine($"\t\tvar updatedEntity = new {entityType}");
		builder.AppendLine("\t\t{");
		foreach (var line in initializer)
			builder.AppendLine($"\t\t\t{line}");
		builder.AppendLine("\t\t};");
		builder.AppendLine();
		builder.AppendLine($"\t\tawait _{serviceFieldName}.{method.Name}(updatedEntity);");
		if (updatedProperty != null)
		{
			builder.AppendLine();
			builder.AppendLine($"\t\tvar updated = _db.{entityType}.First(entity => entity.Id == updatedEntity.Id);");
			builder.AppendLine($"\t\tAssert.Equal(updatedEntity.{updatedProperty.Name}, updated.{updatedProperty.Name});");
		}
		builder.AppendLine("\t}");
		return builder.ToString();
	}

	private static IReadOnlyList<string> BuildRequestAssignment(MethodInfo method, string module, string repoRoot, string sampleVar, bool includeIdentity, out string requestTypeName, out bool isPaged, out bool hasSort)
	{
		requestTypeName = string.Empty;
		isPaged = false;
		hasSort = false;

		var assignments = new List<string>();
		if (method.Parameters.Count != 1)
			return assignments;

		var parameter = method.Parameters[0];
		if (!parameter.Type.EndsWith("Req", StringComparison.Ordinal))
			return assignments;

		requestTypeName = parameter.Type;
		var requestInfo = TryGetRequestInfo(parameter.Type, module, repoRoot);
		if (requestInfo == null)
			return assignments;

		isPaged = requestInfo.ImplementsPaging;
		hasSort = requestInfo.ImplementsSorting;

		if (requestInfo.ImplementsPaging)
		{
			assignments.Add("request.PageSize = 3;");
			assignments.Add("request.RecalcRowCount = true;");
			assignments.Add("request.GetRowCountOnly = false;");
		}

		if (requestInfo.ImplementsSorting)
		{
			assignments.Add("request.SortBy = \"Id\";");
			assignments.Add("request.SortDesc = false;");
		}

		foreach (var property in requestInfo.Properties)
		{
			if (IsRequestInfrastructureProperty(property.Name, includeIdentity))
				continue;

			assignments.Add($"request.{property.Name} = {GetSampleValueExpression(property, sampleVar)};");
		}

		return assignments;
	}

	private static string BuildParameterExpression(MethodInfo method, string sampleVar, string requestTypeName)
	{
		if (!string.IsNullOrWhiteSpace(requestTypeName))
			return "request";

		if (method.Parameters.Count == 1)
		{
			var parameter = method.Parameters[0];
			return parameter.Type switch
			{
				"Guid" => $"{sampleVar}.Id",
				"string" or "string?" => GetStringParameterExpression(parameter.Name, sampleVar),
				_ => parameter.Name
			};
		}

		return string.Empty;
	}

	private static string GetStringParameterExpression(string parameterName, string sampleVar)
	{
		if (parameterName.Contains("last", StringComparison.OrdinalIgnoreCase))
			return $"{sampleVar}.LastName";
		if (parameterName.Contains("email", StringComparison.OrdinalIgnoreCase))
			return $"{sampleVar}.Email";
		if (parameterName.Contains("username", StringComparison.OrdinalIgnoreCase))
			return $"{sampleVar}.Username";
		if (parameterName.Contains("memo", StringComparison.OrdinalIgnoreCase))
			return $"{sampleVar}.Memo";

		return "string.Empty";
	}

	private static ReturnInfo? ExtractReturnInfo(string returnType)
	{
		if (string.Equals(returnType, "void", StringComparison.Ordinal))
			return null;
		if (string.Equals(returnType, "byte[]", StringComparison.Ordinal))
			return null;

		if (returnType.StartsWith("ListPage<", StringComparison.Ordinal))
			return new ReturnInfo(true, true, ExtractGenericArgument(returnType));
		if (returnType.StartsWith("List<", StringComparison.Ordinal))
			return new ReturnInfo(true, false, ExtractGenericArgument(returnType));

		return new ReturnInfo(false, false, returnType);
	}

	private static string ExtractGenericArgument(string returnType)
	{
		var open = returnType.IndexOf('<');
		var close = returnType.LastIndexOf('>');
		return returnType[(open + 1)..close].Trim();
	}

	private static string ToCamel(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return value;

		return char.ToLowerInvariant(value[0]) + value[1..];
	}

	private static bool IsReadMethod(MethodInfo method)
	{
		if (method.ReturnType is "void" or "byte[]")
			return false;

		if (method.Name.StartsWith("Get", StringComparison.Ordinal)
			|| method.Name.StartsWith("Search", StringComparison.Ordinal)
			|| method.Name.StartsWith("Query", StringComparison.Ordinal)
			|| method.Name.StartsWith("Req", StringComparison.Ordinal))
			return true;

		return method.ReturnType.StartsWith("List", StringComparison.Ordinal) || method.ReturnType.Contains('<');
	}

	private static bool IsDeleteMethod(MethodInfo method) => method.Name.StartsWith("Delete", StringComparison.Ordinal);

	private static bool IsCreateMethod(MethodInfo method) => method.Name.StartsWith("Create", StringComparison.Ordinal);

	private static RequestInfo? TryGetRequestInfo(string requestTypeName, string module, string repoRoot)
	{
		var requestDir = Path.Combine(repoRoot, "src", module, $"{module}.Shared", "Requests", "v1");
		var requestFile = Directory.GetFiles(requestDir, $"{requestTypeName}*.cs", SearchOption.TopDirectoryOnly)
			.FirstOrDefault();
		if (string.IsNullOrWhiteSpace(requestFile))
			return null;

		var lines = File.ReadAllLines(requestFile);
		var implementsPaging = lines.Any(line => line.Contains("IPagingRequest", StringComparison.Ordinal));
		var implementsSorting = lines.Any(line => line.Contains("ISortingRequest", StringComparison.Ordinal));
		var properties = ParseProperties(lines);
		return new RequestInfo(requestTypeName, implementsPaging, implementsSorting, properties);
	}

	private static List<PropertyInfo> ParseProperties(string[] lines)
	{
		var properties = new List<PropertyInfo>();
		foreach (var line in lines)
		{
			var trimmed = line.Trim();
			if (!trimmed.StartsWith("public ", StringComparison.Ordinal) || !trimmed.Contains("{ get; set; }", StringComparison.Ordinal))
				continue;

			var declaration = trimmed.Replace("public ", string.Empty, StringComparison.Ordinal);
			var parts = declaration.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2)
				continue;

			var type = parts[0].Trim();
			var name = parts[1].Trim();
			properties.Add(new PropertyInfo(type, name));
		}

		return properties;
	}

	private static List<PropertyInfo> ParseEntityProperties(string entityType, string repoRoot)
	{
		var entityFile = Directory.GetFiles(Path.Combine(repoRoot, "src", "Common", "Common.Data.Shared", "Entities"), $"{entityType}*.cs")
			.FirstOrDefault();
		if (string.IsNullOrWhiteSpace(entityFile))
			return [];

		return ParseProperties(File.ReadAllLines(entityFile))
			.Where(prop => !prop.Type.StartsWith("List<", StringComparison.Ordinal))
			.ToList();
	}

	private static IReadOnlyList<string> BuildEntityInitializer(string entityType, IReadOnlyList<PropertyInfo> properties, string sampleVar, bool isCreate, string? updatedPropertyName, string? updatedValueExpression)
	{
		var lines = new List<string>();
		foreach (var property in properties)
		{
			if (IsEntityInfrastructureProperty(property.Name))
			{
				if (property.Name == "Id")
				{
					lines.Add(isCreate ? "Id = Guid.NewGuid()," : $"Id = {sampleVar}.Id,");
				}
				else if (property.Name == "RowVersion" && !isCreate)
				{
					lines.Add($"RowVersion = {sampleVar}.RowVersion,");
				}

				continue;
			}

			if (!string.IsNullOrWhiteSpace(updatedPropertyName) && string.Equals(updatedPropertyName, property.Name, StringComparison.Ordinal))
			{
				lines.Add($"{property.Name} = {updatedValueExpression},");
				continue;
			}

			lines.Add($"{property.Name} = {GetEntitySampleValueExpression(property, sampleVar)},");
		}

		return lines;
	}

	private static string GetEntitySampleValueExpression(PropertyInfo property, string sampleVar)
	{
		return property.Type switch
		{
			"Guid" => $"{sampleVar}.{property.Name}",
			"string" => $"{sampleVar}.{property.Name}",
			"string?" => $"{sampleVar}.{property.Name}",
			"decimal" => $"{sampleVar}.{property.Name}",
			"bool" => $"{sampleVar}.{property.Name}",
			"byte[]" => $"{sampleVar}.{property.Name}",
			_ => $"{sampleVar}.{property.Name}"
		};
	}

	private static string GetSampleValueExpression(PropertyInfo property, string sampleVar)
	{
		return property.Type switch
		{
			"Guid" => $"{sampleVar}.Id",
			"string" or "string?" => GetStringParameterExpression(property.Name, sampleVar),
			"decimal" => $"{sampleVar}.Amount",
			"bool" => $"{sampleVar}.IsActive",
			"byte[]" => $"{sampleVar}.RowVersion",
			_ => "default!"
		};
	}

	private static bool IsEntityInfrastructureProperty(string propertyName) =>
		string.Equals(propertyName, "Id", StringComparison.Ordinal)
		|| string.Equals(propertyName, "RowVersion", StringComparison.Ordinal);

	private static bool IsRequestInfrastructureProperty(string propertyName, bool includeIdentity) =>
		string.Equals(propertyName, "PageSize", StringComparison.Ordinal)
		|| string.Equals(propertyName, "PageOffset", StringComparison.Ordinal)
		|| string.Equals(propertyName, "RecalcRowCount", StringComparison.Ordinal)
		|| string.Equals(propertyName, "GetRowCountOnly", StringComparison.Ordinal)
		|| string.Equals(propertyName, "SortBy", StringComparison.Ordinal)
		|| string.Equals(propertyName, "SortDesc", StringComparison.Ordinal)
		|| (!includeIdentity && string.Equals(propertyName, "Id", StringComparison.Ordinal))
		|| (!includeIdentity && string.Equals(propertyName, "RowVersion", StringComparison.Ordinal));

	private sealed record InterfaceInfo(string InterfaceName, string Namespace, List<MethodInfo> Methods);

	private sealed record MethodInfo(string Name, string ReturnType, List<ParameterInfo> Parameters);

	private sealed record ParameterInfo(string Type, string Name);

	private sealed record PropertyInfo(string Type, string Name);

	private sealed record RequestInfo(string Name, bool ImplementsPaging, bool ImplementsSorting, List<PropertyInfo> Properties);

	private sealed record ReturnInfo(bool IsList, bool IsPaged, string EntityType);
}
