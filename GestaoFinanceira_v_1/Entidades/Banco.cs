using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GestaoFinanceira_v_1.Entidades
{
    public class Banco
    {
        [Key]
        public int Id_banco { get; set; }

        public string? Descricao { get; set;}

        public virtual ICollection<Caixa>? CaixasdoBanco { get; set; } = new List<Caixa>();
    }
}
