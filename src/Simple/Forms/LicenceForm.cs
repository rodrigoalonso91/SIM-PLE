using Newtonsoft.Json;
using Simple.Domain;
using Simple.Properties;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Simple.Forms
{
    public partial class LicenceForm : Form
    {
        public bool IsAuthorized { get; private set; } = false;
        private readonly string _databasePath;
        private readonly List<License> _database = new List<License>();

        public LicenceForm(List<License> database, string databasePath)
        {
            InitializeComponent();
            _database = database;
            _databasePath = databasePath;
        }

        private void Btn_Licence_Click(object sender, System.EventArgs e)
        {
            var guid = Txtbox_Licence.Text.Trim();
            IsAuthorized = _database.Exists(obj => obj.Id == guid);

            var updatedDatabase = _database.Where(obj => obj.Id != guid);
            var jsonText = JsonConvert.SerializeObject(updatedDatabase, Formatting.Indented);
            File.WriteAllText(_databasePath, jsonText);

            Settings.Default.Activated = IsAuthorized;
            Settings.Default.Save();
            Close();
        }
    }
}