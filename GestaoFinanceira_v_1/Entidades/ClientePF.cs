using GestaoFinanceira_v_1.Utilitarios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("clientesPF")]
    public class ClientePF:Cliente
    {
        [Required]
        [Column("nome_PF")]
        public string? Nome_cli { get; set; }
        
        [Required(ErrorMessage = "CPF é obrigatório")]
        [ValidationCPFAttribute(ErrorMessage = "CPF inválido")]
        [Column("cpf_PF")]
        [MaxLength(50)]
        public string? Cpf_cli { get; set; }

   
        [Column("rg_PF")]
        [MaxLength(30)]
        public string? Rg_cli { get; set; }

  
        [Column("orgaoEmissorRG_PF")]
        [MaxLength(20)]
        public string? OrgaoEmissorRG_cli { get; set; }

        [Column("sexo_PF")]
        [MaxLength(50)]
        public string? Sexo_PF { get; set; }

        [Column("data_nascimento_PF")]
        public DateOnly? Data_nascimento { get; set; }

        public ClientePF Clone()
        {
            return (ClientePF)this.MemberwiseClone();
        }
    }
}
