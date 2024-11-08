using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("PlanoDeContas")]
    public class PlanoDeContas
    {
        [Key]
        public int Id_planoConta { get; set; }

        [Required]
        public string? Codigo { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Required]
        public string? Abreviatura { get; set; }

        [Required]
        public string? NaturezaConta { get; set; }

        [Required]
        public string? Observacoes { get; set; }

		public virtual ICollection<Despesa> DespesasDoPlano { get; set; } = new List<Despesa>();

		public virtual ICollection<Recebimento>? RecebimentosDoplano { get; set; } = new List<Recebimento>();
	}
}
