using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestaoFinanceira_v_1.Controllers
{
	public class DespesaController : Controller
	{
		private readonly ApplicationDbContext _context;
		public DespesaController(ApplicationDbContext context)
		{
			_context = context;
		}


		[HttpPost]
		public List<Despesa> ListarDespesas()
		{
			try
			{
				return _context.Despesas.Include(u => u.FornecedorDespesa).Include(x=>x.CaixaPagamento).ThenInclude(x=>x.Funcionario_Caixa).ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        [HttpPost]
        public async Task<List<Despesa>> ListarDespesasawait()
        {
            try
            {
                return await _context.Despesas
                     .Include(u => u.FornecedorDespesa)
                     .Include(j => j.PlanoPag)
                     .Include(x => x.CaixaPagamento)
                     .ThenInclude(y => y.Funcionario_Caixa)
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        public async Task<List<Despesa>> ListarDespesasEmpresaawait(int? id_empresa)
        {
            try
            {
                return await _context.Despesas
                     .Include(u => u.FornecedorDespesa)
                     .Include(j => j.PlanoPag)
                     .Include(x => x.CaixaPagamento)
                     .ThenInclude(y => y.Funcionario_Caixa)
                     .Where(x=>x.Fk_id_empresa == id_empresa)
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public List<Despesa> ListarDespesasAbertas()
        {
            try
            {
                return _context.Despesas.Include(u => u.FornecedorDespesa).Where(x=>x.Status_des.ToLower() == "aberto").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task Salvar(Despesa despesa)
		{
			try
			{
				_context.Despesas.Add(despesa);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException.Message);
			}
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Deletar(int? id)
        {
            if (_context.Despesas == null || id == null || id == 0)
            {
                throw new Exception("Despesa não localizado!");
            }
            try
            {
                Despesa despesa = _context.Despesas.Find(id);
                if (despesa != null)
                {
                    _context.Despesas.Remove(despesa);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Despesa não encontrado!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Despesa BuscarId(int? id)
        {
            if (id == null || _context.Despesas == null)
            {
                throw new Exception("Id não permitido!");
            }

            Despesa desp = _context.Despesas.Include(x => x.PlanoPag).Include(x=>x.FornecedorDespesa).Include(x=>x.CaixaPagamento).FirstOrDefault(m => m.Id_despesa == id);
            if (desp == null)
            {
                throw new Exception("Despesa não encontrado!");
            }

            return desp;
        }

        [HttpPost]
        public async Task<Despesa> BuscarIdWait(int? id)
        {
            if (id == null || _context.Despesas == null)
            {
                throw new Exception("Id não permitido!");
            }
            try { 
            Despesa desp = await _context.Despesas.Include(x => x.PlanoPag).Include(x=>x.FornecedorDespesa).FirstOrDefaultAsync(m => m.Id_despesa == id);
            if (desp == null)
            {
                throw new Exception("Despesa não encontrado!");
            }
            return desp;
            }catch(Exception ex)
            {
                throw new Exception("Erro ao buscar a despesa!");
            }
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task Atualizar(Despesa veiculo)
		{
			try
			{
				_context.Despesas.Update(veiculo);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException ex)
			{
                throw new Exception(ex.Message);
			}
        }

        public async Task PagarDespesa(Despesa despesa)
        {
            if (despesa == null)
            {
                throw new ArgumentNullException(nameof(despesa));
            }
            IDbContextTransaction transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();                
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var cx = await _context.caixas.FindAsync(despesa.Fk_id_caixa);
                if (cx == null)
                {
                    throw new Exception("Caixa não encontrado.");
                }
                if (cx.Saldo_final < despesa.Valor)
                {
                    throw new PagamentoException("Saldo insuficiente no caixa para realizar o pagamento da despesa!");
                }
                despesa.Status_des = "Fechado";
                cx.Total_saidas += Math.Round(despesa.Valor, 2);
                cx.Saldo_final -= Math.Round(despesa.Valor, 2);
                _context.Add(cx);
                _context.Entry(cx).State = EntityState.Modified;
                despesa.CaixaPagamento = cx;
                _context.Despesas.Update(despesa);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }catch(PagamentoException ex)
            {
                throw new PagamentoException("Saldo insuficiente no caixa para realizar o pagamento da despesa!");
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                } // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }
        }


        public async Task CancelarPagamentoDespesa(Despesa despesa)
        {
            if (despesa == null)
            {
                throw new ArgumentNullException(nameof(despesa));
            }
            IDbContextTransaction transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var cx = await _context.caixas.FindAsync(despesa.Fk_id_caixa);
                if (cx != null)
                {
                    despesa.Status_des = "Aberto";
                    cx.Total_saidas -= Math.Round(despesa.Valor, 2);
                    cx.Saldo_final += Math.Round(despesa.Valor, 2);
                    _context.Add(cx);
                    _context.Entry(cx).State = EntityState.Modified;
                    despesa.CaixaPagamento = cx;
                }
                else
                {
                    throw new Exception("Caixa não encontrado.");
                }
                _context.Despesas.Update(despesa);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }
        }
    }
}
