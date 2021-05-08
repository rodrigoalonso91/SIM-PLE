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
        private void eligeCaminante() //asigna el mensaje "Elige caminante" a todos los ComboBOX
        {
            comboBox_SIMcaminantes.Text = "Elija caminante";
        }

        #region //Indices de los campos en reportes
        //Indexs PSR agencia.
        const int agencia_codpsrIndex = 1;
        const int agencia_nameIndex = 2;
        const int agencia_direccionIndex = 3;
        const int agencia_alturaIndex = 4;
        const int agencia_caminanteIndex = 11;
        const int agencia_posIndex = 16;
        //Indexs Primera Recarga.
        const int pRecarga_codpsrIndex = 0;
        const int pRecarga_nombreIndex = 1;
        const int pRecarga_caminanteIndex = 7;
        const int pRecarga_Id_Index = 9;
        const int pRecarga_montoIndex = 14;
        const int pRecarga_nimIndex = 10;
        //index Productos vendidos.
        const int pVendidos_codpsrIndex = 1;
        const int pVendidos_loteIndex = 16;
        const int pVendidos_checkerIndex = 10;
        const int pVendidos_caminanteIndex = 8;
        const int pVendidos_transferenciaIndex = 13;
        //index Ventas Dealer.
        const int dealer_codpsrIndex = 2;
        const int dealer_ventasFinalesIndex = 4;
        const int dealer_ventasPsrIndex = 5;
        const int dealer_posIndex = 3;
        #endregion

        //Botones menu lateral que manejan el cambio de pestañas.
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

        #region //Botones Examinar en pestaña de Reportes

        //metado para habilitar botones de calcular.
        private void HabilitarCalcular()
        {
            if (txtbox_REP_psragencia.Text != "" && txtbox_REP_pRecarga.Text != "" && txtbox_REP_productosVendidos.Text != "")
            {
                btn_SIM_calcular.Enabled = true;
            }
            if (txtbox_REP_psragencia.Text != "" && txtBox_REPsellout_dealer.Text != "" && txtbox_REP_productosVendidos.Text != "")
            {
                btn_calcularSellout.Enabled = true;
            }
        }
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

        private void btn_SIM_calcular_Click_1(object sender, EventArgs e) //Calcula objetivo SIM
        {

            listBox_SIM.Items.Clear();
            int objVenta = Convert.ToInt32(txtbox_montoObjSIM.Text);

            //se leen las lineas de los reportes.
            string[] reporte_PSRagencia = File.ReadAllLines(txtbox_REP_psragencia.Text);
            string[] reporte_pRecarga = File.ReadAllLines(txtbox_REP_pRecarga.Text);
            string[] reporte_prodVendidos = File.ReadAllLines(txtbox_REP_productosVendidos.Text);
            var psrSIM = new Dictionary<string, PSR>(); //DICCIONARIO PARA PSR.
            int objCumplido = 0;
            int psrTotales = 0;
            int dadosBaja = 0;
            int psrAgenciaLen = reporte_PSRagencia.Length;
            int pRecargasLen = reporte_pRecarga.Length;
            int prodVendidosLen = reporte_prodVendidos.Length;

            for (int i = 2; i < psrAgenciaLen; i++)
            {
                // Array con el contenido de dichas lineas.
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                //Si pertenece al caminante seleccionado entonces se asignan los datos al objeto PSR.
                if (itemsAgencia[agencia_caminanteIndex] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    PSR client = new PSR
                    {
                        CodPSR = itemsAgencia[agencia_codpsrIndex],
                        Caminante = itemsAgencia[agencia_caminanteIndex],
                        Pos = itemsAgencia[agencia_posIndex],
                        Nombre = itemsAgencia[agencia_nameIndex].Replace('"', ' ').Trim(),
                        Calle = itemsAgencia[agencia_direccionIndex],
                        Altura = itemsAgencia[agencia_alturaIndex],
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

                if (itemspRecarga[pRecarga_caminanteIndex] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    string auxPSRcode = itemspRecarga[pRecarga_codpsrIndex];
                    int auxPrecarga = Convert.ToInt32(itemspRecarga[pRecarga_montoIndex]);
                    string auxNim = itemspRecarga[pRecarga_nimIndex];
                    string auxIdSim = itemspRecarga[pRecarga_Id_Index];


                    if (!psrSIM.ContainsKey(auxPSRcode))
                    {
                        PSR client = new PSR
                        {
                            CodPSR = itemspRecarga[pRecarga_codpsrIndex],
                            Caminante = itemspRecarga[pRecarga_caminanteIndex],
                            Pos = "dado de baja",
                            Nombre = itemspRecarga[pRecarga_nombreIndex],
                            NimCliente = itemspRecarga[pRecarga_nimIndex],
                            PrimeraRecarga = Convert.ToInt32(itemspRecarga[pRecarga_montoIndex]),
                            IdSIM = itemspRecarga[pRecarga_Id_Index],
                            Lote = "",
                            EsCumplidor = false,
                        };

                        if (client.PrimeraRecarga >= objCumplido)
                        {
                            client.EsCumplidor = true;
                        }
                        psrSIM[client.CodPSR] = client;
                        dadosBaja++;
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

                if (psrSIM.ContainsKey(itemspVendidos[pVendidos_codpsrIndex]) && psrSIM[itemspVendidos[pVendidos_codpsrIndex]].PrimeraRecarga == 0 && itemspVendidos[pVendidos_checkerIndex] != "Carga Virtual")
                {
                    psrSIM[itemspVendidos[pVendidos_codpsrIndex]].Lote = itemspVendidos[pVendidos_loteIndex];
                }
            }
            int invercion = 0;

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
            txtBox_deBaja.Text = Convert.ToString(dadosBaja);
            txtBox_Efectividad.Text = Convert.ToString(efectividad) + "%";
            txtBox_inversion.Text = "$" + invercion;

            listBox_SIM.Sorted = true;
        } 

        private void btn_calcularSellout_Click(object sender, EventArgs e) //Calcula objetivo SO.
        {
            listBox_Sellout.Items.Clear();
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
            int dadosBaja = 0; 

            //Reporte PSR de la agencia
            for (int i = 2; i < psrAgenciaLen; i++)
            {
                string[] itemsAgencia = reporte_PSRagencia[i].Split(';');
                if (itemsAgencia[agencia_caminanteIndex] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    PSR clientSellout = new PSR();
                    clientSellout.CodPSR = itemsAgencia[agencia_codpsrIndex];
                    clientSellout.Nombre = itemsAgencia[agencia_nameIndex];
                    clientSellout.Caminante = itemsAgencia[agencia_caminanteIndex];
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
                string auxpsrcode = itemspVendidos[pVendidos_codpsrIndex];

                if (itemspVendidos[pVendidos_checkerIndex] == "Carga Virtual" && itemspVendidos[pVendidos_caminanteIndex] == Convert.ToString(comboBox_SIMcaminantes.SelectedItem))
                {
                    psrSellout[auxpsrcode].Transferencias += Convert.ToInt32(itemspVendidos[pVendidos_transferenciaIndex]);
                }
            }
          
            //Reporte Ventas Analitico
            for (int y = 5; y < ventaAnaliticoLen; y++)
            {
                string[] ventasFinales = reporte_dealer[y].Split(';');
                string auxPsrcode = "0"+ventasFinales[dealer_codpsrIndex];
                if (psrSellout.ContainsKey(auxPsrcode))
                {
                    psrSellout[auxPsrcode].VentaMensual = Convert.ToDouble(ventasFinales[dealer_ventasFinalesIndex]);
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
                    +Convert.ToString(psr.VentaMensual) + "  |  " + "Total Transferido: " + Convert.ToString(psr.Transferencias));
            }
            SO_psrTotales.Text = Convert.ToString(psrTotales);
            txtB_soConObjetivo.Text = Convert.ToString(objCumplido);
            txtBox_soFaltan.Text = Convert.ToString(listBox_Sellout.Items.Count);
            txtB_soVolumen.Text = "$"+Convert.ToString(volumenVendedor);
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
                foreach (string items in noCumplidores_SO)
                {
                    strNoCumplidores_Sim += saltoDeLinea + "    " + Convert.ToString(j) + ") " + items;
                    j++;
                }
                int j = 1;
                foreach (string items in noCumplidores_SIM)
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


        #region //Metodo para modificar contadores al hacer doble click en elemento de listbox
        public void modContadores_Sim(string cumplidos, string faltantes)
        {
            string itemSelected = (string)listBox_SIM.SelectedItem;
            string[] arrayItemSelected = itemSelected.Split('$', '/');
            arrayItemSelected[2].Replace(" ", String.Empty);

            int objVenta = Convert.ToInt32(txtbox_montoObjSIM.Text);
            int ventaActual = int.Parse(arrayItemSelected[2]);
            int aRestar = objVenta - ventaActual;
            int invercionActual = int.Parse(txtBox_inversion.Text.Replace("$",String.Empty));
            string inversionFutura = Convert.ToString(invercionActual - aRestar);
            txtBox_inversion.Text = "$" + inversionFutura;

            listBox_SIM.Items.Remove(listBox_SIM.SelectedItem);
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
        public void modContadores_SO(string cumplidos, string faltantes)
        {
            listBox_Sellout.Items.Remove(listBox_Sellout.SelectedItem);
            int objCumplido = int.Parse(cumplidos);
            objCumplido++;
            txtB_soConObjetivo.Text = Convert.ToString(objCumplido);
            int faltaCumplir = int.Parse(faltantes);
            faltaCumplir--;
            txtBox_soFaltan.Text= Convert.ToString(faltaCumplir);
            int psrTotales = int.Parse(SO_psrTotales.Text);
            int efectividad = (objCumplido * 100) / psrTotales;
            SO_efectividad.Text = efectividad + "%";
        }
        #endregion
        private void listBox_SIM_DoubleClick(object sender, EventArgs e) //Quita un cliente de el listbox y modifica los contadores
        {
            if (listBox_SIM.Items.Count > 0)
            {
                modContadores_Sim(txtBox_SimConObj.Text,txtBox_faltaCumplir.Text);
            }
        }

        private void listBox_Sellout_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_Sellout.Items.Count > 0)
            {
                modContadores_SO(txtB_soConObjetivo.Text, txtBox_soFaltan.Text);
            }
        }
        //Textbox "objetivos" solo aceptan numeros.
        private void txtbox_montoObjSIM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtBox_Sellout_objVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
