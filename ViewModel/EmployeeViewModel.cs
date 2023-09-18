using System.ComponentModel.DataAnnotations;

namespace Database_RelationShips.ViewModel
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeID { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }

        public required string Address { get; set; }
    }
}
