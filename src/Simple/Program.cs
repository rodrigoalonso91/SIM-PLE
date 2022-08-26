using Newtonsoft.Json;
using Simple.Domain;
using Simple.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            if (database is null || database.Count == 0)
            {
                MessageBox.Show(_licenceMessage, "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var isAuthorized = database.Where(obj => obj.IsActive).ToList()
                                       .Exists(obj => obj.IsActive && obj.MachineName == Environment.MachineName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!isAuthorized)
            {
                var licenseForm = new LicenceForm(database, _databasePath);
                Application.Run(licenseForm);
                isAuthorized = licenseForm.IsAuthorized;
            }

            if (isAuthorized) Application.Run(new MainForm());
            else MessageBox.Show(_licenceMessage, "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private static void CkeckDatabaseFolder()
        {
            var databaseFolder = Path.GetDirectoryName(_databasePath);
            if (!Directory.Exists(databaseFolder))
            {
                Directory.CreateDirectory(databaseFolder);
                File.Create(_databasePath).Close();
            }
        }

        private static List<License> GetDatabase()
        {
            var jsonText = File.ReadAllText(_databasePath);
            return JsonConvert.DeserializeObject<List<License>>(jsonText);
        }
    }
}