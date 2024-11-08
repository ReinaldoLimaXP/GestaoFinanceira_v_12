using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("servicos")]
    public class Servico
	{
        [Key]
        public int Id_servico { get; set; }

        //Exemplo commit
		[Required]
		[StringLength(255)]
		public string? Nome_ser{ get; set; }

		//bolar uma expressão para esse valor.
		[Required]
		public double ValorSugerido_ser { get; set; }

        [MaxLength(25)]
        public string? TempoEstimado_ser { get; set; }

        [MaxLength(25)]
        public string? Garantia_ser { get; set; }

        [MaxLength(150)]
        public string? Tipo_ser { get; set; }

    }
}
