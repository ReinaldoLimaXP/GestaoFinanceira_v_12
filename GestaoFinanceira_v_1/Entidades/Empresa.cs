using GestaoFinanceira_v_1.Utilitarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("Empresas")]
    public class Empresa

    {
        [Key]
        public int Id_empresa { get; set; }

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [ValidarCNPJ(ErrorMessage = "CNPJ inválido")]
        [MaxLength(150)]
        public string? Cnpj { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Razao_social { get; set; }

        [MaxLength(255)]
        public string? Nome_fantasia { get; set; }

        [MaxLength(130)]
        public string? InscricaoEstadual { get; set; }

        [Required]
        [MaxLength(130)]
        public string? AtividadeEconomica { get; set; }

        [MaxLength(50)]
        public string? Situacao_cadastral { get; set; }

        [MaxLength(255)]
        public string? Responsavel { get; set; }

        [MaxLength(255)]
        public string? Foto { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Telefone1 { get; set; }

        [MaxLength(50)]
        public string? Telefone2 { get; set; }

        [MaxLength(50)]
        public string? Telefone_fax { get; set; }

        [Required]
        [MaxLength(120)]
        public string? Email { get; set; }

        [MaxLength(120)]
        public string? Email2 { get; set; }

        [ValidateComplexType]
        [ForeignKey("fk_id_endereco")]
        public virtual Endereco? Endereco { get; set; }

		public virtual ICollection<Venda>? VendadaEmpresa { get; set; } = new List<Venda>();

        public virtual ICollection<Funcionario>? FuncionariosdaEmpresa { get; set; } = new List<Funcionario>();

        public virtual ICollection<Despesa>? DespesadaEmpresa { get; set; } = new List<Despesa>();

        public virtual ICollection<Recebimento>? RecebimentosdaEmpresa { get; set; } = new List<Recebimento>();

        public virtual ICollection<Caixa>? CaixasdaEmpresa { get; set; } = new List<Caixa>();

        public Empresa()
        {
            Endereco = new Endereco();
        }
    }
}
