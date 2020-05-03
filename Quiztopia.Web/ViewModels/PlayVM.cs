using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class PlayVM
    {
        // Properties
        
        public Guid UserId { get; set; }
        public Quiz Quiz { get; set; }
        public Dictionary<Question, List<Answer>> QuestionAnswers { get; set; }
        public PlayVM() { }

    }
}
