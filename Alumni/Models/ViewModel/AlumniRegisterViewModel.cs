using System.ComponentModel.DataAnnotations;

namespace Alumni.Models.ViewModel
{
    public class AlumniRegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string FieldOfStudy { get; set; }
        public string JobPosition { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Company { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }

    public class FacultyRepRegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public string Faculty { get; set; }
        public string FieldOfStudy { get; set; }
        public IFormFile ProfilePicture { get; set; }

    }

}
