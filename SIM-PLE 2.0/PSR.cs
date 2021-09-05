using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_PLE_2._0
{
    public class PSR
    {
        public string CodPSR { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string NumAddress { get; private set; }
        public string Walker { get; private set; }
        public string Pos { get; private set; }
        public int FirstCharge { get; private set; }
        public string Lote { get; private set; }
        public string ClientNim { get; private set; }
        public string IdSIM { get; private set; }
        public bool IsCompliant { get; private set; }
        public int StochCharges { get; private set; }
        public int StockSim { get; private set; }
        public int Transactions { get; private set; }
        public double MonthSolds { get; private set; }

        #region "Index from reports"
        // Index Reporte PSR de la agencia
        const byte INDEX_AGENCY_CODPSR = 1;
        const byte INDEX_AGENCY_NAME = 2;
        const byte INDEX_AGENCY_ADRESS = 3;
        const byte INDEX_AGENCY_NUMBER = 4;
        const byte INDEX_AGENCY_WALKER = 11;
        const byte INDEX_AGENCY_POS = 16;
        // Index Reporte Primera Recarga
        const byte INDEX_CHARGES_CODPSR = 0;
        const byte INDEX_CHARGES_NAME = 1;
        const byte INDEX_CHARGES_WALKER = 7;
        const byte INDEX_CHARGES_ID = 9;
        const byte INDEX_CHARGES_AMOUNT = 14;
        const byte INDEX_CHARGES_NIM = 10;
        // Index Reporte Productos Vendidos
        const byte INDEX_PRODUCTS_CODPSR = 1;
        const byte INDEX_PRODUCTS_LOTE = 16;
        const byte INDEX_PRODUCTS_CHECKER = 10;
        const byte INDEX_PRODUCTS_WALKER = 8;
        const byte INDEX_PRODUCTS_TRANSACTIONS = 13;
        //Index Venta de saldo - Analitico
        const byte INDEX_DEALER_CODPSR = 2;
        const byte INDEX_DEALER_TOTALSOLDS = 4;
        const byte INDEX_DEALER_VENTASPSR = 5;
        const byte INDEX_DEALER_POS = 3;
        #endregion

        public Dictionary<string, PSR> SetDataForSims(string reportAgency, string reportFirstCharge, string reportSoldProducts, string walkerSelected, int salesTarget) 
        {
            var psrSIM = new Dictionary<string, PSR>();

            //======== Se crea un array a partir de cada linea de los reportes =======\\
            string[] arrayReportAgency = File.ReadAllLines(reportAgency);
            int reportAgency_len = arrayReportAgency.Length;


            for (int i = 2; i < reportAgency_len; i++)
            {
                // Array con el contenido de dichas lineas.
                string[] itemsAgency = arrayReportAgency[i].Split(';');
                // Si pertenece al caminante seleccionado entonces se asignan los datos al objeto PSR.
                if (itemsAgency[INDEX_AGENCY_WALKER] == walkerSelected)
                {
                    PSR client = new PSR
                    {
                        CodPSR = itemsAgency[INDEX_AGENCY_CODPSR],
                        Walker = itemsAgency[INDEX_AGENCY_WALKER],
                        Pos = itemsAgency[INDEX_AGENCY_POS],
                        Name = itemsAgency[INDEX_AGENCY_NAME].Replace('"', ' ').Trim(),
                        Address = itemsAgency[INDEX_AGENCY_ADRESS],
                        NumAddress = itemsAgency[INDEX_AGENCY_NUMBER],
                        FirstCharge = 0,
                        ClientNim = "S/N",
                        IsCompliant = false
                    };
                    psrSIM[client.CodPSR] = client; // Se guarda el objeto cliente en el diccionario.
                }
            }

            string[] arrayReportFirstCharge = File.ReadAllLines(reportFirstCharge);
            int reportFirstCharge_len = arrayReportFirstCharge.Length;

            for (int y = 2; y < reportFirstCharge_len; y++)
            {
                string[] itemspRecarga = arrayReportFirstCharge[y].Trim().Split(';');

                if (itemspRecarga[INDEX_CHARGES_WALKER] == walkerSelected)
                {
                    var auxPSRcode = itemspRecarga[INDEX_CHARGES_CODPSR];
                    var auxPrecarga = Convert.ToInt32(itemspRecarga[INDEX_CHARGES_AMOUNT]);
                    var auxNim = itemspRecarga[INDEX_CHARGES_NIM];
                    var auxIdSim = itemspRecarga[INDEX_CHARGES_ID];

                    if (psrSIM.ContainsKey(auxPSRcode))
                    {
                        if (psrSIM[auxPSRcode].FirstCharge == 0)
                        {
                            psrSIM[auxPSRcode].FirstCharge = auxPrecarga;
                            psrSIM[auxPSRcode].ClientNim = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        else if (auxPrecarga > psrSIM[auxPSRcode].FirstCharge)
                        {
                            psrSIM[auxPSRcode].FirstCharge = auxPrecarga;
                            psrSIM[auxPSRcode].ClientNim = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        if (psrSIM[auxPSRcode].FirstCharge >= salesTarget)
                        {
                            psrSIM[auxPSRcode].IsCompliant = true;
                        }
                    }
                }
            }

            string[] arrayReportSoldProducts = File.ReadAllLines(reportSoldProducts);
            int reportSoldProducts_len = arrayReportSoldProducts.Length;

            for (int z = 2; z < reportSoldProducts_len; z++)
            {
                string[] itemsSoldProducts = arrayReportSoldProducts[z].Split(';');
                var auxPSRcode = itemsSoldProducts[INDEX_PRODUCTS_CODPSR];

                if (psrSIM.ContainsKey(auxPSRcode) && psrSIM[auxPSRcode].FirstCharge == 0 && itemsSoldProducts[INDEX_PRODUCTS_CHECKER] != "Carga Virtual")
                    psrSIM[auxPSRcode].Lote = itemsSoldProducts[INDEX_PRODUCTS_LOTE];
            }

            return psrSIM;
        }
        public Dictionary<string, PSR> SetDataForSellout(string reportAgency, string reportDealer, string reportSoldProducts, string selectedWalker, int salesTarget)
        {
            var psrSellout = new Dictionary<string, PSR>();

            string[] arrayReportAgency = File.ReadAllLines(reportAgency);
            int reportAgency_len = arrayReportAgency.Length;

            for (int i = 2; i < reportAgency_len; i++)
            {
                string[] itemsAgency = arrayReportAgency[i].Split(';');
                if (itemsAgency[INDEX_AGENCY_WALKER] == selectedWalker)
                {
                    PSR clientSellout = new PSR()
                    {
                        CodPSR = itemsAgency[INDEX_AGENCY_CODPSR],
                        Name = itemsAgency[INDEX_AGENCY_NAME].Replace('"', ' ').Trim(),
                        Walker = itemsAgency[INDEX_AGENCY_WALKER],
                        Pos = itemsAgency[INDEX_AGENCY_POS],
                        Transactions = 0,
                        StochCharges = 0,
                        StockSim = 0,
                        MonthSolds = 0,
                        IsCompliant = false
                    };
                    psrSellout[clientSellout.CodPSR] = clientSellout;
                }
            }

            string[] arrayReportSoldProducts = File.ReadAllLines(reportSoldProducts);
            int reportSoldProducts_len = arrayReportSoldProducts.Length;

            for (int z = 2; z < reportSoldProducts_len; z++)
            {
                string[] itemsSoldProducts = arrayReportSoldProducts[z].Split(';');
                var auxpsrcode = itemsSoldProducts[INDEX_PRODUCTS_CODPSR];

                if (itemsSoldProducts[INDEX_PRODUCTS_CHECKER] == "Carga Virtual" && itemsSoldProducts[INDEX_PRODUCTS_WALKER] == selectedWalker)
                {
                    psrSellout[auxpsrcode].Transactions += Convert.ToInt32(itemsSoldProducts[INDEX_PRODUCTS_TRANSACTIONS]);
                }
            }

            string[] arrayReportDealer = File.ReadAllLines(reportDealer);
            int reporDealer_len = arrayReportDealer.Length;

            for (int y = 5; y < reporDealer_len; y++)
            {
                string[] totalSolds = arrayReportDealer[y].Split(';');
                string auxPsrcode = totalSolds[INDEX_DEALER_CODPSR].Insert(0, "0"); //algunos PSR cod no incliyen el "0" del inicio.

                if (psrSellout.ContainsKey(auxPsrcode))
                {
                    psrSellout[auxPsrcode].MonthSolds = Convert.ToDouble(totalSolds[INDEX_DEALER_TOTALSOLDS]);
                    if (psrSellout[auxPsrcode].MonthSolds >= salesTarget)
                        psrSellout[auxPsrcode].IsCompliant = true;
                }
            }
            return psrSellout;
        }
        public Dictionary<string, Reward40> SetDataForRewards(string reportFirstCharges, int maxForClient, int maxForWalker)
        {

            var arrayReportFirstCharges = File.ReadAllLines(reportFirstCharges);
            int reporFirstCharges_len = arrayReportFirstCharges.Length;
            var allFirstCharges = new Dictionary<string, Reward40>();

            for (int i = 2; i < reporFirstCharges_len; i++)
            {
                var itemsFirstCharges = arrayReportFirstCharges[i].Split(';');
                var codPsr = itemsFirstCharges[INDEX_CHARGES_CODPSR].Trim();
                var walker = itemsFirstCharges[INDEX_CHARGES_WALKER].Trim();
                var amount = itemsFirstCharges[INDEX_CHARGES_AMOUNT].Trim();

                if (allFirstCharges.ContainsKey(codPsr))
                {
                    allFirstCharges[codPsr].TotalSim++;
                    allFirstCharges[codPsr].Amount += Get40percent(amount);
                    if (allFirstCharges[codPsr].Amount > maxForClient)
                        allFirstCharges[codPsr].TopReward = maxForClient;
                    else
                        allFirstCharges[codPsr].TopReward = allFirstCharges[codPsr].Amount;
                }
                else
                {
                    var sim = new Reward40()
                    {
                        CodPsr = codPsr,
                        Walker = walker,
                        Amount = Get40percent(amount),
                        TotalSim = 1,
                        TopReward = (Get40percent(amount) > maxForClient) ? maxForClient : Get40percent(amount)
                    };
                    allFirstCharges[sim.CodPsr] = sim;
                }
            }

            var employeeRewards = new Dictionary<string, Reward40>();

            foreach (var item in allFirstCharges.Values)
            {
                if (!employeeRewards.ContainsKey(item.Walker))
                {
                    employeeRewards[item.Walker] = item;
                    employeeRewards[item.Walker].WalkerReward = item.TopReward;
                }
                else
                {
                    employeeRewards[item.Walker].Amount += item.Amount;
                    employeeRewards[item.Walker].TotalSim += item.TotalSim;
                    employeeRewards[item.Walker].WalkerReward += item.TopReward;
                    if (employeeRewards[item.Walker].WalkerReward > maxForWalker)
                        employeeRewards[item.Walker].WalkerReward = maxForWalker;
                }
            }
            return employeeRewards;
        }
        private int Get40percent(string input) //obtine el 40% del input
        {
            double mathOperation = double.Parse(input) * 0.4;
            int result = Convert.ToInt32(Math.Round(mathOperation));
            return result;
        }
    }
}
