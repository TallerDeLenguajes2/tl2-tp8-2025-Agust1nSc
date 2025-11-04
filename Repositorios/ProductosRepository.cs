using Microsoft.Data.Sqlite;

public class ProductoRepository
{
    private string cadenaConexion = "Data Source=DB/Tienda.db;";

    public List<Productos> Listar()
    {
        var lista = new List<Productos>();

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos";

        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            lista.Add(new Productos
            {
                IdProducto = lector.GetInt32(0),
                Descripcion = lector.GetString(1),
                Precio = lector.GetInt32(2)
            });
        }

        return lista;
    }

    public Productos ObtenerPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            return new Productos
            {
                IdProducto = lector.GetInt32(0),
                Descripcion = lector.GetString(1),
                Precio = lector.GetInt32(2)
            };
        }

        return null;
    }

    public void Crear(Productos producto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
        comando.Parameters.AddWithValue("@precio", producto.Precio);

        comando.ExecuteNonQuery();
    }

    public bool Modificar(int id, Productos nuevoProducto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = "UPDATE Productos SET Descripcion=@desc, Precio=@precio WHERE IdProducto=@id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@desc", nuevoProducto.Descripcion);
        comando.Parameters.AddWithValue("@precio", nuevoProducto.Precio);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() > 0;
    }

   public bool Eliminar(int id)
{
    using var conexion = new SqliteConnection(cadenaConexion);
    conexion.Open();

   
    string sqlDetalle = @"DELETE FROM PresupuestosDetalle WHERE idProducto = @id";

    using (var cmdDetalle = new SqliteCommand(sqlDetalle, conexion))
    {
        cmdDetalle.Parameters.AddWithValue("@id", id);
        cmdDetalle.ExecuteNonQuery();
    }

    
    string sqlProducto = @"DELETE FROM Productos WHERE IdProducto = @id";

    using var cmdProducto = new SqliteCommand(sqlProducto, conexion);
    cmdProducto.Parameters.AddWithValue("@id", id);

    return cmdProducto.ExecuteNonQuery() > 0;
}
}
