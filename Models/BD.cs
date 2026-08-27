namespace TP06_Martinez_Loufer.Models;
using Dapper;
using Microsoft.Data.SqlClient;
public class BD
{
    private string _connectionString = "Server=localhost;Database= BD ;Trusted_Connection=True;TrustServerCertificate=True;";

    public bool RecuperarUsuario(string usuario)
    {
        string result;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario AND EstadoPartida != 'Finalizada'";
            result = connection.QueryFirstOrDefault(query, new { usuario });
        }
        return result == null;
    }
    public bool BuscarUsuario(string usuario)
    {
        string result;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuarios WHERE NombreUsuario = @usuario";
            result = connection.QueryFirstOrDefault(query, new { usuario });
        }
        return result == null;
    }
    public void GuardarUsuario(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Usuario (NombreUsuario, EstadoPartida, ComodinActual, NivelActual, Plata) VALUES (@usuario, 'En curso', NULL, 1, 0)";
            connection.Execute(query, new { usuario });
        }
    }
    
    public int ObtenerNivel(string usuario)
    {
        int nivelActual;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NivelActual FROM Usuarios WHERE NombreUsuario = @usuario";
            nivelActual = connection.QueryFirstOrDefault<int>(query, new { usuario });
        }
        return nivelActual;
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
        int plata;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Plata FROM Usuarios WHERE NombreUsuario = @usuario";
            plata = connection.QueryFirstOrDefault<int>(query, new { usuario });
        }
        return plata;
    }

    public Comodin ObtenerComodin(string usuario)
    {
        Comodin comodin = new Comodin();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            
            string query = "SELECT c.Id, c.Descripcion, c.Nombre, c.Precio FROM Usuarios u JOIN Comodines c ON u.Comodin = c.Nombre WHERE u.NombreUsuario = @usuario";
            comodin = connection.QueryFirstOrDefault<Comodin>(query, new { usuario }); 
        }
        return comodin;
    }

    public string ObtenerIdsNivelesAleatorios(int nivel)
    {
        List<int> ids = new List<int>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string tableName = $"Nivel{nivel}";
            string query = $"SELECT Id FROM {tableName} ORDER BY NEWID()";
            ids = connection.Query<int>(query).ToList();
        }
        return string.Join(",", ids);
    }

    public Nivel ObtenerDatosNivel(int id)
    {
        Nivel nivel = new Nivel();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Desafio, Respuesta, Pista FROM Nivel WHERE Id = @id";
            nivel = connection.QueryFirstOrDefault<Nivel>(query, new { id });
        }
        return nivel;
    }
}