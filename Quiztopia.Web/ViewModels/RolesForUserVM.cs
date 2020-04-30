using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class RolesForUserVM
    {
        // Properties

        public IdentityUser User { get; set; } //De ApplicationUser
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public IList<string> AssignedRoles { get; set; }
        public IList<string> UnAssignedRoles { get; set; }
    }
}
