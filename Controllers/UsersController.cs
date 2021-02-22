using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace abtest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return new User().GetAudit();
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserMSG msg)
        {
            new User().SetAudit(msg);

            return Ok();
        }
    }
}