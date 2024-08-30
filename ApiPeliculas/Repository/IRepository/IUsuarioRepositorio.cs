using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuario();
        Usuario GetUsuario(int usuarioId);
        bool IsUniqueUsuario(string nombre);
    
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
