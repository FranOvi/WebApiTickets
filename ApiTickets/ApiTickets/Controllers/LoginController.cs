using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Text;
using ConectarDatos;

namespace ApiTickets.Controllers
{
    public class LoginController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();
        // POST: api/Login
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(usuario usuarioLogin)
        {
            if (usuarioLogin == null)  return BadRequest("Usuario y Contraseña requeridos.");

            var _userInfo = AutenticarUsuario(usuarioLogin.email, usuarioLogin.contrasena);
            if (_userInfo != null)
            {
                return Ok(new { token = GenerarTokenJWT(_userInfo) });
            }
            else
            {
                return Unauthorized();
            }
        }

        // COMPROBAMOS SI EL USUARIO EXISTE EN LA BASE DE DATOS 
        private usuario AutenticarUsuario(string email, string password)
        {
            var usr = (from u in dbContext.usuario
                            where u.email == email
                            && u.contrasena == password
                       select u).FirstOrDefault();

            if (usr == null)
            {
                return null;
            }
            else
            {
                return usr;
            }
        }

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(usuario usuarioInfo)
        {
            // RECUPERAMOS LAS VARIABLES DE CONFIGURACIÓN
            var _ClaveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
            var _Issuer = ConfigurationManager.AppSettings["Issuer"];
            var _Audience = ConfigurationManager.AppSettings["Audience"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["Expires"], out int _Expires))
                _Expires = 24;


            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_ClaveSecreta));
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.usuario_id.ToString()),
                new Claim("nombres", usuarioInfo.nombres),
                new Claim("apellidos", usuarioInfo.apellidos),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.email)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _Issuer,
                    audience: _Audience,
                    claims: _Claims,
                    notBefore: DateTime.Now,
                    // Exipra a la 24 horas.
                    expires: DateTime.Now.AddHours(_Expires)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}