
using GestaoFinanceira_v_1.Components.Pages.Vendas;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("Vendas")]
	public class Venda
    {
        [Key]
        [Column("id_venda")]
        public int Id_venda { get; set; }

        public DateOnly DataVenda { get; set; }

        public string? Hora { get; set; }

        public double ValorTotal { get; set; }

        public double Desconto { get; set; }

		public double ValorFinal { get; set; }

        public string? Observacoes { get; set; }

        public string? Emisao { get; set; }

        public string? TotalParcelas { get; set; }

		[Required]
		public string? FormaPagamento { get; set; }

        public string? TipoPagamento { get; set; }

        public double PorcentagemAliquota { get; set; }

		[NotMapped]
        public string ValorAlicota { get; set; }


        public int? Fk_id_cliente { get; set; }
		public virtual  Cliente? Cliente { get; set; }

		public int? Fk_id_veiculo { get; set; }
		public virtual Veiculo? Veiculo { get; set; }

        [Required]
        public int? Fk_id_vistoriador { get; set; }
		public virtual Funcionario? Vistoriador { get; set; }


        public int? Fk_id_funcionario { get; set; }
        public virtual Funcionario? FuncionarioVendedor { get; set; }

		public int? Fk_id_empresa { get; set; }
		public virtual Empresa? Empresa { get; set; }


        //[ForeignKey("fk_id_caixa")]
        public long? Fk_id_caixa { get; set; }
        public virtual Caixa? Caixa_venda { get; set; }
        

		public List<Recebimento>? RecebimentoLista { get; set; } = new List<Recebimento>();
        public List<ItemVenda>? ItensVenda { get; set; } = new List<ItemVenda>();

        public Venda()
        {
			//DispositivoRecebimento = new Dispositivo();
			ItensVenda = new List<ItemVenda>();
			ValorAlicota = "0.00";
			DataVenda = DateOnly.FromDateTime(DateTime.Now);
			Hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            TotalParcelas = "1";
            FormaPagamento = "Vista";
        }

		public Venda Clone()
		{
			Object o = MemberwiseClone();
			return o as Venda;
		}
	}

}
