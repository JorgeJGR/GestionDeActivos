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
using System.Windows.Shapes;

namespace GestionDeActivos.Personas
{
    /// <summary>
    /// Lógica de interacción para PersonDTOView.xaml
    /// </summary>
    public partial class PersonDTOView : Window
    {
        public PersonDTOView()
        {
            InitializeComponent();
            LoadTypePersons();
            LoadCompanies();
            LoadPersons();
        }

        private void LoadPersons()
        {
            var persons = GetPersonsFromDatabase();
            personasDataGrid.ItemsSource = persons;
        }

        private List<PersonDTO> GetPersonsFromDatabase()
        {
            var persons = new List<PersonDTO>();

            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();
                string query = "SELECT IdPerson, Name, Surname, NameCompany, TypePerson, FullName, Telephone, Email FROM Personas";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conexion))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var person = new PersonDTO
                            {
                                IdPerson = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                NameCompany = reader.GetString(3),
                                TypePerson = reader.GetString(4),
                                FullName = reader.GetString(5),
                                Telephone = reader.GetString(6),
                                Email = reader.GetString(7)
                            };
                            persons.Add(person);
                        }
                    }
                }
            }

            return persons;
        }

        private void LoadCompanies()
        {
            var companies = GetCompaniesFromDatabase();
            compañiaComboBox.ItemsSource = companies;
        }

        private List<string> GetCompaniesFromDatabase()
        {
            var companies = new List<string>();

            using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
            {
                conexion.Open();
                string query = "SELECT Name FROM Compañias";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conexion))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            companies.Add(reader["Name"].ToString());
                        }
                    }
                }
            }

            return companies;
        }

        private void LoadTypePersons()
        {
            var typePersons = PersonHelper.GetTypePersons();
            tipoComboBox.ItemsSource = typePersons;
        }

        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
