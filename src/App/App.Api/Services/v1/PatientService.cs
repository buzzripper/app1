//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 9:13 PM. Any changes made to it will be lost.
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
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Extensions;

namespace Dyvenix.App1.App.Services.v1;

public interface IPatientService
{
	Task<Result<Guid>> CreatePatient(Patient patient);
	Task<Result> DeletePatient(Guid id);
	Task<Result> UpdatePatient(Patient patient);
	Task<Result> UpdateFirstName(Guid id, string firstName);
	Task<Result> UpdateLastNameAndEmail(Guid id, string lastName, string email);
	Task<Result<Patient>> GetById(Guid id);
	Task<Result<Patient>> GetByEmail(string email);
	Task<Result<Patient>> GetByIdWithInvoices(Guid id);
	Task<Result<EntityList<Patient>>> GetAllPaging(GetAllPagingReq request);
	Task<Result<EntityList<Patient>>> SearchByLastNamePaging(SearchByLastNamePagingReq request);
	Task<Result<List<Patient>>> SearchByLastNameSorting(SearchByLastNameSortingReq request);
	Task<Result<EntityList<Patient>>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request);
	Task<Result<List<Patient>>> GetAllSorting(GetAllSortingReq request);
	Task<Result<List<Patient>>> SearchByLastEmailOpt(SearchByLastEmailOptReq request);
	Task<Result<List<Patient>>> SearchByEmail(SearchByEmailReq request);
	Task<Result<List<Patient>>> GetActive();
	Task<Result<EntityList<Patient>>> GetAllPagingSorting(GetAllPagingSortingReq request);
	Task<Result<List<Patient>>> SearchActiveLastName(string lastName);
}

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

	public async Task<Result<Guid>> CreatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);

		try {
			_db.Add(patient);
			await _db.SaveChangesAsync();

			return Result<Guid>.Ok(patient.Id);

		} catch (DbUpdateConcurrencyException) {
			return Result<Guid>.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task<Result> DeletePatient(Guid id)
	{
		var rowsAffected = await _db.Patient.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			return Result.NotFound($"Patient {id} not found");

		return Result.Ok();
	}

	#endregion

	#region Update

	public async Task<Result> UpdatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);

		try {
			_db.Attach(patient);
			_db.Entry(patient).State = EntityState.Modified;
			await _db.SaveChangesAsync();

		return Result.Ok();
		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	public async Task<Result> UpdateFirstName(Guid id, string firstName)
	{
		ArgumentNullException.ThrowIfNull(firstName);

		try {
			var patient = new Patient {
				Id = id,
				FirstName = firstName,
			};

			_db.Attach(patient);
			_db.Entry(patient).Property(u => u.FirstName).IsModified = true;

			await _db.SaveChangesAsync();

			return Result.Ok();

		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	public async Task<Result> UpdateLastNameAndEmail(Guid id, string lastName, string email)
	{
		ArgumentNullException.ThrowIfNull(lastName);
		ArgumentNullException.ThrowIfNull(email);

		try {
			var patient = new Patient {
				Id = id,
				LastName = lastName,
				Email = email,
			};

			_db.Attach(patient);
			_db.Entry(patient).Property(u => u.LastName).IsModified = true;
			_db.Entry(patient).Property(u => u.Email).IsModified = true;

			await _db.SaveChangesAsync();

			return Result.Ok();

		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Result<Patient>> GetById(Guid id)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			return Result<Patient>.NotFound($"Patient not found");
	
		return Result<Patient>.Ok(patient);
	}
	
	public async Task<Result<Patient>> GetByEmail(string email)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(email))
			dbQuery = dbQuery.Where(x => x.Email == email);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			return Result<Patient>.NotFound($"Patient not found");
	
		return Result<Patient>.Ok(patient);
	}
	
	public async Task<Result<Patient>> GetByIdWithInvoices(Guid id)
	{
		var dbQuery = _db.Patient.AsNoTracking();
		dbQuery = dbQuery.Include(x => x.Invoices);
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var patient = await dbQuery.FirstOrDefaultAsync();
	
		if (patient is null)
			return Result<Patient>.NotFound($"Patient not found");
	
		return Result<Patient>.Ok(patient);
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<Result<EntityList<Patient>>> GetAllPaging(GetAllPagingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		var entityList = new EntityList<Patient>();
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (only when requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			entityList.TotalRowCount = await dbQuery.CountAsync();
	
			if (request.GetRowCountOnly)
				return Result<EntityList<Patient>>.Ok(entityList);
		}
	
		var data = await dbQuery.ToListAsync();
	
		return Result<EntityList<Patient>>.Ok(data.ToEntityList<Patient>());
	}
	
	public async Task<Result<EntityList<Patient>>> SearchByLastNamePaging(SearchByLastNamePagingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		var entityList = new EntityList<Patient>();
		// Stable ordering for paging
		dbQuery = dbQuery.OrderBy(x => x.LastName).ThenBy(x => x.Id);
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (only when requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			entityList.TotalRowCount = await dbQuery.CountAsync();
	
			if (request.GetRowCountOnly)
				return Result<EntityList<Patient>>.Ok(entityList);
		}
	
		var data = await dbQuery.ToListAsync();
	
		return Result<EntityList<Patient>>.Ok(data.ToEntityList<Patient>());
	}
	
	public async Task<Result<List<Patient>>> SearchByLastNameSorting(SearchByLastNameSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
	}
	
	public async Task<Result<EntityList<Patient>>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var entityList = new EntityList<Patient>();
		// Stable ordering for paging
		dbQuery = dbQuery.OrderBy(x => x.LastName).ThenBy(x => x.Id);
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (only when requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			entityList.TotalRowCount = await dbQuery.CountAsync();
	
			if (request.GetRowCountOnly)
				return Result<EntityList<Patient>>.Ok(entityList);
		}
	
		var data = await dbQuery.ToListAsync();
	
		return Result<EntityList<Patient>>.Ok(data.ToEntityList<Patient>());
	}
	
	public async Task<Result<List<Patient>>> GetAllSorting(GetAllSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
	}
	
	public async Task<Result<List<Patient>>> SearchByLastEmailOpt(SearchByLastEmailOptReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{request.LastName}%"));
			// Optional
	
		if (!string.IsNullOrWhiteSpace(request.Email))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Email, $"%{request.Email}%"));
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
	}
	
	public async Task<Result<List<Patient>>> SearchByEmail(SearchByEmailReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.Email))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Email, $"%{request.Email}%"));
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
	}
	
	public async Task<Result<List<Patient>>> GetActive()
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Internal
		dbQuery = dbQuery.Where(x => x.IsActive == true);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
	}
	
	public async Task<Result<EntityList<Patient>>> GetAllPagingSorting(GetAllPagingSortingReq request)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		// Sorting
		if (!string.IsNullOrWhiteSpace(request.SortBy))
			this.AddSorting(ref dbQuery, request);
	
		var entityList = new EntityList<Patient>();
	
		if (request.PageSize > 0)
			dbQuery = dbQuery.Skip(request.PageOffset * request.PageSize).Take(request.PageSize);
	
		// Count (only when requested)
		if (request.RecalcRowCount || request.GetRowCountOnly)
		{
			entityList.TotalRowCount = await dbQuery.CountAsync();
	
			if (request.GetRowCountOnly)
				return Result<EntityList<Patient>>.Ok(entityList);
		}
	
		var data = await dbQuery.ToListAsync();
	
		return Result<EntityList<Patient>>.Ok(data.ToEntityList<Patient>());
	}
	
	public async Task<Result<List<Patient>>> SearchActiveLastName(string lastName)
	{
		var dbQuery = _db.Patient.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(lastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, $"%{lastName}%"));
	
		// Internal
		dbQuery = dbQuery.Where(x => x.IsActive == true);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Patient>>.Ok(data);
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
		}
	
}
