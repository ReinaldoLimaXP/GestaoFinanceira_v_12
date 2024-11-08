using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    public class Encargo
    {
        [Key]
        public int Id_encargo {  get; set; }

        public double Valor { get; set; }

        [Required]
        [ForeignKey("fk_id_recebimento")]
        public Recebimento? RecebimentoEncargo { get; set; }
    }
}
