using System;
using System.Drawing;
using System.Windows.Forms;

namespace Investigacion_operativa
{
    class Grafico
    {
        private int zoom;
        private int Igual = 0;

        public int Zoom
        {
            set { zoom = value; }
        }

        Pen lapiznegro = new Pen(Color.Black);
        Graphics dibujo;
        private double valorX1, valorX2;
        public ListaRestricciones lstRestriccion;
        ListaRestriccionesGrafos ltsGrafos = new ListaRestriccionesGrafos();//Seguarda las restricciones
        ListaRestriccionesGrafos ltsGrafosCoordenadas = new ListaRestriccionesGrafos();//Aqui se guarda las coordenadas finales
        public string sub(int a)
        {
            switch (a)
            {
                case 0:
                    return "\u2080";
                case 1:
                    return "\u2081";
                case 2:
                    return "\u2082";
                case 3:
                    return "\u2083";
                case 4:
                    return "\u2084";
                case 5:
                    return "\u2085";
                case 6:
                    return "\u2086";
                case 7:
                    return "\u2087";
                case 8:
                    return "\u2088";
                case 9:
                    return "\u2089";
                default:
                    return "What's your problen man?";

            }
        }
        public void Plano(PictureBox cuadro)
        {
            int x = cuadro.Width / 2 - 10;
            int y = cuadro.Height / 2 - 5;

            dibujo = cuadro.CreateGraphics();
            dibujo.TranslateTransform(x, y);
            dibujo.ScaleTransform(1, -1);

            dibujo.DrawLine(lapiznegro, -x, -y + 5, x + 20, -y + 5);       //Dibija el eje X
            dibujo.DrawLine(lapiznegro, -x + 10, -y - 10, -x + 10, y);  //Dibuja el eje Y
        }
        public void PlanoCoordenadas(PictureBox cuadro, int pos)
        {
            Plano(cuadro);

            int x = cuadro.Width / 2;
            int y = cuadro.Height / 2 - 5;

            dibujo = cuadro.CreateGraphics();
            dibujo.TranslateTransform(x, y);
            dibujo.ScaleTransform(1, -1);

            for (int i = -x + 10; i < x; i = i + 10* zoom)
            {
                dibujo.DrawLine(lapiznegro, -235, i, -245, i);//Eje Y
                dibujo.DrawLine(lapiznegro, i, -240 + 5, i, -250 + 5);//Eje X
            }
            dibujo.DrawLine(LapizRestriccion(pos), (Convert.ToInt32(valorX1) + 1) * 10 * zoom + Igual - 250 + 16, -250, -250, (Convert.ToInt32(valorX2) + 1) * 10 * zoom + Igual - 250 + 22);
            //                       coordenadas X, Ajuste de del origen en pixeles, coordenadas Y 
        }
        public void Paso1(ListBox ltbSalida, PictureBox cuadro, ListBox ltbSalida2)
        {
            int X1 = 0;
            int X2 = 0;
            for (int i = 0; i < lstRestriccion.N; i++)//Este for recorre las resrticciones 
            {
                int a = 0;
                int val = 0;
                string restriccion = lstRestriccion.mostrarPosicion(i).ToString();
                for (int k = 0; k < restriccion.IndexOf(sub(2)); k++) //este for recorre los caracteres de la restriccion
                {
                    if (int.TryParse(restriccion[k].ToString(), out int valInt))//vemos si se puede convertir a entero y obtenemos todo el valor numerico
                        val = val * 10 + valInt;
                    else
                    {
                        if (val == 0)//Cuando es 0 El coeficiente es 1
                        {
                            if (restriccion[k].ToString() + restriccion[k + 1] == "X" + sub(1)) //Vemos si es X1
                            {
                                if (restriccion[a] == '-')
                                    X1 = -1;
                                else
                                    X1 = 1;
                                a = k + 2;
                                k += 2;
                                val = 0;
                            }
                            else if (restriccion[k].ToString() + restriccion[k + 1] == "X" + sub(2)) //Vemos si es X2
                            {
                                if (restriccion[a] == '-')
                                    X2 = -1;
                                else
                                    X2 = 1;
                                a = k + 2;
                                k += 2;
                                val = 0;
                            }
                        }
                        else //Cuando el coeficiente es distinto de 0
                        {
                            if (restriccion[i].ToString() + restriccion[i + 1] == "X" + sub(1)) //Vemos si es X1
                            {
                                if (restriccion[a] == '-')
                                    X1 = -1 * val;
                                else
                                    X1 = val;
                                a = k + 2;
                                k += 2;
                                val = 0;
                            }
                            else if (restriccion[i].ToString() + restriccion[i + 1] == "X" + sub(2)) //Vemos si es X2
                            {
                                if (restriccion[a] == '-')
                                    X1 = -1 * val;
                                else
                                    X1 = 1 * val;
                                a = k + 2;
                                k += 1;
                                val = 0;
                            }
                        }
                    }
                }//Encontramos los valores de X1 y X2
                string signo = "+";
                val = 0;
                for (int k = restriccion.IndexOf('=') + 1; k < restriccion.Length; k++)
                {
                    if (restriccion[k] == '+' || restriccion[k] == '-')
                        signo = restriccion[k].ToString();
                    else if (int.TryParse(restriccion[k].ToString(), out int valInt))
                        val = val * 10 + valInt;
                }
                if (signo == "-")
                    val *= -1;
                ltsGrafos.insertatFin(X1, X2, val);//Se guardan los valores de las X y lado derecho
            }
            Paso2(ltbSalida, cuadro, ltbSalida2);
        }//Separamos todos los valores de las X y el lado derecho
        private void Paso2(ListBox ltbSalida, PictureBox cuadro, ListBox ltbSalida2)
        {
            for (int i = 0; i < ltsGrafos.N; i++)
            {
                ltsGrafos.mostrarPosicion(i, out object X1, out object X2, out object ld);
                ltsGrafosCoordenadas.insertatFin(Dividir(ld.ToString(), X1.ToString()), Dividir(ld.ToString(), X2.ToString()),0);
            }
            ltsGrafosCoordenadas.mostrar(ltbSalida);
            Paso3(cuadro, ltbSalida2);
        }
        private void Paso3(PictureBox cuadro, ListBox ltbSalida2)
        {
            for (int i = 0; i < ltsGrafosCoordenadas.N; i++)
            {
                Igual = 0;
                ltsGrafosCoordenadas.mostrarPosicion(i, out object x1, out object x2,out object ld);
                valorX1 = fracToDecim(x1.ToString());
                valorX2 = fracToDecim(x2.ToString());
                ExisteOtro(i);
                PlanoCoordenadas(cuadro, i);
            }
        }
        private void Paso4(ListBox ltbSalida2)
        {

        }
        private void ExisteOtro(int ii)
        {
                bool uno = false;
            for (int i = ii; i < ltsGrafos.N; i++)
            {
                ltsGrafosCoordenadas.mostrarPosicion(i, out object x1, out object x2, out object ld);
                x1 = fracToDecim(x1.ToString());
                x2 = fracToDecim(x2.ToString());
                if (valorX1 == fracToDecim(x1.ToString()) && valorX2 == fracToDecim(x2.ToString()))
                    if (uno)
                        Igual += 2;
                    else
                        uno = true;
            }
        }
        private string Dividir(string numA, string numB)
        {
            SepararFraccion(out int numeA, out int denA, numA);
            SepararFraccion(out int numeB, out int denB, numB);
            numeA *= denB;
            denA *= numeB;
            return Simplificar(numeA, denA);
        }
        private void SepararFraccion(out int num, out int den, string fraccion)
        {
            if (fraccion == "")
            {
                num = 1;
                den = 1;
            }
            else
            {
                num = 0;
                den = 1;
                string signo = "+";
                int i = 0;
                if (fraccion[0] == '-')
                {
                    signo = "-";
                    i++;
                }
                bool paso = true;
                while (paso && i < fraccion.Length)
                {
                    paso = int.TryParse(fraccion[i].ToString(), out int valInt);
                    if (paso)
                    {
                        num = num * 10 + valInt;
                        i++;
                    }
                }
                num *= Convert.ToInt32(signo + "1");
                if (i < fraccion.Length)
                    if (fraccion[i] == '/')
                    {
                        i++;
                        paso = true;
                        den = 0;
                        while (paso && i < fraccion.Length)
                        {
                            paso = int.TryParse(fraccion[i].ToString(), out int valInt);
                            if (paso)
                                den = den * 10 + valInt;
                            i++;
                        }
                    }
            }
        }
        private string Simplificar(int a, int b)
        {
            if (a == 0 || b == 0)
                return "0";
            double may = a;
            if (b > may)
                may = b;
            for (int i = 2; i <= may; i++)
            {
                while (a % i == 0 && b % i == 0)
                {
                    a = a / i;
                    b = b / i;
                    if (b > a)
                        may = b;
                    else
                        may = a;
                }
            }

            if (b == 1)
                return a.ToString();
            else
                return a.ToString() + "/" + b.ToString();
        }
        private Pen LapizRestriccion(int pos)
        {
            pos = pos % 10;
            switch (pos)
            {
                case 0:
                    return new Pen(Color.Green);
                case 1:
                    return new Pen(Color.Yellow);
                case 2:
                    return new Pen(Color.Red);
                case 3:
                    return new Pen(Color.Purple);
                case 4:
                    return new Pen(Color.Blue);
                case 5:
                    return new Pen(Color.Brown);
                case 6:
                    return new Pen(Color.Orange);
                case 7:
                    return new Pen(Color.Pink);
                case 8:
                    return new Pen(Color.YellowGreen);
                case 9:
                    return new Pen(Color.Turquoise);
                default:
                    return new Pen(Color.Gold);
            }
        }
        public double fracToDecim(string Frac)
        {
            if (Frac == "0" || Frac == "")
                return 0;
            else
            {
                int i = 0;
                string signo = "+";
                double num = 0;
                double den = 0;
                if (Frac[i] == '-')
                {
                    signo = "-";
                    i++;
                }
                bool cambio = true;
                while (cambio && i < Frac.Length)
                {
                    cambio = int.TryParse(Frac[i].ToString(), out int valInt);
                    if (cambio)
                        num = num * 10 + valInt;
                    i++;
                }
                num *= Convert.ToDouble(signo + "1");
                if (num == 0)
                    num = 1;
                while (i < Frac.Length)
                {
                    cambio = int.TryParse(Frac[i].ToString(), out int valInt);
                    if (cambio)
                        den = den * 10 + valInt;
                    i++;
                }
                if (den > 0)
                    return num / den;
                else
                    return num;
            }
        }

    }
}
