using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpesaMaster.ConsoleApp
{
    public static class ConnectedModeClient
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Spesa;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        internal static void ApprovaSpesa()
        {
            List<Spesa> spese = GetAll();
            List<Spesa> speseDaApprovare = spese.Where(s=>s.Approvato==false).ToList(); 
            if (speseDaApprovare.Count > 0)
            {
                Console.WriteLine("Le spese da approvare sono:");
                foreach (var item in speseDaApprovare)
                {
                    Console.WriteLine(item.ToString());

                }
               int idSpesa = DisconnectedModeClient.InsertInteger("Inserire l'id della spesa che si vuole approvare");
                bool isSpesaFound = IsSpesaFound(idSpesa);
                if (isSpesaFound == true)
                {
                    Spesa spesaApprovata = speseDaApprovare.SingleOrDefault(s=>s.Id==idSpesa);
                    if (speseDaApprovare != null)
                    {
                        using SqlConnection connessione = new SqlConnection(connectionStringSQL);
                        try
                        {
                            connessione.Open();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connessione;
                            command.CommandType = CommandType.Text;
                            command.CommandText = "update Spese set Approvato=1 where Id=@id";

                            command.Parameters.AddWithValue("@id", idSpesa);

                            int numRighe = command.ExecuteNonQuery();
                            if (numRighe == 1)
                            {
                                connessione.Close();
                                Console.WriteLine("La spesa è stata approvata correttamente");
                            }
                            else
                            {
                                Console.WriteLine("Si è verificato un problema");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine($"Errore SQL: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Errore generico: {ex.Message}");
                        }
                        finally
                        {
                            connessione.Close();
                        }
                    }
                    else
                    {
                        Console.WriteLine("La spesa è già approvata");
                    }                  
                }
                else
                {
                    Console.WriteLine("Non è stata trovata alcuna spesa corrispondente a questo id");
                }
            }
            else 
            {
                Console.WriteLine("Non ci sono spese da approvare");                
            }
        }   
        internal static List<Spesa> SpeseApprovate()
        {
            List<Spesa> spese = GetAll();
            List<Spesa> speseApprovate = spese.Where(s=>s.Approvato==true).ToList();
            if (speseApprovate.Count > 0)
            {
                Console.WriteLine("Le spese approvate sono:");
                foreach (var item in speseApprovate)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else
            {
                Console.WriteLine("Non ci sono spese approvate");
            }
            return speseApprovate;
        }
        internal static bool IsCategoriaFound(int categoriaId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStringSQL))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Categorie where Id=@id";
                command.Parameters.AddWithValue("@id", categoriaId);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == false)
                {
                    connection.Close();
                    return false;
                }
                else
                {
                    connection.Close();
                    return true;
                }
            }
        }
        internal static void EliminaSpesa()
        {
            
            try
            {
                List<Spesa> spese = GetAll();
                foreach (var item in spese)
                {
                    Console.WriteLine(item.ToString());
                }
                int idSpesa = DisconnectedModeClient.InsertInteger("Inserire l'id della spesa che si vuole eliminare");
                bool isSpesaFound = IsSpesaFound(idSpesa);
                if (isSpesaFound == true)
                {
                    using (SqlConnection connection = new SqlConnection(connectionStringSQL))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "delete from Spese where Id=@id";
                        command.Parameters.AddWithValue("@id", idSpesa);
                        int rigaEliminata = command.ExecuteNonQuery();
                        if (rigaEliminata == 1)
                        {
                            connection.Close();
                            Console.WriteLine("Riga eliminata correttamente");
                        }
                        else
                        {
                            connection.Close();
                            Console.WriteLine("Ops qualcosa è andato storto");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Non è stata trovata nessuna spesa corrispondente a questo id");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);            
            }
        }
        internal static bool IsSpesaFound(int spesaId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStringSQL))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Spese where Id=@id";
                command.Parameters.AddWithValue("@id", spesaId);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == false)
                {
                    connection.Close();
                    return false;
                }
                else
                {
                    connection.Close();
                    return true;
                }
            }
        }
        internal static List<Spesa> GetAll()
        {
            using (SqlConnection connessione = new SqlConnection(connectionStringSQL))
            {
                connessione.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connessione;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Spese";
                SqlDataReader reader = command.ExecuteReader();
                List<Spesa> spese = new List<Spesa>();

                while (reader.Read())
                {
                    var id = reader["Id"];
                    var data = reader["DataSpesa"];
                    var catId = reader["CategoriaId"];
                    var descrizione = reader["Descrizione"];
                    var utente = reader["Utente"];
                    var importo = reader["Importo"];
                    var approvato = reader["Approvato"];

                    Spesa nuovaSpesa = new Spesa();
                    nuovaSpesa.Id = (int)id;
                    nuovaSpesa.Data = (DateTime)data;
                    nuovaSpesa.Importo = (decimal)importo;
                    nuovaSpesa.Approvato = (bool)approvato;
                    nuovaSpesa.CategoriaId = (int)catId;
                    nuovaSpesa.Utente = (string)utente;
                    nuovaSpesa.Descrizione= (string)descrizione;
                    spese.Add(nuovaSpesa);
                }
                connessione.Close();
                return spese;

            }

            
        }
        internal static void SpesePerUtente()
        {
            string utente = DisconnectedModeClient.InsertString("Inserire il nome dell'utente");
            List<Spesa> spese = GetAll();
            var spesePerUtente = spese.Where(s=>s.Utente== utente).ToList();
            if(spesePerUtente.Count > 0)
            {
                Console.WriteLine($"Le spese di {utente} sono:");
                foreach (var item in spesePerUtente)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Utente non presente");
            }
        }
        internal static void TotaleSpesePerCategoria()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open(); 
                if (connessione.State == System.Data.ConnectionState.Open) 
                {
                    Console.WriteLine("Siamo connessi al db");
                }
                else
                {
                    Console.WriteLine("NON siamo connessi al db");
                }
                
                string query = @"select c.Categoria, sum(s.Importo) as [Totale] from Spese s join Categorie c on s.CategoriaId = c.Id group by c.Categoria";
                
                SqlCommand comando = new SqlCommand();
                comando.Connection = connessione;
                comando.CommandType = System.Data.CommandType.Text; 
                comando.CommandText = query;

                SqlDataReader reader = comando.ExecuteReader();    

                while (reader.Read()) 
                {                  
                    var categoria = (string)reader["Categoria"]; 
                    var totale = (decimal)reader["Totale"];

                    Console.WriteLine($"{categoria} - {totale}");

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore sql: {ex.Message}");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Errore generito: {ex.Message}");
            }
            finally
            {
                connessione.Close();
            }


        }





    }
}
