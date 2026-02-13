//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Extensions;

namespace Dyvenix.App1.App.Api.Services.v1;

public partial class PatientService : IPatientService
{
	private readonly ILogger<PatientService> _logger;
	private readonly App1Db _db;

	public PatientService(App1Db db, ILogger<PatientService> logger)
	{
		_db = db;
		_logger = logger;
	}

	#region Create

	public async Task CreatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);

		try {
			_db.Add(patient);
			await _db.SaveChangesAsync();
			return;
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task DeletePatient(Guid id)
	{
		var rowsAffected = await _db.Patient.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"Patient {id} not found");
	}

	#endregion

	#region Update

	public async Task<byte[]> UpdatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);

		try {
			_db.Attach(patient);
			_db.Entry(patient).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			return patient.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	public async Task<byte[]> UpdateFirstName(UpdateFirstNameReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var patient = new Patient {
				Id = request.Id,
				RowVersion = request.RowVersion,
				FirstName = request.FirstName,
			};

			_db.Attach(patient);
			_db.Entry(patient).Property(u => u.FirstName).IsModified = true;

			await _db.SaveChangesAsync();

			return patient.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	public async Task<byte[]> UpdateLastNameAndEmail(UpdateLastNameAndEmailReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var patient = new Patient {
				Id = request.Id,
				RowVersion = request.RowVersion,
				LastName = request.LastName,
				Email = request.Email,
			};

			_db.Attach(patient);
			_db.Entry(patient).Property(u => u.LastName).IsModified = true;
			_db.Entry(patient).Property(u => u.Email).IsModified = true;

			await _db.SaveChangesAsync();

			return patient.RowVersion;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Patient> GetById(Guid id)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			throw new NotFoundException($"Patient not found");
	
		return patient;
	}
	
	public async Task<Patient> GetByEmail(string email)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(email))
			dbQuery = dbQuery.Where(x => x.Email == email);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			throw new NotFoundException($"Patient not found");
	
		return patient;
	}
	
	public async Task<Patient> GetByIdWithInvoices(Guid id)
	{
		var dbQuery = _db.Patient.AsNoTracking();
		dbQuery = dbQuery.Include(x => x.Invoices);
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			throw new NotFoundException($"Patient not found");
	
		return patient;
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<ListPage<Patient>> GetAllPaging(GetAllPagingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		var listPage = new ListPage<Patient>();
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (if requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			listPage.TotalRowCount = await dbQuery.CountAsync();
			if (request.GetRowCountOnly)
				return listPage;
		}
		else if (!request.RecalcRowCount && !request.GetRowCountOnly)
		{
			// Make it clear that row count is not calculated
			listPage.TotalRowCount = -1;
		}
	
		var data = await dbQuery.ToListAsync();
	
		return data.ToListPage<Patient>();
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePaging(SearchByLastNamePagingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		var listPage = new ListPage<Patient>();
		// Stable ordering for paging
		dbQuery = dbQuery.OrderBy(x => x.LastName).ThenBy(x => x.Id);
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (if requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			listPage.TotalRowCount = await dbQuery.CountAsync();
			if (request.GetRowCountOnly)
				return listPage;
		}
		else if (!request.RecalcRowCount && !request.GetRowCountOnly)
		{
			// Make it clear that row count is not calculated
			listPage.TotalRowCount = -1;
		}
	
		var data = await dbQuery.ToListAsync();
	
		return data.ToListPage<Patient>();
	}
	
	public async Task<List<Patient>> SearchByLastNameSorting(SearchByLastNameSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var listPage = new ListPage<Patient>();
		// Stable ordering for paging
		dbQuery = dbQuery.OrderBy(x => x.LastName).ThenBy(x => x.Id);
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (if requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			listPage.TotalRowCount = await dbQuery.CountAsync();
			if (request.GetRowCountOnly)
				return listPage;
		}
		else if (!request.RecalcRowCount && !request.GetRowCountOnly)
		{
			// Make it clear that row count is not calculated
			listPage.TotalRowCount = -1;
		}
	
		var data = await dbQuery.ToListAsync();
	
		return data.ToListPage<Patient>();
	}
	
	public async Task<List<Patient>> GetAllSorting(GetAllSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	public async Task<List<Patient>> SearchByLastEmailOpt(SearchByLastEmailOptReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
			// Optional
	
		if (!string.IsNullOrWhiteSpace(request.Email))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Email, $"%{request.Email}%"));
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	public async Task<List<Patient>> SearchByEmail(SearchByEmailReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.Email))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Email, $"%{request.Email}%"));
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	public async Task<List<Patient>> GetActive()
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Internal
		dbQuery = dbQuery.Where(x => x.IsActive == true);
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	public async Task<ListPage<Patient>> GetAllPagingSorting(GetAllPagingSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var listPage = new ListPage<Patient>();
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (if requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			listPage.TotalRowCount = await dbQuery.CountAsync();
			if (request.GetRowCountOnly)
				return listPage;
		}
		else if (!request.RecalcRowCount && !request.GetRowCountOnly)
		{
			// Make it clear that row count is not calculated
			listPage.TotalRowCount = -1;
		}
	
		var data = await dbQuery.ToListAsync();
	
		return data.ToListPage<Patient>();
	}
	
	public async Task<List<Patient>> SearchActiveLastName(string lastName)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(lastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{lastName}%"));
	
		// Internal
		dbQuery = dbQuery.Where(x => x.IsActive == true);
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	#endregion
	
	private void AddSorting(ref IQueryable<Patient> dbQuery, ISortingRequest sortingRequest)
	{
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.Id, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.Id);
			else
				dbQuery.OrderBy(x => x.Id);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.FirstName, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.FirstName);
			else
				dbQuery.OrderBy(x => x.FirstName);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.LastName, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.LastName);
			else
				dbQuery.OrderBy(x => x.LastName);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.Email, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.Email);
			else
				dbQuery.OrderBy(x => x.Email);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.IsActive, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.IsActive);
			else
				dbQuery.OrderBy(x => x.IsActive);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.PracticeId, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.PracticeId);
			else
				dbQuery.OrderBy(x => x.PracticeId);
	
		if (string.Equals(sortingRequest.SortBy, Patient.PropNames.RowVersion, StringComparison.OrdinalIgnoreCase))
			if (sortingRequest.SortDesc)
				dbQuery.OrderByDescending(x => x.RowVersion);
			else
				dbQuery.OrderBy(x => x.RowVersion);
	}
	
}
