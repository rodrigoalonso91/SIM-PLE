using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SIM_PLE_2._0.Properties;

namespace SIM_PLE_2._0
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SelectWalker();
        }

        //================ Globals =================\\
        Dictionary<string, PSR> psrSIM = new Dictionary<string, PSR>();
        Dictionary<string, PSR> psrSellout = new Dictionary<string, PSR>();
        Dictionary<int, Walker> allEmployee = new Dictionary<int, Walker>();
        List<string> nonCompiliantSim = new List<string>();
        List<string> nonCompiliantSo = new List<string>();
        Stack<string> itemsUndoSim = new Stack<string>();
        Stack<string> itemsUndoSO = new Stack<string>();
  
        #region //========== METHODS ==========\\
        private void ShowSelloutInfo()
        {
            if (cb_walkers.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtBox_Sellout_objVenta.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpia toda la informacion relacionada al SellOut
            itemsUndoSO.Clear();
            dgv_So.Rows.Clear();
            nonCompiliantSo.Clear();
            psrSellout.Clear();

            var salesTarget = Convert.ToInt32(txtBox_Sellout_objVenta.Text);
            var walkerSelected = Convert.ToString(cb_walkers.SelectedItem);

            var selloutData = new PSR();
            psrSellout = selloutData.SetDataForSellout(txtbox_REP_psragencia.Text, txtBox_REPsellout_dealer.Text, txtbox_REP_productosVendidos.Text, walkerSelected, salesTarget);

            var psrTotales = psrSellout.Count;
            var objCumplido = 0;
            double walkerVolumen = 0;

            foreach (var psr in psrSellout.Values)
            {
                walkerVolumen += psr.MonthSolds;

                if (psr.IsCompliant) objCumplido++;
                else
                {
                    int rowIndex = dgv_So.Rows.Add();
                    dgv_So.Rows[rowIndex].Cells[0].Value = psr.Name;
                    dgv_So.Rows[rowIndex].Cells[1].Value = psr.MonthSolds;
                    dgv_So.Rows[rowIndex].Cells[2].Value = psr.Transactions;
                    dgv_So.Rows[rowIndex].Cells[3].Value = psr.Pos;
                    dgv_So.Rows[rowIndex].Cells[4].Value = "Ok";

                    nonCompiliantSo.Add(psr.Name);
                }
            }

            SO_psrTotales.Text = psrTotales.ToString();
            txtB_soConObjetivo.Text = objCumplido.ToString();
            txtBox_soFaltan.Text = nonCompiliantSo.Count().ToString();
            txtB_soVolumen.Text = "$" + walkerVolumen;
            int efectividad = (objCumplido * 100) / psrTotales;
            SO_efectividad.Text = efectividad + "%";
        }
        private bool PressNumber(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) return true;
            else return false;
        }
        private void SelectWalker() //asigna el mensaje "Elige caminante" a todos los ComboBOX
        {
            cb_walkers.Text = "Elija caminante";
        }
        private void EnableCalculate() //Metodo para habilitar botones de calcular
        {
            if (txtbox_REP_psragencia.Text != "" && txtbox_REP_pRecarga.Text != "" && txtbox_REP_productosVendidos.Text != "")
            {
                btn_SIM_calcular.Enabled = true;
                btn_SIM_calcular.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
            if (txtbox_REP_psragencia.Text != "" && txtBox_REPsellout_dealer.Text != "" && txtbox_REP_productosVendidos.Text != "")
            {
                btn_calcularSellout.Enabled = true;
                btn_calcularSellout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
            if (txtbox_REP_pRecarga.Text.ToLower().Contains("primera"))
            {
                btn_calcularPremios.Enabled = true;
                btn_calcularPremios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
        }
        public void ModCounters_Sim(string counterValue, int currentRow)
        {
            string itemSelected = dgv_Sim.Rows[currentRow].Cells[0].Value.ToString();
            nonCompiliantSim.Remove(itemSelected);
            itemsUndoSim.Push(itemSelected);
            //Aumento y decremento de contadores
            txtBox_SimConObj.Text = IncrementCounter(counterValue);
            txtBox_faltaCumplir.Text = nonCompiliantSim.Count.ToString();
            // Modificacion de inversion
            int salesTarget = Convert.ToInt32(txtbox_montoObjSIM.Text);
            int currentSale = Convert.ToInt32(dgv_Sim.Rows[currentRow].Cells[1].Value);
            int newInversion = int.Parse(txtBox_inver.Text.Replace("$", String.Empty)) - (salesTarget - currentSale);
            txtBox_inver.Text = "$" + Convert.ToString(newInversion);
            //Modificacion de efectividad
            txtBox_Efectividad.Text = ((int.Parse(txtBox_SimConObj.Text) * 100) / int.Parse(txtBox_PSRTotales.Text)) + "%";
        }
        public string IncrementCounter(string counter) // Incrementa el valor en uno [string]
        {
            int num = int.Parse(counter);
            num++;
            return num.ToString(); ;
        }
        public string ReduceCounter(string counter) // Decremente el valor en uno [string]
        {
            int num = int.Parse(counter);
            num--;
            return num.ToString();
        }
        public void ModContadores_SO(string counterValue, int currentRow)
        {
            // Seleccion y movimiento de item
            string itemSelected = dgv_So.Rows[currentRow].Cells[0].Value.ToString();
            nonCompiliantSo.Remove(itemSelected);
            itemsUndoSO.Push(itemSelected);

            // Aumento y decremento de contadores
            txtB_soConObjetivo.Text = IncrementCounter(counterValue);
            txtBox_soFaltan.Text = nonCompiliantSo.Count.ToString();

            // Modificacion Efectividad SO
            SO_efectividad.Text = ((int.Parse(txtB_soConObjetivo.Text) * 100) / int.Parse(SO_psrTotales.Text))+ "%";
        }
        #endregion

        #region //========== BTNs LEFT PANEL ==========\\
        private void btn_tabSIM_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1Reportes;
        }
        private void btn_tabSellout_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2Sellout;
        }
        private void btn_tabBO_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3SIM;
        }
        private void btn_tabPremios40_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tp_premios40;
        }
        #endregion

        #region //========== BOTONES EXAMINAR EN PESTAÑA REPORTES ==========\\

        private void btn_Examinar1_Click(object sender, EventArgs e)
        {
            //encuentra la ruta de un archivo.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_REP_psragencia.Text = openFileDialog1.FileName;

                if (txtbox_REP_psragencia.Text.ToLower().Contains("agencia"))
                {
                    EnableCalculate();
                    foreach (var walker in Walker.GetWalkerFromReport(txtbox_REP_psragencia.Text)) 
                        cb_walkers.Items.Add(walker);
                }
                else
                {
                    MessageBox.Show("Asegurate de estar seleccionando el reporte PSR de la Agencia", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }  
        private void btn_Examinar2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_REP_pRecarga.Text = openFileDialog1.FileName;
            }
            EnableCalculate();
        }
        private void btn_Examinar3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_REP_productosVendidos.Text = openFileDialog1.FileName;
            }
            EnableCalculate();
        }
        private void examinar_sellout_dealer_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBox_REPsellout_dealer.Text = openFileDialog1.FileName;
            }
            EnableCalculate();
        }
        #endregion

        #region //=========== EVENTOS KEYPRESS QUE SOLO ACEPTAN NUMEROS ==========\\
        private void txtbox_montoObjSIM_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtBox_Sellout_objVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_maxPorCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_maxPorCaminante_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_defaultValueSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_100ValueSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_120ValueSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_150ValueSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_targetVol_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_commissionVol_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_150ValueSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_120ValueSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_100ValueSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        private void txtbox_defaultValueSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PressNumber(e);
        }
        #endregion

        #region // ======= Settings ====== \\
        private void Form1_Load(object sender, EventArgs e)
        {
            txtbox_montoObjSIM.Text = (string)Settings.Default["objSim"];
            txtBox_Sellout_objVenta.Text = (string)Settings.Default["objSO"];
            txtbox_maxWalker.Text = (string)Settings.Default["maxCaminante"];
            txtbox_maxClient.Text = (string)Settings.Default["maxCliente"];
            txtbox_defaultValueSim.Text = (string)Settings.Default["simValue"];
            txtbox_100ValueSim.Text = (string)Settings.Default["sim100"];
            txtbox_120ValueSim.Text = (string)Settings.Default["sim120"];
            txtbox_150ValueSim.Text = (string)Settings.Default["sim150"];
            txtbox_defaultValueSo.Text = (string)Settings.Default["soValue"];
            txtbox_100ValueSo.Text = (string)Settings.Default["so100"];
            txtbox_120ValueSo.Text = (string)Settings.Default["so120"];
            txtbox_150ValueSo.Text = (string)Settings.Default["so150"];
            txtbox_commissionVol.Text = (string)Settings.Default["volumen"];
            txtbox_targetVol.Text = (string)Settings.Default["volumenTarget"];
        }
        private void txtbox_montoObjSIM_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSim"] = txtbox_montoObjSIM.Text;
            Settings.Default.Save();
        }
        private void txtbox_maxPorCliente_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCliente"] = txtbox_maxClient.Text;
            Settings.Default.Save();
        }
        private void txtbox_maxPorCaminante_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCaminante"] = txtbox_maxWalker.Text;
            Settings.Default.Save();
        }
        private void txtBox_Sellout_objVenta_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSO"] = txtBox_Sellout_objVenta.Text;
            Settings.Default.Save();
        }
        private void txtbox_defaultValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["simValue"] = txtbox_defaultValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_100ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim100"] = txtbox_100ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_120ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim120"] = txtbox_120ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_150ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim150"] = txtbox_150ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_defaultValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["soValue"] = txtbox_defaultValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_100ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["so100"] = txtbox_100ValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_120ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["so120"] = txtbox_120ValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_150ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["so150"] = txtbox_150ValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_targetVol_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["volumenTarget"] = txtbox_targetVol.Text;
            Settings.Default.Save();
        }

        private void txtbox_commissionVol_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["volumen"] = txtbox_commissionVol.Text;
            Settings.Default.Save();
        }
        #endregion

        private void btn_SIM_calcular_Click_1(object sender, EventArgs e) 
        {
            if (cb_walkers.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtbox_montoObjSIM.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ===== Se limpia la informacion relacionada a los sims ===== \\
            itemsUndoSim.Clear();
            dgv_Sim.Rows.Clear();
            psrSIM.Clear();
            nonCompiliantSim.Clear();
            // ================================================== \\

            var salesTarget = int.Parse(txtbox_montoObjSIM.Text);
            var walkerSelected = Convert.ToString(cb_walkers.SelectedItem);

            var simData = new PSR();
            psrSIM = simData.SetDataForSims(txtbox_REP_psragencia.Text, txtbox_REP_pRecarga.Text, txtbox_REP_productosVendidos.Text, walkerSelected, salesTarget);

            var objCumplido = 0;
            var investment = 0;

            foreach (var psr in psrSIM.Values)
            {
                if (psr.IsCompliant) objCumplido++;
                else
                {
                    // Carga de datos en DGV
                    int rowsIndex = dgv_Sim.Rows.Add();
                    dgv_Sim.Rows[rowsIndex].Cells[0].Value = psr.Name;
                    dgv_Sim.Rows[rowsIndex].Cells[1].Value = psr.FirstCharge;
                    dgv_Sim.Rows[rowsIndex].Cells[2].Value = psr.ClientNim;
                    dgv_Sim.Rows[rowsIndex].Cells[3].Value = psr.Lote;
                    dgv_Sim.Rows[rowsIndex].Cells[4].Value = "Ok";

                    investment += (salesTarget - psr.FirstCharge);
                    nonCompiliantSim.Add(psr.Name);
                }
            }

            int psrTotal = psrSIM.Count;
            int efectividad = (objCumplido * 100) / psrTotal;
            txtBox_PSRTotales.Text = Convert.ToString(psrTotal);
            txtBox_SimConObj.Text = Convert.ToString(objCumplido);
            txtBox_faltaCumplir.Text = Convert.ToString(nonCompiliantSim.Count);
            txtBox_inver.Text = "$" + investment;
            txtBox_Efectividad.Text = efectividad + "%";
        } 
        private void btn_calcularSellout_Click(object sender, EventArgs e)
        {
            ShowSelloutInfo();
        }


        private void btn_clipBoard_Click(object sender, EventArgs e) //Codigo para reporte en ClipBoard
        {
            //FIXME:
            if (!txtBox_PSRTotales.Text.Contains('-') || !SO_psrTotales.Text.Contains('-'))
            {
                Clipboard.Clear();
              //string[] noCumplidores_SIM = listBox_SIM.Items.OfType<string>().ToArray();
              //string[] noCumplidores_SO = listBox_Sellout.Items.OfType<string>().ToArray();
                string strNoCumplidores_Sim = "";
                string strNoCumplidores_SO = "";
                string cumObjetivo_SIM = txtBox_SimConObj.Text;
                string simFaltantes = txtBox_faltaCumplir.Text;
                string cumObjetivo_SO = txtB_soConObjetivo.Text;
                string soFaltantes = txtBox_soFaltan.Text;

                string saltoDeLinea = Environment.NewLine;
                string tituloReporte = "SIM-PLE Reporte: " + cb_walkers.SelectedItem;
                string linea1 = "Obj. SIM: " + cumObjetivo_SIM + " PSR";
                string linea2 = "Faltan " + simFaltantes + " psr: ";
                string linea3 = "tu volumen es: " + txtB_soVolumen.Text;
                string linea4 = "Obj. SO: " + cumObjetivo_SO;
                string linea5 = "Faltan " + soFaltantes + " psr: ";
                int j = 1;
             /* foreach (string items in noCumplidores_SIM)
                {
                    strNoCumplidores_Sim += saltoDeLinea + "    " + Convert.ToString(j) + ") " + items;
                    j++;
                }
                j = 1;
                foreach (string items in noCumplidores_SO)
                {
                    strNoCumplidores_SO += saltoDeLinea + "    " + Convert.ToString(j) + ") " + items;
                    j++;
                }
             */
                string reporte = tituloReporte + saltoDeLinea + String.Empty + saltoDeLinea + linea1 + saltoDeLinea + String.Empty + saltoDeLinea  + linea2 + strNoCumplidores_Sim + saltoDeLinea + String.Empty + saltoDeLinea + linea3
                    + saltoDeLinea + String.Empty + saltoDeLinea + linea4 + saltoDeLinea + String.Empty + saltoDeLinea + linea5 + strNoCumplidores_SO;

                Clipboard.SetText(reporte);
                MessageBox.Show("Los resultados se copiaron en el portapapeles :)", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Primero debes calcular todos los objetivos", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void iconButton1_Click(object sender, EventArgs e) //Codigo para exportacion de Reporte formato (.txt)
        {
            //FIXME:
            if (!txtBox_PSRTotales.Text.Contains('-') || !SO_psrTotales.Text.Contains('-'))
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                  //string[] noCumplidores_SIM = listBox_SIM.Items.OfType<string>().ToArray();
                  //string[] noCumplidores_SO = listBox_Sellout.Items.OfType<string>().ToArray();
                    string rutaGuardado = saveFileDialog1.FileName + ".txt";
                    string cumObjetivo_SIM = txtBox_SimConObj.Text;
                    string simFaltantes = txtBox_faltaCumplir.Text;
                    string cumObjetivo_SO = txtB_soConObjetivo.Text;
                    string soFaltantes = txtBox_soFaltan.Text;

                    string tituloReporte = "SIM-PLE Reporte: " + cb_walkers.SelectedItem;
                    string lineaVacia = "";
                    string linea1 = "Obj. SIM: " + cumObjetivo_SIM + " PSR";
                    string linea2 = "Faltan "+ simFaltantes+" psr: ";
                    string linea3 = "tu volumen es: " + txtB_soVolumen.Text;
                    string linea4 = "Obj. SO: " + cumObjetivo_SO;
                    string linea5 = "Faltan " + soFaltantes + " psr: ";
                    int j = 1;

                    using (StreamWriter file = new StreamWriter(rutaGuardado))
                    {
                        file.WriteLine(tituloReporte);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea1);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea2);
                        file.WriteLine(lineaVacia);
                     /* foreach (string items in noCumplidores_SIM)
                        {
                            file.WriteLine(Convert.ToString(j)+") "+items);
                            j++;
                        }*/
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea3);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea4);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea5);
                        file.WriteLine(lineaVacia);
                        j = 1;
                       /* foreach (string items in noCumplidores_SO)
                        {
                            file.WriteLine(Convert.ToString(j) + ") " + items);
                            j++;
                        }*/
                    }
                    MessageBox.Show("Se ha guardado su resultado.","SIM-PLE",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Primero debes calcular todos los objetivos","SIM-PLE",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void ShowSalary() //TODO: Mover a seccion de metodos una vez terminado.
        {
            bool txtboxNotCompleted = String.IsNullOrWhiteSpace(txtbox_defaultValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_100ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_120ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_150ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_defaultValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_100ValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_120ValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_150ValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_maxClient.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_maxClient.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_commissionVol.Text) ||
                                      String.IsNullOrWhiteSpace(txtbox_targetVol.Text);

            if (txtboxNotCompleted)
            {
                MessageBox.Show("Debe ingresar los valores de comisiones.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!txtbox_REP_pRecarga.Text.ToLower().Contains("primera"))
            {
                MessageBox.Show("El reporte ingresado en 'Primera Recargas' no es correcto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            //========================== Reward 40% Section ==========================\\
            dgv_reward40.Rows.Clear();
                
            int maxValueClient = int.Parse(txtbox_maxClient.Text);
            int maxValueWalker = int.Parse(txtbox_maxWalker.Text);

            var rewardSim = new PSR();
            var employedRewards = rewardSim.SetDataForRewards(txtbox_REP_pRecarga.Text, maxValueClient, maxValueWalker);

            foreach (var reward in employedRewards.Values)
            {
                int rowCount = dgv_reward40.Rows.Add();
                dgv_reward40.Rows[rowCount].Cells[0].Value = reward.Walker;
                dgv_reward40.Rows[rowCount].Cells[1].Value = "$ " + reward.WalkerReward;
                dgv_reward40.Rows[rowCount].Cells[2].Value = reward.TotalSim + " Sims";
                dgv_reward40.Rows[rowCount].Cells[3].Value = "$ " + reward.Amount;
            }

            var arrEmployee = new string[cb_walkers.Items.Count];
            for (int i = 0; i < cb_walkers.Items.Count; i++) arrEmployee[i] = cb_walkers.Items[i].ToString();

        }
        private void btn_calcularPremios_Click(object sender, EventArgs e)
        {
            ShowSalary();
        }
        private void dgv_Sim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellTarget = e.ColumnIndex;
            int rowTarget = e.RowIndex;

            if (rowTarget != -1 && cellTarget == 4 && dgv_Sim.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Ok")
            {
                ModCounters_Sim(txtBox_SimConObj.Text, rowTarget);
                dgv_Sim.Rows[rowTarget].Cells[cellTarget].Value = "Undo";

                DataGridViewCellStyle rowOff = new DataGridViewCellStyle();
                rowOff.BackColor = Color.LightSeaGreen;
                rowOff.ForeColor = Color.WhiteSmoke;

                dgv_Sim.Rows[rowTarget].DefaultCellStyle = rowOff;
            }
            else if (rowTarget != -1 && cellTarget == 4 && dgv_Sim.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Undo")
            {
                dgv_Sim.Rows[rowTarget].Cells[cellTarget].Value = "Ok";
                DataGridViewCellStyle rowOn = new DataGridViewCellStyle();
                rowOn.BackColor = Color.White;
                rowOn.ForeColor = Color.Black;
                dgv_Sim.Rows[rowTarget].DefaultCellStyle = rowOn;

                if (itemsUndoSim.Count() > 0)
                {
                    string itemRestored = itemsUndoSim.Pop();
                    nonCompiliantSim.Add(itemRestored);
                    txtBox_faltaCumplir.Text = nonCompiliantSim.Count().ToString();

                    txtBox_SimConObj.Text = ReduceCounter(txtBox_SimConObj.Text);

                    int psrTotales = int.Parse(txtBox_PSRTotales.Text);
                    int efectividad = (int.Parse(txtBox_SimConObj.Text) * 100) / psrTotales;
                    txtBox_Efectividad.Text = efectividad + "%";

                    int invercionActual = int.Parse(txtBox_inver.Text.Replace("$", String.Empty));
                    int recargeValue = int.Parse(txtbox_montoObjSIM.Text) - Convert.ToInt32(dgv_Sim.Rows[rowTarget].Cells[1].Value);
                    txtBox_inver.Text = "$" + (invercionActual + recargeValue);
                }
            }
        }
        private void dgv_So_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellTarget = e.ColumnIndex;
            int rowTarget = e.RowIndex;

            if (rowTarget != -1 && cellTarget == 4 && dgv_So.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Ok")
            {
                ModContadores_SO(txtB_soConObjetivo.Text, rowTarget);
                dgv_So.Rows[rowTarget].Cells[cellTarget].Value = "Undo";

                DataGridViewCellStyle rowOff = new DataGridViewCellStyle();
                rowOff.BackColor = Color.LightSeaGreen;
                rowOff.ForeColor = Color.WhiteSmoke;

                dgv_So.Rows[rowTarget].DefaultCellStyle = rowOff;
            }
            else if (rowTarget != -1 && cellTarget == 4 && dgv_So.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Undo")
            {
                dgv_So.Rows[rowTarget].Cells[cellTarget].Value = "Ok";
                DataGridViewCellStyle rowOn = new DataGridViewCellStyle();
                rowOn.BackColor = Color.White;
                rowOn.ForeColor = Color.Black;
                dgv_So.Rows[rowTarget].DefaultCellStyle = rowOn;

                if (itemsUndoSO.Count > 0)
                {
                    string itemRestored = itemsUndoSO.Pop();
                    nonCompiliantSo.Add(itemRestored);
                    txtB_soConObjetivo.Text = ReduceCounter(txtB_soConObjetivo.Text);
                    txtBox_soFaltan.Text = nonCompiliantSo.Count().ToString();
                    int psrTotal = int.Parse(SO_psrTotales.Text);
                    SO_efectividad.Text = (int.Parse(txtB_soConObjetivo.Text) *100) / psrTotal + "%";

                }
            }
        }


        //FIXME: No funciona como lo espero se ejecuta el metodo cuando hago foco y no cuando hago click.
        private void btn_calcularSellout_Enter(object sender, EventArgs e)
        {
            //ShowSelloutInfo();
        }

        private void txtBox_Sellout_objVenta_Enter(object sender, EventArgs e)
        {
            //ShowSelloutInfo();
        }

        
    }

}
