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
        public string Nombre { get; private set; }
        public string Calle { get; private set; }
        public string Altura { get; private set; }
        public string Caminante { get; private set; }
        public string Pos { get; private set; }
        public int PrimeraRecarga { get; private set; }
        public string Lote { get; private set; }
        public string NimCliente { get; private set; }
        public string IdSIM { get; private set; }
        public bool EsCumplidor { get; private set; }
        public int StockCarga { get; private set; }
        public int StockSim { get; private set; }
        public int Transferencias { get; private set; }
        public double VentaMensual { get; private set; }

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
        const byte INDEX_DEALER_VENTASFINALES = 4;
        const byte INDEX_DEALER_VENTASPSR = 5;
        const byte INDEX_DEALER_POS = 3;
        #endregion

        public Dictionary<string, PSR> SetDataForSims(string reportAgency, string reportFirstCharge, string reportSoldProducts, string walkerSelected, int salesTarget)
        {
            Dictionary<string, PSR> psrSIM = new Dictionary<string, PSR>();

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
                        Caminante = itemsAgency[INDEX_AGENCY_WALKER],
                        Pos = itemsAgency[INDEX_AGENCY_POS],
                        Nombre = itemsAgency[INDEX_AGENCY_NAME].Replace('"', ' ').Trim(),
                        Calle = itemsAgency[INDEX_AGENCY_ADRESS],
                        Altura = itemsAgency[INDEX_AGENCY_NUMBER],
                        PrimeraRecarga = 0,
                        NimCliente = "S/N",
                        EsCumplidor = false
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
                    string auxPSRcode = itemspRecarga[INDEX_CHARGES_CODPSR];
                    int auxPrecarga = Convert.ToInt32(itemspRecarga[INDEX_CHARGES_AMOUNT]);
                    string auxNim = itemspRecarga[INDEX_CHARGES_NIM];
                    string auxIdSim = itemspRecarga[INDEX_CHARGES_ID];

                    if (psrSIM.ContainsKey(auxPSRcode))
                    {
                        if (psrSIM[auxPSRcode].PrimeraRecarga == 0)
                        {
                            psrSIM[auxPSRcode].PrimeraRecarga = auxPrecarga;
                            psrSIM[auxPSRcode].NimCliente = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        else if (auxPrecarga > psrSIM[auxPSRcode].PrimeraRecarga)
                        {
                            psrSIM[auxPSRcode].PrimeraRecarga = auxPrecarga;
                            psrSIM[auxPSRcode].NimCliente = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        if (psrSIM[auxPSRcode].PrimeraRecarga >= salesTarget)
                        {
                            psrSIM[auxPSRcode].EsCumplidor = true;
                        }
                    }
                }
            }

            string[] arrayReportSoldProducts = File.ReadAllLines(reportSoldProducts);
            int reportSoldProducts_len = arrayReportSoldProducts.Length;

            for (int z = 2; z < reportSoldProducts_len; z++)
            {
                string[] itemsSoldProducts = arrayReportSoldProducts[z].Split(';');
                string auxPSRcode = itemsSoldProducts[INDEX_PRODUCTS_CODPSR];

                if (psrSIM.ContainsKey(auxPSRcode) && psrSIM[auxPSRcode].PrimeraRecarga == 0 && itemsSoldProducts[INDEX_PRODUCTS_CHECKER] != "Carga Virtual")
                    psrSIM[auxPSRcode].Lote = itemsSoldProducts[INDEX_PRODUCTS_LOTE];
            }

            return psrSIM;
        }
        public Dictionary<string, PSR> SetDataForSellout()
        {
            Dictionary<string, PSR> psrSellout = new Dictionary<string, PSR>();
            //TODO: Logica de objetivo SellOut
            return psrSellout;
        }
    }
}
