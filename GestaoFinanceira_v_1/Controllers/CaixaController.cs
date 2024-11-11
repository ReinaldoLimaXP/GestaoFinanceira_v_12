using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    public class CaixaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaixaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public List<Caixa> ListarTodos(int? id_empresa)
        {
            try
            {
                return _context.caixas.Include(x => x.Funcionario_Caixa).Include(b=>b.BancoCaixa).Where(x=>x.Fk_id_empresa == id_empresa).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        public async Task<List<Caixa>> ListarUsuario(int? id_funcionario)
        {
            try
            {
                return await _context.caixas
                    .Include(x => x.Funcionario_Caixa)
                    .Include(x=>x.BancoCaixa)
                    .Where(x => x.Funcionario_Caixa.Id_funcionario == id_funcionario).OrderByDescending(x=>x.Id_caixa).ToListAsync();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		[Authorize]
		public async Task<List<Caixa>> ListarAbertoUsuario(int? id_funcionario, int? id_empresa)
		{
			try
			{
				return await _context.caixas.Include(x => x.Funcionario_Caixa).Include(x=>x.BancoCaixa).Where(x => x.Funcionario_Caixa.Id_funcionario == id_funcionario 
                    && x.Status.ToLower() == "aberto" && x.Fk_id_empresa == id_empresa).OrderByDescending(x => x.Id_caixa).ToListAsync();
			}

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        public async Task<Caixa> UltimoFuncionarioOperacional(int? id_funcionario, int? id_empresa)
        {
            try
            {
                if (id_funcionario == null || _context.caixas == null)
                {
                    throw new Exception("Funcionário não permitido!");
                }
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }

                var cx = await _context.caixas
                    .Include(x => x.Funcionario_Caixa)
                    .Include(x => x.BancoCaixa)
                    .Include(x => x.EmpresaCaixa)
                    .Where(c => c.Fk_id_funcionario == id_funcionario
                                 && c.Tipo.ToLower() == "operacional"
                                 && c.Fk_id_empresa == id_empresa)
                    .OrderByDescending(c => c.Id_caixa) // Ordena pelo ID do caixa em ordem decrescente
                    .FirstOrDefaultAsync(); // Busca o primeiro resultado

                //_context.Entry(pessoaExistente).CurrentValues.SetValues(caixa);
                if (cx == null)
                {
                    throw new Exception("Caixa não encontrado!");
                }
                return cx;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o caixa!");
            }
        }

        public async Task<Caixa> UltimoFuncionarioBanco(int? id_funcionario, int? id_empresa, int? fk_id_banco)
        {
            try
            {
                if (id_funcionario == null || _context.caixas == null)
                {
                    throw new Exception("Funcionário não permitido!");
                }
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }

                var cx = await _context.caixas
                    .Include(x => x.Funcionario_Caixa)
                    .Include(x => x.BancoCaixa)
                    .Include(x => x.EmpresaCaixa)
                    .Where(c => c.Fk_id_funcionario == id_funcionario
                                 && c.Tipo.ToLower() == "banco"
                                 && c.Fk_id_empresa == id_empresa
                                 && c.Fk_id_banco == fk_id_banco)
                    .OrderByDescending(c => c.Id_caixa) // Ordena pelo ID do caixa em ordem decrescente
                    .FirstOrDefaultAsync(); // Busca o primeiro resultado

                //_context.Entry(pessoaExistente).CurrentValues.SetValues(caixa);
                if (cx == null)
                {
                    throw new Exception("Caixa não encontrado!");
                }
                return cx;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o caixa!");
            }
        }

        [Authorize]
        public async Task<List<Caixa>> ListarCaixaPorPeriodo(DateOnly dataInicio, DateOnly dataFim, string tipo)
        {
            try
            {
                var caixas = _context.caixas
                    .Include(c => c.Funcionario_Caixa)
                    .Include(c => c.EmpresaCaixa)
                    .Include(c=> c.BancoCaixa)
                    .Include(c => c.SangriaDestinos)
                    .Include(c => c.SangriaOrigem)
                    .Include(c => c.DespesasdoCaixa).ThenInclude(p => p.PlanoPag)
                    .Include(c => c.RecebimentosdoCaixa).ThenInclude(p => p.Plano)
                    .Include(c => c.RecebimentosdoCaixa).ThenInclude(d => d.DispositivoRec)
                    .Include(c => c.RecebimentosdoCaixa).ThenInclude(v => v.Venda)
                    .Include(c => c.VendasdoCaixa)
                    .ThenInclude(v => v.Cliente)  // Inclui a entidade Cliente (geral)
                    .Where(c => c.Data_abertura >= dataInicio && c.Data_abertura <= dataFim && c.Tipo.ToUpper() == tipo.ToUpper())
                    .ToListAsync();


                return await caixas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        public async Task<List<Caixa>> ListarCaixaPorPeriodo(DateOnly dataInicio, DateOnly dataFim)
        {
            try
            {
                var caixas = _context.caixas
                    .Include(c => c.Funcionario_Caixa)
                    .Include(c => c.EmpresaCaixa)
                    .Include(c => c.BancoCaixa)
                    .Include(c => c.SangriaDestinos)
                    .Include(c => c.SangriaOrigem)
                    .Include(c => c.DespesasdoCaixa).ThenInclude(p => p.FornecedorDespesa)
                    .Include(c => c.DespesasdoCaixa).ThenInclude(p=>p.PlanoPag)
                    .Include(c=>c.RecebimentosdoCaixa).ThenInclude(p=>p.Plano)
                    .Include(c => c.RecebimentosdoCaixa).ThenInclude(d => d.DispositivoRec)
                    .Include(c => c.RecebimentosdoCaixa).ThenInclude(v => v.Venda)
                    .Include(c => c.VendasdoCaixa)
                    .ThenInclude(v => v.Cliente)  // Inclui a entidade Cliente (geral)
                    .Where(c => c.Data_abertura >= dataInicio && c.Data_abertura <= dataFim)
                    .ToListAsync();


                return await caixas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(Caixa caixa)
        {
            try
            {
                _context.caixas.Add(caixa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SalvarOp(Caixa cx)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Caixa cxAnterior = new Caixa();
                if (string.Equals(cx.Tipo, "operacional", StringComparison.OrdinalIgnoreCase))
                {
                    cxAnterior = await this.UltimoFuncionarioOperacional(cx.Fk_id_funcionario, cx.Fk_id_empresa);
                }
                else
                {
                    cxAnterior = await this.UltimoFuncionarioBanco(cx.Fk_id_funcionario, cx.Fk_id_empresa, cx.Fk_id_banco);
                }
                    if (cxAnterior.Saldo_final > 0)
                    {
                        cx.Saldo_inicial = cxAnterior.Saldo_final;
                        cx.Saldo_final = cx.Saldo_inicial;
                        if(cx.Saldo_inicial > 0) { 
                            Sangria s = new Sangria
                                {
                                    ValorSangria = cx.Saldo_final,
                                    CaixaDestino = cx,
                                    CaixaOrigem = cxAnterior,
                                    Observacao = "Automática",
                                    Tiposangria = "saldoInicial",
                                };
                            /*Cadastrar a sangria caso seja operacional e com saldo vindo do cx anterior*/
                            _context.Sangrias.Add(s);
                            /*Atualizar o caixa anterior*/
                            cxAnterior.Total_saidas += s.ValorSangria;
                            cxAnterior.Saldo_final -= s.ValorSangria;
                            _context.caixas.Update(cxAnterior);
                        }
                    }
                _context.caixas.Add(cx);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        /*
         * Fiz esse método só para estatus para não correr o risco de atualizar valores;
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AtualizarStatus(Caixa? caixa)
        {
            try
            {
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }

                var cx = _context.caixas.FirstOrDefault(x => x.Id_caixa == caixa.Id_caixa);
                if (cx != null)
                {
                    cx.Status = caixa.Status;
                    cx.Data_fechamento = caixa.Data_fechamento;
                    cx.Horafechamento = DateTime.Now.TimeOfDay;
                    _context.caixas.Update(cx);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Caixa não encontrado");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Caixa> BuscarId(long? id)
        {
            try
            {
                if (id == null || _context.caixas == null)
                {
                    throw new Exception("Id não permitido!");
                }
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }

                var cx = await _context.caixas
                    .Include(x => x.Funcionario_Caixa)
                    .Include(x=>x.BancoCaixa)
                    .Include(x=>x.EmpresaCaixa).SingleAsync(m => m.Id_caixa == id);
                //_context.Entry(pessoaExistente).CurrentValues.SetValues(caixa);
                if (cx == null)
                {
                    throw new Exception("Caixa não encontrado!");
                }
                return cx;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o caixa!");
            }
        }

        public async Task<Caixa> BuscarOperacionalAbertoFuncionario(int? id_funcionario)
        {

            if (id_funcionario == null || _context.caixas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try
            {
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var caixaAberto = await _context.caixas.Include(u => u.Funcionario_Caixa).FirstOrDefaultAsync(x => x.Funcionario_Caixa.Id_funcionario == id_funcionario && x.Status == "Aberto" && x.Tipo == "Operacional");
                return caixaAberto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*
         Retornará true caso permita o cadastro e false caso não possa realizar o cadastro
         */
        public async Task<bool> VerificarCaixaOperacionalOuBanco(int? id_funcionario, string? tipo, int? id_banco)
        {
            if (id_funcionario == null || _context.caixas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try
            {
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                if(tipo?.ToLower() == "operacional")
                {
                    var caixaAberto = await _context.caixas.Include(u => u.Funcionario_Caixa).FirstOrDefaultAsync(x => x.Funcionario_Caixa.Id_funcionario == id_funcionario && x.Status.ToLower() == "aberto" && x.Tipo.ToLower() == "operacional");
                    if(caixaAberto == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(tipo?.ToLower() == "banco")
                {
                    var caixaAberto = await _context.caixas.Include(u => u.Funcionario_Caixa).FirstOrDefaultAsync(x => x.Funcionario_Caixa.Id_funcionario == id_funcionario && x.Status.ToLower() == "aberto" && x.Tipo.ToLower() == "banco" && x.Fk_id_banco == id_banco);
                    if (caixaAberto == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Tipo de caixa não detectado!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /*
         id_caixaOrigem: caixa que sairá o dinheiro
         id_caixaDestino: local que irá o valor
         valor: valor a ser transferido

         Realiza a transferência e cadastra a sangria
         */
        public async Task RealizarSangria(Sangria sangria)
        {
            if (sangria.Fk_id_caixaDestino == null || sangria.Fk_id_caixaOrigem == null || sangria.ValorSangria <= 0 || _context.caixas == null)
            {
                throw new ArgumentException("Parâmetros inválidos fornecidos.");
            }
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                /*Limpar o rastreamento da entidade Caixa*/
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var caixaOrigem = await _context.caixas
                    .FindAsync(sangria.Fk_id_caixaOrigem);

                var caixaDestino = await _context.caixas
                    .FindAsync(sangria.Fk_id_caixaDestino);

                //Condição para evitar sangrias maiores que o valor do caixa
                if (caixaOrigem.Saldo_final < sangria.ValorSangria)
                {
                    throw new ApplicationException("Valor maior que o saldo do caixa: ");
                }

                caixaOrigem.Total_saidas += sangria.ValorSangria;
                caixaOrigem.Saldo_final -= sangria.ValorSangria;

                caixaDestino.Total_entradas += sangria.ValorSangria;
                caixaDestino.Saldo_final += sangria.ValorSangria;

                //Atualizar a sangria
                sangria.CaixaDestino = caixaDestino;
                sangria.CaixaOrigem = caixaOrigem;
                sangria.ValorSangria = sangria.ValorSangria;

                _context.caixas.Update(caixaDestino);
                _context.caixas.Update(caixaOrigem);
                _context.Sangrias.Add(sangria);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Erro ao realizar a sangria: " + ex.Message, ex);
            }
        }

        public async Task CancelarSangria(Sangria sangria)
        {
            if (sangria.Fk_id_caixaDestino == null || sangria.Fk_id_caixaOrigem == null || sangria.ValorSangria <= 0 || _context.caixas == null)
            {
                throw new ArgumentException("Parâmetros inválidos fornecidos.");
            }
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                /*Limpar o rastreamento da entidade Caixa*/
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var caixaOrigem = await _context.caixas
                    .FindAsync(sangria.Fk_id_caixaOrigem);

                var caixaDestino = await _context.caixas
                    .FindAsync(sangria.Fk_id_caixaDestino);


                caixaOrigem.Total_saidas -= sangria.ValorSangria;
                caixaOrigem.Saldo_final += sangria.ValorSangria;

                caixaDestino.Total_entradas -= sangria.ValorSangria;
                caixaDestino.Saldo_final -= sangria.ValorSangria;

                //Atualizar a sangria
                sangria.CaixaDestino = caixaDestino;
                sangria.CaixaOrigem = caixaOrigem;
                sangria.ValorSangria = sangria.ValorSangria;

                _context.caixas.Update(caixaDestino);
                _context.caixas.Update(caixaOrigem);
                _context.Sangrias.Remove(sangria);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Erro ao realizar a sangria: " + ex.Message, ex);
            }
        }

        public async Task<List<Sangria>> ListaSangriaCaixa(long? id_caixa)
        {

            if (id_caixa == null || _context.caixas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try
            {
                return await _context.Sangrias.Include(x => x.CaixaDestino).Include(y => y.CaixaOrigem).Where(x => x.CaixaOrigem.Id_caixa == id_caixa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<Sangria>> SangriaRecebida(long? id_caixa)
        {

            if (id_caixa == null || _context.caixas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try
            {
                return await _context.Sangrias.Include(x => x.CaixaDestino).Include(y => y.CaixaOrigem).Where(x => x.CaixaDestino.Id_caixa == id_caixa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<Sangria>> ListarSangriasEfetuadas(long? id_caixa)
        {

            if (id_caixa == null || _context.caixas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try
            {
                return await _context.Sangrias.Include(x => x.CaixaDestino).Include(y => y.CaixaOrigem).Where(x => x.CaixaOrigem.Id_caixa == id_caixa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}

