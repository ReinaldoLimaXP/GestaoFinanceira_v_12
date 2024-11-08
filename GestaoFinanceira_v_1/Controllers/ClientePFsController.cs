using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using MySqlConnector;


namespace GestaoFinanceira_v_1.Controllers
{
    public class ClientePFsController
    {
        private readonly ApplicationDbContext _context;

        public ClientePFsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientePF>> listarTodos()
        {
            
                List<ClientePF> retorno = await _context.clientePF.Include(u => u.Cidade).OrderBy(x=>x.Nome_cli).ToListAsync();
                return retorno;
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(ClientePF clientePF)
        {
            try
            {
                    _context.clientePF.Add(clientePF);
                    await _context.SaveChangesAsync();
                
            }catch(Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        /*Deleta qualquer tipo de cliente, mover para o controller cliente*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Deletar(int? id)
        {
                if (_context.clientes == null || id == null || id == 0)
                {
                    throw new Exception("Cliente não localizado!");
                }
                try
                {
                    Cliente cliente = await _context.clientes.SingleAsync(e => e.Id_cliente == id);
                    if (cliente != null)
                    {
                        _context.clientes.Remove(cliente);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Cliente não encontrado!");
                    }
                }
                catch(DbUpdateException ex)
                {
                    var mysqlException = ex.InnerException as MySqlException;

                    // Verificar o número do erro de SQL
                    if (mysqlException != null && mysqlException.Number == 1451)
                    {
                        // 1451 é o código para violação de integridade referencial no MySQL
                        throw new ReferentialIntegrityException("Não é possível realizar a exclusão: O cliente é utlizado em outros procedimentos");
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

        public async void Atualizar(int id, ClientePF clientePF)
        {
            try
            {

                    _context.clientePF.Update(clientePF);
                    await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ClientePF> BuscarId(int? id)
        {
          
                if (id == null || _context.clientes == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var cliente = await _context.clientePF
                    .FirstOrDefaultAsync(m => m.Id_cliente == id);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }

                return cliente;
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ClientePF> BuscarCPF(string? cpf)
        {
  
                if (cpf == null || _context.clientes == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var cliente = await _context.clientePF
                    .FirstOrDefaultAsync(m => m.Cpf_cli == cpf);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado!");
                }

                return cliente;
            
        }

    }


    public class ReferentialIntegrityException : Exception
    {
        public ReferentialIntegrityException(string message) : base(message)
        {
        }
    }
}
