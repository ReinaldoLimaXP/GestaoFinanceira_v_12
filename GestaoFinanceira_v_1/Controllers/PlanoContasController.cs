
using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{ 
    public class PlanoContasController
    {
        private readonly ApplicationDbContext _context;
        public PlanoContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<PlanoDeContas> ListarPlanos()
        {
            try
            {
                return  _context.PlanoContas.OrderBy(x=>x.Codigo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<List<PlanoDeContas>> ListarPlanosawait()
        {
            try
            {
                return await _context.PlanoContas.OrderBy(x => x.Codigo).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Salvar(PlanoDeContas plano)
        {
            try
            {
                _context.PlanoContas.Add(plano);
                _context.SaveChanges();
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
                throw new Exception("Plano de conta não localizado!");
            }
            try
            {
                PlanoDeContas plano = _context.PlanoContas.Find(id);
                if (plano != null)
                {
                    _context.PlanoContas.Remove(plano);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Plano de conta não encontrado!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public PlanoDeContas BuscarId(int? id)
		{
			if (id == null || _context.PlanoContas == null)
			{
				throw new Exception("Id não permitido!");
			}

			PlanoDeContas plano = _context.PlanoContas.FirstOrDefault(m => m.Id_planoConta == id);
			if (plano == null)
			{
				throw new Exception("Veículo não encontrado!");
			}

			return plano;
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PlanoDeContas BuscarDescricai(string? id)
        {
            if (id == null || _context.PlanoContas == null)
            {
                throw new Exception("Descrição não permitido!");
            }

            PlanoDeContas plano = _context.PlanoContas.FirstOrDefault(m => m.Descricao.ToLower().Contains(id.ToLower()));
            if (plano == null)
            {
                throw new Exception("Veículo não encontrado!");
            }

            return plano;
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public void Atualizar(PlanoDeContas plano)
		{
			try
			{
				_context.PlanoContas.Update(plano);
				_context.SaveChanges();
			}
			catch (DbUpdateConcurrencyException ex)
			{
				throw new Exception(ex.Message);
			}

		}
	}
}
