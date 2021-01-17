using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisitRegistrationApplication.Models
{
    public class UserModel
    {
        
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Imię jest wymagane.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nazwisko jest wymagane.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email jest wymagane.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email nie jest prawidłowy")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimalna długość hasła to 6 znaków")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Powtórzone hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasło i powtórzone hasło musi być takie same")]
        [Display(Name = "Powtórz hasło")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Role { get; set; }

    }
}