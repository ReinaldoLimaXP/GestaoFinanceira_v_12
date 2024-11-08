using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        public int Id_cliente { get; set; }
        
        [Required]
        [MaxLength(40)]
        public string? Celular { get; set; }

        [MaxLength(40)]
        public string? Tel_trabalho { get; set;}

        [MaxLength(40)]
        public string? Tel_casa { get; set; }

        [MaxLength(120)]
        public string? Email_profissional { get; set; }

        [MaxLength(120)]
        public string? Email_pessoal { get; set; }

        [MaxLength(120)]
        public string? Foto { get; set; }


        [Required]
        [Column("cep")]
        [MaxLength(30)]
        public string? Cep { get; set; }

        [Required]
        [Column("logradouro")]
        [MaxLength(120)]
        public string? Logradouro { get; set; }

        [Required]
        [Column("numero")]
        [MaxLength(15)]
        public string? Numero { get; set; }

        [Required]
        [Column("bairro")]
        [MaxLength(150)]
        public string? Bairro { get; set; }


        [Required]
        public int? fk_id_cidades { get; set; }

        [Required]
        [ForeignKey("fk_id_cidades")]
        public virtual Cidade? Cidade { get; set; }

        public virtual ICollection<Veiculo>? Veiculos { get; set; } = new List<Veiculo>();

        public virtual ICollection<Venda>? ComprasdoCliente { get; set; } = new List<Venda>();

		public Cliente()
        {
            Cidade = new Cidade();
        }
    }
}
