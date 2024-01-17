using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MarketPlacedouh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace NetCoreYouTube.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IConfiguration configuration, ILogger<UsuarioController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] Object optData)
        {
            try
            {
                if (optData == null)
                {
                    _logger.LogError("Datos de inicio de sesión nulos. - {DateTime.UtcNow}");
                    return new
                    {
                        success = false,
                        message = "Datos de inicio de sesión nulos",
                        result = ""
                    };
                }

                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                string user = data?.usuario?.ToString();
                string password = data?.password?.ToString();

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                    _logger.LogError("Nombre de usuario o contraseña nulos o vacíos.");
                    return new
                    {
                        success = false,
                        message = "Nombre de usuario o contraseña nulos o vacíos",
                        result = ""
                    };
                }

                Usuario usuario = Usuario.DB().FirstOrDefault(x => x.usuario == user);

                if (usuario != null)
                {
                    // Antes de la comparación de contraseñas
                    _logger.LogInformation($"Comparando contraseñas para el usuario: {user} - {DateTime.UtcNow}");

                    if (usuario.VerificarContraseña(password))
                    {
                        // Después de la comparación de contraseñas exitosa
                        _logger.LogInformation("Comparación de contraseñas exitosa.");

                        // Resto del código...
                        var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("id", usuario.idUsuario),
                            new Claim("usuario", usuario.usuario)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                        var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: singIn
                        );

                        _logger.LogInformation("Inicio de sesión exitoso.");
                        return new
                        {
                            success = true,
                            message = "Inicio de sesión exitoso",
                            result = new JwtSecurityTokenHandler().WriteToken(token)
                        };
                    }
                    else
                    {
                        // Log si la comparación de contraseñas falla
                        _logger.LogError("La comparación de contraseñas ha fallado. ");
                    }
                }

                _logger.LogError("Credenciales incorrectas.");
                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión.");
                return new
                {
                    success = false,
                    message = "Error durante el inicio de sesión",
                    result = ""
                };
            }
        }
    }
}
