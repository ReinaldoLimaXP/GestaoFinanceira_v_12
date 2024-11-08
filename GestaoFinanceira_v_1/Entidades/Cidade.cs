using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("cidades")]
    public class Cidade
    {
        [Key]
        [Column("id_cidade")]
        public int Id_cidade { get; set; }

       
        [Column("nome_cid")]
        [MaxLength(130)]
        public string? Nome_cid { get; set; }

        [Column("ibge_cid")]
        public int? Ibge_cid { get; set; }

        //[Column("fk_id_estado")]
        //public int? Fk_id_estado{ get; set; }
    
        [ForeignKey("fk_id_estado")]
        public virtual Estado? Estado { get; set; }

        public virtual ICollection<Cliente>? Clientes { get; set; } = new List<Cliente>();

        public virtual ICollection<Endereco>? ClientesdeEnderecos { get; set; } = new List<Endereco>();

        public Cidade()
        {
            Estado = new Estado();
        }
    }
}
