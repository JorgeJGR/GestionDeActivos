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
            Companies.Clear();

            string connectionString = "Data Source = GestActiveDB.db";

            using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
            {
                conexion.Open();

                string query = "SELECT IdCompany, Name, TipoCompañia, Telephone, Email FROM Compañias";
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
                            companyService.Companies.Add(company);
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

        private void GrabarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Company newCompany = new Company(nombreTextBox.Text, telefonoTextBox.Text, emailTextBox.Text);
                companyService.AddCompany(newCompany);
                MessageBox.Show("Compañía guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarButton_Click(sender, e);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = nombreTextBox.Text;

                Company companyToRemove = companyService.Companies
                    .FirstOrDefault(c => c.Name.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                if (companyToRemove != null)
                {
                    companyService.RemoveCompanyData(companyToRemove);
                }

                LimpiarButton_Click(sender, e);
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = nombreTextBox.Text;
                string telefono = telefonoTextBox.Text;
                string email = emailTextBox.Text;

                Company companyToUpdate = companyService.Companies
                    .FirstOrDefault(c => c.Name.Equals(nombre, StringComparison.OrdinalIgnoreCase));

                if (companyToUpdate != null)
                {
                    companyToUpdate.Telephone = telefono;
                    companyToUpdate.Email = email;

                    companyService.UpdateCompanyData(companyToUpdate);

                    LimpiarButton_Click(sender, e);
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna compañía con ese nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}