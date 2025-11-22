using System.Collections.Generic;
using System.Data.SQLite; // Asegúrate de tener instalado el paquete System.Data.SQLite
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using MVC.Interfaces;
using MVC.Models; // Aquí está tu clase Productos

namespace MVC.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string cadenaConexion;

        public ProductoRepository(IConfiguration configuration)
        {
            
            cadenaConexion = "Data Source=DB/Tienda.db";
        }

        public List<Productos> Listar()
        {
            var lista = new List<Productos>();

            using (var connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "SELECT IdProducto, Descripcion, Precio FROM Productos;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Productos();
                            producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                            producto.Descripcion = reader["Descripcion"].ToString();
                            producto.Precio = Convert.ToDecimal(reader["Precio"]);

                            lista.Add(producto);
                        }
                    }
                }
            }
            return lista;
        }

        public Productos ObtenerPorId(int id)
        {
            Productos producto = null;

            using (var connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @IdProducto;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Productos();
                            producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);
                            producto.Descripcion = reader["Descripcion"].ToString();
                            producto.Precio = Convert.ToDecimal(reader["Precio"]);
                        }
                    }
                }
            }
            return producto;
        }

        public void Crear(Productos producto)
        {
            using (var connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                // No insertamos IdProducto porque asumimos que es AUTOINCREMENT en la BD
                string query = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Modificar(int id, Productos nuevoProducto)
        {
            int filasAfectadas = 0;
            using (var connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE IdProducto = @IdProducto;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripcion", nuevoProducto.Descripcion);
                    command.Parameters.AddWithValue("@Precio", nuevoProducto.Precio);
                    command.Parameters.AddWithValue("@IdProducto", id);

                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            // Devuelve true si se modificó al menos una fila
            return filasAfectadas > 0;
        }

        public bool Eliminar(int id)
        {
            int filasAfectadas = 0;
            using (var connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "DELETE FROM Productos WHERE IdProducto = @IdProducto;";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", id);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            // Devuelve true si se eliminó al menos una fila
            return filasAfectadas > 0;
        }
    }
}