using Microsoft.AspNetCore.Mvc.Rendering;
using Quiztopia.Models;
using Quiztopia.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.Web.ViewModels
{
    public class CreateUpdateQuizVM
    {
        // Properties

        public Quiz Quiz { get; set; }
        public SelectList TopicList { get; set; }
        public SelectList DifficultyList { get; set; }

        // Constructor

        public CreateUpdateQuizVM()
        {
        }

        public CreateUpdateQuizVM(Quiz quiz, ITopicRepo topicRepo, IDifficultyRepo difficultyRepo)
        {
            this.Quiz = quiz;
            this.TopicList = new SelectList(topicRepo.GetAllTopicsAsync().Result, "Id", "Name", quiz.TopicId);
            this.DifficultyList = new SelectList(difficultyRepo.GetAllDifficultiesAsync().Result, "Id", "Name", quiz.DifficultyId);
        }

        
    }
}
