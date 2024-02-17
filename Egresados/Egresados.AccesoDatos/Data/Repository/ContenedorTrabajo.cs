using Egresados.AccesoDatos.Data.Repository.IRepository;
using Egresados.Data;
using Egresados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egresados.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;
        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Articulo = new ArticuloRepository(_db);
            Slider =  new SliderRepository(_db);
            Usuario = new UsuarioRepository(_db);
        }
        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }
        public ISliderRepository Slider { get; private set; }
        public IUsuarioRepository Usuario { get; private set; }

        public void Dispose()
        {
            _db.Dispose(); //Metodo libera memoria en deshuso
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
