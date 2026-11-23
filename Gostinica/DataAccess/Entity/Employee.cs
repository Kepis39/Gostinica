using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gostinica.DataAccess.Entity
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Position { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
