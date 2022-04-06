using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_jobs")]
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int MinSalary { get; set; }
        [Required]
        public int MaxSalary { get; set; }

        //Relation
        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
