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
       private Dictionary<string, PSR> psrSIM = new Dictionary<string, PSR>();
       private Dictionary<string, PSR> psrSellout = new Dictionary<string, PSR>();
       private List<string> nonCompiliantSim = new List<string>();
       private List<string> nonCompiliantSo = new List<string>();
       private Stack<string> itemsUndoSim = new Stack<string>();
       private Stack<string> itemsUndoSO = new Stack<string>();
       private double totalVol;
       private Walker report = new Walker();
        //=============== End Globals ================\\

        #region //========== METHODS ==========\\
        private void ShowSelloutInfo()
        {
            if (Cb_Walkers.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Txtbox_SaleTarjet_so.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Limpia toda la informacion relacionada al SellOut
            itemsUndoSO.Clear();
            DgvSellout.Rows.Clear();
            nonCompiliantSo.Clear();
            psrSellout.Clear();

            var salesTarget = Convert.ToInt32(Txtbox_SaleTarjet_so.Text);
            var walkerSelected = Convert.ToString(Cb_Walkers.SelectedItem);

            var selloutData = new PSR();
            psrSellout = selloutData.SetDataForSellout(Txtbox_ReportAgency.Text, TxtBox_ReportDealer.Text, Txtbox_ReportSoldProducts.Text, walkerSelected, salesTarget);

            var psrTotales = psrSellout.Count;
            var objCumplido = 0;
            double walkerVolumen = 0;

            foreach (var psr in psrSellout.Values)
            {
                walkerVolumen += psr.MonthSolds;

                if (psr.IsCompliant) objCumplido++;
                else
                {
                    int rowIndex = DgvSellout.Rows.Add();
                    DgvSellout.Rows[rowIndex].Cells[0].Value = psr.Name;
                    DgvSellout.Rows[rowIndex].Cells[1].Value = psr.MonthSolds;
                    DgvSellout.Rows[rowIndex].Cells[2].Value = psr.Transactions;
                    DgvSellout.Rows[rowIndex].Cells[3].Value = psr.Pos;
                    DgvSellout.Rows[rowIndex].Cells[4].Value = "Ok";

                    nonCompiliantSo.Add(psr.Name);
                }
            }

            CounterTotalPsr_so.Text = psrTotales.ToString();
            CounterObj_so.Text = objCumplido.ToString();
            CounterFail_so.Text = nonCompiliantSo.Count().ToString();
            CounterVol_so.Text = "$" + walkerVolumen;
            int efectividad = (objCumplido * 100) / psrTotales;
            CounterEffectiveness_so.Text = efectividad + "%";
        }
        private void ShowSimInfo()
        {
            if (Cb_Walkers.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Txtbox_SaleTarget_sim.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ===== Se limpia la informacion relacionada a los sims ===== \\
            itemsUndoSim.Clear();
            DgvSim.Rows.Clear();
            psrSIM.Clear();
            nonCompiliantSim.Clear();
            // ================================================== \\

            var salesTarget = int.Parse(Txtbox_SaleTarget_sim.Text);
            var walkerSelected = Convert.ToString(Cb_Walkers.SelectedItem);

            var simData = new PSR();
            psrSIM = simData.SetDataForSims(Txtbox_ReportAgency.Text, Txtbox_ReportFirstCharges.Text, Txtbox_ReportSoldProducts.Text, walkerSelected, salesTarget);

            var objCumplido = 0;
            var investment = 0;

            foreach (var psr in psrSIM.Values)
            {
                if (psr.IsCompliant) objCumplido++;
                else
                {
                    // Carga de datos en DGV
                    int rowsIndex = DgvSim.Rows.Add();
                    DgvSim.Rows[rowsIndex].Cells[0].Value = psr.Name;
                    DgvSim.Rows[rowsIndex].Cells[1].Value = psr.FirstCharge;
                    DgvSim.Rows[rowsIndex].Cells[2].Value = psr.ClientNim;
                    DgvSim.Rows[rowsIndex].Cells[3].Value = psr.Lote;
                    DgvSim.Rows[rowsIndex].Cells[4].Value = "Ok";

                    investment += (salesTarget - psr.FirstCharge);
                    nonCompiliantSim.Add(psr.Name);
                }
            }

            int psrTotal = psrSIM.Count;
            int efectividad = (objCumplido * 100) / psrTotal;
            CounterTotalPsr_sim.Text = Convert.ToString(psrTotal);
            CounterObj_sim.Text = Convert.ToString(objCumplido);
            CounterFail_sim.Text = Convert.ToString(nonCompiliantSim.Count);
            CounterInver_sim.Text = "$" + investment;
            CounterEffectiveness_sim.Text = efectividad + "%";
        }
        private void ShowSalary() 
        {
            bool txtboxNotCompleted = String.IsNullOrWhiteSpace(Txtbox_defaultValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_100ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_120ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_150ValueSim.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_defaultValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_requiredPsr.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_120ValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_150ValueSo.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_maxClient.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_maxClient.Text) ||
                                      String.IsNullOrWhiteSpace(Txtbox_commissionVol.Text);

            if (txtboxNotCompleted)
            {
                MessageBox.Show("Debe ingresar los valores de comisiones.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Txtbox_ReportFirstCharges.Text.ToLower().Contains("primera"))
            {
                MessageBox.Show("El reporte ingresado en 'Primera Recargas' no es correcto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //========================== Reward 40% Section ==========================\\
            DgvReward40.Rows.Clear();
                
            int maxValueClient = int.Parse(Txtbox_maxClient.Text);
            int maxValueWalker = int.Parse(Txtbox_maxWalker.Text);

            var walkerName = Cb_Walkers.SelectedItem.ToString();

            var rewardSim = new PSR();
            var employedRewards = rewardSim.SetDataForRewards(Txtbox_ReportFirstCharges.Text, maxValueClient, maxValueWalker);
            int reward40SelectWalker = 0;
            foreach (var reward in employedRewards.Values)
            {
                if (walkerName == reward.Walker)
                    reward40SelectWalker = reward.WalkerReward;

                int rowCount = DgvReward40.Rows.Add();
                DgvReward40.Rows[rowCount].Cells[0].Value = reward.Walker;
                DgvReward40.Rows[rowCount].Cells[1].Value = reward.TotalSim + " Sims";
                DgvReward40.Rows[rowCount].Cells[2].Value = "$ " + reward.Amount;
                DgvReward40.Rows[rowCount].Cells[3].Value = "$ " + reward.WalkerReward;
            }
            //========================================================================\\

            //========================== Salary Section ==========================\\
            if (CounterObj_sim.Text == "-" || CounterObj_so.Text == "-")
            {
                MessageBox.Show("Debe calcurar los objetivos Sim y Sellout para calcular el salario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var walkerSalary = new Walker();

            var simCounter = int.Parse(CounterObj_sim.Text);
            var simDefaultValue = int.Parse(Txtbox_defaultValueSim.Text);
            var sim100 = int.Parse(Txtbox_100ValueSim.Text);
            var sim120 = int.Parse(Txtbox_120ValueSim.Text);
            var sim150 = int.Parse(Txtbox_150ValueSim.Text);

            var simReward = walkerSalary.GetSalaryForSim(simCounter, simDefaultValue, sim100, sim120, sim150);

            var soCounter = int.Parse(CounterObj_so.Text);
            var soDefaultValue = int.Parse(Txtbox_defaultValueSo.Text);
            var so120 = int.Parse(Txtbox_120ValueSo.Text);
            var so150 = int.Parse(Txtbox_150ValueSo.Text);

            var soReward = walkerSalary.GetSalaryForSellout(soCounter, soDefaultValue, so120, so150);

            //Quita el signo $
            var volCounter = CounterVol_so.Text.Substring(1);
            var soldVol = int.Parse(volCounter);
            //var volTarget = int.Parse(txtbox_targetVol.Text);
            var volCommission = int.Parse(Txtbox_commissionVol.Text);
            var requiredPsr = int.Parse(Txtbox_requiredPsr.Text);

            var walkerVol = walkerSalary.GetVolForWalker(soldVol, volCommission, totalVol);
            if (simCounter < requiredPsr || soCounter < requiredPsr) walkerVol = 0;

            DgvSalary.Rows.Clear();
            int totalSalary = simReward + soReward + walkerVol + reward40SelectWalker;

            var reportSetting = new ReportSettings
            {
                WalkerName = walkerName,
                SimCounter = simCounter,
                SimReward = simReward,
                SoCounter = soCounter,
                SoReward = soReward,
                WalkerVol = walkerVol,
                SoldVol = soldVol,
                Reward40 = reward40SelectWalker,
                TotalSalary = totalSalary
            };

            report = Walker.SetReport(reportSetting);

            int rowCount2 = DgvSalary.Rows.Add();
            DgvSalary.Rows[rowCount2].Cells[0].Value = simReward;
            DgvSalary.Rows[rowCount2].Cells[1].Value = soReward;
            DgvSalary.Rows[rowCount2].Cells[2].Value = walkerVol;
            DgvSalary.Rows[rowCount2].Cells[3].Value = reward40SelectWalker;
            DgvSalary.Rows[rowCount2].Cells[4].Value = totalSalary;
        }
        private bool PressNumber(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) return true;
            else return false;
        }
        private void SelectWalker() //asigna el mensaje "Elige caminante" a todos los ComboBOX
        {
            Cb_Walkers.Text = "Elija caminante";
        }
        private void EnableCalculate() //Metodo para habilitar botones de calcular
        {
            if (Txtbox_ReportAgency.Text != "" && Txtbox_ReportFirstCharges.Text != "" && Txtbox_ReportSoldProducts.Text != "")
            {
                Btn_Calculate_sim.Enabled = true;
                Btn_Calculate_sim.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
            if (Txtbox_ReportAgency.Text != "" && TxtBox_ReportDealer.Text != "" && Txtbox_ReportSoldProducts.Text != "")
            {
                Btn_Calculate_so.Enabled = true;
                Btn_Calculate_so.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
            if (Txtbox_ReportFirstCharges.Text.ToLower().Contains("primera"))
            {
                Btn_CalculateRewards.Enabled = true;
                Btn_CalculateRewards.FlatAppearance.BorderColor = Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
        }
        public void ModCounters_Sim(string counterValue, int currentRow)
        {
            string itemSelected = DgvSim.Rows[currentRow].Cells[0].Value.ToString();
            nonCompiliantSim.Remove(itemSelected);
            itemsUndoSim.Push(itemSelected);
            //Aumento y decremento de contadores
            CounterObj_sim.Text = IncrementCounter(counterValue);
            CounterFail_sim.Text = nonCompiliantSim.Count.ToString();
            // Modificacion de inversion
            int salesTarget = Convert.ToInt32(Txtbox_SaleTarget_sim.Text);
            int currentSale = Convert.ToInt32(DgvSim.Rows[currentRow].Cells[1].Value);
            int newInversion = int.Parse(CounterInver_sim.Text.Replace("$", String.Empty)) - (salesTarget - currentSale);
            CounterInver_sim.Text = "$" + Convert.ToString(newInversion);
            //Modificacion de efectividad
            CounterEffectiveness_sim.Text = ((int.Parse(CounterObj_sim.Text) * 100) / int.Parse(CounterTotalPsr_sim.Text)) + "%";
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
            string itemSelected = DgvSellout.Rows[currentRow].Cells[0].Value.ToString();
            nonCompiliantSo.Remove(itemSelected);
            itemsUndoSO.Push(itemSelected);

            // Aumento y decremento de contadores
            CounterObj_so.Text = IncrementCounter(counterValue);
            CounterFail_so.Text = nonCompiliantSo.Count.ToString();

            // Modificacion Efectividad SO
            CounterEffectiveness_so.Text = ((int.Parse(CounterObj_so.Text) * 100) / int.Parse(CounterTotalPsr_so.Text))+ "%";
        }
        #endregion

        #region //========== Left Panel ==========\\
        private void btn_tabSIM_Click_1(object sender, EventArgs e)
        {
            TabControl_Main.SelectedTab = TabPage_Reports;
        }
        private void btn_tabSellout_Click(object sender, EventArgs e)
        {
            TabControl_Main.SelectedTab = TabPage_Sellout;
        }
        private void btn_tabBO_Click(object sender, EventArgs e)
        {
            TabControl_Main.SelectedTab = TabPage_Sim;
        }
        private void btn_tabPremios40_Click(object sender, EventArgs e)
        {
            TabControl_Main.SelectedTab = TabPage_Reward40;
        }
        #endregion

        #region //========== Buttons "Examinar" ==========\\
        private void Btn_Examinar1_Click(object sender, EventArgs e)
        {
            //encuentra la ruta de un archivo.
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Txtbox_ReportAgency.Text = OpenFileDialog1.FileName;

                if (Txtbox_ReportAgency.Text.ToLower().Contains("agencia"))
                {
                    EnableCalculate();
                    Cb_Walkers.Items.Clear();
                    foreach (var walker in Walker.GetWalkerFromReport(Txtbox_ReportAgency.Text)) 
                        Cb_Walkers.Items.Add(walker);
                }
                else
                {
                    MessageBox.Show("Asegurate de estar seleccionando el reporte PSR de la Agencia", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }  
        private void btn_Examinar2_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Txtbox_ReportFirstCharges.Text = OpenFileDialog1.FileName;
            }
            EnableCalculate();
        }
        private void btn_Examinar3_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Txtbox_ReportSoldProducts.Text = OpenFileDialog1.FileName;
            }
            EnableCalculate();
        }
        private void examinar_sellout_dealer_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtBox_ReportDealer.Text = OpenFileDialog1.FileName;
                totalVol = Walker.GetTotalVol(TxtBox_ReportDealer.Text);
                Txtbox_sendVol.Text = "$ " + totalVol;
            }
            EnableCalculate();
        }
        #endregion

        #region //=========== KeyPress: Only numbers ==========\\
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
            Txtbox_SaleTarget_sim.Text = (string)Settings.Default["objSim"];
            Txtbox_SaleTarjet_so.Text = (string)Settings.Default["objSO"];
            Txtbox_maxWalker.Text = (string)Settings.Default["maxCaminante"];
            Txtbox_maxClient.Text = (string)Settings.Default["maxCliente"];
            Txtbox_defaultValueSim.Text = (string)Settings.Default["simValue"];
            Txtbox_100ValueSim.Text = (string)Settings.Default["sim100"];
            Txtbox_120ValueSim.Text = (string)Settings.Default["sim120"];
            Txtbox_150ValueSim.Text = (string)Settings.Default["sim150"];
            Txtbox_defaultValueSo.Text = (string)Settings.Default["soValue"];
            Txtbox_requiredPsr.Text = (string)Settings.Default["psrReq"];
            Txtbox_120ValueSo.Text = (string)Settings.Default["so120"];
            Txtbox_150ValueSo.Text = (string)Settings.Default["so150"];
            Txtbox_commissionVol.Text = (string)Settings.Default["volCommision"];
        }
        private void txtbox_montoObjSIM_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSim"] = Txtbox_SaleTarget_sim.Text;
            Settings.Default.Save();
        }
        private void txtbox_maxPorCliente_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCliente"] = Txtbox_maxClient.Text;
            Settings.Default.Save();
        }
        private void txtbox_maxPorCaminante_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCaminante"] = Txtbox_maxWalker.Text;
            Settings.Default.Save();
        }
        private void txtBox_Sellout_objVenta_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSO"] = Txtbox_SaleTarjet_so.Text;
            Settings.Default.Save();
        }
        private void txtbox_defaultValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["simValue"] = Txtbox_defaultValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_100ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim100"] = Txtbox_100ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_120ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim120"] = Txtbox_120ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_150ValueSim_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["sim150"] = Txtbox_150ValueSim.Text;
            Settings.Default.Save();
        }

        private void txtbox_defaultValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["soValue"] = Txtbox_defaultValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_100ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["psrReq"] = Txtbox_requiredPsr.Text;
            Settings.Default.Save();
        }

        private void txtbox_120ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["so120"] = Txtbox_120ValueSo.Text;
            Settings.Default.Save();
        }

        private void txtbox_150ValueSo_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["so150"] = Txtbox_150ValueSo.Text;
            Settings.Default.Save();
        }


        private void txtbox_commissionVol_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["volCommision"] = Txtbox_commissionVol.Text;
            Settings.Default.Save();
        }
        #endregion

        private void Btn_Calculate_sim_Click(object sender, EventArgs e) 
        {
            ShowSimInfo();
        } 
        private void Btn_Calculate_so_Click(object sender, EventArgs e)
        {
            ShowSelloutInfo();
        }
        private void btn_Save_Click(object sender, EventArgs e) //Codigo para exportacion de Reporte formato (.txt)
        {
            if (!CounterTotalPsr_sim.Text.Contains('-') || !CounterTotalPsr_so.Text.Contains('-'))
            {

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var savePath = SaveFileDialog1.FileName + ".txt";
                    var unpaidSim = CounterFail_sim.Text;
                    var unpaidSellout = CounterFail_so.Text;
                    var simEffectiveness = CounterEffectiveness_sim.Text;
                    var soEffectiveness = CounterEffectiveness_so.Text;

                    nonCompiliantSim.Sort();
                    nonCompiliantSo.Sort();
                    using (StreamWriter file = new StreamWriter(savePath))
                    {
                        string reportContent = $"SIM-PLE Reporte para: {report.FullName}\n\n" +
                            $"Total Comisiones: ${report.TotalReward}\n\n" +
                            $"Detalle:\n\n" +
                            $"- Premio 40%: ${report.Gift40percent}\n" +
                            $"- Volumen vendido: ${report.SoldVol} de ${totalVol}" +
                            $"- Comision por volumen: ${report.Volumen}\n\n" +
                            $"- Objetivo SIM: ${report.RewardSim} ({report.ObjSim} PSR)\n" +
                            $"- Efectividad Sim: {simEffectiveness}\n" +
                            $"- No liquidan: {unpaidSim}\n";
                        file.WriteLine(reportContent);

                        foreach (var item in nonCompiliantSim)
                            file.WriteLine(item);

                        file.WriteLine("\n");
                        file.WriteLine($"- Objetivo Sellout: ${report.RewardSo} ({report.ObjSellOut} PSR)\n" +
                            $"- Efectividad Sellout: {soEffectiveness}\n" +
                        $"- No liquidan: {unpaidSellout}\n");

                        foreach (var item in nonCompiliantSo)
                            file.WriteLine(item);
                    }
                    MessageBox.Show("Se ha guardado su resultado.","SIM-PLE",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Primero debes calcular todos los objetivos","SIM-PLE",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Btn_CalculateRewards_Click(object sender, EventArgs e)
        {
            ShowSalary();
        }
        private void dgv_Sim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellTarget = e.ColumnIndex;
            int rowTarget = e.RowIndex;

            if (rowTarget != -1 && cellTarget == 4 && DgvSim.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Ok")
            {
                ModCounters_Sim(CounterObj_sim.Text, rowTarget);
                DgvSim.Rows[rowTarget].Cells[cellTarget].Value = "Undo";

                var rowOff = new DataGridViewCellStyle
                {
                    BackColor = Color.LightSeaGreen,
                    ForeColor = Color.WhiteSmoke
                };

                DgvSim.Rows[rowTarget].DefaultCellStyle = rowOff;
            }
            else if (rowTarget != -1 && cellTarget == 4 && DgvSim.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Undo")
            {
                DgvSim.Rows[rowTarget].Cells[cellTarget].Value = "Ok";
                var rowOn = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black
                };
                DgvSim.Rows[rowTarget].DefaultCellStyle = rowOn;

                if (itemsUndoSim.Count() > 0)
                {
                    string itemRestored = itemsUndoSim.Pop();
                    nonCompiliantSim.Add(itemRestored);
                    CounterFail_sim.Text = nonCompiliantSim.Count().ToString();

                    CounterObj_sim.Text = ReduceCounter(CounterObj_sim.Text);

                    int psrTotales = int.Parse(CounterTotalPsr_sim.Text);
                    int efectividad = (int.Parse(CounterObj_sim.Text) * 100) / psrTotales;
                    CounterEffectiveness_sim.Text = efectividad + "%";

                    int invercionActual = int.Parse(CounterInver_sim.Text.Replace("$", String.Empty));
                    int recargeValue = int.Parse(Txtbox_SaleTarget_sim.Text) - Convert.ToInt32(DgvSim.Rows[rowTarget].Cells[1].Value);
                    CounterInver_sim.Text = "$" + (invercionActual + recargeValue);
                }
            }
        }
        private void dgv_So_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int cellTarget = e.ColumnIndex;
            int rowTarget = e.RowIndex;

            if (rowTarget != -1 && cellTarget == 4 && DgvSellout.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Ok")
            {
                ModContadores_SO(CounterObj_so.Text, rowTarget);
                DgvSellout.Rows[rowTarget].Cells[cellTarget].Value = "Undo";

                var rowOff = new DataGridViewCellStyle
                {
                    BackColor = Color.LightSeaGreen,
                    ForeColor = Color.WhiteSmoke
                };

                DgvSellout.Rows[rowTarget].DefaultCellStyle = rowOff;
            }
            else if (rowTarget != -1 && cellTarget == 4 && DgvSellout.Rows[rowTarget].Cells[cellTarget].Value.ToString() == "Undo")
            {
                DgvSellout.Rows[rowTarget].Cells[cellTarget].Value = "Ok";
                var rowOn = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.Black
                };
                DgvSellout.Rows[rowTarget].DefaultCellStyle = rowOn;

                if (itemsUndoSO.Count > 0)
                {
                    string itemRestored = itemsUndoSO.Pop();
                    nonCompiliantSo.Add(itemRestored);
                    CounterObj_so.Text = ReduceCounter(CounterObj_so.Text);
                    CounterFail_so.Text = nonCompiliantSo.Count().ToString();
                    int psrTotal = int.Parse(CounterTotalPsr_so.Text);
                    CounterEffectiveness_so.Text = (int.Parse(CounterObj_so.Text) *100) / psrTotal + "%";

                }
            }
        }

        //FIXME: No funciona como lo espero se ejecuta el metodo cuando hago foco y no cuando se presiona ENTER.
        private void Btn_Calculate_so_Enter(object sender, EventArgs e)
        {
            //ShowSelloutInfo();
        }

        private void txtBox_Sellout_objVenta_Enter(object sender, EventArgs e)
        {
            //ShowSelloutInfo();
        }

        private void Btn_clipboard_so_Click(object sender, EventArgs e)
        {
            if (CounterTotalPsr_so.Text.Contains('-'))
            {
                MessageBox.Show("Primero debes calcular todos los objetivos", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Clipboard.Clear();
            var report = new Report();
            var output = report.CopyReport(DgvSellout, product:"so");
            Clipboard.SetText(output);

            MessageBox.Show("Los resultados se copiaron en el portapapeles :)", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Btn_clipboard_sim_Click(object sender, EventArgs e)
        {
            if (CounterTotalPsr_sim.Text.Contains('-'))
            {
                MessageBox.Show("Primero debes calcular todos los objetivos", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Clipboard.Clear();
            var report = new Report();
            var output = report.CopyReport(DgvSim, product: "sim");
            Clipboard.SetText(output);

            MessageBox.Show("Los resultados se copiaron en el portapapeles :)", "SIM-PLE", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

    }

}
