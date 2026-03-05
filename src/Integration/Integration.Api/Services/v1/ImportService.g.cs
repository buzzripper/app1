//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.App.Shared.Contracts.v1;

namespace Dyvenix.App1.App.Api.Services.v1;

public partial class ImportService : IImportService
{
	private readonly ILogger<ImportService> _logger;

	public ImportService(ILogger<ImportService> logger)
	{
		_logger = logger;
	}

	public async Task<string> ImportMethod1()
	{
		return $"{nameof(ImportMethod1)}!!";
	}

	public async Task<string> ImportMethod2()
	{
		return $"{nameof(ImportMethod2)}!!";
	}

	public async Task<string> ImportMethod3()
	{
		return $"{nameof(ImportMethod3)}!!";
	}
}
