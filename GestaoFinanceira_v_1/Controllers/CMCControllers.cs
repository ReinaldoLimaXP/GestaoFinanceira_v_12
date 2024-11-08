using GestaoFinanceira_v_1.Entidades;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira_v_1.Controllers
{
    /*Esse controller gernecia as cores, marcas e categorias*/
    public class CMCControllers
    {
        private readonly ApplicationDbContext _context;
        public CMCControllers(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<MarcaVeiculos> ListarMarcas()
        {
            try
            {
                return _context.marcaVeiculos.OrderBy(c => c.NomeMarca).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MarcaVeiculos>> ListarMarcasWait()
        {
            try
            {
                return await _context.marcaVeiculos.OrderBy(c => c.NomeMarca).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<Cores> ListarCores()
        {
            try
            {
                return  _context.cores.OrderBy(c => c.NomeCor).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Cores>> ListarCoresWait()
        {
            try
            {
                return await _context.cores.OrderBy(c => c.NomeCor).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public List<CategoriaVeiculos> ListarCategorias()
        {
            try
            {
                return _context.categoriaVeiculos.OrderBy(c => c.NomeCategoria).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<CategoriaVeiculos>> ListarCategoriaswait()
        {
            try
            {
                return await _context.categoriaVeiculos.OrderBy(c => c.NomeCategoria).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
