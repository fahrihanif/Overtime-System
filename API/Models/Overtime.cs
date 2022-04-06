using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_overtimes")]
    public class Overtime
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime SubmitDate { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public int Paid { get; set; }
        [Required]
        public Types Type { get; set; }

        //Relation
        public virtual ICollection<EmployeeOvertime> EmployeeOvertimes { get; set; }
    }

    public enum Status
    {
        Pending,
        [Display(Name = "Approval Manager")] ApprovalManager,
        Approved,
        Rejected
    }

    public enum Types
    {
        Weekday,
        Weekend
    }
}
