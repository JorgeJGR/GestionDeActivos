using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static GestionDeActivos.Activos.Active;


namespace GestionDeActivos.Activos
{
    /// <summary>
    /// Lógica de interacción para ActiveView.xaml
    /// </summary>
    public partial class ActiveView : Window
    {
        ObservableCollection<Active> Actives { get; set; }
        private ActiveService activeService;
        public ActiveView()
        {
            InitializeComponent();
            Actives = new ObservableCollection<Active>();
            activeService = new ActiveService();
            compañiasDataGrid.ItemsSource = Actives;
            lineaComboBox.ItemsSource = Enum.GetValues(typeof(Lines)).Cast<Lines>();
            zonaComboBox.ItemsSource = Enum.GetValues(typeof(Zones)).Cast<Zones>();
            CargarDatos();
        }

        private void CargarDatos()
        {
            Actives.Clear(); 

            string connectionString = "Data Source=GestActiveDB.db";

            using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
            {
                conexion.Open();

                string query = "SELECT * FROM Activos";
                using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                {
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {

                                var active = new Active(
                                    reader.GetInt32(0),         
                                    reader.GetString(1),        
                                    reader.GetString(2),        
                                    reader.GetString(3),        
                                    reader.GetString(4)         
                                );

                                Actives.Add(active);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error al cargar datos del activo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void LimpiarButton_Click(object sender, RoutedEventArgs e)
        {

            idActivoTextBox.IsReadOnly = false;
            idActivoTextBox.ClearValue(BackgroundProperty);
            idActivoTextBox.Text = "";

            descripcionTextBox.IsReadOnly = false;
            descripcionTextBox.ClearValue(BackgroundProperty);
            descripcionTextBox.Text = "";

            lineaComboBox.IsEnabled = true;
            lineaComboBox.SelectedIndex = -1;
            lineaTextBox.Text = "";

            zonaComboBox.IsEnabled = true;
            zonaComboBox.SelectedIndex = -1;
            zonaTextBox.Text = "";

            imagenTextBox.Text = "";
            imagenTextBox.IsReadOnly = true;

            activoImage.Source = null;

            grabarButton.IsEnabled = false;
            actualizarButton.IsEnabled = false;
            eliminarButton.IsEnabled = false;
            buscarButton.IsEnabled = true;
            abrirArchivoButton.IsEnabled = false;
        }

        private void LineaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lineaComboBox.SelectedItem != null)
            {
                Lines selectedLine = (Lines)lineaComboBox.SelectedItem;
                lineaTextBox.Text = selectedLine.ToString();  
            }
        }


        private void ZonaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lineaComboBox.SelectedItem != null)
            {
                Lines selectedLine = (Lines)lineaComboBox.SelectedItem;
                zonaTextBox.Text = selectedLine.ToString();
            }
        }

        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*";


            if (openFileDialog.ShowDialog() == true)
            {
                imagenTextBox.Text = openFileDialog.FileName;
                activoImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            string descripcion = descripcionTextBox.Text;
            string idActivoText = idActivoTextBox.Text;
            Active activoEncontrado = null;

            if (!string.IsNullOrEmpty(idActivoText) && int.TryParse(idActivoText, out int idActivo))
            {
                activoEncontrado = activeService.SearchByActiveidActive(idActivo);
            }
            else if (!string.IsNullOrEmpty(descripcion))
            {
                activoEncontrado = activeService.SearchByActiveDescription(descripcion);
            }

            if (activoEncontrado != null)
            {
                idActivoTextBox.Text = activoEncontrado.IdActive.ToString();
                descripcionTextBox.Text = activoEncontrado.Description;
                lineaTextBox.Text = activoEncontrado.Line;
                zonaTextBox.Text = activoEncontrado.Zone;
                imagenTextBox.Text = activoEncontrado.Image;

                string rutaAbsoluta = activoEncontrado.Image;

                buscarButton.IsEnabled = false;
                lineaComboBox.IsEnabled = false;
                zonaComboBox.IsEnabled = false;

                if (System.IO.File.Exists(rutaAbsoluta))
                {
                    activoImage.Source = new BitmapImage(new Uri(rutaAbsoluta, UriKind.Absolute));
                    MessageBox.Show("Activo encontrado y datos cargados exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"El archivo de imagen no se encontró en la ruta: {rutaAbsoluta}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No se encontró un activo con los criterios especificados.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
