using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace GestionDeActivos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CrearBaseDeDatosYTablas();
            InsertarDatosIniciales();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CrearBaseDeDatosYTablas();
        }

        private void CrearBaseDeDatosYTablas()
        {

            SQLiteConnection conexion = new SQLiteConnection("Data Source=prueba.db");
            conexion.Open();

            SQLiteCommand comando = conexion.CreateCommand();

            comando.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Compañias (
                        IdCompany INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL UNIQUE,
                        TipoCompañia TEXT NOT NULL,
                        Telefono TEXT,
                        Email TEXT
                    );
                ";
            comando.ExecuteNonQuery();
        }

        private void InsertarDatosIniciales()
        {
            SQLiteConnection conexion = new SQLiteConnection("Data Source=prueba.db");
            conexion.Open();

            SQLiteCommand comando = conexion.CreateCommand();

            comando.CommandText = @"
                INSERT INTO Compañias (Name, TipoCompañia, Telefono, Email)
                VALUES (@name, @tipo, @telefono, @email);
                       
            ";

            comando.Parameters.AddWithValue("@name", "Monbake");
            comando.Parameters.AddWithValue("@tipo", "Interna");
            comando.Parameters.AddWithValue("@telefono", "948902900");
            comando.Parameters.AddWithValue("@email", "MantenimientoAlica@monbake.es");

            try
            {
                comando.ExecuteNonQuery();
                MessageBox.Show("Datos iniciales insertados correctamente.");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error al insertar datos iniciales: " + ex.Message);
            }

            conexion.Close();
        }


        private void ProbarButton_Click(object sender, RoutedEventArgs e)
        {
            Compañias.CompanyView companyView = new Compañias.CompanyView
            {
                Owner = this
            };
            companyView.ShowDialog();
        }
    }
}
