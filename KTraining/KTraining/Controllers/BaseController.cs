using KTraining.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Controllers
{
    public class BaseController:Controller
    {
        public IUserService userService;
        public CloudinaryService cloudinaryService;
        public ICourseService courseService;
        public IStudentService studentService;
        public IPostService postService;
        public IRequestToJoinService requestToJoinService;
        public ICloudFileService cloudFilesService;
        public ICourseImageService courseImageService;
        public IVideoService videoService;
        public ITopicService topicService;
        public ICloseQuestionService closeQuestionService;
        public IImageService imageService;
        public ICloseAnswerService closeAnswerService;
        public IOpenQuestionService openQuestionService;
        public IAutomaticTestService automaticTestService;
        public IManualTestService manualTestService;
        public ITestService testService;
        public ISolvedAutomaticTestService solvedAutomaticTestService;
        public ISolvedCloseQuestionService solvedCloseQuestionService;
        public IAutomaticTestForSolvingService autoTestForSolvingService;
        public IMarkService markService;
        public IManualTestForSolving manualTestForSolvingService;
        public ISolvedManualTestService solvedManualTestService;
        public ISolvedOpenQuestionService solvedOpenQuestionService;
        public AlphabetFuncions alphabetFunctions;
        public IStudentCompletedCourse studentCompletedCoursesService;
        public ImportQuestionService importQuestionService;
        public ExportTestService exportTestService;
        public INotificationService notificationService;
        public ICourseLevelService courseLevelService;
        public ILevelTestForSolvingService levelTestForSolvingService;
        public ISolvedManualTestForLevelService solvedManualTestForLevelService;
        public ConvertResource convertResource;

        public BaseController()
        {
            this.userService = new UserService();
            this.cloudinaryService = new CloudinaryService("onlinesystemtesting", "198959495156847", "xaESFiOp5pYOqH4EbQOs_dhnaiY");
            this.courseService = new CourseService();
            this.studentService = new StudentService();
            this.postService = new PostService();
            this.requestToJoinService = new RequestToJoinService();
            this.cloudFilesService = new CloudFileService();
            this.courseImageService = new CourseImageService();
            this.videoService = new VideoService();
            this.topicService = new TopicService();
            this.closeQuestionService = new CloseQuestionService();
            this.imageService = new ImageService();
            this.closeAnswerService = new CloseAnswerService();
            this.openQuestionService = new OpenQuestionService();
            this.automaticTestService = new AutomaticTestService();
            this.manualTestService = new ManualTestService();
            this.testService = new TestService();
            this.solvedAutomaticTestService = new SolvedAutomaticTestService();
            this.solvedCloseQuestionService = new SolvedCloseQuestionService();
            this.autoTestForSolvingService = new AutomaticTestForSolvingService();
            this.markService = new MarkService();
            this.manualTestForSolvingService = new ManualTestForSolvingService();
            this.solvedManualTestService = new SolvedManualTestService();
            this.solvedOpenQuestionService = new SolvedOpenQuestionService();
            this.alphabetFunctions = new AlphabetFuncions();
            this.studentCompletedCoursesService = new StudentCompletedCourseService();
            this.importQuestionService = new ImportQuestionService();
            this.exportTestService = new ExportTestService();
            this.notificationService = new NotificationService();
            this.courseLevelService = new CourseLevelService();
            this.levelTestForSolvingService = new LevelTestForSolvingService();
            this.solvedManualTestForLevelService = new SolvedManualTestForLevelService();
            this.convertResource = new ConvertResource();
        }

        private readonly HashSet<string> allowedLanguages = new HashSet<string>
        {
            "en",
            "bg"
        };

        private readonly string defaultLanguage = "en";

        //Add language cookie
        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            var cultureName = "";

            if (requestContext.HttpContext.Request.Cookies["_lang"] == null)
            {
                HttpCookie cookie = new HttpCookie("_lang");
                if (cookie.Value == null)
                {
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cookie.Value = "en";
                }
                requestContext.HttpContext.Request.Cookies.Add(cookie);
            }
            cultureName = requestContext.HttpContext.Request.Cookies["_lang"].Value.ToString();

            if (!allowedLanguages.Contains(cultureName))
            {
                cultureName = defaultLanguage;
            }

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
            return base.BeginExecute(requestContext, callback, state);
        }

        //Remove language cookie
        protected override void EndExecute(IAsyncResult asyncResult)
        {
            Response.Cookies.Add(Request.Cookies["_lang"]);
            base.EndExecute(asyncResult);
        }
    }
}