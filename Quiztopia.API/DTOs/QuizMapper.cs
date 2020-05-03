using Quiztopia.Models;
using Quiztopia.Models.Repositories;
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
            quizDTO.Description = (quiz.Description is null) ? "" : quiz.Description;

            return quizDTO;
        }

        public async static Task<Quiz> ConvertTo_Entity(QuizDTO dto, ITopicRepo topicRepo, IDifficultyRepo difficultyRepo, Quiz quiz)
        {
            quiz.Name = (dto.Name is null) ? "No Title" : dto.Name;

            //2. velden die NULL kunnen zijn (Bij create AND editering)
            quiz.Description = quiz.Description ?? dto.Description;

            //3. gerelateerde objecten (object graphs worden in het repo behandeld)
            //controleer waar nodig op Null zijn:
            quiz.Topic = await topicRepo.GetTopicByNameAsync(dto.Topic);
            quiz.Difficulty = await difficultyRepo.GetDifficultyByNameAsync(dto.Difficulty);

            if (quiz.Topic == null)
            {
                quiz.Topic = await topicRepo.GetTopicByNameAsync("General Knowledge");
            }

            if (quiz.Difficulty == null)
            {
                quiz.Difficulty = await difficultyRepo.GetDifficultyByNameAsync("Easy");
            }

            return quiz;
        }
    }
}
