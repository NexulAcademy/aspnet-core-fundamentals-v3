using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SimpleCrm.WebApi.Filters;
using SimpleCrm.WebApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Logging;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customers")]
    [Authorize(Policy = "ApiUser")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly ILogger<CustomerController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public CustomerController(ICustomerData customerData,
            ILogger<CustomerController> logger,
            LinkGenerator linkGenerator)
        {
            _customerData = customerData;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name ="GetCustomers")] //  ./api/customers
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult GetAll([FromQuery]CustomerListParameters listParameters)
        {
            if (listParameters.Page < 1)
                return UnprocessableEntity(new ValidationFailedResult("Page must be 1 or greater."));
            if (listParameters.Take < 1)
                return UnprocessableEntity(new ValidationFailedResult("Take must be 1 or greater."));
            if (listParameters.Take > 500)
            {
                _logger.LogError("Get Customers max items exceeded.");
                return UnprocessableEntity(new ValidationFailedResult("Take cannot be larger than 500."));
            }

            var customers = _customerData.GetAll(listParameters);
            var models = customers.Select(c => new CustomerDisplayViewModel(c));

            var pagination = new PaginationModel
            {
                Previous = CreateCustomersResourceUri(listParameters, -1),
                Next = CreateCustomersResourceUri(listParameters, 1)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
            Response.Headers.Add("ETag", "\"abc\"");

            return Ok(models);
        }
        private string CreateCustomersResourceUri(CustomerListParameters listParameters, int pageAdjust)
        {
            if (listParameters.Page + pageAdjust <= 0)
                return null;

            return _linkGenerator.GetPathByName(HttpContext, "GetCustomers", values: new
            {
                take = listParameters.Take,
                page = listParameters.Page + pageAdjust,
                orderBy = listParameters.OrderBy,
                lastName = listParameters.LastName,
                term = listParameters.Term
            });
        }

        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")] //  ./api/customers/:id
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }
            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            var model = new CustomerDisplayViewModel(customer);
            return Ok(model);
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody] CustomerCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Customer Create failed due to validation");
                return new ValidationFailedResult(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod,
                LastContactDate = DateTime.UtcNow
            };

            _customerData.Add(customer);
            _customerData.Commit();

            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            return Ok(new CustomerDisplayViewModel(customer)); //includes new auto-assigned id
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody] CustomerUpdateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Customer Update failed due to validation");
                return new ValidationFailedResult(ModelState);
            }

            var customer = _customerData.Get(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }

            string ifMatch = Request.Headers["If-Match"];
            if (ifMatch != customer.LastContactDate.ToString())
            {
                _logger.LogInformation("Customer update failed due to concurrency issue: {0}", id);
                return StatusCode(422, "Customer has been changed by another user since it was loaded. Reload and try again.");
            }

            //update only editable properties from model
            customer.EmailAddress = model.EmailAddress;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.PreferredContactMethod = model.PreferredContactMethod;
            customer.LastContactDate = DateTime.UtcNow;

            _customerData.Update(customer);
            _customerData.Commit();
            return Ok(new CustomerDisplayViewModel(customer)); //server version, updated per request
        }
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Deleting customer: {0}", id);
            _customerData.Delete(customer);
            _customerData.Commit();
            return NoContent();
        }
    }
}
