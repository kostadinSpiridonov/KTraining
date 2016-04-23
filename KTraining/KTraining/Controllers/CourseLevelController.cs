using KTraining.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Service;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    public class CourseLevelController : BaseController
    {
        //GET: Course levels
        [HttpGet]
        public ActionResult Levels(int id)
        {
            var levels = this.courseLevelService.GetForCourse(id);
            var course = this.courseService.GetById(id);
            var viewModel = new CourseLevelsViewModel
            {
                Levels = levels.ToList().ConvertAll(x =>
                new CourseLevelViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CourseId = x.CourseId
                }),
                CourseId = id,
                CourseName = course.Name
            };
            if(this.User.Identity.GetUserId()==course.Teacher.ApplicationUserId)
            {
                return View("LevelsTeacher",viewModel);
            }
            else if(this.User.IsInRole("Student"))
            {
                if(course.Students.Where(x=>x.ApplicationUserId==this.User.Identity.GetUserId()).Count()==0)
                {
                    return View("Levels", viewModel);
                }
                return View("LevelsStudent", viewModel);
            }
            else
            {
                return View("Levels", viewModel);
            }
        }

        //GET: Add course level
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Add(int id)
        {
            if (BellongToCurrentUSeR(id, this.User.Identity.GetUserId()))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var autoTest = this.automaticTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ShowAutomaticTestBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                var manTest = this.manualTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ManualTestShowBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                return View(
                            new CourseLevelAddFullViewModel
                            {
                                AddCourseLevelViewModel = new AddCourseLevelViewModel
                                                          {
                                                              CourseId = id
                                                          },
                                AutomaticTests = autoTest,
                                ManualTests = manTest
                            });
            }
            return Redirect("/");
        }

        //POST: Add course level
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult Add(CourseLevelAddFullViewModel model)
        {
            if ((model.AddCourseLevelViewModel.Description != null && OtherFunctions.IsHasJS(model.AddCourseLevelViewModel.Description))||
                !ModelState.IsValid ||
              (model.AddCourseLevelViewModel.AutomaticTestId == 0 &&model.AddCourseLevelViewModel.ManualTestId == 0) )
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var autoTest = this.automaticTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ShowAutomaticTestBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                var manTest = this.manualTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ManualTestShowBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                model.AutomaticTests = autoTest;
                model.ManualTests = manTest;
            }
            if (model.AddCourseLevelViewModel.Description != null && OtherFunctions.IsHasJS(model.AddCourseLevelViewModel.Description))
            {
                ModelState.AddModelError("",Common.FieldDescriptionDanger);
                return View(model);
            }
            if (!ModelState.IsValid || (model.AddCourseLevelViewModel.AutomaticTestId == 0 &&
                model.AddCourseLevelViewModel.ManualTestId == 0))
            {
                ModelState.AddModelError("", Common.NoChooseTest);
                return View(model);
            }
            var addModel = new CourseLevel
            {
                CourseId = model.AddCourseLevelViewModel.CourseId,
                Name = model.AddCourseLevelViewModel.Name,
                Description=model.AddCourseLevelViewModel.Description
                
            };
            if (model.AddCourseLevelViewModel.AutomaticTestId != 0)
            {
                addModel.AutomaticTestId = model.AddCourseLevelViewModel.AutomaticTestId;
            }
            else if (model.AddCourseLevelViewModel.ManualTestId != 0)
            {
                addModel.ManualTestId = model.AddCourseLevelViewModel.ManualTestId;
            }
            this.courseLevelService.Add(addModel);
            return Redirect("/CourseLevel/Levels/" + model.AddCourseLevelViewModel.CourseId);
        }

        //GET: Level details
        [HttpGet]
        public ActionResult Details(int id)
        {
            var level = this.courseLevelService.GetById(id);
            var viewModel = new CourseLevelDetailsViewModel
            {
                Id = level.Id,
                IsManualTest = level.ManualTestId.HasValue ? true : false,
                Name = level.Name,
                Description=level.Description
            };
            if (viewModel.IsManualTest)
            {
                viewModel.TestId = level.ManualTestId.Value;
                viewModel.TestTitle = level.ManualTest.Title;
            }
            else
            {
                viewModel.TestId = level.AutomaticTestId.Value;
                viewModel.TestTitle = level.AutomaticTest.Title;
            }
            if (level.Course.Teacher.ApplicationUserId == this.User.Identity.GetUserId())
            {
                return View("DetailsTeacher",viewModel);
            }
            return View("DetailsStudent", viewModel);
        }

        //GET: Edit level
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (BellongToCurrentUSeRLevel(id, this.User.Identity.GetUserId()))
            {
                var level = this.courseLevelService.GetById(id);
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var autoTest = this.automaticTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ShowAutomaticTestBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                var manTest = this.manualTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ManualTestShowBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                var viewModel = new CourseLevelAddFullViewModel
                            {
                                AddCourseLevelViewModel = new AddCourseLevelViewModel
                                {
                                    CourseId = level.CourseId,
                                    Name = level.Name,
                                    Id = level.Id,
                                    Description=level.Description
                                },
                                AutomaticTests = autoTest,
                                ManualTests = manTest
                            };
                if (level.AutomaticTestId != null)
                {
                    viewModel.AddCourseLevelViewModel.AutomaticTestId = level.AutomaticTestId.Value;
                }
                if (level.ManualTestId != null)
                {
                    viewModel.AddCourseLevelViewModel.ManualTestId = level.ManualTestId.Value;
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Edit level
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult Edit(CourseLevelAddFullViewModel model)
        {
            if ((model.AddCourseLevelViewModel.Description != null && OtherFunctions.IsHasJS(model.AddCourseLevelViewModel.Description)) ||
               !ModelState.IsValid ||
             (model.AddCourseLevelViewModel.AutomaticTestId == 0 && model.AddCourseLevelViewModel.ManualTestId == 0))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var autoTest = this.automaticTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ShowAutomaticTestBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                var manTest = this.manualTestService.GetForTeacher(teacher.Id).ToList().ConvertAll(x =>
                    new ManualTestShowBaseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title
                    });
                model.AutomaticTests = autoTest;
                model.ManualTests = manTest;
            }
            if (model.AddCourseLevelViewModel.Description != null && OtherFunctions.IsHasJS(model.AddCourseLevelViewModel.Description))
            {
                ModelState.AddModelError("", Common.FieldDescriptionDanger);
                return View(model);
            }
            if (!ModelState.IsValid || (model.AddCourseLevelViewModel.AutomaticTestId == 0 &&
                model.AddCourseLevelViewModel.ManualTestId == 0))
            {
                ModelState.AddModelError("", Common.NoChooseTest);
                return View(model);
            }
            var addModel = new CourseLevel
            {
                CourseId = model.AddCourseLevelViewModel.CourseId,
                Name = model.AddCourseLevelViewModel.Name,
                Id = model.AddCourseLevelViewModel.Id,
                Description=model.AddCourseLevelViewModel.Description
            };
            if (model.AddCourseLevelViewModel.AutomaticTestId != 0)
            {
                addModel.AutomaticTestId = model.AddCourseLevelViewModel.AutomaticTestId;
            }
            else if (model.AddCourseLevelViewModel.ManualTestId != 0)
            {
                addModel.ManualTestId = model.AddCourseLevelViewModel.ManualTestId;
            }
            this.courseLevelService.Update(addModel);
            return Redirect("/CourseLevel/Details/" + model.AddCourseLevelViewModel.Id);
        }

        //GET: Get stundet levels in courses
        [Authorize(Roles = "Student,Teacher")]
        [HttpGet]
        public ActionResult StudentCourseLevels()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var levels = student.CourseLevels.OrderBy(x => x.Course.Name);
            var viewModel = new List<CourseLevelsViewModel>();
            foreach (var item in levels)
            {
                if (viewModel.Where(x => x.CourseId == item.CourseId).Count() > 0)
                {
                    viewModel.Where(x => x.CourseId == item.CourseId).First().Levels.Add(
                        new CourseLevelViewModel
                        {
                            CourseId=item.CourseId,
                            Id=item.Id,
                            Name=item.Name
                        });
                }
                else
                {
                    viewModel.Add(new CourseLevelsViewModel
                    {
                        CourseId = item.CourseId,
                        CourseName = item.Course.Name,
                        Levels = new List<CourseLevelViewModel> 
                        {
                            new CourseLevelViewModel 
                            {
                               CourseId=item.CourseId,
                               Id=item.Id,
                               Name=item.Name
                            }
                        }
                    });
                }
            }
            return View(viewModel);
        }

        //Check if the course  belongs to current user
        private bool BellongToCurrentUSeR(int id,string userId)
        {
            return this.courseService.GetById(id).Teacher.ApplicationUserId == userId;
        }
        
        //Check if the level  belongs to current user
        private bool BellongToCurrentUSeRLevel(int id, string userId)
        {
            return this.courseLevelService.GetById(id).Course.Teacher.ApplicationUserId == userId;
        }
    }
}