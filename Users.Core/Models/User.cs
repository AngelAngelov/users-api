using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Core.Models
{
    public class User : UserCreate
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime? Created { get; set; }
    }
}
