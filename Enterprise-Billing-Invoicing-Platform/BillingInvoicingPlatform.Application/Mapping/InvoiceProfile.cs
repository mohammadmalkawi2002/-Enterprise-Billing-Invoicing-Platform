using AutoMapper;
using BillingInvoicingPlatform.Application.Dto.Invoice;
using BillingInvoicingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Mapping
{
    public class InvoiceProfile:Profile
    {
        public InvoiceProfile()
        {
            //from Dto To Entity:
            CreateMap<CreateInvoiceDto, Invoice>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());

            CreateMap<CreateInvoiceItemDto, InvoiceItem>()
                .ForMember(dest=>dest.LineTotal,opt=>opt.Ignore());

            CreateMap<UpdateInvoiceDto, Invoice>()
                 .ForMember(dest => dest.Items, opt => opt.Ignore());


            CreateMap<UpdateInvoiceItemDto, InvoiceItem>();

            //from Entity To Dto:
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.TotalPaid, opt => opt.MapFrom(src => src.TotalPaid))
                .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => src.RemainingBalance))
                .ForMember(dest=>dest.DaysOverdue,opt=>opt.MapFrom(src=>src.DaysOverdue));

           


            CreateMap<InvoiceItem, InvoiceItemDto>()
    
    .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.LineTotal));
        }
    }
}
        