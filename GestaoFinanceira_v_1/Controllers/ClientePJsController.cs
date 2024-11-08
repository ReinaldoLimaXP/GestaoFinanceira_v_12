using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
namespace GestaoFinanceira_v_1.Controllers
{
    public class ClientePJsController
    {
        private readonly ApplicationDbContext _context;

        public ClientePJsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientePJ>> listarTodos()
        {
            try
            {
                return await _context.clientePJ.OrderBy(x=>x.RazaoSocial_PJ).ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Salvar(ClientePJ clientePJ)
        {
            try
            {
                _context.clientePJ.Add(clientePJ);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async void Atualizar(int id, ClientePJ clientePJ)
        {
            try
            {
                _context.clientePJ.Update(clientePJ);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ClientePJ> BuscarCNPJ(string? cnpj)
        {
            if (cnpj == null || _context.clientes == null)
            {
                throw new Exception("Id não permitido!");
            }

            var cliente = await _context.clientePJ
                .FirstOrDefaultAsync(m => m.CNPJ_PJ == cnpj);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado!");
            }

            return cliente;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ClientePJ> BuscarId(int? id)
        {
            if (id == null || _context.clientes == null)
            {
                throw new Exception("Id não permitido!");
            }

            var cliente = await _context.clientePJ
                .FirstOrDefaultAsync(m => m.Id_cliente == id);
            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado!");
            }

            return cliente;
        }

    }
}
