using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Application.Service;
using BillingInvoicingPlatform.Application.Service.Abstraction;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BillingInvoicingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
    

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
           
        }


        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAll([FromQuery] CustomerQueryDto queryDto) 
        {
            
            var pagedResult = await _customerService.GetAllCustomer(queryDto);

            var metaData = new 
            { 
            pagedResult.TotalPages,
            pagedResult.TotalCount,
            pagedResult.PageSize,
            pagedResult.PageNumber,
            pagedResult.HasPrevious,
            pagedResult.HasNext
            };

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(metaData));


            return Ok(pagedResult.Items);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer is null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto customerDto)
        {
           

            var createdCustomer = await _customerService.CreateCustomer(customerDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }


        [HttpPut("{id}")]

        public async Task<ActionResult> Update(int id, UpdateCustomerDto dto) 
        {
            if (id != dto.Id)
                return BadRequest();

            

            await _customerService.UpdateCustomer(dto);
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
          
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}