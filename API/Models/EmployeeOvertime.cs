﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_r_employees_overtimes")]
    public class EmployeeOvertime
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime BreakOvertime { get; set; }
        [Required]
        public DateTime StartOvertime { get; set; }
        [Required]
        public DateTime EndOvertime { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public int OvertimeId { get; set; }

        //Relation
        public virtual Overtime Overtime { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
