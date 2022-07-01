using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BMS.ViewModel
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [Key]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}