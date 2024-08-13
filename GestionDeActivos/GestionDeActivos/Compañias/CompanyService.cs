using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionDeActivos.Compañias
{
    class CompanyService
    {
        public ObservableCollection<Company> Companies { get; }
        public CompanyService() => Companies = new ObservableCollection<Company>();

        public void AddCompany(Company c)
        {
            if (Companies.Any(comp => comp.Name == c.Name))
            {
                MessageBox.Show($"{c.Name} ya se encuentra en la lista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();

                string query = "INSERT INTO Compañias (Name, TipoCompañia, Telephone, Email) VALUES (@Name, @TipoCompañia, @Telephone, @Email)";
                using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Name", c.Name);
                    comando.Parameters.AddWithValue("@TipoCompañia", c.Type.ToString());
                    comando.Parameters.AddWithValue("@Telephone", c.Telephone);
                    comando.Parameters.AddWithValue("@Email", c.Email);

                    comando.ExecuteNonQuery();
                }

                string lastInsertIdQuery = "SELECT last_insert_rowid()";
                using (SQLiteCommand comando = new SQLiteCommand(lastInsertIdQuery, conexion))
                {
                    c.IdCompany = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            Companies.Add(c);
        }
        public void UpdateCompanyData(Company c)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "UPDATE Compañias SET Name = @Name, Telephone = @Telephone, Email = @Email WHERE IdCompany = @IdCompany";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Name", c.Name);
                        comando.Parameters.AddWithValue("@Telephone", c.Telephone);
                        comando.Parameters.AddWithValue("@Email", c.Email);
                        comando.Parameters.AddWithValue("@IdCompany", c.IdCompany);

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo actualizar la compañía con IdCompany = {c.IdCompany}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("Compañía actualizada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la compañía: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveCompanyData(Company c)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "DELETE FROM Compañias WHERE IdCompany = @IdCompany";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdCompany", c.IdCompany);
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo eliminar la compañía con IdCompany = {c.IdCompany}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                Companies.Remove(c);
                MessageBox.Show("Compañía eliminada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la compañía: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Company SearchByCompanyName(string name)
        {
            return Companies.FirstOrDefault(comp => comp.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}