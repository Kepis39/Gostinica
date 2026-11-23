using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gostinica.DataAccess.Entity
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }
        [Required]
        public string? Number { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public decimal PricePerDay { get; set;  }
        [Required]
        [Range(minimum:1, maximum:10)]
        public int Capacity { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
