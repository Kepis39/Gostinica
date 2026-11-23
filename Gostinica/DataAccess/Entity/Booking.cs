using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gostinica.DataAccess.Entity
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public ICollection<HotelService> UsedServices { get; set; } = new List<HotelService>();
    }
}
