using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class LoginViewModel
    {
        
        [Display(Name = "Your Nick")]
        [Required]
        public string Username { get; set; }
    }
}