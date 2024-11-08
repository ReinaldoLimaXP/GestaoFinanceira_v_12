using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("estados")]
    public class Estado
    {
        [Key]
        [Column("id_estado")]
        public int Id_estado { get; set; }
        
        [Required]
        [Column("nome_est")]
        [MaxLength(255)]
        public string? Nome_est { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("uf_est")]
        public string? Uf_est { get; set; }

        [Column("ibge_est")]
        public int? Ibge_est { get; set; }

        [Column("pais_est")]
        public int? Pais_est { get; set; }

        [Column("ddd_est")]
        [MaxLength(30)]
        public string? Ddd_est { get; set; }

        public List<Cidade>? cidadeList { get; set; } = new List<Cidade>();
    }
}
