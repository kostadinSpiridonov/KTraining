using KTreining.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace KTraining.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RequestToJoin> RequestsToJoin { get; set; }
        public DbSet<CloudFile> CloudFiles { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<CloseQuestion> CloseQuestions { get; set; }
        public DbSet<CloseAnswer> CloseAnswers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<OpenQuestion> OpenQuestions { get; set; }
        public DbSet<AutomaticTest> AutomaticTests { get; set; }
        public DbSet<ManualTest> ManualTests { get; set; }
        public DbSet<SolvedAutomaticTest> SolvedAutomaticTests { get; set; }
        public DbSet<SolvedCloseQuestion> SolvedCloseQuestions { get; set; }
        public DbSet<AutomaticTestForSolving> AutomaticTestsForSolving { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<ManualTestForSolving> ManualTestsForSolving { get; set; }
        public DbSet<SolvedManualTest> SolvedManualTests { get; set; }
        public DbSet<SolvedOpenQuestion> SolvedOpenQuestions { get; set; }
        public DbSet<StudentCompletedCourse> StudentCompletedCourses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<LevelTestForSolving> LevelTestsForSolving { get; set; }
        public DbSet<SolvedManualTestForLevel> SolvedManualTestsForLevel { get; set; }

        public static ApplicationDbContext Create()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Student"))
                roleManager.Create(new IdentityRole("Student"));

            if (!roleManager.RoleExists("Teacher"))
                roleManager.Create(new IdentityRole("Teacher"));
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Student>().HasMany(x => x.Courses)
                .WithMany(y => y.Students);

            modelBuilder.Entity<AutomaticTest>().HasMany(x => x.CloseQuestions)
                .WithMany(y => y.AutomaticTests);

            modelBuilder.Entity<Teacher>().HasMany(m => m.Courses);

            modelBuilder.Entity<ManualTest>().HasMany(x => x.CloseQuestions)
                .WithMany(y => y.ManualTests);

            modelBuilder.Entity<ManualTest>().HasMany(x => x.OpenQuestions)
                .WithMany(y => y.ManualTests);

            modelBuilder.Entity<SolvedCloseQuestion>().HasMany(x => x.SelectedAnswers)
                .WithMany();

            modelBuilder.Entity<Student>()
                .HasMany(x => x.CourseLevels).WithMany();

            base.OnModelCreating(modelBuilder);
        }
    }
}
