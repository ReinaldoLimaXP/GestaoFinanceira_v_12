using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("ItensVenda")]
    public class ItemVenda
    {
        [Key]
        [Column("id_itenVenda")]
        public long Id_ItemVenda {  get; set; }

        public double ValorUnitario { get; set; }

        public int Quantidade { get; set;}

        public double ValorTotalItens { get; set; }

        [ForeignKey("fk_id_venda")]
		public virtual Venda Venda { get; set; } = new Venda();

		[ForeignKey("fk_id_servico")]
		public virtual Servico Servico { get; set; } = new Servico();

        public ItemVenda()
        {
            Venda = new Venda();
            Servico = new Servico();
        }
	}
}
