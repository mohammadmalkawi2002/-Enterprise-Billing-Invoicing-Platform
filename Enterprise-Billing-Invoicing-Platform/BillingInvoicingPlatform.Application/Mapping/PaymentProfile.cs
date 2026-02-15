using AutoMapper;
using BillingInvoicingPlatform.Application.Dto.Payment;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Mapping
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            //Dto -> Entity:
            CreateMap<CreatePaymentDto, Payment>()
            .ForMember(dest => dest.PaymentMethod,
                opt => opt.MapFrom(src => ParsePaymentMethod(src.PaymentMethod)));



            // Payment -> PaymentDto
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.InvoiceNumber,
                    opt => opt.MapFrom(src => src.Invoice.InvoiceNumber))

                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Invoice.Customer.Name))

                .ForMember(dest => dest.PaymentMethod,
                    opt => opt.MapFrom(src => src.PaymentMethod.ToString()));
                
        }
        private PaymentMethod ParsePaymentMethod(string paymentMethodString) 
        {
         return Enum.TryParse<PaymentMethod>(paymentMethodString,true,out var result) 
                ? result :PaymentMethod.Cash; 
        }
    }
}
