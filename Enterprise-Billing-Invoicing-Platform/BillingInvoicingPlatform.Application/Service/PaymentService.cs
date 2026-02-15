using AutoMapper;
using BillingInvoicingPlatform.Application.Dto.Payment;
using BillingInvoicingPlatform.Application.Exceptions;
using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Service
{
    public class PaymentService:IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentService(IPaymentRepository paymentRepository,IInvoiceRepository invoiceRepository
            ,IMapper mapper,IUnitOfWork unitOfWork,ILogger<PaymentService> logger) 
        { 
             _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //TODO:Add Refund Method:
        public async Task DeletePaymentAsync(int paymentId)
        {
            //1] Load Payment with invoice from DB
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment is null)
                throw new NotFoundException();

            var invoice = await _invoiceRepository.GetByIdAsync(payment.InvoiceId);
            if (invoice is null)
                throw new NotFoundException();

            //2] Validate Invoice Status for Delete
            ValidateCanDeletePayment(invoice);

            //3] Begin Transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                //4] Delete Payment
                await _paymentRepository.DeleteAsync(payment);

                //5]  RELOAD Invoice with Payments to recalculate totals
                invoice = await _invoiceRepository.GetByIdAsync(payment.InvoiceId);
                if (invoice is null)
                    throw new NotFoundException();

                //6] Update Invoice Status after deletion
                invoice.Status = DetermineInvoiceStatus(invoice);
                invoice.UpdatedAt = DateTime.UtcNow;

                //7] Update Invoice in DB
                await _invoiceRepository.UpdateAsync(invoice);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex) 
            { 
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async Task<PaymentDto> RecordPayment(CreatePaymentDto dto)
        {
            //1] Load Invoice From DB
            var invoice = await _invoiceRepository.GetByIdAsync(dto.InvoiceId);
            if (invoice is null)
                throw new NotFoundException();

            //2] Validate Business Rules: Draft / Cancelled / Paid
            ValidateInvoiceStatusForPayment(invoice);

            // 3] Check paymentAmount Cannot exceeds remaining balance
            if (dto.PaymentAmount > invoice.RemainingBalance)
                throw new BusinessException
                    (
                  $"Payment amount ({dto.PaymentAmount:C2}) exceeds remaining balance ({invoice.RemainingBalance:C2})"
                   );

            // 4] Check Payment Date Warning
            if (dto.PaymentDate < invoice.IssueDate)
            {
                _logger.LogWarning(
                    "⚠️ Payment date ({PaymentDate:yyyy-MM-dd}) is before invoice issue date ({IssueDate:yyyy-MM-dd}). Verify correctness.",
                    dto.PaymentDate, invoice.IssueDate
                );
            }

            //5] Begin Transaction
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //6] Map DTO to Entity
                var payment = _mapper.Map<Payment>(dto);
                payment.CreatedAt = DateTime.UtcNow;

                //7] Add Payment to DB (SavePayment)
                await _paymentRepository.AddAsync(payment);

                //8]  RELOAD Invoice with Payments to recalculate totals
                invoice = await _invoiceRepository.GetByIdAsync(dto.InvoiceId);
                if (invoice is null)
                    throw new NotFoundException();

                //9] Determine New Invoice Status (now with updated payments)
                invoice.Status = DetermineInvoiceStatus(invoice);
                invoice.UpdatedAt = DateTime.UtcNow;

                //10] Update Invoice in DB
                await _invoiceRepository.UpdateAsync(invoice);

                await _unitOfWork.CommitAsync();

                // Return result
                var createdPayment = await _paymentRepository.GetByIdAsync(payment.Id);
                return _mapper.Map<PaymentDto>(createdPayment);
            }
            catch (Exception ex) 
            { 
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private InvoiceStatus DetermineInvoiceStatus(Invoice invoice) 
        {
            //Rule: once fully paid, invoice is Paid(even if it was Overdue):
            if (invoice.RemainingBalance==0)
                return InvoiceStatus.Paid;
        
            return InvoiceStatus.PartiallyPaid;
        }

        private void ValidateInvoiceStatusForPayment(Invoice invoice) 
        {
            switch (invoice.Status)
            {
                case InvoiceStatus.Draft:
                    throw new BusinessException("Cannot record payment. Invoice is still in Draft status. Please send the invoice first.");
                case InvoiceStatus.Cancelled:
                    throw new BusinessException("Cannot record payment. Invoice is cancelled.");
                case InvoiceStatus.Paid:
                    throw new BusinessException("Invoice is already paid.");
            }

        }

        private  void ValidateCanDeletePayment(Invoice invoice) 
        { 
            switch (invoice.Status)
            {
                case InvoiceStatus.Draft:
                    throw new BusinessException("Cannot delete payment. Invoice is still in Draft status. Please send the invoice first.");
                case InvoiceStatus.Cancelled:
                    throw new BusinessException("Cannot delete payment. Invoice is cancelled.");
                case InvoiceStatus.Paid:
                    throw new BusinessException($"Cannot delete payment. Invoice {invoice.InvoiceNumber} is fully paid.");
            }

        }

        public async Task<PaymentDto?> GetPaymentById(int paymentId)
        {
            var payment= await _paymentRepository.GetByIdAsync(paymentId);
            if (payment is null)
                throw new NotFoundException($"Payment with ID {paymentId} not found.");
            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
