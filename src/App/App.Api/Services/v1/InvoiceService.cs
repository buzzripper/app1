//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/8/2026 8:50 PM. Any changes made to it will be lost.
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

namespace Dyvenix.App1.App.Services.v1;

public interface IInvoiceService
{
	Task<Result<Guid>> CreateInvoice(Invoice invoice);
	Task<Result> DeleteInvoice(Guid id);
	Task<Result> UpdateMemo(Guid id, string memo);
	Task<Result<Invoice>> GetById(Guid id);
	Task<Result<List<Invoice>>> QueryByMemo(string memo);
}

public partial class InvoiceService : IInvoiceService
{
	private readonly ILogger<InvoiceService> _logger;
	private readonly App1Db _db;

	public InvoiceService(App1Db db, ILogger<InvoiceService> logger)
	{
		_db = db;
		_logger = logger;
	}

	#region Create

	public async Task<Result<Guid>> CreateInvoice(Invoice invoice)
	{
		ArgumentNullException.ThrowIfNull(invoice);

		try {
			_db.Add(invoice);
			await _db.SaveChangesAsync();

			return Result<Guid>.Ok(invoice.Id);

		} catch (DbUpdateConcurrencyException) {
			return Result<Guid>.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task<Result> DeleteInvoice(Guid id)
	{
		var rowsAffected = await _db.Invoice.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			return Result.NotFound($"Invoice {id} not found");

		return Result.Ok();
	}

	#endregion

	#region Update

	public async Task<Result> UpdateMemo(Guid id, string memo)
	{
		ArgumentNullException.ThrowIfNull(memo);

		try {
			var invoice = new Invoice {
				Id = id,
				Memo = memo,
			};

			_db.Attach(invoice);
			_db.Entry(invoice).Property(u => u.Memo).IsModified = true;

			await _db.SaveChangesAsync();

			return Result.Ok();

		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Result<Invoice>> GetById(Guid id)
	{
		var dbQuery = _db.Invoice.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var invoice = await dbQuery.FirstOrDefaultAsync();
	
		if (invoice is null)
			return Result<Invoice>.NotFound($"Invoice not found");
	
		return Result<Invoice>.Ok(invoice);
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<Result<List<Invoice>>> QueryByMemo(string memo)
	{
		var dbQuery = _db.Invoice.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(memo))
			dbQuery = dbQuery.Where(x => x.Memo == memo);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<Invoice>>.Ok(data);
	}
	
	#endregion
}
