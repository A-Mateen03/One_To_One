using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database_RelationShips.Models
{
    public class EmployeeAddress
    {
        [Key, ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public string Address { get; set; }
        //
        //Navigation property Returns the Employee object
        public virtual Employee? Employee { get; set; }
    }
}
