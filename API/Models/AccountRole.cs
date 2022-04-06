using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_accounts_roles")]
    public class AccountRole
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Account")]
        public string AccountNIK { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        //Relation
        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
