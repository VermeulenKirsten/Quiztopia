using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class AddRoleVM
    {
        // Properties

        public string RoleName { get; set; }

        // Constructor
        public AddRoleVM(string roleName)
        {
            this.RoleName = roleName;
        }
    }
}
