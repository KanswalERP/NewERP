using NtierMvc.BusinessLogic.Interface;
using NtierMvc.BusinessLogic.Worker;
using NtierMvc.Model.Customer;
using System.Web.Http;
using System.Web.Http.Description;

namespace NtierMvc.API.Controllers.Application
{
    public class CustomerDetailsController : ApiController
    {
        ICustomerWorker _repository = new CustomerWorker();


        [HttpPost]
        [ResponseType(typeof(string))]
        [Route("api/CustomerDetails/SaveCustomerDetails")]
        public IHttpActionResult SaveCustomerDetails(CustomerEntity viewModel)
        {
            return Ok(_repository.SaveCustomerDetails(viewModel));
        }

        [HttpGet]
        [Route("api/CustomerDetails/GetUserDetails")]
        public IHttpActionResult GetUserDetails(string unitNo)
        {
            return Ok(_repository.GetUserCustDetails(unitNo));
        }

        [HttpPost]
        [ResponseType(typeof(CustomerEntity))]
        [Route("api/CustomerDetails/CustomerDetailsPopup")]
        public IHttpActionResult CustomerDetailsPopup(CustomerEntity Model)
        {
            return Ok(_repository.CustomerDetailsPopup(Model));
        }

        [Route("api/CustomerDetails/GetCustomerDetails")]
        public IHttpActionResult GetCustomerDetails(int pageIndex, int pageSize, string SearchCountry = null, string SearchCustomerID = null, string SearchCustomerIsActive = null)
        {
            return Ok(_repository.GetCustomerDetails(pageIndex, pageSize, SearchCountry, SearchCustomerID, SearchCustomerIsActive));
        }

        [HttpPost]
        [Route("api/CustomerDetails/DeleteCustomerDetail")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteCustomerDetail(int[] param)
        {
            return Ok(_repository.DeleteCustomerDetail(param[0]));
        }

        [HttpGet]
        [Route("api/CustomerDetails/GetDdlValueForCustomer")]
        public IHttpActionResult GetDdlValueForCustomer(string type, string CountryId, string CustomerId = null)
        {
            return Ok(_repository.GetDdlValueForCustomer(type, CountryId, CustomerId));
        }





    }
}
