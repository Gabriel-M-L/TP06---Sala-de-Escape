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
            string query = "SELECT NombreUsuario FROM Usuario WHERE NombreUsuario = @usuario AND EstadoPartida != 'Finalizada'";
            result = connection.QueryFirstOrDefault<string>(query, new { usuario });
        }
        return result != null;
    }
    public bool BuscarUsuario(string usuario)
    {
        string result;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuario WHERE NombreUsuario = @usuario";
            result = connection.QueryFirstOrDefault<string>(query, new { usuario });
        }
        return result != null;
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
            string query = "SELECT NivelActual FROM Usuario WHERE NombreUsuario = @usuario";
            nivelActual = connection.QueryFirstOrDefault<int>(query, new { usuario });
        }
        return nivelActual;
    }

    public void CambiarNivel(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuario SET NivelActual = NivelActual + 1 WHERE NombreUsuario = @usuario AND NivelActual < 4";
            connection.Execute(query, new { usuario });
            Console.WriteLine($"Nivel cambiado para el usuario: {usuario}");
        }
    }

    public void cambiarPlata(string usuario, int plata)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuario SET Plata = Plata + @plata WHERE NombreUsuario = @usuario";
            connection.Execute(query, new { usuario, plata });
        }
    }

    public int ObtenerPlata(string usuario)
    {
        int plata;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Plata FROM Usuario WHERE NombreUsuario = @usuario";
            plata = connection.QueryFirstOrDefault<int>(query, new { usuario });
        }
        return plata;
    }

    public Comodin ObtenerComodin(string usuario)
    {
        Comodin comodin = new Comodin();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            
            string query = "SELECT c.Id, c.Descripcion, c.Nombre, c.Imagen, c.Precio FROM Usuario u JOIN Comodines c ON u.ComodinActual = c.Id WHERE u.NombreUsuario = @usuario";
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

    public Nivel ObtenerDatosNivel(int numeroNivel, int id)
    {
        Nivel nivel = new Nivel();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string tableName = $"Nivel{numeroNivel}";
            string query = $"SELECT Id, Desafio, Respuesta, Pista FROM {tableName} WHERE Id = @id";
            nivel = connection.QueryFirstOrDefault<Nivel>(query, new { id });
        }
        return nivel;
    }

    public List<Comodin> ObtenerComodinAleatorio()
    {
        List<Comodin> comodin = new List<Comodin>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT TOP 2 Id, Descripcion, Nombre, Imagen, Precio FROM Comodines ORDER BY NEWID()";
            comodin = connection.Query<Comodin>(query).ToList();
        }
        return comodin;
    }

    public void CambiarComodin(string usuario, int idComodin)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuario SET ComodinActual = @idComodin WHERE NombreUsuario = @usuario";
            connection.Execute(query, new { usuario, idComodin });
        }
    }

    public Comodin ObtenerComodinPorId(int id)
    {
        Comodin comodin = new Comodin();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Id, Descripcion, Nombre, Imagen, Precio FROM Comodines WHERE Id = @id";
            comodin = connection.QueryFirstOrDefault<Comodin>(query, new { id });
        }
        return comodin;
    }

    public string ObtenerPregunta(int id)
    {
        string pregunta;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Pregunta FROM Preguntas WHERE Id = @id";
            pregunta = connection.QueryFirstOrDefault<string>(query, new { id });
        }
        return pregunta;
    }

    public void FinalizarPartida(string usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Usuario SET EstadoPartida = 'Finalizada' WHERE NombreUsuario = @usuario";
            connection.Execute(query, new { usuario });
        }
    }
}