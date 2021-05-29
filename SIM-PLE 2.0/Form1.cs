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

namespace SIM_PLE_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            eligeCaminante();
        }
        #region //========== METODOS ==========\\
        private void eligeCaminante() //asigna el mensaje "Elige caminante" a todos los ComboBOX
        {
            comboBox_SIMcaminantes.Text = "Elija caminante";
        }
        private void HabilitarCalcular()
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
            if (txtbox_REP_pRecarga.Text.Contains("rimera"))
            {
                btn_calcularPremios.Enabled = true;
                btn_calcularPremios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            }
        } //Metodo para habilitar botones de calcular.
        public int Obtener40Porciento(string input) //obtine el 40% del input
        {
            double cuenta = double.Parse(input) * 0.4;
            int resultado = Convert.ToInt32(Math.Round(cuenta));
            return resultado;
        }
        public void ModContadores_Sim(string cumplidos, string faltantes)
        {
            string itemSeleccionado = (string)listBox_SIM.SelectedItem;
            elementosUndoSim.Push(itemSeleccionado);
            string[] arrayItemSelected = itemSeleccionado.Split('$', '|');
            arrayItemSelected[2].Replace(" ", String.Empty);

            int objVenta = Convert.ToInt32(txtbox_montoObjSIM.Text);
            int ventaActual = int.Parse(arrayItemSelected[2]);
            int aRestar = objVenta - ventaActual;
            int invercionActual = int.Parse(txtBox_inver.Text.Replace("$", String.Empty));
            string inversionFutura = Convert.ToString(invercionActual - aRestar);
            txtBox_inver.Text = "$" + inversionFutura;

            listBox_SIM.Items.Remove(itemSeleccionado);
            int objCumplido = int.Parse(cumplidos);
            objCumplido++;
            txtBox_SimConObj.Text = Convert.ToString(objCumplido);
            int faltaCumplir = int.Parse(faltantes);
            faltaCumplir--;
            txtBox_faltaCumplir.Text = Convert.ToString(faltaCumplir);
            int psrTotales = int.Parse(txtBox_PSRTotales.Text);
            int efectividad = (objCumplido * 100) / psrTotales;
            txtBox_Efectividad.Text = efectividad + "%";

        }
        public void ModContadores_SO(string cumplidos, string faltantes)
        {
            string itemSeleccionado = (string)listBox_Sellout.SelectedItem;
            elementosUndoSO.Push(itemSeleccionado);
            listBox_Sellout.Items.Remove(itemSeleccionado);
            int objCumplido = int.Parse(cumplidos);
            objCumplido++;
            txtB_soConObjetivo.Text = Convert.ToString(objCumplido);
            int faltaCumplir = int.Parse(faltantes);
            faltaCumplir--;
            txtBox_soFaltan.Text = Convert.ToString(faltaCumplir);
            int psrTotales = int.Parse(SO_psrTotales.Text);
            int efectividad = (objCumplido * 100) / psrTotales;
            SO_efectividad.Text = efectividad + "%";
        }
        #endregion

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
                // Codigo para abrir y leer el archivo
                txtbox_REP_psragencia.Text = openFileDialog1.FileName;
            }
            HabilitarCalcular();
        }  
        private void btn_Examinar2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_REP_pRecarga.Text = openFileDialog1.FileName;
            }
            HabilitarCalcular();
        }
        private void btn_Examinar3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_REP_productosVendidos.Text = openFileDialog1.FileName;
            }
            HabilitarCalcular();
        }
        private void examinar_sellout_dealer_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBox_REPsellout_dealer.Text = openFileDialog1.FileName;
            }
            HabilitarCalcular();
        }
        #endregion

        #region //=========== EVENTOS KEYPRESS SOLO ACEPTAN NUMEROS ==========\\
        private void txtbox_montoObjSIM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtBox_Sellout_objVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtbox_maxPorCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtbox_maxPorCaminante_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region //========== EVENTO DOBLE CLICK EN LISTBOX'S =========\\
        private void listBox_SIM_DoubleClick(object sender, EventArgs e) //Quita un cliente de el listbox y modifica los contadores
        {
            if (listBox_SIM.Items.Count > 0)
            {
                ModContadores_Sim(txtBox_SimConObj.Text,txtBox_faltaCumplir.Text);
            }
        }
        private void listBox_Sellout_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_Sellout.Items.Count > 0)
            {
                ModContadores_SO(txtB_soConObjetivo.Text, txtBox_soFaltan.Text);
            }
        }
        #endregion

        #region //========Stack + BTN Undo ========\\
        Stack<string> elementosUndoSim = new Stack<string>();
        Stack<string> elementosUndoSO = new Stack<string>();
        private void btn_undo_Click(object sender, EventArgs e)
        {
            if(elementosUndoSim.Count() > 0)
            {
                string elementoRestaurado = elementosUndoSim.Pop();
                listBox_SIM.Items.Add(elementoRestaurado);
                int contadorActualCumplidos = int.Parse(txtBox_SimConObj.Text);
                contadorActualCumplidos--;
                int contadorActualFaltan = int.Parse(txtBox_faltaCumplir.Text);
                contadorActualFaltan++;
                txtBox_SimConObj.Text = Convert.ToString(contadorActualCumplidos);
                txtBox_faltaCumplir.Text = Convert.ToString(contadorActualFaltan);
                int psrTotales = int.Parse(txtBox_PSRTotales.Text);
                int efectividad = (contadorActualCumplidos * 100) / psrTotales;
                txtBox_Efectividad.Text = efectividad + "%";


                string[] arrayItemSelected = elementoRestaurado.Split('$', '|');
                arrayItemSelected[2].Replace(" ", String.Empty);

                int objVenta = Convert.ToInt32(txtbox_montoObjSIM.Text);
                int ventaActual = int.Parse(arrayItemSelected[2]);
                int aSumar = objVenta - ventaActual;
                int invercionActual = int.Parse(txtBox_inver.Text.Replace("$", String.Empty));
                string inversionFutura = Convert.ToString(invercionActual + aSumar);
                txtBox_inver.Text = "$" + inversionFutura;
            }
        }
        private void btn_undo_SO_Click(object sender, EventArgs e)
        {
            if(elementosUndoSO.Count() > 0)
            {
                string elementoRestaurado = elementosUndoSO.Pop();
                listBox_Sellout.Items.Add(elementoRestaurado);
                int contadorActualCumplidos = int.Parse(txtB_soConObjetivo.Text);
                contadorActualCumplidos--;
                int contadorActualFaltan = int.Parse(txtBox_soFaltan.Text);
                contadorActualFaltan++;
                txtB_soConObjetivo.Text = Convert.ToString(contadorActualCumplidos);
                txtBox_soFaltan.Text = Convert.ToString(contadorActualFaltan);
                int psrTotales = int.Parse(SO_psrTotales.Text);
                int efectividad = (contadorActualCumplidos * 100) / psrTotales;
                SO_efectividad.Text = efectividad + "%";
            }
        }
        #endregion



        private void btn_SIM_calcular_Click_1(object sender, EventArgs e) //Calcula objetivo SIM
        {
            if (comboBox_SIMcaminantes.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtbox_montoObjSIM.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ===== Elimina datos del listbox y del stack ===== \\
            listBox_SIM.Items.Clear();
            elementosUndoSim.Clear();
            // ================================================== \\
            //======== Se leen las lineas de los reportes =======\\
            string[] reporte_PSRagencia = File.ReadAllLines(txtbox_REP_psragencia.Text);
            string[] reporte_pRecarga = File.ReadAllLines(txtbox_REP_pRecarga.Text);
            string[] reporte_prodVendidos = File.ReadAllLines(txtbox_REP_productosVendidos.Text);
            //======== Variables & Diccionario =======\\
            var psrSIM = new Dictionary<string, PSR>();
            int objVenta = Convert.ToInt32(txtbox_montoObjSIM.Text);
            int objCumplido = 0;
            int psrTotales = 0;
            int invercion = 0;
            int psrAgenciaLen = reporte_PSRagencia.Length;
            int pRecargasLen = reporte_pRecarga.Length;
            int prodVendidosLen = reporte_prodVendidos.Length;

            for (int i = 2; i < psrAgenciaLen; i++)
            {
                // Array con el contenido de dichas lineas.
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                // Si pertenece al caminante seleccionado entonces se asignan los datos al objeto PSR.
                if (itemsAgencia[INDEX_AGENCIA_CAMINANTE] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
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
            psrTotales = psrSIM.Count;

            for (int y = 2; y < pRecargasLen; y++)
            {
                //Array con el contenido del segundo reporte
                string[] itemspRecarga = reporte_pRecarga[y].Trim().Split(';');

                if (itemspRecarga[INDEX_RECARGAS_CAMINANTE] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    string auxPSRcode = itemspRecarga[INDEX_RECARGAS_CODPSR];
                    int auxPrecarga = Convert.ToInt32(itemspRecarga[INDEX_RECARGAS_MONTO]);
                    string auxNim = itemspRecarga[INDEX_RECARGAS_NIM];
                    string auxIdSim = itemspRecarga[INDEX_RECARGAS_ID];


                    if (!psrSIM.ContainsKey(auxPSRcode))
                    {
                        PSR client = new PSR
                        {
                            CodPSR = itemspRecarga[INDEX_RECARGAS_CODPSR],
                            Caminante = itemspRecarga[INDEX_RECARGAS_CAMINANTE],
                            Pos = "dado de baja",
                            Nombre = itemspRecarga[INDEX_RECARGAS_RAZONSOCIAL],
                            NimCliente = itemspRecarga[INDEX_RECARGAS_NIM],
                            PrimeraRecarga = Convert.ToInt32(itemspRecarga[INDEX_RECARGAS_MONTO]),
                            IdSIM = itemspRecarga[INDEX_RECARGAS_ID],
                            Lote = "",
                            EsCumplidor = false,
                        };

                        /*  if (client.PrimeraRecarga >= objCumplido)
                          {
                              client.EsCumplidor = true;           //Anulacion de el contador de "dados de baja"
                          }
                          psrSIM[client.CodPSR] = client;
                          dadosBaja++;*/
                    }
                    else
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
                        if (psrSIM[auxPSRcode].PrimeraRecarga >= objVenta)
                        {
                            psrSIM[auxPSRcode].EsCumplidor = true;
                        }
                    }
                }
            }
            for (int z = 2; z < prodVendidosLen; z++)
            {
                string[] itemspVendidos = reporte_prodVendidos[z].Split(';');
                //string auxPSRcode2 = itemspVendidos[pVendidos_codpsrIndex];

                if (psrSIM.ContainsKey(itemspVendidos[INDEX_PVENDIDOS_CODPSR]) && psrSIM[itemspVendidos[INDEX_PVENDIDOS_CODPSR]].PrimeraRecarga == 0 && itemspVendidos[INDEX_PVENDIDOS_CHECKER] != "Carga Virtual")
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
                    listBox_SIM.Items.Add(psr.Nombre + "  |  " + "Primera Recarga: $" + Convert.ToString(psr.PrimeraRecarga) + "  |  " + "Numero: " + psr.NimCliente + "  |  " + "Lote: " + psr.Lote);
                    int paraCumplirObj = objVenta - psr.PrimeraRecarga;
                    invercion += paraCumplirObj;
                }
            }
            int efectividad = (objCumplido * 100) / psrTotales;
            txtBox_SimConObj.Text = Convert.ToString(objCumplido);
            txtBox_PSRTotales.Text = Convert.ToString(psrTotales);
            txtBox_faltaCumplir.Text = Convert.ToString(listBox_SIM.Items.Count);
            //txtBox_inver.Text = Convert.ToString(dadosBaja);
            txtBox_Efectividad.Text = Convert.ToString(efectividad) + "%";
            txtBox_inver.Text = "$" + invercion;

            listBox_SIM.Sorted = true;
        } 
        private void btn_calcularSellout_Click(object sender, EventArgs e) //Calcula objetivo SO.
        {
            if (comboBox_SIMcaminantes.Text == "Elija caminante")
            {
                MessageBox.Show("Primero debe elegir un caminante.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtBox_Sellout_objVenta.Text == String.Empty)
            {
                MessageBox.Show("Primero debe ingresar un objetivo de venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            listBox_Sellout.Items.Clear();
            elementosUndoSO.Clear();
            int objVenta = Convert.ToInt32(txtBox_Sellout_objVenta.Text);
            string[] reporte_PSRagencia = File.ReadAllLines(txtbox_REP_psragencia.Text); //Contemplar exeptions
            string[] reporte_dealer = File.ReadAllLines(txtBox_REPsellout_dealer.Text); //Contemplar exeptions
            string[] reporte_prodVendidos = File.ReadAllLines(txtbox_REP_productosVendidos.Text);//Contemplar exeptions
            int psrAgenciaLen = reporte_PSRagencia.Length;
            int ventaAnaliticoLen = reporte_dealer.Length;
            int prodVendidosLen = reporte_prodVendidos.Length;

            var psrSellout = new Dictionary<string, PSR>();
            int objCumplido = 0;
            int psrTotales = 0;

            //Reporte PSR de la agencia
            for (int i = 2; i < psrAgenciaLen; i++)
            {
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                if (itemsAgencia[INDEX_AGENCIA_CAMINANTE] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    PSR clientSellout = new PSR();
                    clientSellout.CodPSR = itemsAgencia[INDEX_AGENCIA_CODPSR];
                    clientSellout.Nombre = itemsAgencia[INDEX_AGENCIA_RAZONSOCIAL].Replace('"', ' ').Trim();
                    clientSellout.Caminante = itemsAgencia[INDEX_AGENCIA_CAMINANTE];
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

                if (itemspVendidos[INDEX_PVENDIDOS_CHECKER] == "Carga Virtual" && itemspVendidos[INDEX_PVENDIDOS_CAMINANTE] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
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
                    listBox_Sellout.Items.Add(psr.Nombre + "  |  " + "Venta mensual: "
                    + Convert.ToString(psr.VentaMensual) + "  |  " + "Total Transferido: " + Convert.ToString(psr.Transferencias));
            }
            SO_psrTotales.Text = Convert.ToString(psrTotales);
            txtB_soConObjetivo.Text = Convert.ToString(objCumplido);
            txtBox_soFaltan.Text = Convert.ToString(listBox_Sellout.Items.Count);
            txtB_soVolumen.Text = "$" + Convert.ToString(volumenVendedor);
            int efectividad = (objCumplido * 100) / psrTotales;
            SO_efectividad.Text = Convert.ToString(efectividad) + "%";

            listBox_Sellout.Sorted = true;
        }
        private void btn_clipBoard_Click(object sender, EventArgs e) //Codigo para reporte en ClipBoard
        {
            if (!txtBox_PSRTotales.Text.Contains('-') || !SO_psrTotales.Text.Contains('-'))
            {
                Clipboard.Clear();
                string[] noCumplidores_SIM = listBox_SIM.Items.OfType<string>().ToArray();
                string[] noCumplidores_SO = listBox_Sellout.Items.OfType<string>().ToArray();
                string strNoCumplidores_Sim = "";
                string strNoCumplidores_SO = "";
                string cumObjetivo_SIM = txtBox_SimConObj.Text;
                string simFaltantes = txtBox_faltaCumplir.Text;
                string cumObjetivo_SO = txtB_soConObjetivo.Text;
                string soFaltantes = txtBox_soFaltan.Text;

                string saltoDeLinea = Environment.NewLine;
                string tituloReporte = "SIM-PLE Reporte: " + comboBox_SIMcaminantes.SelectedItem;
                string linea1 = "Obj. SIM: " + cumObjetivo_SIM + " PSR";
                string linea2 = "Faltan " + simFaltantes + " psr: ";
                string linea3 = "tu volumen es: " + txtB_soVolumen.Text;
                string linea4 = "Obj. SO: " + cumObjetivo_SO;
                string linea5 = "Faltan " + soFaltantes + " psr: ";
                int j = 1;
                foreach (string items in noCumplidores_SIM)
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
            if (!txtBox_PSRTotales.Text.Contains('-') || !SO_psrTotales.Text.Contains('-'))
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] noCumplidores_SIM = listBox_SIM.Items.OfType<string>().ToArray();
                    string[] noCumplidores_SO = listBox_Sellout.Items.OfType<string>().ToArray();
                    string rutaGuardado = saveFileDialog1.FileName + ".txt";
                    string cumObjetivo_SIM = txtBox_SimConObj.Text;
                    string simFaltantes = txtBox_faltaCumplir.Text;
                    string cumObjetivo_SO = txtB_soConObjetivo.Text;
                    string soFaltantes = txtBox_soFaltan.Text;

                    string tituloReporte = "SIM-PLE Reporte: " + comboBox_SIMcaminantes.SelectedItem;
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
                        foreach (string items in noCumplidores_SIM)
                        {
                            file.WriteLine(Convert.ToString(j)+") "+items);
                            j++;
                        }
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea3);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea4);
                        file.WriteLine(lineaVacia);
                        file.WriteLine(linea5);
                        file.WriteLine(lineaVacia);
                        j = 1;
                        foreach (string items in noCumplidores_SO)
                        {
                            file.WriteLine(Convert.ToString(j) + ") " + items);
                            j++;
                        }
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
                            allRecargas[itemsRecargas[INDEX_RECARGAS_CODPSR].Trim()].monto += Obtener40Porciento(itemsRecargas[INDEX_RECARGAS_MONTO]);
                        }
                        else
                        {
                            SimRecargada sim = new SimRecargada
                            {
                                codpsr = itemsRecargas[INDEX_RECARGAS_CODPSR].Trim(),
                                caminante = itemsRecargas[INDEX_RECARGAS_CAMINANTE],
                                monto = Obtener40Porciento(itemsRecargas[INDEX_RECARGAS_MONTO])
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

        
    }

}
