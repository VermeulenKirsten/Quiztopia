using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.API.DTOs
{
    public class LoginDTO
    {
        //Alle Identity elementen die via Javascript/JSON verstuurd worden
        //Basis voor CookieAutenticate en/of JWT authenticatie
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
