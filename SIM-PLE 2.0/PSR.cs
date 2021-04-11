using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_PLE_2._0
{
    public class PSR
    {
        public string CodPSR { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string Altura { get; set; }
        public string Caminante { get; set; }
        public string Pos { get; set; }
        public int PrimeraRecarga { get; set; }
        public string Lote { get; set; }
        public string NimCliente { get; set; }
        public string IdSIM { get; set; }
        public bool EsCumplidor = false;
        public int StockCarga { get; set; }
        public int StockSim { get; set; }
        public int Transferencias { get; set; }
        public double VentaMensual { get; set; }
    }
}
