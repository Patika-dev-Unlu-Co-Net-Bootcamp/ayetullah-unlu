using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UnluCo.Egitim.API.Ikinci.Hafta.CustomAttributes;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Permissions("admin")]
        public IActionResult Get()
        {
            var role = "client";
            var permissions = typeof(UsersController).GetMethod("Get")
                .GetCustomAttributes(typeof(PermissionsAttribute), true)
                .Cast<PermissionsAttribute>().Select(x => x.Role).ToArray();
            if(permissions.Any(x => x.Equals(role)))
            {
                return Ok();
            } else
            {
                return Forbid();
            }
        }
    }
}
