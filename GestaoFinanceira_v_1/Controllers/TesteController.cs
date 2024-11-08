using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1_0.Controllers
{
    public class TesteController
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public TesteController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Caixa> BuscarId(long? id)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    if (id == null || context.caixas == null)
                    {
                        throw new Exception("Id não permitido!");
                    }
                    var caixa = await context.caixas.Include(u => u.Funcionario_Caixa).FirstOrDefaultAsync(m => m.Id_caixa == id);
                    if (caixa == null)
                    {
                        throw new Exception("Caixa não encontrado!");
                    }
                    return caixa;
                }
            }
            catch
            {
                throw new Exception("Erro ao buscar o caixa!");
            }
        }


        public async Task Atualizar2(Venda? venda)
        {
            try
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    var cx = _context.caixas.Single(x => x.Id_caixa == venda.Caixa_venda.Id_caixa);

                    foreach (Recebimento r in venda.RecebimentoLista)
                    {
                        if (r.StatusRecebimento.ToLower() == "fechado")
                        {
                            cx.Total_entradas -= Math.Round(r.ValorParcela, 2);
                            cx.Saldo_final -= Math.Round(r.ValorParcela, 2);
                            if (double.Parse(r.AliquotaParcela) > 0)
                            {
                                cx.Total_saidas -= Math.Round(double.Parse(r.AliquotaParcela), 2);
                                cx.Saldo_final += Math.Round(double.Parse(r.AliquotaParcela), 2);
                            }
                        }
                    }
                    List<Recebimento> recVenda = _context.Recebimentos.Include(x => x.Venda).Where(x => x.Venda.Id_venda == venda.Id_venda).ToList();
                    _context.caixas.Update(cx);
                    _context.Recebimentos.RemoveRange(recVenda);
                    _context.Vendas.Remove(venda);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
