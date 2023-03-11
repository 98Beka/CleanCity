using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CleanCity.Models
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
