using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MarketPlacedouh.Models
{
    public class Usuario
    {
        public string idUsuario { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string rol { get; set; }

        public string HashContraseña { get; set; }

        public void GenerarHashContraseña(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                HashContraseña = builder.ToString();
            }
        }

        public bool VerificarContraseña(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return HashContraseña == builder.ToString();
            }
        }

        public static List<Usuario> DB()
        {
            List<Usuario> usuarios = new List<Usuario>
            {
                new Usuario
                {
                    idUsuario = "1",
                    usuario = "Mateo",
                    password = "123.",
                    rol = "empleado"
                },
                new Usuario
                {
                    idUsuario = "2",
                    usuario = "Marcos",
                    password = "123.",
                    rol = "empleado"
                },
                new Usuario
                {
                    idUsuario = "3",
                    usuario = "Lucas",
                    password = "123.",
                    rol = "empleado",
                },
                new Usuario
                {
                    idUsuario = "4",
                    usuario = "Juan",
                    password = "123.",
                    rol = "administrador"
                }
            };

            // Generar el hash de las contraseñas
            foreach (var usuario in usuarios)
            {
                usuario.GenerarHashContraseña(usuario.password);
            }

            return usuarios;
        }
    }
}
