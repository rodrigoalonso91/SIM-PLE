using Newtonsoft.Json;
using Simple.Domain;
using System;
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

        private void Btn_ValidateLicense_Click(object sender, EventArgs e)
        {
            var guid = Txtbox_Licence.Text.Trim();
            IsAuthorized = _database.Exists(obj => obj.Id == guid && !obj.IsActive && string.IsNullOrWhiteSpace(obj.MachineName));

            var updatedDatabase = _database.Select(obj => new License
            {
                Id = obj.Id,
                IsActive = obj.Id == guid,
                MachineName = obj.Id == guid ? Environment.MachineName.Trim() : "",
            });
            var jsonText = JsonConvert.SerializeObject(updatedDatabase, Formatting.Indented);
            File.WriteAllText(_databasePath, jsonText);
            Close();
        }
    }
}