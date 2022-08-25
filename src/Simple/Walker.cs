using System;
using System.Collections.Generic;
using System.IO;

namespace Simple
{
    public class Walker
    {
        private const byte INDEX_AGENCY_WALKER = 11;
        private const byte INDEX_DEALER_TOTAL = 0;
        private const byte INDEX_DEALER_TOTALAMOUNT = 4;
        private const int simTarget1 = 100;
        private const int simTarget2 = 120;
        private const int simTarget3 = 150;
        private const int soTarget2 = 120;
        private const int soTarget3 = 150;

        public string FullName { get; private set; }
        public string ObjSim { get; private set; }
        public string RewardSim { get; private set; }
        public string ObjSellOut { get; private set; }
        public string RewardSo { get; private set; }
        public string Volumen { get; private set; }
        public string SoldVol { get; private set; }
        public string Gift40percent { get; private set; }
        public string TotalReward { get; private set; }

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
                    isEmpty = string.IsNullOrWhiteSpace(itemReport[INDEX_AGENCY_WALKER]);
                    if (!isEmpty) walkersList.Add(itemReport[INDEX_AGENCY_WALKER]);
                }
            }
            return walkersList;
        }

        public int GetSalaryForSim(int simCounter, int simValueDefault, int simValue2, int simValue3, int simValue4)
        {
            if (simCounter < simTarget1) return simCounter * simValueDefault;
            if (simCounter >= simTarget1 && simCounter < simTarget2) return simCounter * simValue2;
            if (simCounter >= simTarget2 && simCounter < simTarget3) return simCounter * simValue3;
            else return simCounter * simValue4;
        }

        public int GetSalaryForSellout(int soCounter, int soDefaultValue, int soValue2, int soValue3)
        {
            if (soCounter < soTarget2) return soCounter * soDefaultValue;
            if (soCounter >= soTarget2 && soCounter < soTarget3) return soCounter * soValue2;
            else return soCounter * soValue3;
        }

        public int GetVolForWalker(int volWalker, int volCommission, double totalVol)
        {
            var longTotalVol = (long)totalVol;
            var longVolWalker = (long)volWalker;
            var longVolComm = (long)volCommission;
            long result = (longVolWalker * longVolComm) / longTotalVol;

            return (int)result;
        }

        public static double GetTotalVol(string reportDealer)
        {
            double vol = 0;

            string[] arrayReportDealer = File.ReadAllLines(reportDealer);
            int reporDealer_len = arrayReportDealer.Length;

            for (int i = 5; i < reporDealer_len; i++)
            {
                string[] totalSolds = arrayReportDealer[i].Split(';');
                if (totalSolds[INDEX_DEALER_TOTAL].ToLower().Trim() == "total")
                {
                    vol += Convert.ToDouble(totalSolds[INDEX_DEALER_TOTALAMOUNT]);
                }
            }
            return vol;
        }

        public static Walker SetReport(ReportSettings reportSetting)
        {
            return new Walker()
            {
                FullName = reportSetting.WalkerName,
                ObjSim = reportSetting.SimCounter.ToString(),
                RewardSim = reportSetting.SimReward.ToString(),
                ObjSellOut = reportSetting.SoCounter.ToString(),
                RewardSo = reportSetting.SoReward.ToString(),
                Volumen = reportSetting.WalkerVol.ToString(),
                SoldVol = reportSetting.SoldVol.ToString(),
                Gift40percent = reportSetting.Reward40.ToString(),
                TotalReward = reportSetting.TotalSalary.ToString()
            };
        }
    }
}