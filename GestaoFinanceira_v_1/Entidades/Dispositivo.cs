using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
	[Table("Dispositivos")]
	public class Dispositivo
	{
		[Key]
		public int Id_dispositivo { get; set; }

		[Required]
		public string? Tipo { get; set; }

		[Required]
		public string? Descricao { get; set; }

		[Required]
		public string? Operacao { get; set; }

		[Required]
		public double Alicota { get; set; }

        [Required]
		[NotMapped]
        public string AlicotaMap { get; set; }

        public bool? Ativo { get; set; }

		public virtual ICollection<Recebimento> RecebimentosdoDispositivo { get; set; } = new List<Recebimento>();

	}
}
