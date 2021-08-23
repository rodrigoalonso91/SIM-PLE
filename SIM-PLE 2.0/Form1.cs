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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SelectWalker();
        }

        //================ Globals =================\\
        Dictionary<string, PSR> psrSIM = new Dictionary<string, PSR>();
        Dictionary<string, PSR> psrSellout = new Dictionary<string, PSR>();
        List<string> nonCompiliantSim = new List<string>();
        List<string> nonCompiliantSo = new List<string>();
        Stack<string> itemsUndoSim = new Stack<string>();
        Stack<string> itemsUndoSO = new Stack<string>();

        #region //========== INDEX ==========\\
        //Indexs PSR Agencia
        const int INDEX_AGENCIA_CODPSR = 1;
        const int INDEX_AGENCIA_RAZONSOCIAL = 2;
        const int INDEX_AGENCIA_DIRECCION = 3;
        const int INDEX_AGENCIA_ALTURA = 4;
        const int INDEX_AGENCIA_CAMINANTE = 11;
        const int INDEX_AGENCIA_POS = 16;
        //Indexs Primera Recarga
        const int INDEX_RECARGAS_CODPSR = 0;
        const int INDEX_RECARGAS_RAZONSOCIAL = 1;
        const int INDEX_RECARGAS_CAMINANTE = 7;
        const int INDEX_RECARGAS_ID = 9;
        const int INDEX_RECARGAS_MONTO = 14;
        const int INDEX_RECARGAS_NIM = 10;
        //Index Productos Vendidos
        const int INDEX_PVENDIDOS_CODPSR = 1;
        const int INDEX_PVENDIDOS_LOTE = 16;
        const int INDEX_PVENDIDOS_CHECKER = 10;
        const int INDEX_PVENDIDOS_CAMINANTE = 8;
        const int INDEX_PVENDIDOS_TRANSFERENCIAS = 13;
        //Index Venta de saldo - Analitico
        const int INDEX_DEALER_CODPSR = 2;
        const int INDEX_DEALER_VENTASFINALES = 4;
        const int INDEX_DEALER_VENTASPSR = 5;
        const int INDEX_DEALER_POS = 3;
        #endregion

        #region //========== METODOS ==========\\


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

        public int Get40percent(string input) //obtine el 40% del input
        {
            double mathOperation = double.Parse(input) * 0.4;
            int result = Convert.ToInt32(Math.Round(mathOperation));
            return result;
        }
        public void ModCounters_Sim(string counterValue, int currentRow)
        {
            string itemSelected = dgv_Sim.Rows[currentRow].Cells[0].Value.ToString();
            nonCompiliantSim.Remove(itemSelected);
            itemsUndoSim.Push(itemSelected);
            //Aumento y decremento de contadores
            txtBox_SimConObj.Text = incrementCounter(counterValue);
            txtBox_faltaCumplir.Text = nonCompiliantSim.Count.ToString();
            // Modificacion de inversion
            int salesTarget = Convert.ToInt32(txtbox_montoObjSIM.Text);
            int currentSale = Convert.ToInt32(dgv_Sim.Rows[currentRow].Cells[1].Value);
            int newInversion = int.Parse(txtBox_inver.Text.Replace("$", String.Empty)) - (salesTarget - currentSale);
            txtBox_inver.Text = "$" + Convert.ToString(newInversion);
            //Modificacion de efectividad
            txtBox_Efectividad.Text = ((int.Parse(txtBox_SimConObj.Text) * 100) / int.Parse(txtBox_PSRTotales.Text)) + "%";
        }

        public string incrementCounter(string counter) // Incrementa el valor en uno [string]
        {
            int num = int.Parse(counter);
            num++;
            return num.ToString(); ;
        }

        public string reduceCounter(string counter) // Decremente el valor en uno [string]
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
            txtB_soConObjetivo.Text = incrementCounter(counterValue);
            txtBox_soFaltan.Text = nonCompiliantSo.Count.ToString();

            // Modificacion Efectividad SO
            SO_efectividad.Text = ((int.Parse(txtB_soConObjetivo.Text) * 100) / int.Parse(SO_psrTotales.Text))+ "%";
        }
        #endregion

        #region //========== BTNS MENU LATERAL ==========\\
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

                    var walkersList = new List<string>();
                    string[] arrReport = File.ReadAllLines(txtbox_REP_psragencia.Text);
                    bool isNamed;

                    int reportLen = arrReport.Length;
                    for (int i = 2; i < reportLen; i++)
                    {
                        string[] itemReport = arrReport[i].Split(';');
                        if (walkersList.Contains(itemReport[INDEX_AGENCIA_CAMINANTE]))
                        {
                            continue;
                        }
                        else
                        {
                            isNamed = itemReport[INDEX_AGENCIA_CAMINANTE].Contains(" ");
                            if (isNamed) walkersList.Add(itemReport[INDEX_AGENCIA_CAMINANTE]);
                        }
                    }

                    foreach (var walker in walkersList)
                    {
                        cb_walkers.Items.Add(walker);
                    }
                    
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void txtBox_Sellout_objVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void txtbox_maxPorCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void txtbox_maxPorCaminante_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        #endregion

        #region // ======= Settings ====== \\
        private void Form1_Load(object sender, EventArgs e)
        {
            txtbox_montoObjSIM.Text = (string)Settings.Default["objSim"];
            txtBox_Sellout_objVenta.Text = (string)Settings.Default["objSO"];
            txtbox_maxPorCaminante.Text = (string)Settings.Default["maxCaminante"];
            txtbox_maxPorCliente.Text = (string)Settings.Default["maxCliente"];
        }

        private void txtbox_montoObjSIM_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSim"] = txtbox_montoObjSIM.Text;
            Settings.Default.Save();
        }


        private void txtbox_maxPorCliente_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCliente"] = txtbox_maxPorCliente.Text;
            Settings.Default.Save();
        }

        private void txtbox_maxPorCaminante_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["maxCaminante"] = txtbox_maxPorCaminante.Text;
            Settings.Default.Save();
        }

        private void txtBox_Sellout_objVenta_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["objSO"] = txtBox_Sellout_objVenta.Text;
            Settings.Default.Save();
        }
        #endregion

        #region //Editar

        private void btn_undo_SO_Click(object sender, EventArgs e)
        {
           
        }
        #endregion 

        private void btn_SIM_calcular_Click_1(object sender, EventArgs e) //Calcula objetivo SIM
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
            // ===== Elimina datos del list, Dgv y stack ===== \\
            itemsUndoSim.Clear();
            dgv_Sim.Rows.Clear();
            psrSIM.Clear();
            nonCompiliantSim.Clear();
            // ================================================== \\

            //======== Se leen las lineas de los reportes =======\\
            string[] reporte_PSRagencia = File.ReadAllLines(txtbox_REP_psragencia.Text);
            string[] reporte_pRecarga = File.ReadAllLines(txtbox_REP_pRecarga.Text);
            string[] reporte_prodVendidos = File.ReadAllLines(txtbox_REP_productosVendidos.Text);
            //======== Variables & Diccionario =======\\
            int salesTarget = int.Parse(txtbox_montoObjSIM.Text);
            int objCumplido = 0;
            int psrTotal = 0;
            int investment = 0;
            int psrAgenciaLen = reporte_PSRagencia.Length;
            int pRecargasLen = reporte_pRecarga.Length;
            int prodVendidosLen = reporte_prodVendidos.Length;
            string walkerSelected = Convert.ToString(cb_walkers.SelectedItem);

            for (int i = 2; i < psrAgenciaLen; i++)
            {
                // Array con el contenido de dichas lineas.
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                // Si pertenece al caminante seleccionado entonces se asignan los datos al objeto PSR.
                if (itemsAgencia[INDEX_AGENCIA_CAMINANTE] == walkerSelected)
                {
                    PSR client = new PSR
                    {
                        CodPSR = itemsAgencia[INDEX_AGENCIA_CODPSR],
                        Caminante = itemsAgencia[INDEX_AGENCIA_CAMINANTE],
                        Pos = itemsAgencia[INDEX_AGENCIA_POS],
                        Nombre = itemsAgencia[INDEX_AGENCIA_RAZONSOCIAL].Replace('"', ' ').Trim(),
                        Calle = itemsAgencia[INDEX_AGENCIA_DIRECCION],
                        Altura = itemsAgencia[INDEX_AGENCIA_ALTURA],
                        NimCliente = "",
                        PrimeraRecarga = 0,
                        IdSIM = "",
                        Lote = "",
                        EsCumplidor = false
                    };
                    psrSIM[client.CodPSR] = client; // Se guarda el objeto cliente en el diccionario.
                }
            }
            psrTotal = psrSIM.Count;

            for (int y = 2; y < pRecargasLen; y++)
            {
                //Array con el contenido del segundo reporte
                string[] itemspRecarga = reporte_pRecarga[y].Trim().Split(';');

                if (itemspRecarga[INDEX_RECARGAS_CAMINANTE] == walkerSelected)
                {
                    string auxPSRcode = itemspRecarga[INDEX_RECARGAS_CODPSR];
                    int auxPrecarga = Convert.ToInt32(itemspRecarga[INDEX_RECARGAS_MONTO]);
                    string auxNim = itemspRecarga[INDEX_RECARGAS_NIM];
                    string auxIdSim = itemspRecarga[INDEX_RECARGAS_ID];


                    if (psrSIM.ContainsKey(auxPSRcode))
                    {
                        if (psrSIM[auxPSRcode].PrimeraRecarga == 0)
                        {
                            psrSIM[auxPSRcode].PrimeraRecarga = auxPrecarga;
                            psrSIM[auxPSRcode].NimCliente = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        else if (auxPrecarga > psrSIM[auxPSRcode].PrimeraRecarga)
                        {
                            psrSIM[auxPSRcode].PrimeraRecarga = auxPrecarga;
                            psrSIM[auxPSRcode].NimCliente = auxNim;
                            psrSIM[auxPSRcode].IdSIM = auxIdSim;
                        }
                        if (psrSIM[auxPSRcode].PrimeraRecarga >= salesTarget)
                        {
                            psrSIM[auxPSRcode].EsCumplidor = true;
                        }
                    }
                }
            }
            for (int z = 2; z < prodVendidosLen; z++)
            {
                string[] itemspVendidos = reporte_prodVendidos[z].Split(';');

                if (psrSIM.ContainsKey(itemspVendidos[INDEX_PVENDIDOS_CODPSR]) && 
                    psrSIM[itemspVendidos[INDEX_PVENDIDOS_CODPSR]].PrimeraRecarga == 0 && 
                    itemspVendidos[INDEX_PVENDIDOS_CHECKER] != "Carga Virtual")
                {
                    psrSIM[itemspVendidos[INDEX_PVENDIDOS_CODPSR]].Lote = itemspVendidos[INDEX_PVENDIDOS_LOTE];
                }
            }

            foreach (var psr in psrSIM.Values)
            {
                if (psr.EsCumplidor)
                {
                    objCumplido++;
                }
                else
                {
                    // Carga de datos en DGV
                    int rowsIndex = dgv_Sim.Rows.Add();
                    dgv_Sim.Rows[rowsIndex].Cells[0].Value = psr.Nombre;
                    dgv_Sim.Rows[rowsIndex].Cells[1].Value = psr.PrimeraRecarga;
                    dgv_Sim.Rows[rowsIndex].Cells[2].Value = psr.NimCliente;
                    dgv_Sim.Rows[rowsIndex].Cells[3].Value = psr.Lote;
                    dgv_Sim.Rows[rowsIndex].Cells[4].Value = "Ok";

                    investment += (salesTarget - psr.PrimeraRecarga);
                    nonCompiliantSim.Add(psr.Nombre);
                }
            }

            int efectividad = (objCumplido * 100) / psrTotal;
            txtBox_SimConObj.Text = Convert.ToString(objCumplido);
            txtBox_PSRTotales.Text = Convert.ToString(psrTotal);
            txtBox_faltaCumplir.Text = Convert.ToString(nonCompiliantSim.Count);
            txtBox_Efectividad.Text = efectividad + "%";
            txtBox_inver.Text = "$" + investment;
        } 
        private void btn_calcularSellout_Click(object sender, EventArgs e) //Calcula objetivo SO.
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

            itemsUndoSO.Clear();
            int objVenta = Convert.ToInt32(txtBox_Sellout_objVenta.Text);
            string[] reporte_PSRagencia = File.ReadAllLines(txtbox_REP_psragencia.Text); //Contemplar exeptions
            string[] reporte_dealer = File.ReadAllLines(txtBox_REPsellout_dealer.Text); //Contemplar exeptions
            string[] reporte_prodVendidos = File.ReadAllLines(txtbox_REP_productosVendidos.Text);//Contemplar exeptions
            int psrAgenciaLen = reporte_PSRagencia.Length;
            int ventaAnaliticoLen = reporte_dealer.Length;
            int prodVendidosLen = reporte_prodVendidos.Length;

            int objCumplido = 0;
            int psrTotales = 0;

            //Reporte PSR de la agencia
            for (int i = 2; i < psrAgenciaLen; i++)
            {
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                if (itemsAgencia[INDEX_AGENCIA_CAMINANTE] == Convert.ToString(cb_walkers.SelectedItem))
                {
                    PSR clientSellout = new PSR();
                    clientSellout.CodPSR = itemsAgencia[INDEX_AGENCIA_CODPSR];
                    clientSellout.Nombre = itemsAgencia[INDEX_AGENCIA_RAZONSOCIAL].Replace('"', ' ').Trim();
                    clientSellout.Caminante = itemsAgencia[INDEX_AGENCIA_CAMINANTE];
                    clientSellout.Pos = itemsAgencia[INDEX_AGENCIA_POS];
                    clientSellout.Transferencias = 0;
                    clientSellout.StockCarga = 0;
                    clientSellout.StockSim = 0;
                    clientSellout.VentaMensual = 0;
                    clientSellout.EsCumplidor = false;
                    psrSellout[clientSellout.CodPSR] = clientSellout;
                }
            }
            psrTotales = psrSellout.Count;

            //Reporte Productos Vendidos
            for (int z = 2; z < prodVendidosLen; z++)
            {
                string[] itemspVendidos = reporte_prodVendidos[z].Split(';');
                string auxpsrcode = itemspVendidos[INDEX_PVENDIDOS_CODPSR];

                if (itemspVendidos[INDEX_PVENDIDOS_CHECKER] == "Carga Virtual" && itemspVendidos[INDEX_PVENDIDOS_CAMINANTE] == Convert.ToString(cb_walkers.SelectedItem))
                {
                    psrSellout[auxpsrcode].Transferencias += Convert.ToInt32(itemspVendidos[INDEX_PVENDIDOS_TRANSFERENCIAS]);
                }
            }

            //Reporte Ventas Analitico
            for (int y = 5; y < ventaAnaliticoLen; y++)
            {
                string[] ventasFinales = reporte_dealer[y].Split(';');
                string auxPsrcode = "0" + ventasFinales[INDEX_DEALER_CODPSR];
                if (psrSellout.ContainsKey(auxPsrcode))
                {
                    psrSellout[auxPsrcode].VentaMensual = Convert.ToDouble(ventasFinales[INDEX_DEALER_VENTASFINALES]);
                }
            }
            double volumenVendedor = 0;
            foreach (var psr in psrSellout.Values)
            {
                volumenVendedor += psr.VentaMensual;
                if (psr.VentaMensual >= objVenta)
                {
                    psr.EsCumplidor = true;
                }
                if (psr.EsCumplidor)
                    objCumplido++;
                else
                {
                    int rowIndex = dgv_So.Rows.Add();
                    dgv_So.Rows[rowIndex].Cells[0].Value = psr.Nombre;
                    dgv_So.Rows[rowIndex].Cells[1].Value = psr.VentaMensual;
                    dgv_So.Rows[rowIndex].Cells[2].Value = psr.Transferencias;
                    dgv_So.Rows[rowIndex].Cells[3].Value = psr.Pos;
                    dgv_So.Rows[rowIndex].Cells[4].Value = "Ok";

                    nonCompiliantSo.Add(psr.Nombre);
                }
            }
            SO_psrTotales.Text = psrTotales.ToString();
            txtB_soConObjetivo.Text = objCumplido.ToString();
            txtBox_soFaltan.Text = nonCompiliantSo.Count().ToString();
            txtB_soVolumen.Text = "$" + volumenVendedor;
            int efectividad = (objCumplido * 100) / psrTotales;
            SO_efectividad.Text = efectividad + "%";

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
        private void btn_calcularPremios_Click(object sender, EventArgs e) //Calcula Premio de 40%
        {
            if (txtbox_maxPorCliente.Text == String.Empty || txtbox_maxPorCaminante.Text == String.Empty)
            {
                MessageBox.Show("Debe ingresar los valores maximos por PSR y por Caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtbox_REP_pRecarga.Text.Contains("Primera"))
                {
                    dgv_Premios40.Rows.Clear();
                    //============================= VARIABLES =============================\\
                    int montoMaxPSR = int.Parse(txtbox_maxPorCliente.Text);
                    int montoMaxCaminante = int.Parse(txtbox_maxPorCaminante.Text);
                    //=========================== DICCIONARIOS =============================\\
                    var allRecargas = new Dictionary<string, SimRecargada>();
                    var empleadosPremiados = new Dictionary<string, Premio40>();
                    //=======================================================================\\

                    string[] arrPrimeraRecargas = File.ReadAllLines(txtbox_REP_pRecarga.Text);
                    int LenRecargas = arrPrimeraRecargas.Length;

                    for (int i = 2; i < LenRecargas; i++)
                    {
                        string[] itemsRecargas = arrPrimeraRecargas[i].Split(';');
                        if (allRecargas.ContainsKey(itemsRecargas[INDEX_RECARGAS_CODPSR].Trim()))
                        {
                            allRecargas[itemsRecargas[INDEX_RECARGAS_CODPSR].Trim()].monto += Get40percent(itemsRecargas[INDEX_RECARGAS_MONTO]);
                        }
                        else
                        {
                            SimRecargada sim = new SimRecargada
                            {
                                codpsr = itemsRecargas[INDEX_RECARGAS_CODPSR].Trim(),
                                caminante = itemsRecargas[INDEX_RECARGAS_CAMINANTE],
                                monto = Get40percent(itemsRecargas[INDEX_RECARGAS_MONTO])
                            };
                            allRecargas[sim.codpsr] = sim;
                        }
                    }
                    foreach (var item in allRecargas.Values)
                    {
                        if (item.monto > montoMaxPSR)
                        {
                            item.monto = montoMaxPSR;
                        }
                        if (empleadosPremiados.ContainsKey(item.caminante))
                        {
                            empleadosPremiados[item.caminante].simConRecarga++;
                            empleadosPremiados[item.caminante].montoPremio += item.monto;
                        }
                        else
                        {
                            Premio40 empleadoPremiado = new Premio40
                            {
                                simConRecarga = 1,
                                montoPremio = item.monto,
                                nombre = item.caminante
                            };
                            empleadosPremiados[item.caminante] = empleadoPremiado;
                        }
                    }

                    foreach (var item in empleadosPremiados.Values)
                    {
                        int montoReal = item.montoPremio;
                        if (item.montoPremio > montoMaxCaminante)
                            item.montoPremio = montoMaxCaminante;
                        int cantidadFilas = dgv_Premios40.Rows.Add();
                        dgv_Premios40.Rows[cantidadFilas].Cells[0].Value = item.nombre;
                        dgv_Premios40.Rows[cantidadFilas].Cells[1].Value = item.simConRecarga + " Sims";
                        dgv_Premios40.Rows[cantidadFilas].Cells[2].Value = "$ " + montoReal;
                        dgv_Premios40.Rows[cantidadFilas].Cells[3].Value = "$ " + item.montoPremio;
                        dgv_Premios40.Rows[cantidadFilas].Cells[4].Value = (montoReal*100) / montoMaxCaminante + "%";
                    }
                }
                else
                {
                    MessageBox.Show("Debe elegir un reporte de Primera Recarga!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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

                    txtBox_SimConObj.Text = reduceCounter(txtBox_SimConObj.Text);

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
                    txtBox_faltaCumplir.Text = nonCompiliantSo.Count().ToString();

                    txtB_soConObjetivo.Text = reduceCounter(txtB_soConObjetivo.Text);

                    int psrTotal = int.Parse(SO_psrTotales.Text);
                    SO_efectividad.Text = (int.Parse(txtB_soConObjetivo.Text) *100) / psrTotal + "%";
                }
            }
        }
    }

}
