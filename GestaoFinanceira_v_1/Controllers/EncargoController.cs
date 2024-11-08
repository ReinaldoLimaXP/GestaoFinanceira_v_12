using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    public class EncargoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncargoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Salvar(Encargo encargo)
        {
            try
            {
                _context.Encargos.Add(encargo);
                _context.SaveChangesAsync();
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
            if (_context.Encargos == null || id == null || id == 0)
            {
                throw new Exception("Encargo não localizado!");
            }
            try
            {
                Encargo e = _context.Encargos.Find(id);
                if (e != null)
                {
                    _context.Encargos.Remove(e);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Encargo não encontrado!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Encargo BuscarIdRecebimento(int? id_recebimento)
        {
            if (id_recebimento == null || _context.Encargos == null)
            {
                throw new Exception("Id não permitido!");
            }

            Encargo desp = _context.Encargos.Include(x=>x.RecebimentoEncargo).FirstOrDefault(m => m.RecebimentoEncargo.Id_recebimento == id_recebimento);
            if (desp == null)
            {
                throw new Exception("Despesa não encontrado!");
            }

            return desp;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<List<Encargo>> EncagosCaixa(long? id_caixa)
        {
            if (id_caixa == null || _context.Encargos == null)
            {
                throw new Exception("Id não permitido!");
            }

            List<Encargo> desp = _context.Encargos.Include(x => x.RecebimentoEncargo).ThenInclude(x=> x.CaixaRecebimento).Where(m => m.RecebimentoEncargo.CaixaRecebimento.Id_caixa == id_caixa).ToList();
            if (desp == null)
            {
                throw new Exception("Despesa não encontrado!");
            }

            return desp;
        }
    }
}
