using System;
using System.Linq;

namespace KTraining.Service
{
    public interface ITestService
    {
        double CalculateMark(double maxPoints, double yourPoints, double rate);
    }

    public class TestService : BaseService, ITestService
    {
        /// <summary>
        /// Calculate mark
        /// </summary>
        /// <param name="maxPoints"></param>
        /// <param name="yourPoints"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public double CalculateMark(double maxPoints, double yourPoints, double rate)
        {
            if (yourPoints == 0)
            {
                return 2;
            }
            double mark = 2;
            mark = 3 + 3 * (yourPoints - rate * maxPoints / 100) / (maxPoints - rate * maxPoints / 100);
            mark = (double)Math.Round((decimal)mark, 2);
            if (mark < 2)
            {
                mark = 2;
            }
            return mark;
        }
    }
}
