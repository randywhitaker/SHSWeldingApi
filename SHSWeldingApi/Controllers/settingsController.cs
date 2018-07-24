using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SHSWeldingApi.Models;

namespace SHSWeldingApi.Controllers
{
    public class settingsController : ApiController
    {
        public AppSettings Get()
        {
            var settings = new AppSettings();
            return settings;
        }
    }
}
