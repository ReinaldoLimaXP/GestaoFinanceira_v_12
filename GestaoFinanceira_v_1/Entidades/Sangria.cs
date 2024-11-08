using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    public class Sangria
    {
        [Key]
        public int Id_sangria { get; set; }

        [Required]
        public double ValorSangria { get; set; }
        public DateOnly DataSangria { get; set; }
        public string? HoraSangria { get; set; }

        [Required]
        public string? Observacao { get; set; }
        public string? Tiposangria { get; set; }
        public long? Fk_id_caixaOrigem { get; set; }
        public virtual Caixa CaixaOrigem { get; set; }

        [Required]
        public long? Fk_id_caixaDestino { get; set; }
        public virtual Caixa CaixaDestino { get; set; }

        public Sangria()
        {
            DataSangria = DateOnly.FromDateTime(DateTime.Now);
            HoraSangria = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            Tiposangria = "TransferenciaUsuario";
        }
    }
}
