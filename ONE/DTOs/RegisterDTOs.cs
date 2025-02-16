using System.ComponentModel.DataAnnotations;

namespace ONE.DTOs
{
    public class RegisterDTOs
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public IFormFile PictureUser { get; set; }
        
    }
}
