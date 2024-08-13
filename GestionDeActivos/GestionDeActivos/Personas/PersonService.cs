using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionDeActivos.Personas
{
    class PersonService
    {
        public ObservableCollection<PersonDTO> Persons { get; }
        public PersonService() => Persons = new ObservableCollection<PersonDTO>();

        public void InsertPerson(PersonDTO person)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=GestActiveDB.db;Version=3;"))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Personas (Name, Surname, NameCompany, TypePerson, FullName, Telephone, Email)
                        VALUES (@Name, @Surname, @NameCompany, @TypePerson, @FullName, @Telephone, @Email)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", person.Name);
                        command.Parameters.AddWithValue("@Surname", person.Surname);
                        command.Parameters.AddWithValue("@NameCompany", person.NameCompany);
                        command.Parameters.AddWithValue("@TypePerson", person.TypePerson);
                        command.Parameters.AddWithValue("@FullName", person.FullName);
                        command.Parameters.AddWithValue("@Telephone", person.Telephone);
                        command.Parameters.AddWithValue("@Email", person.Email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new PersonsException($"Error al insertar la persona en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PersonsException($"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        public void UpdatePersonData(PersonDTO p)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "UPDATE Personas SET Name = @Name, Surname = @Surname, " +
                                                        "NameCompany = @NameCompany, TypePerson = @TypePerson, " +
                                                        "FullName = @FullName, Telephone = @Telephone, " +
                                                        "Email = @Email WHERE IdPerson = @IdPerson";
                    
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdPerson", p.IdPerson);
                        comando.Parameters.AddWithValue("@Name", p.Name);
                        comando.Parameters.AddWithValue("@Surname", p.Surname);
                        comando.Parameters.AddWithValue("@NameCompany", p.NameCompany);
                        comando.Parameters.AddWithValue("@TypePerson", p.TypePerson);
                        comando.Parameters.AddWithValue("@FullName", p.FullName);
                        comando.Parameters.AddWithValue("@Telephone", p.Telephone);
                        comando.Parameters.AddWithValue("@Email", p.Email);

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo actualizar {p.FullName} con IdPerson = {p.IdPerson}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show($"{p.FullName} actualizada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar {p.FullName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemovePersonData(PersonDTO p)
        {
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection("Data Source=GestActiveDB.db"))
                {
                    conexion.Open();
                    string query = "DELETE FROM Personas WHERE IdPerson = @IdPerson";
                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdPerson", p.IdPerson);
                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show($"No se pudo eliminar {p.FullName}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                Persons.Remove(p);
                MessageBox.Show($"{p.FullName} eliminada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar {p.FullName}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public PersonDTO FindPersonByFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new PersonsException("El nombre completo no puede estar vacío.");
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=GestActiveDB.db;Version=3;"))
                {
                    connection.Open();
                    string query = "SELECT * FROM Personas WHERE FullName = @FullName";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PersonDTO
                                {
                                    IdPerson = Convert.ToInt32(reader["IdPerson"]),
                                    Name = reader["Name"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    NameCompany = reader["NameCompany"].ToString(),
                                    TypePerson = reader["TypePerson"].ToString(),
                                    Telephone = reader["Telephone"].ToString(),
                                    Email = reader["Email"].ToString()
                                };
                            }
                            else
                            {
                                throw new PersonsException("Persona no encontrada.");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                throw new PersonsException($"Error de base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new PersonsException($"Error al buscar la persona: {ex.Message}");
            }
        }

    }
}