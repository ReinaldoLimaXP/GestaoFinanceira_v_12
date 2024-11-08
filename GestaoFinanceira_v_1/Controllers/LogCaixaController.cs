using GestaoFinanceira_v_1.Data;
using GestaoFinanceira_v_1.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFinanceira_v_1.Controllers
{
	public class LogCaixaController: Controller
	{
		private readonly ApplicationDbContext _context;

		public LogCaixaController(ApplicationDbContext context)
		{
			_context = context;
		}

		[ValidateAntiForgeryToken]
		public async Task Salvar(LogCaixa log)
		{
			try
			{
				_context.LogCaixas.Add(log);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
