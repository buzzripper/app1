//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.App.Api.Context;
using Dyvenix.App1.App.Shared.Dtos;
using Dyvenix.App1.App.Api.Entities;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Api.Services.v1;

public partial class ClientService : IClientService
{
	private readonly ILogger<ClientService> _logger;
	private readonly App1Db _db;

	public ClientService(App1Db db, ILogger<ClientService> logger)
	{
		_db = db;
		_logger = logger;
	}

	#region Delete

	public async Task Delete(Guid id)
	{
		var rowsAffected = await _db.Client.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"Client {id} not found");
	}

	#endregion

	#region Update

	public async Task<byte[]> Create(CreateReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var client = new Client {
				Id = request.Id,
				RowVersion = request.RowVersion,
				Key = request.Key,
				Name = request.Name,
				BaseUrl = request.BaseUrl,
			};

			_db.Add(client);
			await _db.SaveChangesAsync();

			return client.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<ClientDto> GetById(Guid id)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		return await dbQuery.Select(e => new ClientDto(
			e.Id,
			e.Key,
			e.Name,
			e.BaseUrl
		))
		.SingleOrDefaultAsync();
	}
	
	public async Task<ClientDto> GetByKey(string key)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(key))
			dbQuery = dbQuery.Where(x => x.Key == key);
	
		return await dbQuery.Select(e => new ClientDto(
			e.Id,
			e.Key,
			e.Name,
			e.BaseUrl
		))
		.SingleOrDefaultAsync();
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<IReadOnlyList<ClientOptionDto>> GetAllClientOptions(GetAllClientOptionsReq request)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			dbQuery = this.AddSorting(ref dbQuery, request);
	
		return await dbQuery.Select(e => new ClientOptionDto(
			e.Id,
			e.Key,
			e.Name
		))
		.ToListAsync();
	}
	
	public async Task<IReadOnlyList<ClientRouteDto>> GetAllRoutes()
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		return await dbQuery.Select(e => new ClientRouteDto(
			e.Id,
			e.Key,
			e.BaseUrl
		))
		.ToListAsync();
	}
	
	#endregion
	
	private IQueryable<Client> AddSorting(ref IQueryable<Client> dbQuery, ISortingRequest sortingRequest)
	{
		if (string.Equals(sortingRequest.SortBy, Client.PropNames.Id, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				return dbQuery.OrderByDescending(x => x.Id);
			else
				return dbQuery.OrderBy(x => x.Id);
	
		if (string.Equals(sortingRequest.SortBy, Client.PropNames.Key, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				return dbQuery.OrderByDescending(x => x.Key);
			else
				return dbQuery.OrderBy(x => x.Key);
	
		if (string.Equals(sortingRequest.SortBy, Client.PropNames.Name, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				return dbQuery.OrderByDescending(x => x.Name);
			else
				return dbQuery.OrderBy(x => x.Name);
	
		if (string.Equals(sortingRequest.SortBy, Client.PropNames.BaseUrl, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				return dbQuery.OrderByDescending(x => x.BaseUrl);
			else
				return dbQuery.OrderBy(x => x.BaseUrl);
	
		if (string.Equals(sortingRequest.SortBy, Client.PropNames.RowVersion, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				return dbQuery.OrderByDescending(x => x.RowVersion);
			else
				return dbQuery.OrderBy(x => x.RowVersion);
		return dbQuery;
	}
	
}
