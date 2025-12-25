using BillingInvoicingPlatform.Application.Dto.Customer;
using BillingInvoicingPlatform.Application.Service;
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
        private readonly CustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomersController(CustomerService customerService,IValidator<CreateCustomerDto> createValidator, IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerService = customerService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
           var validationResult = await _createValidator.ValidateAsync(customerDto);
            if (!validationResult.IsValid)
               {

                var errorResponse = validationResult.Errors.Select(e => new
               {
                    PropertyName = e.PropertyName,
                    Error = e.ErrorMessage,
                    ErrorCode = e.ErrorCode,
                    attemptedValue = e.AttemptedValue
                });
                return BadRequest(new { Errors = errorResponse });
            }

            var createdCustomer = await _customerService.CreateCustomer(customerDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }


        [HttpPut("{id}")]

        public async Task<ActionResult> Update(int id, UpdateCustomerDto dto) 
        {
            if (id != dto.Id)
                return BadRequest();

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    PropertyName = e.PropertyName,
                    Error = e.ErrorMessage,
                    ErrorCode = e.ErrorCode,
                    attemptedValue = e.AttemptedValue
                });
                return BadRequest(new { Errors = errorResponse });
            }

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