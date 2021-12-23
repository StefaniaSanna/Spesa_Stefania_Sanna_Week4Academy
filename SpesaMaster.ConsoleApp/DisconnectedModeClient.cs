using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpesaMaster.ConsoleApp
{
    public static class DisconnectedModeClient
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Spesa;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static SqlDataAdapter InizializzaDatasetEAdapter(DataSet spese, SqlConnection conn)
        {
            SqlDataAdapter spesaAdapter = new SqlDataAdapter();
            spesaAdapter.SelectCommand = new SqlCommand("Select * from Spese", conn);
            spesaAdapter.InsertCommand = GeneraInsertCommand(conn);          
            spesaAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            spesaAdapter.Fill(spese, "Ingrediente");
            return spesaAdapter;
        }   
        private static SqlCommand GeneraInsertCommand(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into Spese values(@data, @catId, @descr, @utente,@importo, @approvato)";

            cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 0, "DataSpesa"));
            cmd.Parameters.Add(new SqlParameter("@catId", SqlDbType.Int, 0, "CategoriaId"));
            cmd.Parameters.Add(new SqlParameter("@descr", SqlDbType.NVarChar, 500, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@utente", SqlDbType.NVarChar, 100, "Utente"));
            cmd.Parameters.Add(new SqlParameter("@importo", SqlDbType.Decimal, 0, "Importo"));
            cmd.Parameters.Add(new SqlParameter("@approvato", SqlDbType.Bit, 0, "Approvato"));

            return cmd;
        }

        internal static void InsertSpesa()
        {          
            DataSet spese = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionStringSQL);
            try
            {
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al db");
                else
                    Console.WriteLine("NON connessi al db");

                var spesaAdapter = InizializzaDatasetEAdapter(spese,conn);
                spesaAdapter.Fill(spese, "Spese");

                conn.Close();
                Console.WriteLine("Connessione chiusa");

                DataRow nuovaRiga = spese.Tables["Spese"].NewRow();

                DateTime data = InsertData("Inserire la data della spesa");
                int categoriaId = InsertInteger("Inserire l'id della categoria");
                string descrizione = InsertString("Inserire la descrizione");
                string utente = InsertString("Inserire l'utente");
                decimal importo = InsertDecimal("Inserire l'importo");
                bool isFound = ConnectedModeClient.IsCategoriaFound(categoriaId);
                if (isFound == true)
                {
                    nuovaRiga["DataSpesa"] = data;
                    nuovaRiga["CategoriaId"] = categoriaId;
                    nuovaRiga["Descrizione"] = descrizione;
                    nuovaRiga["Utente"] = utente;
                    nuovaRiga["Importo"] = importo;
                    nuovaRiga["Approvato"] = 0;

                    spese.Tables["Spese"].Rows.Add(nuovaRiga);
                    spesaAdapter.Update(spese, "Spese");
                    Console.WriteLine("Database Aggiornato");
                }
                else
                {
                    Console.WriteLine("Nessuna spesa corrispondente a questo id");
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
                conn.Close();
            }
        }
        public static int InsertInteger(string testo)
        {
            int scelta;
            do
            {
                Console.WriteLine(testo);
            }
            while (!int.TryParse(Console.ReadLine(), out scelta));
            return scelta;
        }
        public static string InsertString(string testo)
        {
            string risposta;
            do
            {
                Console.WriteLine(testo);
                risposta = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(risposta));
            return risposta;
        }
        private static DateTime InsertData(string testo)
        {
            DateTime scelta;
            do
            {
                Console.WriteLine(testo);
            }
            while (!DateTime.TryParse(Console.ReadLine(), out scelta));
            return scelta;
        }
        private static decimal InsertDecimal(string testo)
        {
            decimal scelta;
            do
            {
                Console.WriteLine(testo);
            }
            while (!decimal.TryParse(Console.ReadLine(), out scelta));
            return scelta;
        }
    }
}
