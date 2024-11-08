using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("Recebimentos")]
    public class Recebimento
    {
        [Key]
        public int Id_recebimento { get; set; }

        public string StatusRecebimento { get; set; }

        public double ValorParcela { get; set; }

        public int Nparcela { get; set; }

        /*Tipo: dinheiro, pix, cartão*/
        public string? TipoPagamento { get; set; }

        public DateOnly DataVencimento { get; set; }

        public DateOnly DataPagamento { get; set; }

        public TimeSpan? HoraPagamento { get; set; }

        public string? AliquotaParcela { get; set; }

        public string? Observacao { get; set; }

        public string? Descricao { get; set; }

        public string? HoraRegistro { get; set; }

        public DateOnly DataRegistro { get; set; }

		//[ForeignKey("fk_id_caixa")]
		public long? Fk_id_caixa { get; set; }

		[Required]
        public virtual Caixa? CaixaRecebimento { get; set; }

		public int? Fk_id_dispositivo_rec { get; set; }
		//[ForeignKey("fk_id_dispositivo_rec")]
		public virtual Dispositivo? DispositivoRec { get; set; }

		public int? Fk_id_empresa { get; set; }
		public virtual Empresa? EmpresaRecebimento { get; set; }


		public int? Fk_id_venda { get; set; }
        public virtual Venda? Venda { get; set; }

		public int? Fk_id_plano { get; set; }
        public virtual PlanoDeContas? Plano { get; set; }


        public Recebimento()
        {
            DataRegistro = DateOnly.FromDateTime(DateTime.Now);
            HoraRegistro = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }
    }
}
