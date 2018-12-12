using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Investigacion_operativa
{
    public partial class Form1 : Form
    {
        #region PANTALLA INICIAL
        string ruta = @"C:\carpeta\Historial.text";
        bool Histo = true;
        int cantVar = 0;
        bool si = false;
        string a0 = "\u2080";
        string a1 = "\u2081";
        string a2 = "\u2082";
        string a3 = "\u2083";
        string a4 = "\u2084";
        string a5 = "\u2085";
        string a6 = "\u2086";
        string a7 = "\u2087";
        string a8 = "\u2088";
        string a9 = "\u2089";
        Metodo_Simplex ms = new Metodo_Simplex();
        ListaRestricciones lr = new ListaRestricciones();
        public Form1()
        {
            InitializeComponent();
        }

        private void Cambio(object sender, EventArgs e)
        {
            if (maxMin.Checked)
                maxMin.Text = "Max Z=";
            else
                maxMin.Text = "Min Z=";

        }

        private void Entrada(object sender, KeyPressEventArgs e)
        {
            Salida(e, txtFo1);
        }

        private void Salida(KeyPressEventArgs e, TextBox txtSalida)
        {
            if (!si)
            {
                switch (e.KeyChar)
                {
                    case '0':
                        txtSalida.Text += "0";
                        break;
                    case '1':
                        txtSalida.Text += "1";
                        break;
                    case '2':
                        txtSalida.Text += "2";
                        break;
                    case '3':
                        txtSalida.Text += "3";
                        break;
                    case '4':
                        txtSalida.Text += "4";
                        break;
                    case '5':
                        txtSalida.Text += "5";
                        break;
                    case '6':
                        txtSalida.Text += "6";
                        break;
                    case '7':
                        txtSalida.Text += "7";
                        break;
                    case '8':
                        txtSalida.Text += "8";
                        break;
                    case '9':
                        txtSalida.Text += "9";
                        break;
                    case 'x':
                        txtSalida.Text += "X";
                        si = true;
                        break;
                    case 'X':
                        txtSalida.Text += "X";
                        si = true;
                        break;
                    case '^':
                        si = true;
                        break;
                    case '+':
                        txtSalida.Text += "+";
                        break;
                    case '-':
                        txtSalida.Text += "-";
                        break;
                    case '*':
                        txtSalida.Text += "*";
                        break;
                    case '/':
                        txtSalida.Text += "/";
                        break;
                    case '<':
                        txtSalida.Text += "<";
                        break;
                    case '>':
                        txtSalida.Text += ">";
                        break;
                    case '=':
                        txtSalida.Text += "=";
                        break;
                }
            }
            else
            {
                switch (e.KeyChar)
                {
                    case '0':
                        txtSalida.Text += a0;
                        break;
                    case '1':
                        txtSalida.Text += a1;
                        break;
                    case '2':
                        txtSalida.Text += a2;
                        break;
                    case '3':
                        txtSalida.Text += a3;
                        break;
                    case '4':
                        txtSalida.Text += a4;
                        break;
                    case '5':
                        txtSalida.Text += a5;
                        break;
                    case '6':
                        txtSalida.Text += a6;
                        break;
                    case '7':
                        txtSalida.Text += a7;
                        break;
                    case '8':
                        txtSalida.Text += a8;
                        break;
                    case '9':
                        txtSalida.Text += a9;
                        break;
                    case 'x':
                        txtSalida.Text += "X";
                        si = false;
                        break;
                    case 'X':
                        txtSalida.Text += "X";
                        si = false;
                        break;
                    case '^':
                        si = false;
                        break;
                    case '+':
                        txtSalida.Text += "+";
                        si = false;
                        break;
                    case '-':
                        txtSalida.Text += "-";
                        si = false;
                        break;
                    case '*':
                        txtSalida.Text += "*";
                        si = false;
                        break;
                    case '/':
                        txtSalida.Text += "/";
                        si = false;
                        break;
                    case '<':
                        txtSalida.Text += "<";
                        si = false;
                        break;
                    case '>':
                        txtSalida.Text += ">";
                        si = false;
                        break;
                    case '=':
                        txtSalida.Text += "=";
                        si = false;
                        break;
                }
            }
            char Delete = Convert.ToChar(Keys.Back);
            if (Delete == e.KeyChar)
                txtSalida.Text = reducir(txtSalida.Text);
        }

        public string reducir(string val)
        {
            string valu = "";
            for (int i = 0; i < val.Length - 1; i++)
            {
                valu += val[i];
            }
            return valu;
        }

        public void contarX()
        {
            for (int i = 0; i < txtFo1.Text.Length; i++)
            {
                if (txtFo1.Text[i] == 'X')
                    cantVar++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ltbRestricciones1.Items.Clear();
            lr.insertatFin(textBox2.Text);
            lr.mostrar(ltbRestricciones1);
            nudPosicion1.Maximum++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ltbRestricciones1.Items.Clear();
            lr.editar(textBox2.Text, int.Parse(nudPosicion1.Value.ToString()));
            lr.mostrar(ltbRestricciones1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ltbRestricciones1.Items.Clear();
            lr.EliminarPorPosicion(int.Parse(nudPosicion1.Value.ToString()));
            lr.mostrar(ltbRestricciones1);
        }

        private void Entrada2(object sender, KeyPressEventArgs e)
        {
            Salida(e, textBox2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Histo)
                GuardarHistorial();
            CambioDePanel(PanelInicial, panel1);
            txtFo2.Text = txtFo1.Text;
            if (maxMin.Checked)
                lbMaxmin2.Text = "Max Z=";
            else
                lbMaxmin2.Text = "Min Z=";
            lr.mostrar(ltbRestricciones2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtFo1.Text = "3X" + a1 + "+X" + a2;
            ltbRestricciones1.Items.Clear();
            lr.insertatFin("X" + a1 + "+X" + a2 + ">=3");
            lr.insertatFin("2X" + a1 + "+X" + a2 + "<=4");
            lr.insertatFin("X" + a1 + "+X" + a2 + "=3");
            lr.mostrar(ltbRestricciones1);

        }

        private void CambioDePanel(Panel pa, Panel pn)
        {
            pa.Dock = DockStyle.None;
            pn.Dock = DockStyle.Fill;
            pa.Visible = false;
            pn.Visible = true;
        }
        #endregion

        #region Panel1
        private void btnInicio2_Click(object sender, EventArgs e)
        {
            CambioDePanel(panel1, PanelInicial);
        }

        private void btnMetodoSimplex2_Click(object sender, EventArgs e)
        {
            if (maxMin.Checked)
                lbMaxmin3.Text = "Max";
            else
                lbMaxmin3.Text = "Min";
            ms.max = maxMin.Checked;
            ms.restricciones = lr;
            ms.Fo = txtFo1.Text;
            ms.paso2();
            ms.paso3();
            if (ms.ca > 0)
                ms.paso4();
            ms.penalizada.mostrar(ltbPenalizacion3);
            txtFo3.Text = ms.Fo;
            if (ms.ca > 0)
                ms.paso5();
            else
                ms.paso5_1();
            nudPosicion3.Maximum = ms.iteraciones.N;
            CambioDePanel(panel1, panel2);
        }
        private void btnMetodoGrafico_Click(object sender, EventArgs e)
        {
            if (maxMin.Checked)
                lbMaxmin4.Text = "Max";
            else
                lbMaxmin4.Text = "Min";
            txtFo4.Text = txtFo1.Text;
            for (int i = 0; i < lr.N; i++)
                ltbrestricciones4.Items.Add(lr.mostrarPosicion(i).ToString().Replace(">", "").Replace("<", ""));

            CambioDePanel(panel1, panel4);
        }
        #endregion

        #region panel 2
        private void btnInicio3_Click(object sender, EventArgs e)
        {
            CambioDePanel(panel2, PanelInicial);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (nudPosicion3.Value == 1)
                CambioDePanel(paso2, Paso1);
            else
            {
                CambioDePanel(Paso1, paso2);
                ms.iteraciones.mostrarPosicion(dgvSalida3, int.Parse(nudPosicion3.Value.ToString()) - 2);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CambioDePanel(panel1, PanelInicial);
        }
        #endregion

        #region panel 4
        private void btnGrafico_Click(object sender, EventArgs e)
        {
            ltbCoordenada4.Items.Clear();
            Grafico objGra = new Grafico();
            objGra.Zoom = 1;
            objGra.lstRestriccion = lr;
            objGra.Paso1(ltbCoordenada4, ptbGrafica4, ltbResultado);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            ltbCoordenada4.Items.Clear();
            Grafico objGra = new Grafico();
            ptbGrafica4.Refresh();
            objGra.Zoom = Convert.ToInt16(nudZoom4.Value);
            objGra.lstRestriccion = lr;
            objGra.Paso1(ltbCoordenada4, ptbGrafica4, ltbResultado);
        }

        #endregion

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            cbbHistorial1.Visible = true;
            MostrarHistorial();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbHistorial1.Text == "Borrar Todo")
                {
                    File.Delete(ruta);
                    cbbHistorial1.Items.Clear();
                    cbbHistorial1.Visible = false;
                }
                else
                {
                    Histo = false;
                    lr = new ListaRestricciones();
                    ltbRestricciones1.Items.Clear();
                    StreamReader archivo = new StreamReader(ruta);
                    string linea = ModRecuperado(archivo.ReadLine());
                    txtFo1.Text = cbbHistorial1.Text;
                    bool salto = true;
                    while (linea != null)
                        if (linea == cbbHistorial1.Text)
                            while ((linea = archivo.ReadLine()) != null && salto)
                                if (linea == "Salto de Ejercicio")
                                    salto = false;
                                else
                                    lr.insertatFin(ModRecuperado(linea));
                        else
                            linea = archivo.ReadLine();
                    archivo.Close();
                    lr.mostrar(ltbRestricciones1);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void GuardarHistorial()
        {
            try
            {
                if (File.Exists(ruta))
                {
                    StreamWriter escritor = new StreamWriter(ruta, true, Encoding.ASCII);
                    escritor.WriteLine(ModGuardado(txtFo1.Text));
                    for (int i = 0; i < lr.N; i++)
                        escritor.WriteLine(ModGuardado(lr.mostrarPosicion(i).ToString()));
                    escritor.WriteLine("Salto de Ejercicio");
                    escritor.Close();
                }
                else
                {
                    StreamWriter archivo = new StreamWriter(ruta);
                    archivo.Close();
                    GuardarHistorial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MostrarHistorial()
        {
            try
            {
                cbbHistorial1.Items.Clear();
                StreamReader archivo = new StreamReader(ruta);
                string linea = archivo.ReadLine();
                bool primeraLinea = true;
                while (linea != null)
                {
                    if (primeraLinea)
                    {
                        cbbHistorial1.Items.Add(ModRecuperado(linea));
                        primeraLinea = false;
                    }
                    if (linea == "Salto de Ejercicio")
                        primeraLinea = true;
                    linea = archivo.ReadLine();
                }
                archivo.Close();
                cbbHistorial1.Items.Add("Borrar Todo");
            }
            catch (Exception e)
            {
                cbbHistorial1.Visible = false;
                MessageBox.Show("Es posible que no existan datos almacenados en el Historial");
            }
        }
        private string ModGuardado(string clave) {
            return clave.Replace(a0, "a0").Replace(a1, "a1").Replace(a2, "a2").Replace(a3, "a3").Replace(a4, "a4").Replace(a5, "a5").Replace(a6, "a6").Replace(a7, "a7").Replace(a8, "a8").Replace(a9, "a9");
        }
        private string ModRecuperado(string clave) {
            return clave.Replace( "a0" , a0).Replace( "a1", a1).Replace( "a2",a2).Replace("a3", a3).Replace("a4",a4).Replace("a5", a5).Replace("a6", a6).Replace("a7", a7).Replace("a8", a8).Replace("a9", a9);
        }

    }
}