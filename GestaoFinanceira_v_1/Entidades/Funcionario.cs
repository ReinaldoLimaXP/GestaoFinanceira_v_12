using GestaoFinanceira_v_1.Utilitarios;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("funcionarios")]
    public class Funcionario
    {
        [Key]
        [Required]
        public int Id_funcionario { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [ValidationCPFAttribute(ErrorMessage = "CPF inválido")]
        [MaxLength(30)]
        public string? Cpf { get; set; }

        [Column("nomeCompleto_fun")]
        [Required]
        [MaxLength(255)]
        public string? Nome_completo { get; set; }

        [MaxLength(30)]
        public string? Rg { get; set; }

        [MaxLength(30)]
        public string? OrgaoEmissor { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Sexo { get; set; }

        public DateOnly? DataNascimento { get; set; }

        [MaxLength(50)]
        public string? EstadoCivil { get; set; }

        [MaxLength(255)]
        public string? Foto { get; set; }

        
        public DateOnly? DataContratacao { get; set;}

        
        public DateOnly? DataDispensa { get; set; }

        public double? Renumeracao { get; set; }

        [MaxLength(50)]
        public string? Situacao { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Celular { get; set; }

        [MaxLength(50)]
        public string? Tel_casa { get; set; }

        [MaxLength(255)]
        public string? Observacoes { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? Email_profissional { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? Email_pessoal { get; set; }

        [ValidateComplexType]
        [ForeignKey("fk_id_endereco")]
        public virtual Endereco? Endereco_fun { get; set; }

        public int? Fk_id_funcao { get; set; }
        public virtual Funcao? Funcao_fun { get; set; }

        public int? Fk_id_empresa { get; set; }
        public virtual Empresa? Empresa_fun { get; set; }

		public virtual ICollection<Venda> VendasComoVistoriador { get; set; } = new List<Venda>();

		public virtual ICollection<Venda> VendasComoVendedor { get; set; } = new List<Venda>();

        public virtual ICollection<Caixa> CaixaDoFuncionario { get; set; } = new List<Caixa>();

		public virtual ICollection<LogCaixa> LogsFuncCaixas { get; set; } = new List<LogCaixa>();

		public Funcionario()
        {
            Endereco_fun = new Endereco();
            //Funcao_fun = new Funcao();
            //Empresa_fun = new Empresa();
        }
    }

    [Table("funcoes")]
    public class Funcao
    {
        [Key]
        public int Id_funcao { get; set; }

        [MaxLength(255)]
        public string? Nome_funcao { get; set; }

        public virtual ICollection<Funcionario> FuncoesdosFuncionarios { get; set; } = new List<Funcionario>();
    }
}
