using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionDeActivos.Activos
{
    class ActiveService
    {
        public ObservableCollection<Active> Actives { get; }
        public ActiveService() => Actives = new ObservableCollection<Active>();

        public void AddActive(Active a)
        {
            if (Actives.Any(comp => comp.Description == a.Description))
            {
                MessageBox.Show($"{a.Description} ya se encuentra en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();

                string query = "INSERT INTO Activos (IdActive, Description, Line, Zone, Image) VALUES (@IdActive, @Description, @Line, @Zone, @Image)";
                using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdActive", a.IdActive);
                    comando.Parameters.AddWithValue("@Description", a.Description);
                    comando.Parameters.AddWithValue("@Line", a.Line);
                    comando.Parameters.AddWithValue("@Zone", a.Zone);
                    comando.Parameters.AddWithValue("@Image", a.Image);

                    comando.ExecuteNonQuery();
                }

            }

            Actives.Add(a);
        }

        public void UpdateActiveData(Active a)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "UPDATE Activos SET Line = @Line, Zone = @Zone, Image = @Image WHERE IdActive = @IdActive";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        // Asignar valores a todos los parámetros utilizados en la consulta
                        comando.Parameters.AddWithValue("@Line", a.Line);
                        comando.Parameters.AddWithValue("@Zone", a.Zone);
                        comando.Parameters.AddWithValue("@Image", a.Image);
                        comando.Parameters.AddWithValue("@IdActive", a.IdActive); // Aquí es donde faltaba el parámetro

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo actualizar el activo {a.Description}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Activo actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el activo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void RemoveActiveData(Active a)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "DELETE FROM Activos WHERE IdActive = @IdActive";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdActive", a.IdActive);
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo eliminar el activo {a.Description}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                Actives.Remove(a);
                MessageBox.Show("Activo eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el activo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Active SearchByActiveDescription(string description)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "SELECT * FROM Activos WHERE Description = @Description";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Description", description);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Active(
                                    Convert.ToInt32(reader["IdActive"]),
                                    reader["Description"].ToString(),
                                    reader["Line"].ToString(),
                                    reader["Zone"].ToString(),
                                    reader["Image"].ToString()
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ActiveException($"Error al buscar por descripción: {ex.Message}");
            }
        }

        public Active SearchByActiveidActive(int idActive)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "SELECT * FROM Activos WHERE IdActive = @IdActive";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdActive", idActive);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Active(
                                    Convert.ToInt32(reader["IdActive"]),
                                    reader["Description"].ToString(),
                                    reader["Line"].ToString(),
                                    reader["Zone"].ToString(),
                                    reader["Image"].ToString()
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ActiveException($"Error al buscar por ID: {ex.Message}");
            }
        }

    }
}
