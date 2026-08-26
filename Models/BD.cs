namespace TP06_Martinez_Loufer.Models;
using Dapper;
using Microsoft.Data.SqlClient;
public class BD
{
    private string _connectionString = "Server=localhost;Database= BD ;Trusted_Connection=True;TrustServerCertificate=True;";

    public bool RecuperarUsuario(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario AND EstadoPartida != 'Finalizada'";
            string result = connection.QueryFirstOrDefault(query, new { usuario });
            return result == null;
        }
    }
    public bool BuscarUsuario(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario";
            string result = connection.QueryFirstOrDefault(query, new { usuario });
            return result == null;
        }
    }
    public void GuardarUsuario(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Usuarios (NombreUsuario, EstadoPartida, Plata, Comodin) VALUES (@usuario, 'En curso', 0, NULL)";
            connection.Execute(query, new { usuario });
        }
    }
    
    public int ObtenerNivel(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NivelActual FROM Usuarios WHERE NombreUsuario = @usuario";
            int nivelActual = connection.QueryFirstOrDefault<int>(query, new { usuario });
            return nivelActual;
        }
    }

    public void CambiarNivel(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuarios SET NivelActual = NivelActual + 1 WHERE NombreUsuario = @usuario";
            connection.Execute(query, new { usuario });
        }
    }

    public void cambiarPlata(string usuario, int plata)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuarios SET Plata = Plata + @plata WHERE NombreUsuario = @usuario";
            connection.Execute(query, new { usuario, plata });
        }
    }

    public int ObtenerPlata(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Plata FROM Usuarios WHERE NombreUsuario = @usuario";
            int plata = connection.QueryFirstOrDefault<int>(query, new { usuario });
            return plata;
        }
    }

    public Comodin ObtenerComodin(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT c.Id, c.Descripcion, c.Nombre, c.Precio FROM Usuarios u JOIN Comodines c ON u.Comodin = c.Nombre WHERE u.NombreUsuario = @usuario";
            Comodin comodin = connection.QueryFirstOrDefault<Comodin>(query, new { usuario });
            return comodin;
        }
    }
}