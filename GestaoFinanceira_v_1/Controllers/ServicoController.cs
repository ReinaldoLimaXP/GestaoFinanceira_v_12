
using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Globalization;

namespace GestaoFinanceira_v_1.Controllers
{
    [Authorize]
    public class ServicoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public List<Servico> ListarTodos()
        {
            try
            {
                
                    return _context.servicos.OrderBy(x=>x.Nome_ser).ToList();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Servico>> ListarTodoswait()
        {
            try
            {
                return await _context.servicos.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(Servico servico)
        {
            try
            {
                
                    _context.servicos.Add(servico);
                    await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void Atualizar(int id, Servico servico)
        {
            try
            {
                
                    _context.servicos.Update(servico);
                    await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Servico> BuscarId(int? id)
        {
           
                if (id == null || _context.servicos == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var servico = await _context.servicos
                    .FirstOrDefaultAsync(m => m.Id_servico == id);
                if (servico == null)
                {
                    throw new Exception("Serviço não encontrado!");
                }

                return servico;
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Deletar(int? id)
        {

                if (_context.servicos == null || id == null || id == 0)
                {
                    throw new Exception("Serviço não localizado!");
                }
                try
                {
                    Servico servico = await _context.servicos.FindAsync(id);
                    if (servico != null)
                    {
                        _context.servicos.Remove(servico);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Serviço não encontrado!");
                    }
                }
                catch (DbUpdateException ex)
                {
                    var mysqlException = ex.InnerException as MySqlException;

                    // Verificar o número do erro de SQL
                    if (mysqlException != null && mysqlException.Number == 1451)
                    {
                        // 1451 é o código para violação de integridade referencial no MySQL
                        throw new ReferentialIntegrityException("Não é possível realizar a exclusão: há vendas registradas com o serviço!");
                    }
                    else
                    {
                        // Re-lançar a exceção se não for de integridade referencial
                        throw new Exception(ex.Message);
                    }
                }
            catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        

    }
}