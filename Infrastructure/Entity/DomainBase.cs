using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entity
{
    public abstract class DomainBase
    {
        public long Id { get; set; }
    }
}
