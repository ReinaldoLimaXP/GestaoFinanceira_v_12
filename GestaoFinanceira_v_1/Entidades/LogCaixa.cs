using System.ComponentModel.DataAnnotations;

namespace GestaoFinanceira_v_1.Entidades
{
	public class LogCaixa
	{
		[Key]
		public int LogCaixas { get; set; }

		public string? Operacao { get; set; }

		public DateOnly? DataLog { get; set;}

		public TimeSpan? HoraLog { get; set; }

		public string? Observacao { get; set; }


		public double? SaldoAtual { get; set; }

		public long? Fk_id_caixa { get; set; }

		public virtual Caixa? CaixaLog { get; set; }

		public int? Fk_id_funcionario { get; set; }
		public virtual Funcionario? FuncionarioLog { get; set; }

		public LogCaixa()
		{
            this.DataLog = DateOnly.FromDateTime(DateTime.Now);
			this.HoraLog = DateTime.Now.TimeOfDay;
        }
    }
}
