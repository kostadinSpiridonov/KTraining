using KTreining.Model;
using System;
using System.Linq;

namespace KTraining.Service
{
    public interface ISolvedOpenQuestionService
    {
        void AddAnswer(int solvedQuestionId, string content);
        SolvedOpenQuestion GetOpenQuestionByManualTestAndIndex(int testId, int questionIndex);
        SolvedOpenQuestion GetOpenQuestionByManualTestForLevelAndIndex(int testId, int questionIndex);
        void SetPoints(int questionId, double points);
    }

    public class SolvedOpenQuestionService : BaseService, ISolvedOpenQuestionService
    {
        /// <summary>
        /// Add answer to solved open question
        /// </summary>
        /// <param name="solvedQuestionId"></param>
        /// <param name="content"></param>
        public void AddAnswer(int solvedQuestionId, string content)
        {
            this.context.SolvedOpenQuestions.Find(solvedQuestionId).Answer = content;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get open qiestion by manual test id an question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public SolvedOpenQuestion GetOpenQuestionByManualTestAndIndex(int testId, int questionIndex)
        {
            return this.context.SolvedManualTests.Find(testId).SolvedOpenQuestions.ElementAt(questionIndex);
        }

        /// <summary>
        /// Set pointe to solved open question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="points"></param>
        public void SetPoints(int questionId, double points)
        {
            this.context.SolvedOpenQuestions.Find(questionId).Points = points;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get open qiestion by manual test for level id an question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public SolvedOpenQuestion GetOpenQuestionByManualTestForLevelAndIndex(int testId, int questionIndex)
        {
            return this.context.SolvedManualTestsForLevel.Find(testId).SolvedOpenQuestions.ElementAt(questionIndex);
        }
    }
}
