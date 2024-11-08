using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Utilitarios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
	[Table("Fornecedores")]
	public class Fornecedor
	{
		[Key]
		public int Id_fornecedor { get; set; }

		[Required(ErrorMessage = "CNPJ é obrigatório")]
		[ValidarCNPJ(ErrorMessage = "CNPJ inválido")]
		[MaxLength(150)]
		public string? Cnpj { get; set; }

		[Required]
		[MaxLength(250)]
		public string? Razao_social { get; set; }

		[MaxLength(250)]
		public string? Nome_fantasia { get; set; }

		[MaxLength(130)]
		public string? InscricaoEstadual { get; set; }

		[Required]
		[MaxLength(130)]
		public string? AtividadeEconomica { get; set; }

   
        [Column(TypeName = "nvarchar(100)")]
        public Situacao_cadastral Situacao_cadastral { get; set; }

		[MaxLength(255)]
		public string? Responsavel { get; set; }

		[Required]
		[MaxLength(50)]
		public string? Telefone1 { get; set; }

		[MaxLength(50)]
		public string? Telefone2 { get; set; }

		[MaxLength(50)]
		public string? Telefone3 { get; set; }

		[Required]
		[MaxLength(120)]
		public string? Email { get; set; }

		[MaxLength(120)]
		public string? Email2 { get; set; }


        [ValidateComplexType]
		[ForeignKey("fk_id_endereco")]
		public virtual Endereco Endereco { get; set; }

        public virtual ICollection<Despesa> DespesasPagasForncedor { get; set; } = new List<Despesa>();

        public Fornecedor()
		{
			Endereco = new Endereco();
		}
	}

    public enum Situacao_cadastral
    {
        Ativa,
        Desativada
    }
}
