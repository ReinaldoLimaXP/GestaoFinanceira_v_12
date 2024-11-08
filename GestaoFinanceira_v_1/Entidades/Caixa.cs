using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira_v_1.Entidades
{
    public class Caixa
    {
        [Key]
        public long Id_caixa { get; set; }

        public DateOnly? Data_abertura { get; set; }

        public DateOnly? Data_fechamento { get; set; }

        public TimeSpan? Horaabertura { get; set; }

        public TimeSpan? Horafechamento { get; set; }

        [Required]
        public double Saldo_inicial { get; set; }

        public double Saldo_final { get; set; }

        public double Total_entradas { get; set; }

        public double Total_saidas { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Tipo { get; set; }

        [Required]
        public string? Descricao { get; set; }

        [Required]
		public int? Fk_id_funcionario { get; set; }
        public virtual Funcionario? Funcionario_Caixa { get; set; }

        public int? Fk_id_banco { get; set; }
        public virtual Banco? BancoCaixa { get; set; }

        public int? Fk_id_empresa { get; set; }
        public virtual Empresa? EmpresaCaixa { get; set; }

        public virtual ICollection<Sangria>? SangriaDestinos { get; set; } = new List<Sangria>();

        public virtual ICollection<Sangria>? SangriaOrigem { get; set; } = new List<Sangria>();

        public virtual ICollection<Despesa>? DespesasdoCaixa { get; set; } = new List<Despesa>();

        public virtual ICollection<Venda>? VendasdoCaixa { get; set; } = new List<Venda>();

		public virtual ICollection<Recebimento>? RecebimentosdoCaixa { get; set; } = new List<Recebimento>();

		public virtual ICollection<LogCaixa>? LogsDoCaixa { get; set; } = new List<LogCaixa>();

		public Caixa()
        {
            this.Data_abertura = DateOnly.FromDateTime(DateTime.Now);
            this.Horaabertura = DateTime.Now.TimeOfDay;
            this.Status = "Aberto";
            Saldo_inicial = 0.00;
        }
    }
}
