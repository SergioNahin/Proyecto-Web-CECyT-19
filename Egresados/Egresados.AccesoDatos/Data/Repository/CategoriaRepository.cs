using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egresados.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Egresados.AccesoDatos.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoriaRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetSelectCategoria()
        {
            return _db.Categoria.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            }
            );
        }
        public void Update(Categoria categoria)
        {
            var objDesdeDb = _db.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
            objDesdeDb.Nombre = categoria.Nombre;
            objDesdeDb.Orden = categoria.Orden;

            _db.SaveChanges();

        }
    }
}
