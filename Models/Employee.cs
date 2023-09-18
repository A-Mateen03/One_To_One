using System.ComponentModel.DataAnnotations;

namespace Database_RelationShips.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        //Navigation property Returns the Employee Address
        public virtual EmployeeAddress? EmployeeAddress { get; set; }
    }
}
