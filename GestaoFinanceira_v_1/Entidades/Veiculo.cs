using GestaoFinanceira_v_1.Utilitarios;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("veiculos")]
    public class Veiculo
    {
        [Key]
        public int Id_veiculo { get; set; }

        [Required]
        [Column("placa")]
        [MaxLength(100)]
        public string? Placa_vei { get; set; }

        [Required]
        [Column("anofabricacaoModelo")]
        [MaxLength(150)]
        public string? Ano_vei { get; set; }

        [Column("chassis")]
        [ValidarChassis(ErrorMessage = "Chassis inválido")]
        [MaxLength(17)]
        public string? Classis_vei { get; set; }

        [Column("renavan_vei")]
        [ValidarRenavan(ErrorMessage = "Renavan inválido")]
        [MaxLength(11)]
        public string? Renavan_vei { get; set; }

        [Required]
        [Column("modelo")]
        [MaxLength(150)]
        public string? Modelo_vei { get; set; }

        [NotMapped]
        [Required]
        public string? Vincular_cli { get; set; }

  
        public int? fk_id_marca { get; set; }

        [ForeignKey("fk_id_marca")]
        public MarcaVeiculos? Marca_vei { get; set; }

    
        public int? fk_id_cor { get; set; }

        [ForeignKey("fk_id_cor")]
        public Cores? Cor_vei { get; set; }

        public int? fk_id_categoria { get; set; }

        [ForeignKey("fk_id_categoria")]
        public CategoriaVeiculos? Categoria_vei { get; set; }

		// Chave estrangeira opcional
		public int? Fk_id_cliente { get; set; } 
        // Propriedade de navegação para o Cliente, pode ser nulo
        public virtual Cliente? Cliente { get; set; }

		public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();

		public Veiculo()
        {
            Vincular_cli = "";
            Cor_vei = new Cores();
            Categoria_vei = new CategoriaVeiculos();
            Marca_vei = new MarcaVeiculos();
        }
    }
}
