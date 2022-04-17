using API.Models;
using Client.Base;
using Client.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<Overtime, int>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeRepository(Address address, string request = "Overtimes/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        //public async Task<List<ListOvertimeVM>> GetListOvertime()
        //{
        //    List<ListOvertimeVM> entities = new List<ListOvertimeVM>();
        //    var token = _contextAccessor.HttpContext.Session.GetString("JWToken");

        //    using (var response = await httpClient.GetAsync(address.link + request + "List/"))
        //    {
        //        string apiResponse = await response.Content.ReadAsStringAsync();
        //        entities = JsonConvert.DeserializeObject<List<ListOvertimeVM>>(apiResponse);
        //    }

        //    return entities;
        //}
    }
}
