using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ONE.Models
{
    public class ApplicationUser:IdentityUser
    {
        public byte[]? PictureSource { get; set; }

    }
}
