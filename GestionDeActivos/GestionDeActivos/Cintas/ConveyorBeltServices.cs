using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionDeActivos.Cintas
{
    class ConveyorBeltServices
    {
        public ObservableCollection<ConveyorBelt> Belts { get; }
        public ConveyorBeltServices() => Belts = new ObservableCollection<ConveyorBelt>();

        public void AddConveyorBelt(ConveyorBelt b)
        {
            if (Belts.Any(comp => comp.DesConveyorBelt == b.DesConveyorBelt))
            {
                MessageBox.Show($"{b.DesConveyorBelt} ya se encuentra en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();

                string query = "INSERT INTO Cintas (IdConveyorBelt, DesConveyorBelt, Certificte, TechnicalSheet)" +
                                " VALUES (@IdConveyorBelt, @DesConveyorBelt, @Certificte, @TechnicalSheet)";
                using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdConveyorBelt", b.IdConveyorBelt);
                    comando.Parameters.AddWithValue("@DesConveyorBelt", b.DesConveyorBelt);
                    comando.Parameters.AddWithValue("@Certificte", b.Certificte);
                    comando.Parameters.AddWithValue("@technicalSheet", b.TechnicalSheet);

                    comando.ExecuteNonQuery();
                }

                string lastInsertIdQuery = "SELECT last_insert_rowid()";
                using (SQLiteCommand comando = new SQLiteCommand(lastInsertIdQuery, conexion))
                {
                    b.IdConveyorBelt = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            Belts.Add(b);
        }

        public void UpdateConveyorBeltData(ConveyorBelt b)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "UPDATE Cintas SET Name = @DesConveyorBelt, DesConveyorBelt = @Certificte, Certificte = @TechnicalSheet" +
                                        " WHERE IdConveyorBelt = @IdConveyorBelt";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@DesConveyorBelt", b.DesConveyorBelt);
                        comando.Parameters.AddWithValue("@Certificte", b.Certificte);
                        comando.Parameters.AddWithValue("@TechnicalSheet", b.TechnicalSheet);
                        comando.Parameters.AddWithValue("@IdConveyorBelt", b.IdConveyorBelt);

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo actualizar la cinta con IdConveyorBelt = {b.IdConveyorBelt}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Cinta actualizada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la cinta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveConveryorBeltData(ConveyorBelt b)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "DELETE FROM Cintas WHERE IdConveyorBelt = @IdConveyorBelt";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdConveyorBelt", b.IdConveyorBelt);
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo eliminar la cinta con IdConveyorBelt = {b.IdConveyorBelt}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                Belts.Remove(b);
                MessageBox.Show("Cinta eliminada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la cinta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ConveyorBelt SearchByConveyorBeltDescription(string description)
                        => Belts.FirstOrDefault(b => b.DesConveyorBelt.Equals(description, StringComparison.OrdinalIgnoreCase));

    }
}
