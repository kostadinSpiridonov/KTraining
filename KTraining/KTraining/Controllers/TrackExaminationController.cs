using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Models;

namespace KTraining.Controllers
{
    public class TrackExaminationController : BaseController
    {
        // GET: TrackExamination
        [HttpGet]
        public ActionResult Index()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var automaticTestsForSolvinf = this.autoTestForSolvingService.GetSendFromTeacher(teacher.Id)
                .OrderBy(x => x.Course.Name).OrderBy(x => x.Test.Title);
            var solvedAutoTests = this.solvedAutomaticTestService.GetForTeacher(teacher.Id)
                .Where(x=>x.Show==false)
                .OrderBy(x => x.Course.Name).OrderBy(x => x.Test.Title);
      
            var courses = teacher.Courses.ToList().ConvertAll(x =>
                new CourseTrack
                {
                    Id = x.Id,
                    Name = x.Name,
                   // AutoTests= new List<AutomaticTestTrack>(),
                   // ManTests= new List<ManualTestTrack>()
                });
            foreach (var item in automaticTestsForSolvinf)
            {
                if (!courses.Where(x => x.Id == item.CourseId).First().AutoTests.Any(x => x.Id == item.TestId))
                {
                    courses.Where(x => x.Id == item.CourseId).First().AutoTests.Add(
                    new AutomaticTestTrack
                    {
                        Id=item.TestId.Value,
                        Name=item.Test.Title,
                       // SolvedAutoTests= new List<StudentTrack>(),
                       // AutoTestsForSolving= new List<StudentTrack>()
                    });
                }
                    courses.Where(x => x.Id == item.CourseId).First().AutoTests.Where(x => x.Id==item.TestId).First().AutoTestsForSolving
                        .Add(new StudentTrack
                        {
                            Name = item.Student.ApplicationUser.FirstName +" "+ item.Student.ApplicationUser.LastName,
                            Id = item.StudentId,
                            UserId=item.Student.ApplicationUserId  
                        });
                
            }
            foreach (var item in solvedAutoTests)
            {
                if (!courses.Where(x => x.Id == item.CourseId).First().AutoTests.Any(x => x.Id == item.TestId))
                {
                    courses.Where(x => x.Id == item.CourseId).First().AutoTests.Add(
                    new AutomaticTestTrack
                    {
                        Id = item.TestId,
                        Name = item.Test.Title,
                       // SolvedAutoTests = new List<StudentTrack>(),
                       // AutoTestsForSolving = new List<StudentTrack>()
                    });
                } 
                courses.Where(x => x.Id == item.CourseId).First().AutoTests.Where(x => x.Id == item.TestId).First().SolvedAutoTests
                         .Add(new StudentTrack
                         {
                             Name = item.Student.ApplicationUser.FirstName + " " + item.Student.ApplicationUser.LastName,
                             Id = item.StudentId,
                            UserId=item.Student.ApplicationUserId  
                         });
            }

            var manualTestsForSolving = this.manualTestForSolvingService.GetSendFromTeacher(teacher.Id)
               .OrderBy(x=>x.Course.Name).OrderBy(x=>x.Test.Title);
            var solvedManualTests = this.solvedManualTestService.GetForTeacher(teacher.Id)
                .Where(x => x.IsChecked == false)
                .OrderBy(x => x.Course.Name).OrderBy(x => x.Test.Title);
            foreach (var item in manualTestsForSolving)
            {
                if (!courses.Where(x => x.Id == item.CourseId).First().ManTests.Any(x => x.Id == item.TestId))
                {
                    courses.Where(x => x.Id == item.CourseId).First().ManTests.Add(
                    new ManualTestTrack
                    {
                        Id = item.TestId.Value,
                        Name = item.Test.Title,
                        //SolvedManTests = new List<StudentTrack>(),
                      //  ManTestsForSolving = new List<StudentTrack>()
                    });
                }
                courses.Where(x => x.Id == item.CourseId).First().ManTests.Where(x => x.Id == item.TestId).First().ManTestsForSolving
                    .Add(new StudentTrack
                    {
                        Name = item.Student.ApplicationUser.FirstName + " " + item.Student.ApplicationUser.LastName,
                        Id = item.StudentId,
                        UserId = item.Student.ApplicationUserId  
                    });

            }
            foreach (var item in solvedManualTests)
            {
                if (!courses.Where(x => x.Id == item.CourseId).First().ManTests.Any(x => x.Id == item.TestId))
                {
                    courses.Where(x => x.Id == item.CourseId).First().ManTests.Add(
                    new ManualTestTrack
                    {
                        Id = item.TestId,
                        Name = item.Test.Title,
                       // SolvedManTests = new List<StudentTrack>(),
                       // ManTestsForSolving = new List<StudentTrack>()
                    });
                }
                courses.Where(x => x.Id == item.CourseId).First().ManTests.Where(x => x.Id == item.TestId).First().SolvedManTests
                         .Add(new StudentTrack
                         {
                             Name = item.Student.ApplicationUser.FirstName + " " + item.Student.ApplicationUser.LastName,
                             Id = item.StudentId,
                             UserId = item.Student.ApplicationUserId  
                         });
            }
            var empty=courses.Where(x => x.AutoTests.Count == 0 && x.ManTests.Count == 0).ToList();
            foreach (var item in empty)
            {
                courses.Remove(item);
            }
          /*  for(int i=0;i<courses.Count;i++) 
            {
                for(int j=0;j<courses[i].ManTests.Count;j++)
                {
                    if (courses[i].ManTests[j].ManTestsForSolving.Count == 0)
                    {
                        courses[i].ManTests.Remove(courses[i].ManTests[j]);
                    }
                }
            }*/
            return View(courses.ToList());
        }

        //Remove student from examination
        [HttpPost]
        public ActionResult Remove(RemoveTrackViewModel model)
        {
            if(model.IsManualTest)
            {
                this.manualTestForSolvingService.RemoveByStudentCourseTest(model.StudentId,model.CourseId,model.TestId);
            }
            else
            {
                this.autoTestForSolvingService.RemoveByStudentCourseTest(model.StudentId,model.CourseId,model.TestId);
                //if (this.solvedAutomaticTestService.IsWholeTestCompleteTrack(model.TestId,model.CourseId,1))
                if(model.CountForSeen-1==0)
                {
                    var students = this.courseService.GetById(model.CourseId).Students;
                    List<int> testIds = new List<int>();
                    foreach (var item in students)
                    {
                        foreach (var test in item.SolvedAutomaticTests)
                        {
                            testIds.Add(test.Id);
                        }
                    }
                    this.solvedAutomaticTestService.SetShow(testIds);
                }
            }
            return Redirect("/TrackExamination");
        }
    }

   
}