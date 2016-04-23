using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ISolvedCloseQuestionService
    {
        void AddSelectedAnswer(List<int> selectedAnswers, int solvedQuestionId);
        SolvedCloseQuestion GetQuestionByAutoTestAndIndex(int testId, int questionIndex);
        SolvedCloseQuestion GetCloseQuestionByManualTestAndIndex(int testId, int questionIndex);
        SolvedCloseQuestion GetCloseQuestionByManualTestForLevelAndIndex(int testId, int questionIndex);
    }

    public class SolvedCloseQuestionService : BaseService, ISolvedCloseQuestionService
    {
        /// <summary>
        /// Add selected answer to solved question
        /// </summary>
        /// <param name="selectedAnswers"></param>
        /// <param name="solvedQuestionId"></param>
        public void AddSelectedAnswer(List<int> selectedAnswers, int solvedQuestionId)
        {
            var solvedQ = this.context.SolvedCloseQuestions.Find(solvedQuestionId);
            foreach (var item in selectedAnswers)
            {
                var answer = this.context.CloseAnswers.Find(item);
                solvedQ.SelectedAnswers.Add(answer);
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get solved question by automatic test id and question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public SolvedCloseQuestion GetQuestionByAutoTestAndIndex(int testId, int questionIndex)
        {
            return this.context.SolvedAutomaticTests.Find(testId).SolvedQuestions.ElementAt(questionIndex);
        }

        /// <summary>
        /// Get close question by manual test id and question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public SolvedCloseQuestion GetCloseQuestionByManualTestAndIndex(int testId, int questionIndex)
        {
            return this.context.SolvedManualTests.Find(testId).SolvedCloseQuestions.ElementAt(questionIndex);
        }

        /// <summary>
        /// Get close question by manual test for level index and question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public SolvedCloseQuestion GetCloseQuestionByManualTestForLevelAndIndex(int testId, int questionIndex)
        {
            return this.context.SolvedManualTestsForLevel.Find(testId).SolvedCloseQuestions.ElementAt(questionIndex);
        }
    }
}
