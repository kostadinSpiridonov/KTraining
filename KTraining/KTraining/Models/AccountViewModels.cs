using KTraining.Resources.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KTraining.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(
          Name = "Email",
          ResourceType = typeof(Common)
          )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "NotCorrectEmail",
            ErrorMessage = null
            )]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(
            ErrorMessageResourceName = "FieldEmailRequired",
            ErrorMessageResourceType = typeof(Common)
        )]
        [Display(
            Name = "Email",
            ResourceType = typeof(Common)
            )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "NotCorrectEmail",
            ErrorMessage = null
            )]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldPassRequired",
            ErrorMessageResourceType = typeof(Common)
            )]
        [DataType(DataType.Password)]
        [Display(
            Name = "Pass",
            ResourceType = typeof(Common)
            )]
        public string Password { get; set; }

        [Display(
            Name = "RememberMe",
            ResourceType = typeof(Common)
            )]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(
         ErrorMessageResourceName = "FieldEmailRequired",
         ErrorMessageResourceType = typeof(Common)
         )]
        [Display(
            Name = "Email",
            ResourceType = typeof(Common)
            )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "NotCorrectEmail",
            ErrorMessage = null
            )]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldFirstNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name = "FirstName",
            ResourceType=typeof(Common)
            )]
        public string FirstName { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldSecondNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name = "SecondName",
            ResourceType=typeof(Common)
            )]
        public string SecondName { get; set; }

        [Required(
            ErrorMessageResourceName ="FieldLastNameRequired",
            ErrorMessageResourceType = typeof(Common)
            )]
        [Display(
            Name = "LastName",
            ResourceType = typeof(Common)
            )]
        public string LastName { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldPassRequired",
            ErrorMessageResourceType = typeof(Common)
            )]
        [StringLength(100, 
            ErrorMessageResourceName = "ShoudBeAtLeast",
            ErrorMessageResourceType = typeof(Common),
            MinimumLength = 6
            )]
        [DataType(DataType.Password)]
        [Display(
            Name = "Pass",
            ResourceType = typeof(Common)
            )]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(
            Name = "ConfirmPass",
            ResourceType = typeof(Common)
            )]
        [System.ComponentModel.DataAnnotations.Compare(
            "Password",
            ErrorMessageResourceName = "PassSDontMach",
            ErrorMessageResourceType = typeof(Common)
            )]
        public string ConfirmPassword { get; set; }

        public SelectList Roles { get; set; }


          [Display(
            Name = "Role",
            ResourceType = typeof(Common)
            )]
        [Required]
        public string Role { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(
            ErrorMessageResourceName = "FieldEmailRequired",
            ErrorMessageResourceType = typeof(Common)
        )]
        [Display(
            Name = "Email",
            ResourceType = typeof(Common)
            )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "NotCorrectEmail",
            ErrorMessage = null
            )]
        public string Email { get; set; }

        [Required(
             ErrorMessageResourceName = "FieldPassRequired",
             ErrorMessageResourceType = typeof(Common)
             )]
        [StringLength(100,
            ErrorMessageResourceName = "ShoudBeAtLeast",
            ErrorMessageResourceType = typeof(Common),
            MinimumLength = 6
            )]
        [DataType(DataType.Password)]
        [Display(
            Name = "Pass",
            ResourceType = typeof(Common)
            )]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(
            Name = "ConfirmPass",
            ResourceType = typeof(Common)
            )]
        [System.ComponentModel.DataAnnotations.Compare(
            "Password",
            ErrorMessageResourceName = "PassSDontMach",
            ErrorMessageResourceType = typeof(Common)
            )]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(
         ErrorMessageResourceName = "FieldEmailRequired",
         ErrorMessageResourceType = typeof(Common)
        )]
        [Display(
            Name = "Email",
            ResourceType = typeof(Common)
            )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "NotCorrectEmail",
            ErrorMessage = null
            )]
        public string Email { get; set; }
    }
}
