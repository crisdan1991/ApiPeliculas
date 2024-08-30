using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private string claveSecreta;
        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:SecretKey");
        }

        public Usuario GetUsuario(int UsuarioId)
        {
            return _bd.Usuario.FirstOrDefault(c => c.Id == UsuarioId);
        }

        public ICollection<Usuario> GetUsuario()
        {
            return _bd.Usuario.OrderBy(c => c.NombreUsuario).ToList();
        }

        public bool IsUniqueUsuario(string nombre)
        {
            var usuario = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario.ToLower().Trim() == nombre.ToLower().Trim());
            return usuario == null ? true : false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncryp = obtenermd5(usuarioLoginDto.Password);
            var usuario = await _bd.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario.ToLower().Trim() == usuarioLoginDto.NombreUsuario.ToLower().Trim()
                && u.Password == passwordEncryp
            );
            //validamos si el usuario no existe con la combinacion de usuario y contraseña correcta
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Usuario = null,
                    Token = ""
                };
            }
            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                        new Claim(ClaimTypes.Role, usuario.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            var token = manejadoToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuesta = new UsuarioLoginRespuestaDto()
            {
                Usuario = usuario,
                Token = manejadoToken.WriteToken(token)
            };
            return usuarioLoginRespuesta;
            


        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncryp = obtenermd5( usuarioRegistroDto.Password);
            var usuario = new Usuario
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncryp,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role
            };
            await _bd.Usuario.AddAsync(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = passwordEncryp;
            return usuario;
        }


        // Metodo  para encriptar la contraseña con MD5 se usa tanto en el registro como en el login
        public static string obtenermd5(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            //byte[] hash = md5.ComputeHash(inputBytes);
            inputBytes = md5.ComputeHash(inputBytes);
            //StringBuilder sb = new StringBuilder();
            string resp = "";
            for (int i = 0; i < inputBytes.Length; i++)
                resp += inputBytes[i].ToString("X2").ToLower();
            return resp.ToString();
        }

    } 
}
