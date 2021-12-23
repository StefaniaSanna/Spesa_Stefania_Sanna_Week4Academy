using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpesaMaster.ConsoleApp
{
    public class Spesa
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int CategoriaId { get; set; }
        public string Descrizione { get; set; }
        public string Utente { get; set; }
        public decimal Importo { get; set; }
        public bool Approvato { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Data} - {Descrizione} - {Utente} - {Importo}";
        }
    }
}
