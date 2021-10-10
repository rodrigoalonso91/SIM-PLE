using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_PLE_2._0
{
    public class Report
    {
        private string ExportReport { get; set; }

        /// <summary>
        /// Devuelve los nombres de clientes de todos los elementos restantes en la grilla indicada
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public string CopyReport(DataGridView grid, string product = "sim")
        {
            if (product == "sim")
            {
                foreach (DataGridViewRow item in grid.Rows)
                {
                    var isOnGrid = item.Cells[4].Value.ToString() == "Ok";

                    if (isOnGrid)
                    {
                        ExportReport += $"{item.Cells[0].Value} \n";
                    }
                }
            }
            else if(product == "so")
            {
                foreach (DataGridViewRow item in grid.Rows)
                {
                    var isOnGrid = item.Cells[4].Value.ToString() == "Ok";

                    if (isOnGrid)
                    {
                        ExportReport += $"{item.Cells[0].Value}  |  Ventas hasta la fecha: {item.Cells[1].Value}\n";
                    }
                }
            }
            
            return ExportReport;
        }
    }
}
