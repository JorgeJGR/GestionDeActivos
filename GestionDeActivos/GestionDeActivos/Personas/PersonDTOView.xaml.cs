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

namespace GestionDeActivos.Personas
{
    /// <summary>
    /// Lógica de interacción para PersonDTOView.xaml
    /// </summary>
    public partial class PersonDTOView : Window
    {
        private PersonService personService;
        public PersonDTOView()
        {
            InitializeComponent();
            personService = new PersonService();
            LoadTypePersons();
            LoadCompanies();
            LoadPersons();
        }

        private void LoadPersons()
        {
            var persons = GetPersonsFromDatabase();
            personasDataGrid.ItemsSource = persons;
        }

        private ObservableCollection<PersonDTO> GetPersonsFromDatabase()
        {
            var persons = new ObservableCollection<PersonDTO>();

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
                                IdPerson = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                NameCompany = reader.GetString(3),
                                TypePerson = reader.GetString(4),
                                FullName = reader.GetString(5),
                                Telephone = reader.GetString(6),
                                Email = reader.GetString(7)
                            };
                            persons.Add(person);
                            personService.Persons.Add(person);
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

        private ObservableCollection<string> GetCompaniesFromDatabase()
        {
            var companies = new ObservableCollection<string>();

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

        private void LimpiarButton_Click(object sender, RoutedEventArgs e)
        {

            nombreTextBox.IsReadOnly = false;
            nombreTextBox.ClearValue(BackgroundProperty);
            nombreTextBox.Text = "";

            apellidosTextBox.IsReadOnly = false;
            apellidosTextBox.ClearValue(BackgroundProperty);
            apellidosTextBox.Text = "";

            compañiaComboBox.SelectedIndex = -1;
            tipoComboBox.SelectedIndex = -1;

            telefonoTextBox.Text = "";
            telefonoTextBox.IsReadOnly = true;

            emailTextBox.Text = "";
            emailTextBox.IsReadOnly = true;

            grabarButton.IsEnabled = false;
            actualizarButton.IsEnabled = false;
            eliminarButton.IsEnabled = false;
            buscarButton.IsEnabled = true;
        }

        private void CompañiaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (compañiaComboBox.SelectedItem != null)
            {
                compañiaTextBox.Text = compañiaComboBox.SelectedItem.ToString();
            }
            else
            {
                compañiaTextBox.Text = "";
            }
        }

        private void TipoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tipoComboBox.SelectedItem != null)
            {
                tipoTextBox.Text = tipoComboBox.SelectedItem.ToString();
            }
            else
            {
                tipoTextBox.Text = "";
            }

            var selectedItem = tipoComboBox.SelectedItem as string;

            if (selectedItem == "Comercial")
            {
                telefonoTextBox.IsReadOnly = false;
                telefonoTextBox.ClearValue(BackgroundProperty); 

                emailTextBox.IsReadOnly = false;
                emailTextBox.ClearValue(BackgroundProperty); 
            }
            else if (selectedItem == "Montador Externo")
            {
                telefonoTextBox.IsReadOnly = false;
                telefonoTextBox.ClearValue(BackgroundProperty); 

                emailTextBox.IsReadOnly = true;
                emailTextBox.Text = null;
                emailTextBox.Background = Brushes.Lavender; 
            }
            else if (selectedItem == "Montador Propio")
            {
                telefonoTextBox.IsReadOnly = true;
                telefonoTextBox.Text = null;
                telefonoTextBox.Background = Brushes.Lavender; 

                emailTextBox.IsReadOnly = true;
                emailTextBox.Text = null;
                emailTextBox.Background = Brushes.Lavender; 
            }
        }


        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreTextBox.Text) || string.IsNullOrWhiteSpace(apellidosTextBox.Text))
                {
                    MessageBox.Show("Se han de rellenar los campos Nombre y Apellidos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string fullName = $"{nombreTextBox.Text.Trim()} {apellidosTextBox.Text.Trim()}";
                    PersonService service = new PersonService();
                    var person = service.FindPersonByFullName(fullName);

                    if (person != null)
                    {
                        MessageBox.Show($"Se encontró a {fullName} en la base de datos.", "Búsqueda Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);

                        compañiaTextBox.Text = person.NameCompany;
                        tipoTextBox.Text = person.TypePerson;
                        telefonoTextBox.Text = person.Telephone;
                        emailTextBox.Text = person.Email;
                        buscarButton.IsEnabled = false;

                        MessageBoxResult result = MessageBox.Show("¿Deseas eliminar o modificar el registro?", "Seleccionar acción",
                                                            MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            eliminarButton.IsEnabled = true;
                        }
                        else if (result == MessageBoxResult.No)
                        {
                            nombreTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                            nombreTextBox.IsReadOnly = true;
                            apellidosTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                            apellidosTextBox.IsReadOnly = true;
                            telefonoTextBox.ClearValue(BackgroundProperty);
                            emailTextBox.ClearValue(BackgroundProperty);
                            compañiaComboBox.SelectedIndex = -1;
                            tipoComboBox.SelectedIndex = -1;
                            actualizarButton.IsEnabled = true;
                        }
                        else
                        {
                            nombreTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                            nombreTextBox.IsReadOnly = true;
                            apellidosTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Lavender"));
                            apellidosTextBox.IsReadOnly = true;
                            compañiaTextBox.Text = person.NameCompany;
                            telefonoTextBox.Text = person.Telephone;
                            telefonoTextBox.IsReadOnly = false;
                            emailTextBox.Text = person.Email;
                            emailTextBox.IsReadOnly = false;
                            buscarButton.IsEnabled = false;
                        }
                    }

                }
            }
            catch (PersonsException ex)
            {
                MessageBox.Show(ex.Message, "Error en la Búsqueda", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBoxResult result = MessageBox.Show($"La Persona no existe. ¿Deseas crear un nuevo registro?", "Persona no encontrada",
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
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GrabarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PersonDTO newPersonDTO = new PersonDTO
                {
                    Name = nombreTextBox.Text.Trim(),
                    Surname = apellidosTextBox.Text.Trim(),
                    NameCompany = compañiaTextBox.Text.Trim(),
                    TypePerson = tipoTextBox.Text.Trim(),
                    FullName = $"{nombreTextBox.Text.Trim()} {apellidosTextBox.Text.Trim()}",
                    Telephone = telefonoTextBox.Text.Trim(),
                    Email = emailTextBox.Text.Trim()
                };

                personService.InsertPerson(newPersonDTO);

                MessageBox.Show("Persona guardada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadPersons();
                LimpiarButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la persona: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string compañia = compañiaTextBox.Text;
                string tipo = tipoTextBox.Text;
                string nombreCompleto = nombreTextBox.Text + " " + apellidosTextBox.Text;
                string telefono = telefonoTextBox.Text;
                string email = emailTextBox.Text;

                PersonDTO personToUpdate = personService.Persons
                    .FirstOrDefault(p => p.FullName.Equals(nombreCompleto, StringComparison.OrdinalIgnoreCase));

                if (personToUpdate != null)
                {
                    personToUpdate.NameCompany = compañia;
                    personToUpdate.TypePerson = tipo;
                    personToUpdate.FullName = nombreCompleto;
                    personToUpdate.Telephone = telefono;
                    personToUpdate.Email = email;

                    personService.UpdatePersonData(personToUpdate);

                    LimpiarButton_Click(sender, e);
                    LoadPersons();
                }
                else
                {
                    MessageBox.Show($"No se encontró '{nombreCompleto}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                string nombreCompleto = nombreTextBox.Text + " " + apellidosTextBox.Text;

                PersonDTO personToRemove = personService.Persons
                    .FirstOrDefault(p => p.FullName.Equals(nombreCompleto, StringComparison.OrdinalIgnoreCase));

                if (personToRemove != null)
                {
                    personService.RemovePersonData(personToRemove);
                }

                LimpiarButton_Click(sender, e);
                LoadPersons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
