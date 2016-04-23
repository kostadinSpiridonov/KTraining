using KTreining.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KTraining.Service
{
    public class ImportQuestionService : BaseService
    {
        /// <summary>
        /// Check whether the string is the question
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsQuestion(string line)
        {
            string pattern = @"^(?!(ANSWER:))^(?![A-Z]+(\.|\)))^[A-ZА-Яa-zа-з]";
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(line);
        }

        /// <summary>
        /// Check whether the string is answer
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsAnswer(string line)
        {
            string pattern = @"^[A-Z]+(\.|\))";
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(line);
        }

        /// <summary>
        /// Check whether the string is the correct anwer
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsCorrect(string line)
        {
            string pattern = @"^(ANSWER:)";
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(line);
        }

        /// <summary>
        /// Get close questions from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public ICollection<CloseQuestion> GetCloseQuestions(Stream stream)
        {
            List<CloseQuestion> questions = new List<CloseQuestion>();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    if (IsQuestion(line))
                    {
                        questions.Add(new CloseQuestion
                        {
                            Content = line,
                            HelpLink = "",
                            Points = 1
                        });
                    }
                    if (IsAnswer(line))
                    {
                        questions.Last().Answers.Add(
                            new CloseAnswer
                            {
                                Content = line,
                            });
                    }
                    if (IsCorrect(line))
                    {
                        var correct = (line.Substring(line.IndexOf(':') + 1, line.Length - line.IndexOf(':') - 1)).Trim();
                        for (int i = 0; i < questions.Last().Answers.Count; i++)
                        {
                            if (questions.Last().Answers.ElementAt(i).Content.Trim().ElementAt(0).ToString() == correct)
                            {
                                questions.Last().Answers.ElementAt(i).Correct = true;
                            }
                            questions.Last().Answers.ElementAt(i).Content = questions.Last().Answers.ElementAt(i).Content
                                .Substring(3, questions.Last().Answers.ElementAt(i).Content.Length - 3).Trim();
                        }
                    }
                }
            }
            return questions;
        }

        /// <summary>
        /// Get open questions from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public ICollection<OpenQuestion> GetOpenQuestions(Stream stream)
        {
            List<OpenQuestion> questions = new List<OpenQuestion>();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {

                    if (IsQuestion(line))
                    {
                        questions.Add(new OpenQuestion
                        {
                            Content = line,
                            HelpLink = "",
                            Points = 1
                        });
                    }
                }
            }
            return questions;
        }
    }
}
