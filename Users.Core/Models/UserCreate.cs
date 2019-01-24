using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Users.Core.Models
{
    public class UserCreate
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string GivvenName { get; set; }

        [Required]
        public string FamilyName { get; set; }
    }
}
