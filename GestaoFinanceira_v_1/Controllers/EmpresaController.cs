using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GestaoFinanceira_v_1.Controllers
{
    public class EmpresaController
    {
        private readonly ApplicationDbContext _context;

        public EmpresaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Empresa> listarTodos()
        {
            try
            {
                List<Empresa> retorno = _context.empresas.Include(u => u.Endereco.Cidade).ToList();
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void Salvar(Empresa empresa)
        {
            
                _context.empresas.Add(empresa);
                await _context.SaveChangesAsync();
            
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void Atualizar(int id, Empresa empresa)
        {
            try
            {
                _context.empresas.Update(empresa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Deletar(int? id)
        {
            if (_context.empresas == null || id == null || id == 0)
            {
                throw new Exception("Empresa não localizado!");
            }
            try
            {
                Empresa empresa = await _context.empresas.FindAsync(id);
                if (empresa != null)
                {
                    _context.empresas.Remove(empresa);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Empresa não encontrado!");
                }
            }
            catch (DbUpdateException ex)
            {
                var mysqlException = ex.InnerException as MySqlException;

                // Verificar o número do erro de SQL
                if (mysqlException != null && mysqlException.Number == 1451)
                {
                    // 1451 é o código para violação de integridade referencial no MySQL
                    throw new ReferentialIntegrityException("Não é possível realizar a exclusão: A empresa possui outros registros no sistema");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Empresa> BuscarCNPJ(string? id)
        {
            try
            {
                if (id == null || _context.empresas == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var empresa = await _context.empresas.FirstOrDefaultAsync(m => m.Cnpj == id);
                if (empresa == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }

                return empresa;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Empresa> BuscarId(int? id)
        {
            try
            {
                if (id == null || _context.empresas == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var empresa = await _context.empresas.Include(x=>x.Endereco).ThenInclude(x=>x.Cidade).FirstOrDefaultAsync(m => m.Id_empresa == id);
                if (empresa == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }

                return empresa;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}