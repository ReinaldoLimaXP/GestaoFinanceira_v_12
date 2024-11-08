using GestaoFinanceira_v_1.Data;
using GestaoFinanceira_v_1.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    public class BancoController: Controller
    {
        private readonly ApplicationDbContext _context;
        public BancoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<List<Banco>> ListarTodos()
        {
            try
            {
                return await _context.Bancos.OrderBy(x => x.Descricao).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
