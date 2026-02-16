//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/15/2026 7:07 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Api.Services.v1;

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

	public async Task CreateInvoice(Invoice invoice)
	{
		ArgumentNullException.ThrowIfNull(invoice);

		try {
			_db.Add(invoice);
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

	public async Task DeleteInvoice(Guid id)
	{
		var rowsAffected = await _db.Invoice.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"Invoice {id} not found");
	}

	#endregion

	#region Update

	public async Task UpdateMemo(UpdateMemoReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var invoice = new Invoice {
				Id = request.Id,
				Memo = request.Memo,
			};

			_db.Attach(invoice);
			_db.Entry(invoice).Property(u => u.Memo).IsModified = true;

			await _db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	public async Task UpdateAmount(UpdateAmountReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var invoice = new Invoice {
				Id = request.Id,
				Amount = request.Amount,
			};

			_db.Attach(invoice);
			_db.Entry(invoice).Property(u => u.Amount).IsModified = true;

			await _db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Invoice> GetById(Guid id)
	{
		var dbQuery = _db.Invoice.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		return await dbQuery.FirstOrDefaultAsync();
	}
	
	public async Task<Invoice> GetAll()
	{
		var dbQuery = _db.Invoice.AsNoTracking();
	
		return await dbQuery.FirstOrDefaultAsync();
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<List<Invoice>> QueryByMemo(string memo)
	{
		var dbQuery = _db.Invoice.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(memo))
			dbQuery = dbQuery.Where(x => x.Memo == memo);
	
		return await dbQuery.ToListAsync();
	}
	
	#endregion
}
