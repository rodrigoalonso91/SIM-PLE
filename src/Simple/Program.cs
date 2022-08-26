using Newtonsoft.Json;
using Simple.Domain;
using Simple.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Simple
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var _databasePath = "D:\\proyects\\Dotnet\\SIM-PLE\\src\\Simple\\Licences\\database.json";
            var jsonText = File.ReadAllText(_databasePath);

            var database = JsonConvert.DeserializeObject<List<License>>(jsonText);

            var licenceForm = new LicenceForm(database, _databasePath);
            Application.Run(licenceForm);

            if (licenceForm.IsAuthorized)
            {
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show("Debe comprar una licencia para usar esta app. \nContacto: rodrigoalonso.dev@gmail.com");
            }
        }
    }
}