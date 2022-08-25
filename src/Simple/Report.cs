using System.Windows.Forms;

namespace Simple
{
    public class Report
    {
        private string ExportReport { get; set; }

        /// <summary>
        /// Devuelve los nombres de clientes de todos los elementos restantes en la grilla indicada
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="product"></param>
        public string CopyReport(DataGridView grid, string product = "sim")
        {
            if (product == "sim")
            {
                foreach (DataGridViewRow item in grid.Rows)
                {
                    var isOnGrid = item.Cells[4].Value.ToString() == "Ok";

                    if (isOnGrid) ExportReport += $"{item.Cells[0].Value} \n";
                }
            }
            else if (product == "so")
            {
                foreach (DataGridViewRow item in grid.Rows)
                {
                    var isOnGrid = item.Cells[4].Value.ToString() == "Ok";

                    if (isOnGrid) ExportReport += $"{item.Cells[0].Value}  |  Vendido hasta la fecha: ${item.Cells[1].Value}\n";
                }
            }
            return ExportReport;
        }
    }
}