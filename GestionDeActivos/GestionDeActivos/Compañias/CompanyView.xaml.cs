using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using GestionDeActivos.Compañias;
using System.Windows.Media;

namespace GestionDeActivos.Compañias
{
    public partial class CompanyView : Window
    {
        public ObservableCollection<Company> Companies { get; set; }
        private CompanyService companyService;

        public CompanyView()
        {
            InitializeComponent();
            Companies = new ObservableCollection<Company>();
            compañiasDataGrid.ItemsSource = Companies;
            companyService = new CompanyService();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string connectionString = "Data Source = GestActiveDB.db";

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
                            companyService.AddCompany(company);
                        }
                    }
                }
            }
        }

        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void LimpiarButton_Click(object sender, RoutedEventArgs e)
        {
            nombreTextBox.IsReadOnly = false;
            nombreTextBox.ClearValue(BackgroundProperty);
            nombreTextBox.Text = "";

            tipoCompañiaTextBox.Text = "";

            telefonoTextBox.Text = "";
            telefonoTextBox.IsReadOnly = true;

            emailTextBox.Text = "";
            emailTextBox.IsReadOnly = true;

            grabarButton.IsEnabled = false;
            actualizarButton.IsEnabled = false;
            eliminarButton.IsEnabled = false;
            buscarButton.IsEnabled = true;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nombreTextBox.Text))
            {
                MessageBox.Show("El campo 'Nombre Compañia' no puede estar vacío.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Company compañia = companyService.SearchByCompanyName(nombreTextBox.Text);

            if (compañia == null)
            {
                MessageBoxResult result = MessageBox.Show($"La compañía con nombre '{nombreTextBox.Text}' no existe. ¿Deseas crear un nuevo registro?", "Compañía no encontrada",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    telefonoTextBox.IsReadOnly = false;
                    telefonoTextBox.ClearValue(BackgroundProperty); 
                    emailTextBox.IsReadOnly = false;
                    emailTextBox.ClearValue(BackgroundProperty); 
                    grabarButton.IsEnabled = true;
                }
                else
                {
                    LimpiarButton_Click(sender, e);
                }
            }
            else
            {
                nombreTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                nombreTextBox.IsReadOnly = true;
                tipoCompañiaTextBox.Text = compañia.Type.ToString();
                telefonoTextBox.Text = compañia.Telephone;
                telefonoTextBox.IsReadOnly = false;
                emailTextBox.Text = compañia.Email; 
                emailTextBox.IsReadOnly = false;
                buscarButton.IsEnabled = false;

                MessageBoxResult result = MessageBox.Show("¿Deseas eliminar o modificar el registro?", "Seleccionar acción",
                                                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    eliminarButton.IsEnabled = true;
                }
                else if (result == MessageBoxResult.No)
                    {
                    telefonoTextBox.ClearValue(BackgroundProperty);
                    emailTextBox.ClearValue(BackgroundProperty);
                    actualizarButton.IsEnabled = true;
                }
                else
                {
                    nombreTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                    nombreTextBox.IsReadOnly = true;
                    tipoCompañiaTextBox.Text = compañia.Type.ToString();
                    telefonoTextBox.Text = compañia.Telephone;
                    telefonoTextBox.IsReadOnly = false;
                    emailTextBox.Text = compañia.Email;
                    emailTextBox.IsReadOnly = false;
                    buscarButton.IsEnabled = false;
                }
            }
        }
    }
}