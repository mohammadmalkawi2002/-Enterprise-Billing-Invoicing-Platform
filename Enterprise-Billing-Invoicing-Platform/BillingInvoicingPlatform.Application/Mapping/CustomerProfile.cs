using AutoMapper;
using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Domain.Entities;
using BillingInvoicingPlatform.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Mapping
{
    public class CustomerProfile :Profile
    {
        public CustomerProfile()
        {
            //Mapping from DTO To Entity:
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            //Mapping From Entity To Dto:
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.InvoiceCount, opt => opt.MapFrom(src => src.Invoices.Count))
             .ForMember(dest => dest.Status,
               opt => opt.MapFrom(src => src.Status.ToString()));
            // Address mapping
            CreateMap<AddressDto, Address>().ReverseMap();
        }
    }
}
