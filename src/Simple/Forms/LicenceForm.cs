using Newtonsoft.Json;
using Simple.Domain;
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

            var updatedDatabase = _database.Select(l => new License
            {
                Id = l.Id,
                IsActive = l.Id == guid,
            });

            var license = updatedDatabase.Where(l => l.Id.Equals(guid)).First();
            IsAuthorized = license.IsActive;

            var jsonText = JsonConvert.SerializeObject(updatedDatabase);
            File.WriteAllText(_databasePath, jsonText);

            Close();
        }
    }
}