using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class AddRoleVM
    {
        // Properties

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        // Constructor

        public AddRoleVM()
        {

        }

        public AddRoleVM(string roleName)
        {
            this.RoleName = roleName;
        }
    }
}
