using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace abtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        [HttpGet]
        public Calculate Get()
        {
            return new Calculate();
        }
    }
}