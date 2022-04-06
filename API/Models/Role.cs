using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //Relation
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
