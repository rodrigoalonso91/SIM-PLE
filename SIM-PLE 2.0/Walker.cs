using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_PLE_2._0
{


    class Walker
    {
        private const byte INDEX_AGENCIA_CAMINANTE = 11;
        
        public string FullName { get; private set; }
        public int ObjSim { get; private set; }
        public int ObjSellOut { get; private set; }
        public int Volumen { get; private set; }
        public int Gift40percent { get; private set; }

        public Walker(string Fullname, int ObjSim, int ObjSellOut, int Volumen, int Gift40percent)
        {
            this.FullName = Fullname;
            this.ObjSim = ObjSim;
            this.ObjSellOut = ObjSellOut;
            this.Volumen = Volumen;
            this.Gift40percent = Gift40percent;
        }
        public static List<string> GetWalkerFromReport(string _path) //Abre archivo y devuelve una lista de walkers
        {
            var walkersList = new List<string>();
            string[] arrReport = File.ReadAllLines(_path);
            bool isEmpty;

            int reportLen = arrReport.Length;
            for (int i = 2; i < reportLen; i++)
            {
                string[] itemReport = arrReport[i].Split(';');
                if (walkersList.Contains(itemReport[INDEX_AGENCIA_CAMINANTE]))
                    continue;
                else
                {
                    isEmpty = String.IsNullOrWhiteSpace(itemReport[INDEX_AGENCIA_CAMINANTE]);
                    if (!isEmpty) walkersList.Add(itemReport[INDEX_AGENCIA_CAMINANTE]);
                }
            }
            return walkersList;
        }

    }
}
