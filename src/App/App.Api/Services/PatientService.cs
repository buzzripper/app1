////------------------------------------------------------------------------------------------------------------
//// This file was auto-generated on 2/1/2026 4:43 PM. Any changes made to it will be lost.
////------------------------------------------------------------------------------------------------------------
//using Dyvenix.App1.App.Shared.Queries.v1;
//using Dyvenix.App1.Common.Data;
//using Dyvenix.App1.Common.Data.Shared.Entities;
//using Dyvenix.App1.Common.Shared.Exceptions;
//using Dyvenix.App1.Common.Shared.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace Dyvenix.App1.App.Api.Services.v1;

//public interface IPatientService
//{
//	Task<Result<Guid>> CreatePatient(Patient patient);
//	Task<Result> DeletePatient(Guid id);
//	Task<Result> UpdatePatient(Patient patient);
//	Task<Result> UpdateFirstName(Guid id, string firstName);
//	Task<Result> UpdateLastNameAndEmail(Guid id, string lastName, string email);
//	Task<Result<Patient>> GetById(Guid id);
//	Task<Result<Patient>> GetByEmail(string email);
//	Task<Result<List<Patient>>> GetAllPaging(int pageSize = 0, int pageOffset = 0);
//	Task<Result<EntityList<Patient>>> QueryByLastNamePaging(QueryByLastNamePagingQuery query);
//}

//public partial class PatientService : IPatientService
//{
//	private readonly ILogger<PatientService> _logger;
//	private readonly App1Db _db;

//	public PatientService(App1Db db, ILogger<PatientService> logger)
//	{
//		_db = db;
//		_logger = logger;
//	}

//	#region Create

//	public async Task<Result<Guid>> CreatePatient(Patient patient)
//	{
//		ArgumentNullException.ThrowIfNull(patient);

//		try
//		{
//			_db.Add(patient);
//			await _db.SaveChangesAsync();

//			return Result<Guid>.Ok(patient.Id);
//		}
//		catch (DbUpdateConcurrencyException)
//		{
//			return Result<Guid>.Conflict("The item was modified or deleted by another user.");
//		}
//	}

//	#endregion

//	#region Delete

//	public async Task<Result> DeletePatient(Guid id)
//	{
//		var rowsAffected = await _db.Patient.Where(a => a.Id == id).ExecuteDeleteAsync();

//		if (rowsAffected == 0)
//			return Result.NotFound($"Patient {id} not found");

//		return Result.Ok();
//	}

//	#endregion

//	#region Update

//	public async Task<Result> UpdatePatient(Patient patient)
//	{
//		ArgumentNullException.ThrowIfNull(patient);

//		try
//		{
//			_db.Attach(patient);
//			_db.Entry(patient).State = EntityState.Modified;
//			await _db.SaveChangesAsync();

//			return Result.Ok();
//		}
//		catch (DbUpdateConcurrencyException)
//		{
//			return Result.Conflict("The item was modified or deleted by another user.");
//		}
//	}

//	public async Task<Result> UpdateFirstName(Guid id, string firstName)
//	{
//		ArgumentNullException.ThrowIfNull(firstName);

//		try
//		{
//			var patient = new Patient
//			{
//				Id = id,
//				FirstName = firstName,
//			};

//			_db.Attach(patient);
//			_db.Entry(patient).Property(u => u.FirstName).IsModified = true;

//			await _db.SaveChangesAsync();

//			return Result.Ok();
//		}
//		catch (DbUpdateConcurrencyException)
//		{
//			return Result.Conflict("The item was modified or deleted by another user.");
//		}
//	}

//	public async Task<Result> UpdateLastNameAndEmail(Guid id, string lastName, string email)
//	{
//		ArgumentNullException.ThrowIfNull(lastName);
//		ArgumentNullException.ThrowIfNull(email);

//		try
//		{
//			var patient = new Patient
//			{
//				Id = id,
//				LastName = lastName,
//				Email = email,
//			};

//			_db.Attach(patient);
//			_db.Entry(patient).Property(u => u.LastName).IsModified = true;
//			_db.Entry(patient).Property(u => u.Email).IsModified = true;

//			await _db.SaveChangesAsync();

//			return Result.Ok();
//		}
//		catch (DbUpdateConcurrencyException)
//		{
//			return Result.Conflict("The item was modified or deleted by another user.");
//		}
//	}

//	#endregion

//	#region Single Methods

//	public async Task<Result<Patient>> GetById(Guid id)
//	{
//		var patient = await _db.Patient
//			.AsNoTracking()
//			.FirstOrDefaultAsync(x => x.Id == id);

//		if (patient is null)
//			return Result<Patient>.NotFound($"Patient {id} not found");

//		return Result<Patient>.Ok(patient);
//	}

//	public async Task<Result<Patient>> GetByEmail(string email)
//	{
//		var dbQuery = _db.Patient.AsQueryable();

//		if (!string.IsNullOrWhiteSpace(email))
//			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Email, $"%{email}%"));

//		var patient = await dbQuery.AsNoTracking().FirstOrDefaultAsync();

//		if (patient is null)
//			return Result<Patient>.NotFound($"Patient with email '{email}' not found");

//		return Result<Patient>.Ok(patient);
//	}

//	#endregion

//	#region List Methods

//	public async Task<Result<List<Patient>>> GetAllPaging(int pageSize = 0, int pageOffset = 0)
//	{
//		var dbQuery = _db.Patient.AsQueryable();

//		if (pageSize > 0)
//			dbQuery = dbQuery.Skip(pageOffset * pageSize).Take(pageSize);

//		var patients = await dbQuery.AsNoTracking().ToListAsync();

//		return Result<List<Patient>>.Ok(patients);
//	}

//	#endregion

//	#region Query Methods

//	public async Task<Result<EntityList<Patient>>> QueryByLastNamePaging(QueryByLastNamePagingQuery query)
//	{
//		var dbQuery = _db.Patient.AsQueryable();
//		var result = new EntityList<Patient>();

//		// Filters
//		if (!string.IsNullOrWhiteSpace(query.LastName))
//			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, query.LastName));

//		// Paging
//		if (query.RecalcRowCount || query.GetRowCountOnly)
//			result.TotalRowCount = dbQuery.Count();
//		if (query.GetRowCountOnly)
//			return Result<EntityList<Patient>>.Ok(result);
//		if (query.PageSize > 0)
//			dbQuery = dbQuery.Skip(query.PageOffset).Take(query.PageSize);

//		result.Data = await dbQuery.AsNoTracking().ToListAsync();

//		return Result<EntityList<Patient>>.Ok(result);
//	}

//	#endregion
//}
