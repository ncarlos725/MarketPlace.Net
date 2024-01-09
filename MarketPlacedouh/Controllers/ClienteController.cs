// Controller para iniciar sesion 
using MarketPlacedouh.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MarketPlacedouh.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase
    {
        // Listar Clientes 
        [HttpGet]
        [Route("listar")]
        public dynamic ListarClientes()
        {
            List<Cliente> clientes = new List<Cliente>
                {
                    new Cliente
                    {
                        id = "1",
                        correo = "google@gmail.com",
                        edad = "19",
                        nombre = "Bernardo Peña"
                    },
                    new Cliente
                    {
                        id = "2",
                        correo = "miguelgoogle@gmail.com",
                        edad = "23",
                        nombre = "Miguel Mantilla"
                    }
                };

            return clientes;
        }

        // Listar Cliente x ID 
        [HttpGet]
        [Route("listarxid")]
        public dynamic ListarClientesxid(string _id)
        {
            List<Cliente> clientes = new List<Cliente>
                {

                    new Cliente
                    {
                         id = "1",
                        correo = "google@gmail.com",
                        edad = "19",
                        nombre = "Bernardo Peña"
                    }
                };

            return clientes;
        }

        
        [HttpPost]
        [Route("eliminar")]
        public dynamic eliminarCliente(Cliente cliente)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity; // y de ahí lo convertimos en un Claims                  
            var rtoken = Jwt.validarToken(identity); // escribi mal la clase Jwt... y le mande Jtw... 
            if (!rtoken.success)
            {
                // en caso el token.succes es igual a false va retonar lo que diga aqui el token
                return rtoken;
                             
            }
            // de validarToken no avanza ... 
            // a lo mejor no recibe nunca el token
                //cargamos una nueva variable usuario 
            Usuario usuario = rtoken.result; // en este variarable almacenamos el valor que esta en rtoken.result ya que ahi guardamos el tipo de usuario
            if (usuario.rol != "administrador")// si el usuario.rol no es administrador no tiene permisos
            {
                return new
                {
                    success = false,
                    message = "no tienes permisos para eliminar clientes",
                    result = ""
                };
            }
            // caso SEA ADMINISTRADOR contrario lo elimina
            return new
            {
                success = true,
                message = "cliente eliminado",
                result = cliente
            };
        } // fin eliminar cliente
    }

} 
