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

namespace Dyvenix.App1.App.Shared.Contracts.v1;

public interface IInvoiceService
{
	Task CreateInvoice(Invoice invoice);
	Task DeleteInvoice(Guid id);
	Task UpdateMemo(UpdateMemoReq request);
	Task UpdateAmount(UpdateAmountReq request);
	Task<Invoice> GetById(Guid id);
	Task<Invoice> GetAll();
	Task<List<Invoice>> QueryByMemo(string memo);
}
