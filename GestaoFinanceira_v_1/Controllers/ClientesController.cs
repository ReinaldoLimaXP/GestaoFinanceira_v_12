using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;


namespace GestaoFinanceira_v_1.Controllers
{
    public class ClientesController
    {
        private readonly ApplicationDbContext _context;


        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

  
        public List<Cliente> listarTodos()
        {
            try
            {
                return _context.clientes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
