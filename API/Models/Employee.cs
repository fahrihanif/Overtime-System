using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public int Salary { get; set; }
        [Required]
        public int JobId { get; set; }

        //Relation
        public virtual Job Job { get; set; }
        public virtual Account Account { get; set; }
        [JsonIgnore]
        public virtual ICollection<EmployeeOvertime> EmployeeOvertimes { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
