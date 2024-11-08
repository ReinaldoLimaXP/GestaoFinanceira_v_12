using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GestaoFinanceira_v_1.Controllers
{
    public class FuncionarioController
    {
        private readonly ApplicationDbContext _context;
        public FuncionarioController(ApplicationDbContext context)
        {
            _context = context;
        }

 
        public List<Funcao> ListarFuncoes()
        {
            try
            {
                return _context.funcao.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

  
        public List<Funcionario> ListarFuncionarios()
        {
            try
            {
                return _context.funcionarios.Include(u => u.Funcao_fun).Include(u => u.Empresa_fun).OrderBy(x=>x.Nome_completo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<Funcionario>> ListarFuncionariosEmpresa(int? id_empresa)
        {
            try
            {
                return await _context.funcionarios
                    .Include(u => u.Funcao_fun)
                    .Include(u => u.Empresa_fun)
                    .Where(x=>x.Fk_id_empresa == id_empresa)
                    .OrderBy(x => x.Nome_completo).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<Funcionario> ApenasFuncComCaixa(int? fk_id_empresa)
        {
            try
            {
                return _context.funcionarios.Include(u => u.Funcao_fun).Include(u => u.Empresa_fun)
                    .Include(x => x.CaixaDoFuncionario).OrderBy(x => x.Nome_completo)
                    .Where(x=>x.Fk_id_empresa == fk_id_empresa)
                    .Where(f => f.CaixaDoFuncionario.Any()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public void Salvar(Funcionario funcionario)
        {
            try
            {
                _context.funcionarios.Add(funcionario);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task Atualizar(Funcionario funcionario)
        {
            try
            {
                _context.funcionarios.Update(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Deletar(int? id)
        {
            if (_context.funcionarios == null || id == null || id == 0)
            {
                throw new Exception("Funcionário não localizado!");
            }
            try
            {
                Funcionario funcionario = _context.funcionarios.Include(x=>x.Endereco_fun).First(x=>x.Id_funcionario == id);
                Endereco endFun = _context.enderecos.Find(funcionario.Endereco_fun.Id_endereco);
                if (funcionario != null)
                {
                    _context.funcionarios.Remove(funcionario);
                    _context.enderecos.Remove(endFun);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Funcionário não encontrado!");
                }
            }
            catch (DbUpdateException ex)
            {
                var mysqlException = ex.InnerException as MySqlException;

                // Verificar o número do erro de SQL
                if (mysqlException != null && mysqlException.Number == 1451)
                {
                    // 1451 é o código para violação de integridade referencial no MySQL
                    throw new ReferentialIntegrityException("Não é possível realizar a exclusão: O funcionário possui outros registros no sistema.");
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
        public async Task<Funcionario> BuscarCPF(string? cpf)
        {
            if (cpf == null || _context.funcionarios == null)
            {
                throw new Exception("Id não permitido!");
            }

            var funcionario = await _context.funcionarios
                .FirstOrDefaultAsync(m => m.Cpf == cpf);
            if (funcionario == null)
            {
                throw new Exception("Funcionário não encontrado!");
            }

            return funcionario;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Funcionario BuscarID(int? id)
        {
            try { 
                if (id == null || _context.funcionarios == null)
                {
                    throw new Exception("Id não permitido!");
                }

                var funcionario =  _context.funcionarios.Include(x => x.Endereco_fun).Include(x => x.Empresa_fun)
                    .FirstOrDefault(m => m.Id_funcionario == id);
                if (funcionario == null)
                {
                    throw new Exception("Funcionário não encontrado!");
                }
                return funcionario;
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao buscar o funcionário!");
            }
           
        }


		public async Task<Funcionario> BuscarIDAwait(int? id)
		{
			try
			{
				if (id == null || _context.funcionarios == null)
				{
					throw new Exception("Id não permitido!");
				}

				var funcionario = await _context.funcionarios.Include(x => x.Endereco_fun).Include(x => x.Empresa_fun)
					.FirstOrDefaultAsync(m => m.Id_funcionario == id);
				if (funcionario == null)
				{
					throw new Exception("Funcionário não encontrado!");
				}
				return funcionario;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao buscar o funcionário! "+ex.Message);
			}

		}

	}
}
