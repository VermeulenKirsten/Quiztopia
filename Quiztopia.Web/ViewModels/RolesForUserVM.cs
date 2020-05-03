using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class RolesForUserVM
    {
        // Properties

        public IdentityUser User { get; set; } //De ApplicationUser
        public string UserId { get; set; }

        [Display(Name = "Role")]
        public string RoleId { get; set; }

        [Display(Name = "Assigned roles")]
        public IList<string> AssignedRoles { get; set; }

        [Display(Name = "Unassigned roles")]
        public IList<string> UnAssignedRoles { get; set; }
    }
}
