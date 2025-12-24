using AutoMapper;
using BillingInvoicingPlatform.Application.Dto;
using BillingInvoicingPlatform.Domain.Entities;
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
            CreateMap<Customer, CustomerDto>();
        }
    }
}
