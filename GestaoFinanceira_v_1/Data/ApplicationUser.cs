using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestaoFinanceira_v_1.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Nome_Usuario { get; set; }

        [ForeignKey("FK_id_funcionario")]
        [Column("FK_id_funcionario")]
        public int? Id_funcionario { get; set; }
    }

}
