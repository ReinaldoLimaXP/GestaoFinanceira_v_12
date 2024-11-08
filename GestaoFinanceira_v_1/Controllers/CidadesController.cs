using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    public class CidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public List<Cidade> listarTodos()
        {
            try
            {
                 return _context.cidades.ToList();                
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<Cidade>> ListarTodosawait()
        {
            try
            {
                return await _context.cidades.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
