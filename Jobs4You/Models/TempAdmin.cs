using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jobs4You.Models
{
    public class TempAdmin
    {


        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address!")]
        [Required(ErrorMessage = "Please enter your email adress")]
        public string admin_mail { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MaxLength(20, ErrorMessage = "Password must be within 20 characters")]
        [MinLength(6, ErrorMessage = "Password must contain minimum 6 characters")]
        public string admin_pass { get; set; }


    }
}