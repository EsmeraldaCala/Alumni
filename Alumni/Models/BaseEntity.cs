using System.ComponentModel.DataAnnotations;

namespace Alumni.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
