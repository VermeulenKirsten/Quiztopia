using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    // Constructor
    public class OverviewVM
    {
        public OverviewVM()
        {
        }

        // Properties

        public List<string> Questions { get; set; }
        public List<string> Answers { get; set; }

    }
}
