using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestaoFinanceira_v_1.Controllers
{
	public class VendaController : Controller
	{
		private readonly ApplicationDbContext _context;

		public VendaController(ApplicationDbContext context)
		{
			_context = context;
		}

        [HttpPost]
        public async Task<List<Venda>> ListarTodos(int? id_empresa)
		{
			try
			{
				List<Venda> lista = await _context.Vendas.Include(x => x.Cliente).Include(x => x.RecebimentoLista).Include(x => x.Caixa_venda).ThenInclude(x => x.Funcionario_Caixa).Where(x=>x.Fk_id_empresa == id_empresa).ToListAsync();
                return lista;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public int Salvar(Venda venda)
		{
			try
			{
				_context.Vendas.Add(venda);
				_context.SaveChanges();
				return venda.Id_venda;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException.Message);
			}
		}

        [HttpPost]
		public async Task<int> SalvarTransaction(Venda venda, int? id_dispositivoSelecionado)
		{
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
                // Primeiro, detache a entidade caixa se necessário
                if (venda.FormaPagamento == "Vista")
				{
                    var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                    foreach (var entry in entries)
                    {
                        entry.State = EntityState.Detached;
                    }
                    var caixa = await _context.caixas
                     .FirstOrDefaultAsync(x => x.Id_caixa == venda.Fk_id_caixa);

                    if (caixa != null)
                    {
                        if (string.Equals(caixa.Status, "Fechado", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new Exception("Caixa fechado!");
                        }
                        else { 
                            // Tente desanexar a entidade caixa
                            // Atualize os valores do caixa
                            caixa.Total_entradas += Math.Round(venda.ValorFinal, 2);
                            caixa.Saldo_final += Math.Round(venda.ValorFinal, 2);
                            _context.Add(caixa);
                            _context.Entry(caixa).State = EntityState.Modified;
                                     
                            foreach (Recebimento r in venda.RecebimentoLista)
                            {
                                r.CaixaRecebimento = caixa;
                            }
                            _context.Vendas.Add(venda);
                            var rec = venda.RecebimentoLista.FirstOrDefault();
                            if (rec != null && double.TryParse(rec.AliquotaParcela, out var aliquotaParcela) && aliquotaParcela > 0 && id_dispositivoSelecionado > 0)
                            {
                                caixa.Total_saidas += Math.Round(aliquotaParcela, 2);
                                caixa.Saldo_final -= Math.Round(aliquotaParcela, 2);
                                Encargo encargo = new Encargo
                                {
                                    RecebimentoEncargo = rec,
                                    Valor = aliquotaParcela,
                                };
                                //encargoTeste.RecebimentoEncargo = rec;
                                _context.Encargos.Add(encargo);
                                //_context.caixas.Update(caixa);
                            }
                        }
                    }
				}
				else
				{
                    _context.Vendas.Add(venda);
                }
				// Adicione a venda ao contexto
				// Salvar alterações e confirmar a transação
				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
				return venda.Id_venda;
			}
			catch (Exception ex)
			{
				if (transaction != null)
				{
					await transaction.RollbackAsync();
				}
				throw new ApplicationException("Erro ao salvar a venda: " + ex.InnerException.Message);
				
			}
		}

		//Esse método é para perder o rastreamento apenas de uma entidade específica;
		public void DetachEntity<T>(T entity) where T : class
		{
			var entry = _context.Entry(entity);
			if (entry.State != EntityState.Detached)
			{
				entry.State = EntityState.Detached;
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<Venda> BuscarId(int? id)
		{
			try { 
				if (id == null || _context.Vendas == null)
				{
					throw new Exception("Id não permitido!");
				}

				var venda =  _context.Vendas
					.Include(x => x.ItensVenda).ThenInclude(y => y.Servico)
					.Include(x=>x.RecebimentoLista).ThenInclude(x=>x.DispositivoRec)
					.Include(x => x.Vistoriador)
					.Include(x=> x.FuncionarioVendedor)
					.Include(x => x.Veiculo)
					.Include(x=>x.Cliente).Include(x=>x.Empresa)
					.ThenInclude(x=>x.Endereco).ThenInclude(x=>x.Cidade).ThenInclude(x=>x.Estado)
					.FirstOrDefault(m => m.Id_venda == id);
					if (venda == null)
					{
						throw new Exception("Venda não encontrado!");
					}
					return venda;
            }
            catch(Exception ex)
			{
				throw new Exception(ex.Message);

            }
		}


        public async Task CancelarVenda(int? id_venda)
        {
            if (id_venda == null)
            {
                throw new ArgumentNullException(nameof(id_venda));
            }
            IDbContextTransaction transaction = null;
            try
            {
                _context.ChangeTracker.Clear();
                _context.caixas.Include(x => x.Funcionario_Caixa).ToList();
                transaction = await _context.Database.BeginTransactionAsync();
                var venda = await _context.Vendas.FindAsync(id_venda);

                if (venda == null)
                {
                    throw new Exception("Venda não encontrada.");
                }

                var cx = await _context.caixas.FirstOrDefaultAsync(x => x.Id_caixa == venda.Caixa_venda.Id_caixa);

                if (cx == null)
                {
                    throw new Exception("Caixa não encontrado.");
                }
                var existingCx = _context.Entry(cx).State;
                if (existingCx != EntityState.Detached)
                {
                    _context.Entry(cx).State = EntityState.Detached;
                }

                var recVenda = await _context.Recebimentos
                    .Include(x => x.Venda)
                    .Where(x => x.Venda.Id_venda == venda.Id_venda)
                    .ToListAsync();

                foreach (var r in recVenda)
                {
                    if (r.StatusRecebimento.Equals("fechado", StringComparison.OrdinalIgnoreCase))
                    {
                        cx.Total_entradas -= Math.Round(r.ValorParcela, 2);
                        cx.Saldo_final -= Math.Round(r.ValorParcela, 2);

                        if (double.TryParse(r.AliquotaParcela, out double aliquota) && aliquota > 0)
                        {
                            cx.Total_saidas -= Math.Round(aliquota, 2);
                            cx.Saldo_final += Math.Round(aliquota, 2);
                        }
                    }
                }

                _context.caixas.Update(cx);
                _context.Recebimentos.RemoveRange(recVenda);
                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<List<Venda>> BuscarVendasPorData(DateOnly dataInicio, DateOnly dataFinal)
		{
			//List<Venda> vendas = new List<Venda>(); 
			if (_context.Vendas == null)
			{
				throw new Exception("Contexto de vendas não pode ser nulo!");
			}

			//var vendasFiltro = await _context.Vendas.Where(m => m.DataVenda >= dataInicio && m.DataVenda <= dataFinal)
			//    .ToListAsync();

			//foreach ( var venda in vendasFiltro) 
			//{
			//    vendas.Add(BuscarId(venda.Id_venda).Result);
			//}
			try
			{
				var vendas = await _context.Vendas
					.Include(x => x.ItensVenda).ThenInclude(y => y.Servico)
					.Include(x => x.RecebimentoLista).ThenInclude(x => x.DispositivoRec)
					.Include(x => x.Vistoriador)
					.Include(x => x.FuncionarioVendedor)
					.Include(x => x.Veiculo)
					.Include(x => x.Cliente)
					.Include(x => x.Empresa).ThenInclude(x => x.Endereco).ThenInclude(x => x.Cidade).ThenInclude(x => x.Estado)
					.AsSplitQuery() // Adicione esta linha para dividir a consulta
					.Where(m => m.DataVenda >= dataInicio && m.DataVenda <= dataFinal)
					.ToListAsync();

				if (vendas == null || vendas.Count == 0)
				{
					throw new Exception("Nenhuma venda encontrada no período especificado!");
				}
				return vendas;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);

			}
		}


	}
}
