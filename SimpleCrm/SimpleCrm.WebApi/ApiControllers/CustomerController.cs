using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SimpleCrm.WebApi.Filters;
using SimpleCrm.WebApi.Models;
using System.Linq;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly LinkGenerator _linkGenerator;

        public CustomerController(ICustomerData customerData,
            LinkGenerator linkGenerator)
        {
            _customerData = customerData;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name ="GetCustomers")] //  ./api/customers
        public IActionResult GetAll([FromQuery]int page = 1, [FromQuery] int take = 50)
        {
            if (page < 1)
                return UnprocessableEntity(new ValidationFailedResult("Page must be 1 or greater."));
            if (take < 1)
                return UnprocessableEntity(new ValidationFailedResult("Take must be 1 or greater."));
            if (take > 500)
                return UnprocessableEntity(new ValidationFailedResult("Take cannot be larger than 500."));

            var customers = _customerData.GetAll(page - 1, take, "");
            var models = customers.Select(c => new CustomerDisplayViewModel(c));

            var pagination = new PaginationModel
            {
                Previous = page <= 1 ? null : CreateCustomersResourceUri(page - 1, take),
                Next = customers.Count < take ? null : CreateCustomersResourceUri(page + 1, take)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

            return Ok(models);
        }
        private string CreateCustomersResourceUri(int page, int take)
        {
            return _linkGenerator.GetPathByName(HttpContext, "GetCustomers", values: new
            {
                take = take,
                page = page
            });
        }

        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")] //  ./api/customers/:id
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }
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
                return new ValidationFailedResult(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod
            };

            _customerData.Add(customer);
            _customerData.Commit();
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
                return new ValidationFailedResult(ModelState);
            }

            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            //update only editable properties from model
            customer.EmailAddress = model.EmailAddress;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.PreferredContactMethod = model.PreferredContactMethod;

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
                return NotFound();
            }
            _customerData.Delete(customer);
            _customerData.Commit();
            return NoContent();
        }
    }
}
