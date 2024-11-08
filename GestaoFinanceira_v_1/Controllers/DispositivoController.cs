using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
	public class DispositivoController:Controller
	{
		private readonly ApplicationDbContext _context;

		public DispositivoController(ApplicationDbContext context)
		{
			_context = context;
		}

		[Authorize]
		public List<Dispositivo> ListarTodosAtivo()
		{
			try
			{
				return _context.Dispositivos.Where(x => x.Ativo == true).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(Dispositivo dispositivo)
        {
            try
            {
                _context.Dispositivos.Add(dispositivo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Atualizar(Dispositivo dispositivo)
        {
            try
            {
                _context.Dispositivos.Update(dispositivo);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Dispositivo BuscarId(int? id)
        {
            if (id == null || _context.Dispositivos == null)
            {
                throw new Exception("Id não permitido!");
            }

            Dispositivo dispositivo = _context.Dispositivos.FirstOrDefault(m => m.Id_dispositivo == id);
            if (dispositivo == null)
            {
                throw new Exception("Dispositivo não encontrado!");
            }

            return dispositivo;
        }
    }
}
