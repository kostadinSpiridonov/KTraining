using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IUserService
    {
        void AddAppUser(string userId, string role);
        Teacher GetTeacherByName(string name);
        Teacher GetTeacherByAppUserId(string id);
        ICollection<Teacher> GetAllTeachers();
        ApplicationUser GetAppUser(string id);
        void UpdateAppUser(ApplicationUser model);
        ICollection<ApplicationUser> GetAllAppUsers();

    }

    public class UserService : BaseService, IUserService
    {
        /// <summary>
        /// Add application user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        public void AddAppUser(string userId, string role)
        {
            if (role == "Student")
            {
                this.context.Students.Add(new Student
                {
                    ApplicationUserId = userId
                });
            }
            else if (role == "Teacher")
            {
                this.context.Teachers.Add(new Teacher
                {
                    ApplicationUserId = userId
                });
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get teacher by username
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Teacher GetTeacherByName(string name)
        {
            return this.context.Teachers.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.UserName == name).First();
        }

        /// <summary>
        /// Gget teacher by application user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Teacher GetTeacherByAppUserId(string id)
        {
            return this.context.Teachers.Where(x => x.ApplicationUserId == id).First();
        }

        /// <summary>
        /// Get all teachers
        /// </summary>
        /// <returns></returns>
        public ICollection<Teacher> GetAllTeachers()
        {
            return this.context.Teachers.ToList();
        }

        /// <summary>
        /// Get application user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationUser GetAppUser(string id)
        {
            return this.context.Users.Find(id);
        }

        /// <summary>
        /// Update application user details
        /// </summary>
        /// <param name="model"></param>
        public void UpdateAppUser(ApplicationUser model)
        {
            var user = this.context.Users.Find(model.Id);
            user.UserName = model.Email;
            user.AboutMe = model.AboutMe;
            user.City = model.City;
            user.Country = model.Country;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.SecondName = model.SecondName;
            user.Skype = model.Skype;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get all application users
        /// </summary>
        /// <returns></returns>
        public ICollection<ApplicationUser> GetAllAppUsers()
        {
            return this.context.Users.ToList();
        }
    }
}
