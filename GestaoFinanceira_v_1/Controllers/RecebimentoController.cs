using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestaoFinanceira_v_1.Controllers
{
    public class RecebimentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecebimentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public List<Recebimento>  ListarTodos()
        {
            try
            {
                return _context.Recebimentos
                    .Include(x => x.CaixaRecebimento)
                    .ThenInclude(x=>x.Funcionario_Caixa)
                    .Include(x => x.Venda)
                    .Include(x => x.Venda.Cliente)
                    .Include(z => z.Plano)
                    .Include(x=>x.DispositivoRec).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<List<Recebimento>> ListarTodosAwait2()
        {
            try
            {
                List<Recebimento> lista = await _context.Recebimentos.Include(x => x.CaixaRecebimento).ThenInclude(x => x.Funcionario_Caixa).Include(x => x.Venda).Include(x => x.Venda.Cliente).Include(z => z.Plano).Include(x => x.DispositivoRec).ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<List<Recebimento>> ListarTodosAwait(int? id_empresa)
        {
            try
            {
                return await _context.Recebimentos
                    .Include(x => x.CaixaRecebimento).ThenInclude(x => x.Funcionario_Caixa)
                    .Include(x => x.Venda).ThenInclude(x => x.Cliente)
                    .Include(z => z.Plano)
                    .Include(x => x.DispositivoRec)
                    .Where(x=>x.Fk_id_empresa == id_empresa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<List<Recebimento>> ListarTodosCaixaAwait(long id_caixa)
        {
            try
            {
                return await _context.Recebimentos.Include(x => x.CaixaRecebimento).ThenInclude(x => x.Funcionario_Caixa).Include(x => x.Venda).ThenInclude(x => x.Cliente).Include(x => x.Venda.Caixa_venda).Include(z => z.Plano).Include(x => x.DispositivoRec).Where(c => c.CaixaRecebimento.Id_caixa == id_caixa).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public List<Recebimento> ListarAbertas()
        {
            try
            {
                return _context.Recebimentos.Include(x => x.Venda).Include(x => x.Venda.Cliente).Where(x => x.StatusRecebimento.ToLower() == "aberto").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Recebimento BuscarId(int? id)
        {
            if (id == null || _context.Recebimentos == null)
            {
                throw new Exception("Id não permitido!");
            }
            Recebimento? receb = _context.Recebimentos
                .Include(x => x.CaixaRecebimento).ThenInclude(x=>x.BancoCaixa)
                .Include(x => x.Venda)
                .Include(x=>x.DispositivoRec)
                .Include(x=>x.Plano)
                .FirstOrDefault(m => m.Id_recebimento == id);
            if (receb == null)
            {
                throw new Exception("Parcela não encontrada!");
            }
            return receb;
        }

        [ValidateAntiForgeryToken]
        public async Task ReceberAvulso(Recebimento recebimento)
        {
            if (recebimento == null)
            {
                throw new ArgumentNullException(nameof(recebimento));
            }
            IDbContextTransaction transaction = null;
            try
            {
                transaction = await _context.Database.BeginTransactionAsync();

                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var caixa = await _context.caixas
                    .FirstOrDefaultAsync(x => x.Id_caixa == recebimento.Fk_id_caixa);

                //Verificar se o caixa não é nulo
                if (caixa != null)
                {
                    caixa.Total_entradas += Math.Round(recebimento.ValorParcela, 2);
                    caixa.Saldo_final += Math.Round(recebimento.ValorParcela, 2);

                    _context.Add(caixa);
                    _context.Entry(caixa).State = EntityState.Modified;
                    recebimento.CaixaRecebimento = caixa;

                    //Esse if é para gerar a saída de encargo
                    if (double.Parse(recebimento.AliquotaParcela) > 0)
                    {
                        caixa.Total_saidas += Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        caixa.Saldo_final -= Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        Encargo e = new Encargo();
                        e.RecebimentoEncargo = recebimento;
                        e.Valor = double.Parse(recebimento.AliquotaParcela);
                        _context.Encargos.Add(e);
                    }
                    //Adicionar o caixa no contexto, pois ele está cadastrado no banco, mas fora do contexto
                   
                }
                else
                {
                    throw new Exception("Caixa não encontrado.");
                }
                
                _context.Recebimentos.Add(recebimento);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }

        }

        public async Task EstornoParcela(Recebimento recebimento)
        {
            if (recebimento == null)
            {
                throw new ArgumentNullException(nameof(recebimento));
            }
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                recebimento.StatusRecebimento = "Aberto";
                //transaction = _context.Database.BeginTransaction();
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var cx = await _context.caixas.FindAsync(recebimento.Fk_id_caixa);
                if (cx != null)
                {
                    cx.Total_entradas -= Math.Round(recebimento.ValorParcela, 2);
                    cx.Saldo_final -= Math.Round(recebimento.ValorParcela, 2);
                    _context.Add(cx);
                    _context.Entry(cx).State = EntityState.Modified;

                    recebimento.CaixaRecebimento = cx;
                    
                    
                    if (double.Parse(recebimento.AliquotaParcela) > 0)
                    {
                        cx.Total_saidas -= Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        cx.Saldo_final += Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        Encargo e = await _context.Encargos.FirstOrDefaultAsync(x => x.RecebimentoEncargo.Id_recebimento == recebimento.Id_recebimento);
                        _context.Encargos.Remove(e);
                        _context.Update(cx);
                    }
                   
                    if (recebimento.Venda is null)
                    {
                        recebimento.Venda = null;
                    }else if(recebimento.Venda.Id_venda == 0)
                    {
                        recebimento.Venda = null;
                    }
                    if (recebimento.DispositivoRec is null)
                    {
                        recebimento.DispositivoRec = null;
                    }
                    else if (recebimento.DispositivoRec.Id_dispositivo == 0)
                    {
                        recebimento.DispositivoRec = null;
                    }
                }
                else
                {
                    throw new Exception("Caixa não encontrado.");
                }
                _context.Recebimentos.Update(recebimento);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException ex)
            {
                // Captura a exceção e a exceção interna, se existir
                await transaction.RollbackAsync();
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                throw new ApplicationException("Erro ao salvar as alterações: " + innerExceptionMessage);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task ReceberParcela(Recebimento recebimento)
        {
            if (recebimento == null)
            {
                throw new ArgumentNullException(nameof(recebimento));
            }
            IDbContextTransaction transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();
                recebimento.StatusRecebimento = "Fechado";
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var cx =  await _context.caixas.FindAsync(recebimento.Fk_id_caixa);
                if(cx != null)
                {
                    cx.Total_entradas += Math.Round(recebimento.ValorParcela, 2);
                    cx.Saldo_final += Math.Round(recebimento.ValorParcela, 2);
                    _context.Add(cx);
                    _context.Entry(cx).State = EntityState.Modified;
                    recebimento.CaixaRecebimento = cx;
                    
                    if (recebimento.DispositivoRec is null)
                    {
                        recebimento.DispositivoRec = null;
                    }
                    else if (recebimento.DispositivoRec.Id_dispositivo == 0)
                    {
                        recebimento.DispositivoRec = null;
                    }

                    if (double.Parse(recebimento.AliquotaParcela) > 0)
                    {
                        cx.Total_saidas += Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        cx.Saldo_final -= Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        Encargo e = new Encargo();
                        e.RecebimentoEncargo = recebimento;
                        e.Valor = double.Parse(recebimento.AliquotaParcela);                        
                        _context.Encargos.Add(e);
                    }
                    //add o caixa no contexto, pois foi tirei o rastreamento dos caicas
                    //Objetivo de tirar o rastreamento é para pegar os valores atualizados
                    _context.Recebimentos.Update(recebimento);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                else
                {
                    throw new Exception("Caixa não encontrado.");
                }
            }
            catch (DbUpdateException ex)
            {
                // Captura a exceção e a exceção interna, se existir
                await transaction.RollbackAsync();
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                throw new ApplicationException("Erro ao salvar as alterações: " + innerExceptionMessage);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar a venda: " + ex.Message);
            }
        }

        public async Task ExcluirAvulto(Recebimento recebimento)
        {
            if (recebimento == null)
            {
                throw new ArgumentNullException(nameof(recebimento));
            }
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                //transaction = _context.Database.BeginTransaction();
                var entries = _context.ChangeTracker.Entries<Caixa>().ToList();
                foreach (var entry in entries)
                {
                    entry.State = EntityState.Detached;
                }
                var cx = await _context.caixas.FindAsync(recebimento.Fk_id_caixa);

                if(cx != null)
                {
                    cx.Total_entradas -= Math.Round(recebimento.ValorParcela, 2);
                    cx.Saldo_final -= Math.Round(recebimento.ValorParcela, 2);
                    _context.Add(cx);
                    _context.Entry(cx).State = EntityState.Modified;
                    if (double.Parse(recebimento.AliquotaParcela) > 0)
                    {
                        cx.Total_saidas -= Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        cx.Saldo_final += Math.Round(double.Parse(recebimento.AliquotaParcela), 2);
                        Encargo? e = await _context.Encargos.FirstOrDefaultAsync(x => x.RecebimentoEncargo.Id_recebimento == recebimento.Id_recebimento);
                        _context.Encargos.Remove(e);
                        _context.Update(cx);
                    }
                }
                else
                {
                    throw new Exception("Caixa não encontrado.");
                }
                _context.Recebimentos.Remove(recebimento);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync("Aqui "+ex.Message);
                await transaction.RollbackAsync(); // Uso correto de RollbackAsync para operações assíncronas
                throw new Exception("Erro ao cancelar o recebimento avulso: " + ex.InnerException.Message);
            }
        }
    }
}