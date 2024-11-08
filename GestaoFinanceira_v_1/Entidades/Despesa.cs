
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("despesas")]
    public class Despesa
    {
        [Key]
        public int Id_despesa { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public DateOnly? Vencimento { get; set; }

        [Required]
        public DateOnly? Datalacamento { get; set; }

        public TimeSpan? HoraLacamento { get; set; }

        public DateOnly? DataPagamento { get; set; }

        public TimeSpan? HoraPagamento { get; set; }

        public string? Status_des { get; set; }

        public string? Observacao { get; set; }

        public string? TipoPagamento { get; set; }

        //[ForeignKey("fk_id_fornecedor")]
        //public int ? Fk_id_fornecedor { get; set; }
        [Required]
        public int? Fk_id_fornecedor { get; set; }
        public virtual Fornecedor? FornecedorDespesa { get; set; }


        public long? Fk_id_caixa { get; set; }
        //[ForeignKey("fk_id_caixa")]
        public virtual Caixa? CaixaPagamento { get; set; }

        public int? Fk_id_plano { get; set; }
        public virtual PlanoDeContas? PlanoPag { get; set; }

        public int? Fk_id_empresa { get; set; }
        public virtual Empresa? EmpresaDespesa { get; set; }

        public Despesa()
        {

            Datalacamento = DateOnly.FromDateTime(DateTime.Now);
            Status_des = "Aberto";
        }
    }


    public class PagamentoException : Exception
    {
        public PagamentoException() { }

        public PagamentoException(string message)
            : base(message) { }

        public PagamentoException(string message, Exception inner)
            : base(message, inner) { }
    }
}
