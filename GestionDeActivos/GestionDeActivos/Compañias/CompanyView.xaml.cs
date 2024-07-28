using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace GestionDeActivos.Compañias
{
    public partial class CompanyView : Window
    {
        public ObservableCollection<Company> Companies { get; set; }

        public CompanyView()
        {
            InitializeComponent();
            Companies = new ObservableCollection<Company>();
            compañiasDataGrid.ItemsSource = Companies;
            CargarDatos();
        }

        private void CargarDatos()
        {
            string connectionString = "Data Source=prueba.db";

            using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
            {
                conexion.Open();

                string query = "SELECT IdCompany, Name, TipoCompañia, Telefono, Email FROM Compañias";
                using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                {
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var company = new Company(
                                reader.GetString(1),
                                reader.GetString(3),
                                reader.GetString(4))
                            {
                                IdCompany = reader.GetInt32(0),
                                Type = (Company.TypeCompany)Enum.Parse(typeof(Company.TypeCompany), reader.GetString(2))
                            };
                            Companies.Add(company);
                        }
                    }
                }
            }
        }

        private Company BuscarCompañiaPorNombre()
        {
            string nombre = nombreTextBox.Text;
            var company = Companies.FirstOrDefault(c => c.Name.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (company == null)
            {
                MessageBox.Show($"La compañía con nombre '{nombre}' no existe.", "Compañía no encontrada", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return company;
        }


        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

