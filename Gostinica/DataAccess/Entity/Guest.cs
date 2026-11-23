using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gostinica.DataAccess.Entity
{
    public class Guest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? PassportData {  get; set; }
        [Required]
        public string? Phone {  get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
