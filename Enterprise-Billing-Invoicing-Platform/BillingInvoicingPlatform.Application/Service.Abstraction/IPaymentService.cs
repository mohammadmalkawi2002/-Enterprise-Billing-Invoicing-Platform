using BillingInvoicingPlatform.Application.Dto.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service.Abstraction
{
    public interface IPaymentService
    {
        Task<PaymentDto?> GetPaymentById(int paymentId);
        Task<PaymentDto> RecordPayment(CreatePaymentDto dto);
        Task DeletePaymentAsync(int paymentId);

    }
}
