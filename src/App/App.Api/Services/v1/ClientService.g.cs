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
using Dyvenix.App1.Common.Shared.Extensions;
using Dyvenix.App1.Common.Shared.DTOs;
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

	public async Task DeleteClient(Guid id)
	{
		var rowsAffected = await _db.Client.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"Client {id} not found");
	}

	#endregion

	#region Update

	public async Task<byte[]> CreateClient(CreateClientReq request)
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

	public async Task<byte[]> UpdateClient(UpdateClientReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var client = new Client {
				Id = request.Id,
				RowVersion = request.RowVersion,
				Name = request.Name,
				BaseUrl = request.BaseUrl,
				Key = request.Key,
			};

			_db.Attach(client);
			_db.Entry(client).Property(u => u.Name).IsModified = true;
			_db.Entry(client).Property(u => u.BaseUrl).IsModified = true;
			_db.Entry(client).Property(u => u.Key).IsModified = true;
			_db.Entry(client).Property(u => u.RowVersion).IsModified = true;

			await _db.SaveChangesAsync();

			return client.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	public async Task<byte[]> UpdateClientBaseUrl(UpdateClientBaseUrlReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var client = new Client {
				Id = request.Id,
				RowVersion = request.RowVersion,
				BaseUrl = request.BaseUrl,
				Key = request.Key,
			};

			_db.Attach(client);
			_db.Entry(client).Property(u => u.BaseUrl).IsModified = true;
			_db.Entry(client).Property(u => u.Key).IsModified = true;
			_db.Entry(client).Property(u => u.RowVersion).IsModified = true;

			await _db.SaveChangesAsync();

			return client.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<ClientDto> GetClientById(Guid id)
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
	
	public async Task<ClientDto> GetClientByKey(string key)
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
	
	public async Task<IReadOnlyList<ClientLookupDto>> GetAllClientLookupItems(GetAllClientLookupItemsReq request)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			dbQuery = this.AddSorting(ref dbQuery, request);
	
		return await dbQuery.Select(e => new ClientLookupDto(
			e.Id,
			e.Key,
			e.Name
		))
		.ToListAsync();
	}
	
	public async Task<IReadOnlyList<ClientRouteDto>> GetAllClientRoutes()
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		return await dbQuery.Select(e => new ClientRouteDto(
			e.Id,
			e.BaseUrl,
			e.Key
		))
		.ToListAsync();
	}
	
	public async Task<IReadOnlyList<ClientDto>> GetAllClients(GetAllClientsReq request)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			dbQuery = this.AddSorting(ref dbQuery, request);
	
		return await dbQuery.Select(e => new ClientDto(
			e.Id,
			e.Key,
			e.Name,
			e.BaseUrl
		))
		.ToListAsync();
	}
	
	public async Task<ListPage<ClientLookupDto>> SearchClientsByName(SearchClientsByNameReq request)
	{
		var dbQuery = _db.Client.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.Name))
			dbQuery = dbQuery.Where(x => x.Name == request.Name);
	
		var listPage = new ListPage<ClientLookupDto>();
	
		// Count (if requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			listPage.TotalRowCount = await dbQuery.CountAsync();
			if (request.GetRowCountOnly)
				return listPage;
		}
		else if (!request.RecalcRowCount && !request.GetRowCountOnly)
		{
			listPage.TotalRowCount = -1;  // Make it clear that row count was not calculated
		}
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			dbQuery = this.AddSorting(ref dbQuery, request);
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		listPage.Items = await dbQuery.Select(e => new ClientLookupDto(
			e.Id,
			e.Key,
			e.Name
		))
		.ToListAsync();
	
		return listPage;
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
