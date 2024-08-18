using System;
using System.Collections.Generic;
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
        public ActiveView()
        {
            InitializeComponent();
            lineaComboBox.ItemsSource = Enum.GetValues(typeof(Lines)).Cast<Lines>();
            zonaComboBox.ItemsSource = Enum.GetValues(typeof(Zones)).Cast<Zones>();
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

        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
