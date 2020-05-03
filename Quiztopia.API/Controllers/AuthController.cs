using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quiztopia.API.DTOs;

namespace Quiztopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        //Authcontroller.cs
        [HttpPost]
        [Route("login")] //vult de controller basis route aan
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            //LoginViewModel met (Required) UserName en Password aanbrengen.
            var returnMessage = "";
            if (!ModelState.IsValid)
                return BadRequest("Incomplete data");
            try
            {
                //geen persistence, geen lockout -> via false, false
                var result = await
               _signInManager.PasswordSignInAsync(loginDTO.UserName,
               loginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok("Welcome " + loginDTO.UserName);
                }
                throw new Exception("User or paswoord not found.");
                //zo algemeen mogelijk response. Vertelt niet dat het pwd niet juist is.
            }
            catch (Exception exc)
            {
                returnMessage = $"Invalid entry: {exc.Message}";
                ModelState.AddModelError("", returnMessage);
            }
            return BadRequest(returnMessage); //zo weinig mogelijk (hacker) info
        }
    }
}