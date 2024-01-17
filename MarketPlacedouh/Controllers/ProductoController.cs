using MarketPlacedouh.Models;
using MarketPlacedouh.Recursos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace MarketPlacedouh.Controllers
{
    [ApiController]
    [Route("producto")]
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public dynamic ListarProductos()
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@Estado", "1")
            };

            DataTable tCategoria = DBDatos.Listar("Categoria_Listar", parametros);
            DataTable tProducto = DBDatos.Listar("Producto_Listar");

            string jsonCategoria = JsonConvert.SerializeObject(tCategoria);
            string jsonProducto = JsonConvert.SerializeObject(tProducto);

            return new
            {
                success = true,
                message = "exito",
                result = new
                {
                    categoria = JsonConvert.DeserializeObject<List<Categoria>>(jsonCategoria),
                    producto = JsonConvert.DeserializeObject<List<Producto>>(jsonProducto),
                }
            };
        }

        [HttpPost]
        [Route("agregar")]
        public dynamic AgregarProducto(Producto producto)
        {

            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@IDCategoria",producto.IDCategoria ),
                new Parametro("@Nombre",producto.Nombre),
                new Parametro("@Precio",producto.Precio),
            };

            bool exito = DBDatos.Ejecutar("Producto_Agregar", parametros);

            return new
            {
                success = exito,
                message = exito ? "exito" :"Error al guardar asfdsad",
                result = ""
              
            };
        }
    }
}
