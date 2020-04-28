using Quiztopia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiztopia.API.DTOs
{
    public class QuizMapper
    {
        // Convertion
        public static QuizDTO ConvertTo_DTO(Quiz quiz, ref QuizDTO quizDTO)
        {
            // Check if null

            quizDTO.Name = (quiz.Name is null) ? "" : quiz.Name;
            quizDTO.Topic = (quiz.Topic is null) ? "" : quiz.Topic.Name;
            quizDTO.Difficulty = (quiz.Difficulty is null) ? "" : quiz.Difficulty.Name;
            quizDTO.Descriptions = (quiz.Description is null) ? "" : quiz.Description;

            return quizDTO;
        }

    }
}
