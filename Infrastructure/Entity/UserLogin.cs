using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entity
{
    public class UserLogin:DomainBase
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
