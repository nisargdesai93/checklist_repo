using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entity
{
    public class Person : DomainBase
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
