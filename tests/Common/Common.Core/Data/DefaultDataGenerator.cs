using Dyvenix.App1.Common.Data.Shared.Entities;

namespace Dyvenix.App1.Tests.Common.Core.Data;

public class DefaultDataGenerator
{
	public DataSet CreateDataSet()
	{
		var dataSet = new DataSet { Type = DataSetType.Default };

		var appUsers = new List<AppUser>();
		var firstNames = new[] { "John", "Jane", "Alice", "Bob", "Charlie", "Eve", "Mallory", "Trent", "Oscar", "Grace", "Victor", "Walter", "Xander", "Yvonne", "Zara" };
		var lastNames = new[] { "Doe", "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson" };
		//var companyIds = new[] { "ABC", "DEF", "GHI", "JKL", "MNO" };
		var companyIds = new[] { "ABC", "DEF", "GHI" };

		var random = new Random();
		var emailSet = new HashSet<string>();
		var nameSet = new HashSet<string>();

		while (appUsers.Count < 50)
		{
			var firstName = firstNames[random.Next(firstNames.Length)];
			var lastName = lastNames[random.Next(lastNames.Length)];
			string nameCombination = $"{firstName} {lastName}";

			if (nameSet.Contains(nameCombination))
			{
				continue;
			}
			nameSet.Add(nameCombination);

			string email;
			int attempt = 0;
			do
			{
				email = $"{firstName.ToLower()}.{lastName.ToLower()}{attempt}@example.com";
				attempt++;
			} while (!emailSet.Add(email)); // Ensure unique emails

			var appUser = dataSet.CreateAppUser();
			appUser.Id = Guid.NewGuid();
			appUser.ExtId = Guid.NewGuid().ToString();
			appUser.FirstName = firstName;
			appUser.LastName = lastName;
			appUser.Email = email;
			appUser.IsEnabled = random.NextDouble() >= 0.15; // 15% chance of IsEnabled being false
			appUser.CompanyId = companyIds[random.Next(companyIds.Length)];
			appUser.GroupCode = random.NextDouble() >= 0.15 ? random.Next(1, 5) : null;
			appUser.UserType = (UserType)random.Next(0, 3); // Randomly assign UserType between 0 and 2

			// Add 0-8 AccessClaims to each AppUser
			int numberOfClaims = random.Next(0, 9);
			for (int j = 0; j < numberOfClaims; j++)
			{
				var accessClaim = dataSet.CreateAccessClaim();
				accessClaim.Id = Guid.NewGuid();
				accessClaim.ClaimName = $"ClaimName_{j}";
				accessClaim.ClaimValue = $"ClaimValue_{j}";
				accessClaim.AppUserId = appUser.Id;
				appUser.Claims.Add(accessClaim);
			}

			appUsers.Add(appUser);
		}

		dataSet.AppUser = appUsers;

		return dataSet;
	}
}
