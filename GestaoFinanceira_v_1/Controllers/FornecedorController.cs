using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GestaoFinanceira_v_1.Controllers
{
    public class FornecedorController:Controller
    {
        private readonly ApplicationDbContext _context;
        public FornecedorController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]

        public List<Fornecedor> ListarFornecedores()
        {
            try
            {
                return _context.Fornecedores.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		[HttpPost]
		public async Task<List<Fornecedor>> ListarFornecedoresawait()
		{
			try
			{
				return await _context.Fornecedores.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(Fornecedor fornecedor)
        {
            try
            {
                _context.Fornecedores.Add(fornecedor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Deletar(int? id)
        {
            if (_context.veiculos == null || id == null || id == 0)
            {
                throw new Exception("Fornecedor não localizado!");
            }
            try
            {
                Fornecedor fornecedor = _context.Fornecedores.Find(id);
                if (fornecedor != null)
                {
                    _context.Fornecedores.Remove(fornecedor);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Fornecedor não encontrado!");
                }
            }
			catch (DbUpdateException ex)
			{
				var mysqlException = ex.InnerException as MySqlException;

				// Verificar o número do erro de SQL
				if (mysqlException != null && mysqlException.Number == 1451)
				{
					// 1451 é o código para violação de integridade referencial no MySQL
					throw new ReferentialIntegrityException("Não é possível realizar a exclusão: Há pagamentos cadastrados para esse fornecedor!");
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
        public async Task<Fornecedor> BuscarId(int? id)
        {
            if (id == null || _context.Fornecedores == null)
            {
                throw new Exception("Id não permitido!");
            }

            Fornecedor fornecedor = await _context.Fornecedores.Include(x => x.Endereco).ThenInclude(x=>x.Cidade).FirstOrDefaultAsync(m => m.Id_fornecedor == id);
            if (fornecedor == null)
            {
                throw new Exception("Fornecedor não encontrado!");
            }

            return fornecedor;
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            try
            {
                _context.Fornecedores.Update(fornecedor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
