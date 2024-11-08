using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    public class EstadosController 
    {
        private readonly ApplicationDbContext _context;

        public EstadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<List<Estado>> listarTodosawait()
        {
            try
            {
                return await _context.estados.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
