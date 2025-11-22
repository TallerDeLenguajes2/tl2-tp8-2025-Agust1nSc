using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using MVC.Interfaces;

public class PresupuestoRepository : IPresupuestoRepository
{
    private string cadenaConexion = "Data Source=DB/Tienda.db;";

    public void Crear(Presupuesto presupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)
                       VALUES (@nombre, @fecha)";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@nombre", presupuesto.NombreDestinatario);
        comando.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));
        comando.ExecuteNonQuery();
    }

    public List<Presupuesto> Listar()
    {
        var lista = new List<Presupuesto>();

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion 
                       FROM Presupuestos";

        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            lista.Add(new Presupuesto
            {
                IdPresupuesto = lector.GetInt32(0),
                NombreDestinatario = lector.GetString(1),
                FechaCreacion = DateTime.Parse(lector.GetString(2)),
                Detalle = ObtenerDetalle(lector.GetInt32(0))
            });
        }

        return lista;
    }

    public Presupuesto ObtenerPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion 
                       FROM Presupuestos WHERE idPresupuesto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var lector = comando.ExecuteReader();

        if (lector.Read())
        {
            return new Presupuesto
            {
                IdPresupuesto = lector.GetInt32(0),
                NombreDestinatario = lector.GetString(1),
                FechaCreacion = DateTime.Parse(lector.GetString(2)),
                Detalle = ObtenerDetalle(id)
            };
        }

        return null;
    }

    private List<PresupuestoDetalle> ObtenerDetalle(int idPresupuesto)
    {
        var lista = new List<PresupuestoDetalle>();

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"SELECT d.idProducto, d.Cantidad,
                              p.Descripcion, p.Precio
                       FROM PresupuestosDetalle d
                       JOIN Productos p ON d.idProducto = p.IdProducto
                       WHERE d.idPresupuesto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", idPresupuesto);

        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            lista.Add(new PresupuestoDetalle
            {
                Producto = new Productos
                {
                    IdProducto = lector.GetInt32(0),
                    Descripcion = lector.GetString(2),
                    Precio = lector.GetInt32(3)
                },
                Cantidad = lector.GetInt32(1)
            });
        }

        return lista;
    }



    public bool Eliminar(int id)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql =
        @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;
          DELETE FROM Presupuestos WHERE idPresupuesto = @id;";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() > 0;
    }

    public bool Modificar(int id, Presupuesto presupuestoEditado)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"UPDATE Presupuestos 
                       SET NombreDestinatario = @nombre,
                           FechaCreacion = @fecha
                       WHERE idPresupuesto = @id";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@nombre", presupuestoEditado.NombreDestinatario);
        comando.Parameters.AddWithValue("@fecha", presupuestoEditado.FechaCreacion.ToString("yyyy-MM-dd"));
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() > 0;
    }

    public bool AgregarProductoAPresupuesto(int idPresupuesto, Productos producto, int cantidad)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                   VALUES (@presupuestoId, @productoId, @cantidad)";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@presupuestoId", idPresupuesto);
        comando.Parameters.AddWithValue("@productoId", producto.IdProducto);
        comando.Parameters.AddWithValue("@cantidad", cantidad);

        return comando.ExecuteNonQuery() > 0;
    }
}
