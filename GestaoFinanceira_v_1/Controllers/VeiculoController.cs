using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GestaoFinanceira_v_1.Controllers
{
    public class VeiculoController:Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Veiculo> listarVeiculos()
        {
            try
            {
                    return _context.veiculos.Include(u => u.Cor_vei).Include(j => j.Cliente).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<List<Veiculo>> listarVeiculosWait()
        {
            try
            {
                return await _context.veiculos
                    .Include(u => u.Cor_vei)
                    .Include(j => j.Cliente)
                    .Include(x => x.Marca_vei)
                    .Include(y => y.Categoria_vei)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task Salvar(Veiculo veiculo)
        {
            if (veiculo == null)
            {
                throw new ArgumentNullException(nameof(veiculo), "O veículo não pode ser nulo.");
            }

            try
            {
                _context.veiculos.Add(veiculo);
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
            
            if (_context.veiculos == null || id == null || id == 0)
            {
                throw new Exception("Veículo não localizado!");
            }
            try
            {
                Veiculo veiculo = _context.veiculos.Find(id);
                if (veiculo != null)
                {
                    _context.veiculos.Remove(veiculo);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Veículo não encontrado!");
                }
            }
            catch (DbUpdateException ex)
            {
                var mysqlException = ex.InnerException as MySqlException;

                // Verificar o número do erro de SQL
                if (mysqlException != null && mysqlException.Number == 1451)
                {
                    // 1451 é o código para violação de integridade referencial no MySQL
                    throw new ReferentialIntegrityException("Não é possível realizar a exclusão: O veículo é utlizado em outros procedimentos!");
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
        public Veiculo BuscarId(int? id)
        {
            try
            {
                
                    if (id == null || _context.veiculos == null)
                    {
                        throw new Exception("Id não permitido!");
                    }

                    Veiculo veiculo = _context.veiculos.FirstOrDefault(m => m.Id_veiculo == id);
                    if (veiculo == null)
                    {
                        throw new Exception("Veículo não encontrado!");
                    }

                    return veiculo;
                
            }catch(Exception ex)
            {
                throw new Exception("Erro ao buscar o veículo");
            }
        }


        public async Task<Veiculo> BuscarIdWait(int? id)
        {
            try
            {

                if (id == null || _context.veiculos == null)
                {
                    throw new Exception("Id não permitido!");
                }

                Veiculo? veiculo = await _context.veiculos
                    .Include(u => u.Cor_vei)
                    .Include(j => j.Cliente)
                    .Include(x=>x.Marca_vei)
                    .Include(y=>y.Categoria_vei)
                    .FirstOrDefaultAsync(m => m.Id_veiculo == id);
                if (veiculo == null)
                {
                    throw new Exception("Veículo não encontrado!");
                }

                return veiculo;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar o veículo");
            }
        }

        [HttpPost]
        public async Task AtualizarVeiculoAsync(Veiculo veiculo)
        {
            try
            {
                // Certifique-se de que o veículo está sendo rastreado pelo contexto
                _context.veiculos.Update(veiculo);

                // Salvar as alterações de forma assíncrona
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Lidar com exceção de concorrência
                var message = $"Erro de concorrência ao atualizar o veículo com Id {veiculo.Id_veiculo}: {ex.Message}";
                Console.WriteLine(message);

                // Detalhes adicionais, se disponíveis
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                // Recarregar a entidade atualizada, se necessário
                var entry = ex.Entries.Single();
                var clientValues = (Veiculo)entry.Entity;
                var databaseValues = await entry.GetDatabaseValuesAsync();

                if (databaseValues == null)
                {
                    throw new Exception("O registro foi excluído por outro usuário.");
                }
                else
                {
                    var databaseEntity = (Veiculo)databaseValues.ToObject();
                    // Atualizar a entidade com os valores mais recentes do banco de dados, se necessário
                    // e tentar salvar novamente
                }

                throw new Exception(message, ex);
            }
            catch (Exception ex)
            {
                // Lidar com outras exceções
                var message = $"Erro ao atualizar o veículo: {ex.Message}";
                Console.WriteLine(message);
                throw new Exception(message, ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Veiculo BuscarPlaca(string? placa)
        {
            try
            {
                
                    if (placa == null || _context.veiculos == null)
                    {
                        throw new Exception("Id não permitido!");
                    }

                    Veiculo veiculo = _context.veiculos.FirstOrDefault(m => m.Placa_vei == placa);
                    if (veiculo == null)
                    {
                        throw new Exception("Veículo não encontrado!");
                    }

                    return veiculo;
                
            }catch(Exception ex)
            {
                throw new Exception("Erro ao buscar o veículo!");
            }
            
        }
    }
}
