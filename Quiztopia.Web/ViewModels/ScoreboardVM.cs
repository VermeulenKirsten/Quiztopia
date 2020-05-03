using Microsoft.AspNetCore.Identity;
using Quiztopia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class ScoreboardVM
    {
        public Quiz Quiz { get; set; }
        public IdentityUser User { get; set; }
    }
}
