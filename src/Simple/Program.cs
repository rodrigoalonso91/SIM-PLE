﻿using Newtonsoft.Json;
using Simple.Domain;
using Simple.Forms;
using Simple.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace Simple
{
    internal static class Program
    {
        private static readonly string _databasePath = ".\\Database\\database.json";
        private static readonly string _licenceMessage = "Debe adquirir una licencia para usar esta app. \nContacto: rodrigoalonso.dev@gmail.com";

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CkeckDatabaseFolder();
            var database = GetDatabase();

            var licenseTarget = database.Where(obj => obj.IsActive).ToList();
            bool ActiveLicense = licenseTarget.Exists(obj => obj.IsActive && obj.MachineName == Environment.MachineName);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ActiveLicense)
            {
                try
                {
                    if (!database.Any()) throw new Exception("No hay licencias disponibles");

                    var licenseForm = new LicenceForm(database, _databasePath);
                    Application.Run(licenseForm);
                    ActiveLicense = licenseForm.IsAuthorized;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.Concat($"\n{_licenceMessage}").ToString(), "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (ActiveLicense) Application.Run(new MainForm());
            else MessageBox.Show(_licenceMessage, "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private static void CkeckDatabaseFolder()
        {
            var databaseFolder = Path.GetDirectoryName(_databasePath);
            if (!Directory.Exists(databaseFolder)) Directory.CreateDirectory(_databasePath);
        }

        private static List<License> GetDatabase()
        {
            var jsonText = File.ReadAllText(_databasePath);
            return JsonConvert.DeserializeObject<List<License>>(jsonText);
        }
    }
}