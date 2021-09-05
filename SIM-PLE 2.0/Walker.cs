using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_PLE_2._0
{


    public class Walker
    {
        private const byte INDEX_AGENCY_WALKER = 11;
        
        public string FullName { get; private set; }
        public int ObjSim { get; private set; }
        public int ObjSellOut { get; private set; }
        public int Volumen { get; private set; }
        public int Gift40percent { get; private set; }

       
        //Methods
        public static List<string> GetWalkerFromReport(string _path) //Abre archivo y devuelve una lista de walkers
        {
            var walkersList = new List<string>();
            string[] arrReport = File.ReadAllLines(_path);
            bool isEmpty;

            int reportLen = arrReport.Length;
            for (int i = 2; i < reportLen; i++)
            {
                string[] itemReport = arrReport[i].Split(';');
                if (walkersList.Contains(itemReport[INDEX_AGENCY_WALKER]))
                    continue;
                else
                {
                    isEmpty = String.IsNullOrWhiteSpace(itemReport[INDEX_AGENCY_WALKER]);
                    if (!isEmpty) walkersList.Add(itemReport[INDEX_AGENCY_WALKER]);
                }
            }
            return walkersList;
        }

        public Dictionary<int, Walker> GetSalary(string[] arrEmployee)
        {
            return null;
        }
    }
}
