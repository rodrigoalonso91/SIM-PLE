using Newtonsoft.Json;
using Simple.Domain;
using Simple.Forms;
using Simple.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simple
{
    internal static class Program
    {
        private static readonly string _databasePath = ".\\Licences\\database.json";

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var databaseFolder = Path.GetDirectoryName(_databasePath);
            if (!Directory.Exists(databaseFolder)) Directory.CreateDirectory(_databasePath);

            bool ActiveLicense = Settings.Default.Activated;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ActiveLicense)
            {
                var jsonText = File.ReadAllText(_databasePath);
                var database = JsonConvert.DeserializeObject<List<License>>(jsonText);

                var licenseForm = new LicenceForm(database, _databasePath);
                Application.Run(licenseForm);
                ActiveLicense = licenseForm.IsAuthorized;
            }

            if (ActiveLicense) Application.Run(new MainForm());
            else MessageBox.Show("Debe adquirir una licencia para usar esta app. \nContacto: rodrigoalonso.dev@gmail.com", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}