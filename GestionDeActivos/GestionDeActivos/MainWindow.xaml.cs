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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CrearBaseDeDatosYTablas();
        }

        private void CrearBaseDeDatosYTablas()
        {
            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();

                using (SQLiteCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"
                CREATE TABLE IF NOT EXISTS Compañias (
                    IdCompany INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    TipoCompañia TEXT NOT NULL,
                    Telephone TEXT,
                    Email TEXT
                );";
                    comando.ExecuteNonQuery();
                }

                using (SQLiteCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Activos (
                        IdActive INT NOT NULL PRIMARY KEY,
                        Description VARCHAR(255) NOT NULL,
                        Line VARCHAR(50) NOT NULL,
                        Zone VARCHAR(50) NOT NULL,
                        Image VARCHAR(255) NOT NULL
                );";
                    comando.ExecuteNonQuery();
                }

                using (SQLiteCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Personas (
                        IdPerson INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name VARCHAR(255) NULL,
                        Surname VARCHAR(255) NULL,
                        NameCompany VARCHAR(255) NULL,
                        TypePerson VARCHAR(255) NULL,
                        FullName VARCHAR(255) NULL,
                        Telephone VARCHAR(255) NULL,
                        Email VARCHAR(255) NULL
                    );";
                    comando.ExecuteNonQuery();
                }

                using (SQLiteCommand comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Cintas (
                        IdConveyorBelt INTEGER PRIMARY KEY AUTOINCREMENT,
                        DesConveyorBelt TEXT NOT NULL,
                        Certificate TEXT,
                        TechnicalSheet TEXT
                    );";
                    comando.ExecuteNonQuery();
                }

                conexion.Close();
            }
        }

        private void ProbarButtonProbar01_Click(object sender, RoutedEventArgs e)
        {
            Compañias.CompanyView companyView = new Compañias.CompanyView
            {
                Owner = this
            };
            companyView.ShowDialog();
        }

        private void ProbarButtonProbar02_Click(object sender, RoutedEventArgs e)
        {
            Personas.PersonDTOView personDTOView = new Personas.PersonDTOView()
            {
                Owner = this
            };
            personDTOView.ShowDialog();
        }

        private void ProbarButtonProbar03_Click(object sender, RoutedEventArgs e)
        { 
            Activos.ActiveView activeView = new Activos.ActiveView()
            {
                Owner = this
            };
            activeView.ShowDialog();
        }
    }
}
