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
            activosDataGrid.ItemsSource = Actives;
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
            if (zonaComboBox.SelectedItem != null)
            {
                Zones selectedZone = (Zones)zonaComboBox.SelectedItem;
                zonaTextBox.Text = selectedZone.ToString();
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

                MessageBoxResult result = MessageBox.Show("¿Deseas eliminar o modificar el registro?", "Seleccionar acción",
                                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    buscarButton.IsEnabled = false;
                    eliminarButton.IsEnabled = true;
                }

                else if (result == MessageBoxResult.No)
                {
                    idActivoTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                    idActivoTextBox.IsReadOnly = true;

                    descripcionTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                    descripcionTextBox.IsReadOnly = true;

                    lineaComboBox.IsEnabled = true;
                    lineaComboBox.SelectedItem = -1;

                    zonaComboBox.IsEnabled = true;
                    zonaComboBox.SelectedItem = -1;

                    actualizarButton.IsEnabled = true;
                    abrirArchivoButton.IsEnabled = true;
                }
                else
                {
                    idActivoTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                    idActivoTextBox.IsReadOnly = true;

                    descripcionTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                    descripcionTextBox.IsReadOnly = true;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"El activo no existe. ¿Deseas crear un nuevo registro?", "Activo no encontrada",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    lineaComboBox.IsEnabled = true;
                    lineaComboBox.SelectedItem = -1;

                    zonaComboBox.IsEnabled = true;
                    zonaComboBox.SelectedItem = -1;

                    buscarButton.IsEnabled = false;
                    grabarButton.IsEnabled = true;
                    abrirArchivoButton.IsEnabled = true;
                }
                else
                {
                    LimpiarButton_Click(sender, e);
                }
            }
        }

        private void GrabarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Active activo = new Active()
                {
                    IdActive = int.Parse(idActivoTextBox.Text.Trim()),
                    Description = descripcionTextBox.Text.Trim(),
                    Zone = zonaTextBox.Text.Trim(),
                    Line = lineaTextBox.Text.Trim(),
                    Image = imagenTextBox.Text.Trim()
                };

                activeService.AddActive(activo);

                MessageBox.Show("Activo guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarDatos();
                LimpiarButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la persona: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int codigo = int.Parse(idActivoTextBox.Text.Trim());

                Active activoToRemove = Actives.FirstOrDefault(a => a.IdActive == codigo);

                MessageBox.Show(activoToRemove.IdActive.ToString());

                if (activoToRemove != null)
                {
                    activeService.RemoveActiveData(activoToRemove);
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

                int codigo = int.Parse(idActivoTextBox.Text.Trim());
                string linea = lineaTextBox.Text;
                string zona = zonaTextBox.Text;
                string imagen = imagenTextBox.Text;

                Active activeToUpdate = Actives.FirstOrDefault(a => a.IdActive == codigo);

                if (activeToUpdate != null)
                {
                    activeToUpdate.Line = linea;
                    activeToUpdate.Zone = zona;
                    activeToUpdate.Image = imagen;

                    activeService.UpdateActiveData(activeToUpdate);

                    LimpiarButton_Click(sender, e);
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("No se encontró ningún activo con ese codigo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
