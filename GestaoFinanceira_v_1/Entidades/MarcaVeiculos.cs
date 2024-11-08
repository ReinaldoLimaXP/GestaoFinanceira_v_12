using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    [Table("MarcaVeiculos")]
    public class MarcaVeiculos
    {
        [Key]
        public int Id_marca { get; set; }

        [MaxLength(255)]
        public string? NomeMarca { get; set; }

        //public ICollection<Veiculo> VeicuosList { get; set; } = new List<Veiculo>();
    }

    [Table("Cores")]
    public class Cores
    {
        [Key]
        public int Id_cor { get; set; }

        [MaxLength(200)]
        public string? NomeCor { get; set; }

        [MaxLength(50)]
        public string? CodigoRGB { get; set; }

        //public ICollection<Veiculo> VeicuosList { get; set; } = new List<Veiculo>();
    }

    [Table("CategoriaVeiculos")]
    public class CategoriaVeiculos
    {
        [Key]
        public int Id_categoria { get; set; }

        [MaxLength(200)]
        public string? NomeCategoria { get; set; }

        //public ICollection<Veiculo> VeicuosList { get; set; } = new List<Veiculo>();
    }
}
