using System;
using System.Linq;

namespace Investigacion_operativa
{
    class Metodo_Simplex
    {
        public ListaRestricciones restricciones;
        int ch = 0, cs = 0, cx = 0;
        public int ca = 0;
        private object[] vectorZ;
        public ListaRestricciones penalizada = new ListaRestricciones();
        private ListaIteraciones iteracionesSM = new ListaIteraciones();
        public ListaIteraciones iteraciones = new ListaIteraciones();
        public string Fo;
        public bool max;
        public object[,] MatrizIteracion;
        int tamX, tamY;
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
        public void paso2()
        {

            for (int i = 0; i < restricciones.N; i++)
                if (restricciones.mostrarPosicion(i).ToString().Contains("<") && restricciones.mostrarPosicion(i).ToString().Contains("="))
                {
                    //Se le agrega la variable de holgura + H
                    string captura = restricciones.mostrarPosicion(i).ToString().Replace("<", "+h" + sub(i + 1));
                    penalizada.insertatFin(captura);
                    Fo += "+0h" + sub(i + 1);
                    ch++;
                }
                else if (restricciones.mostrarPosicion(i).ToString().Contains(">") && restricciones.mostrarPosicion(i).ToString().Contains("="))
                {
                    //Se le agrega una variable  de olgura y penalizacion - S + A
                    string captura = restricciones.mostrarPosicion(i).ToString().Replace(">", "-s" + sub(i + 1) + "+A" + sub(i + 1));
                    penalizada.insertatFin(captura);
                    if (max)
                        Fo += "-0s" + sub(i + 1) + "-MA" + sub(i + 1);
                    else
                        Fo += "-0s" + sub(i + 1) + "+MA" + sub(i + 1);
                    cs++; ca++;
                }
                else
                {
                    //Se le agrega solo penalizacion + A
                    penalizada.insertatFin(restricciones.mostrarPosicion(i).ToString().Insert(restricciones.mostrarPosicion(i).ToString().IndexOf('='), "+A" + sub(i + 1)));
                    if (max)
                        Fo += "-MA" + sub(i + 1);
                    else
                        Fo += "+MA" + sub(i + 1);
                    ca++;
                }
            Fo = Fo.Replace('+', 'P');
            Fo = Fo.Replace('-', 'O');
            Fo = Fo.Replace('P', '-');
            Fo = Fo.Replace('O', '+');
            Fo = "Z-" + Fo + "=0";
        }//Se penaliza Y se coloca holgura y lo sea "s" donde se necesite
        public void paso3()
        {
            #region ETIQUETAS
            for (int i = 0; i < Fo.Length; i++)
                if (Fo[i].Equals('X'))
                    cx++;
            tamX = ca + ch + cs + cx + 2;
            tamY = 2 + ca + ch;
            MatrizIteracion = new object[tamX, tamY];
            vectorZ = new object[tamX];
            for (int i = 0; i < tamX; i++)
            {
                vectorZ[i] = 0;
                for (int j = 0; j < tamY; j++)
                {
                    MatrizIteracion[i, j] = 0;
                }
            }

            int y = 2;
            MatrizIteracion[tamX - 1, 0] = "LD";
            int penaliza = 0;
            MatrizIteracion[0, 1] = "Z";
            for (int i = 0; i < tamX - 1; i++)
            {
                if (i == 0)
                    MatrizIteracion[0, 0] = "VB";
                else if (i == tamX - 1)
                    MatrizIteracion[tamX - 1, 0] = "LD";
                else if (i <= cx)
                {
                    MatrizIteracion[i, 0] = "X" + sub(i);
                }
                else
                {
                    if (penalizada.mostrarPosicion(penaliza).ToString().Contains('h'))
                    {
                        MatrizIteracion[i, 0] = "h" + sub(penaliza + 1);
                        MatrizIteracion[0, y] = "h" + sub(penaliza + 1);
                    }
                    else if (penalizada.mostrarPosicion(penaliza).ToString().Contains('s'))
                    {
                        MatrizIteracion[i, 0] = "s" + sub(penaliza + 1);
                        MatrizIteracion[i + 1, 0] = "A" + sub(penaliza + 1);
                        MatrizIteracion[0, y] = "A" + sub(penaliza + 1);
                        i++;
                    }
                    else if (penalizada.mostrarPosicion(penaliza).ToString().Contains('A'))
                    {
                        MatrizIteracion[i, 0] = "A" + sub(penaliza + 1);
                        MatrizIteracion[0, y] = "A" + sub(penaliza + 1);
                    }
                    y++;
                    penaliza++;
                }
            }
            LadoDerecho(ref MatrizIteracion, penalizada);
            //se nom
            #endregion
            InsertarMatriz(ref MatrizIteracion, Fo, true, 1);//Se coloca la fo en la matriz
            for (int i = 2; i < penalizada.N + 2; i++)
                InsertarMatriz(ref MatrizIteracion, penalizada.mostrarPosicion(i - 2).ToString(), false, i);//Se coloca las restricciones penalizadas en la matriz en la matriz
            iteracionesSM.insertatFin(MatrizIteracion, 0, 0);
            iteraciones.insertatFin(InsertarM(MatrizIteracion,vectorZ), 0, 0);
            iteraciones.tx = tamX; iteraciones.ty = tamY;
        }//Se traspasa las restricciones y la Fo penalizadas a la matriz para trabajar
        public void paso4()
        {
            for (int i = 1 + cx; i < tamX - 1; i++)
            {
                object[,] itera = new object[tamX, tamY];
                CopiarMatriz(iteracionesSM.mostrarUltimo(), itera);
                if (itera[i, 0].ToString().Contains("A"))
                {
                    int posX = i;
                    int posY = 2;
                    while (itera[posX, posY].ToString() != "1")
                        posY++;
                    int pibot = Convert.ToInt16(vectorZ[posX].ToString());
                    for (int k = 1; k < tamX; k++)
                        vectorZ[k] = Convert.ToInt16(vectorZ[k].ToString()) - pibot  * Convert.ToInt16(itera[k, posY].ToString());
                    iteracionesSM.insertatFin(itera, 0, 0);
                    iteraciones.insertatFin(InsertarM(itera, vectorZ), 0, 0);
                }
            }
        }//Se vuelve 0 el valor de las varables artificiales en Z
        public void paso5()
        {
            bool reitera = false;
            bool Igualdad = false;
            int posY2 = 0;
            int estado = 0;
            int avance = 0;
            object[] vecAux = new object[tamX];
            for (int i = 1; i <= cx; i++)
            {
                if (reitera)
                    CopiarVector(vecAux, vectorZ);
                object[,] itera = new object[tamX, tamY];
                CopiarMatriz(iteracionesSM.mostrarUltimo(), itera);
                int posX = 0;
                int posY = 0;
                double valMin = -100000;
                double aux = 0;
                for (int k = 1; k <= cx; k++)
                {
                    string num = itera[k, 1].ToString();
                    string a = vectorZ[k].ToString();
                    double ass = fracToDecim(a);
                    if (valMin < ass && ass < 0)
                    {
                        valMin = fracToDecim(a);
                        posX = k;
                        aux = fracToDecim(num);
                    }
                    else if (valMin == fracToDecim(a))
                    {
                        if (aux > fracToDecim(a) && fracToDecim(a) != 0)
                        {
                            valMin = fracToDecim(a);
                            posX = k;
                            aux = fracToDecim(num);
                        }
                    }
                }
                valMin = 100000;
                if (reitera)
                {
                    Igualdad = false;
                    posY = posY2;
                    avance = 0;
                    reitera = false;
                }
                else
                    for (int k = 2; k < tamY; k++)
                    {
                        if (valMin > fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString()))
                        {
                            valMin = fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString());
                            posY = k;
                        }
                        else if (valMin == fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString()))
                        {
                            posY2 = k;
                            CopiarVector(vectorZ, vecAux);
                            estado = i;
                            Igualdad = true;
                        }
                    }
                if (Igualdad)
                    avance++;
                string valorPibote = itera[posX, posY].ToString();
                for (int k = 1; k < tamX; k++)
                    itera[k, posY] = Dividir(itera[k, posY].ToString(), valorPibote);
                string pn = itera[posX, 1].ToString();
                string pm = vectorZ[posX].ToString();
                for (int k = 1; k < tamX; k++)
                {
                    itera[k, 1] = Restar(itera[k, 1].ToString(), Multiplicar(pn, itera[k, posY].ToString()));
                    vectorZ[k] = Restar(vectorZ[k].ToString(), Multiplicar(pm, itera[k, posY].ToString()));
                }
                for (int k = 2; k < tamY; k++)
                {
                    valorPibote = Multiplicar(itera[posX, k].ToString(), "-1");
                    if (k != posY)
                        for (int j = 1; j < tamX; j++)
                            itera[j, k] = Sumar(itera[j, k].ToString(), Multiplicar(valorPibote, itera[j, posY].ToString()));
                }
                itera[0, posY] = itera[posX, 0];
                iteracionesSM.insertatFin(itera, posX, posY);
                iteraciones.insertatFin(InsertarM(itera, vectorZ), posX, posY);
                if (HayNegativos(vectorZ) && i == cx && Igualdad)
                {
                    for (int k = 0; k < avance; k++)
                    {
                        iteraciones.EliminarUltimo();
                        iteracionesSM.EliminarUltimo();
                    }
                    i = estado - 1;
                    reitera = true;
                }
            }
        }//Realizamos todas las iteraciones
        public void paso5_1()
        {
            bool reitera = false;
            bool Igualdad = false;
            int posY2 = 0;
            int estado = 0;
            int avance = 0;
            for (int i = 1; i <= cx; i++)
            {
                object[,] itera = new object[tamX, tamY];
                CopiarMatriz(iteraciones.mostrarUltimo(), itera);
                int posX = 0;
                int posY = 0;
                double valMin = 100000;
                for (int k = 1; k <= cx; k++)
                {
                    if (valMin > fracToDecim(itera[k, 1].ToString()))
                    {
                        valMin = fracToDecim(itera[k, 1].ToString());
                        posX = k;
                    }
                }
                valMin = 100000;
                if (reitera)
                {
                    posY = posY2;
                    reitera = 1 == 2;
                    Igualdad = 2 == 1;
                    avance = 0;
                }
                else
                    for (int k = 2; k < tamY; k++)
                    {
                        if (valMin > fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString()))
                        {
                            valMin = fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString());
                            posY = k;
                        }
                        else if (valMin == fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString()))
                        {
                            posY2 = k;
                            estado = i;
                            Igualdad = 1 == 1;
                        }
                    }
                if (Igualdad)
                    avance++;
                string valorPibote = itera[posX, posY].ToString();
                for (int k = 1; k < tamX; k++)
                {
                    itera[k, posY] = Dividir(itera[k, posY].ToString(), valorPibote);
                }
                for (int k = 1; k < tamY; k++)
                {
                    valorPibote = Multiplicar(itera[posX, k].ToString(), "-1");
                    if (k != posY)
                        for (int j = 1; j < tamX; j++)
                            itera[j, k] = Sumar(itera[j, k].ToString(), Multiplicar(valorPibote, itera[j, posY].ToString()));
                }
                itera[0, posY] = itera[posX, 0];
                iteraciones.insertatFin(itera, posX, posY);
                if (HayNegativos(vectorZ) && i == cx && Igualdad)
                {
                    for (int k = 0; k < avance; k++)
                        iteraciones.EliminarUltimo();
                    i = estado;
                    reitera = true;
                }
            }
        }
        public void paso6()
        {
            //presidente de CAF bolivia ha tenido un crecimineto importante
        }
        private int ubicar(string var, object[,] matriz)
        {
            for (int H = 1; H < tamX; H++)
            {
                if (matriz[H, 0].ToString() == var)
                {
                    return H;
                }
            }
            return -1;
        }
        private void InsertarMatriz(ref object[,] matriz, string elementos, bool esFo, int y)
        {
            int val = 0;
            int a = 1;
            string signo = "+";
            int inicio;
            if (esFo)
            {
                a = 1;
                inicio = 2;
                signo = elementos[1].ToString();
            }
            else
            {
                a = 0;
                inicio = 0;
                signo = "+";
                if (elementos[0].ToString()=="-")
                {
                    signo = "-";
                    inicio++;
                }
            }
            for (int i = inicio; i < elementos.Length - 2; i++)
            {
                if (int.TryParse(elementos[i].ToString(), out int valInt))//vemos si se puede convertir a entero y obtenemos todo el valor numerico
                    val = val * 10 + valInt;
                else
                {
                    if (val == 0 && elementos[i] == 'X')//Vemos si es X
                        matriz[ubicar(elementos[i].ToString() + elementos[i + 1].ToString(), matriz), y] = Convert.ToInt64(signo + "1");
                    else if ((elementos[i] == 's' || elementos[i] == 'h' || elementos[i] == 'A') && !esFo)//Vemos si es una restriccion
                        matriz[ubicar(elementos[i].ToString() + elementos[i + 1].ToString(), matriz), y] = Convert.ToInt64(signo + "1");
                    else if (elementos[i] == 'M')
                    {
                        if (esFo)
                            vectorZ[ubicar(elementos[i + 1].ToString() + elementos[i + 2].ToString(), matriz)] = Convert.ToInt16(signo + "1");
                        else
                            matriz[ubicar(elementos[i + 1].ToString() + elementos[i + 2].ToString(), matriz), y] = "1";
                        i++;
                    }
                    else
                        matriz[ubicar(elementos[i].ToString() + elementos[i + 1].ToString(), matriz), y] = val * Convert.ToInt64(signo + "1");
                    val = 0;
                    a = i + 2;
                    signo = elementos[a].ToString();
                    i = i + 2;
                }
            }
        }
        private void LadoDerecho(ref object[,] matriz, ListaRestricciones elem)
        {
            for (int i = 2; i < tamY; i++)
            {
                {
                    int val = 0;
                    string elementos = elem.mostrarPosicion(i - 2).ToString();
                    int inicio = elementos.IndexOf('=') + 1;
                    string signo = "+";
                    if (elementos[inicio] == '+' || elementos[inicio] == '-')
                    {
                        signo = elementos[inicio].ToString();
                        inicio++;
                    }
                    for (int k = inicio; k < elementos.Length; k++)
                        if (int.TryParse(elementos[k].ToString(), out int valInt))
                            val = val * 10 + valInt;
                    matriz[tamX - 1, i] = val * Convert.ToInt64(signo + "1");
                }
            }
        }
        private void CopiarMatriz(object[,] matrizA, object[,] matrizB)
        {
            for (int i = 0; i < tamX; i++)
            {
                for (int j = 0; j < tamY; j++)
                {
                    matrizB[i, j] = matrizA[i, j];
                }
            }
        }
        public string decimTofrac(string num)
        {
            if (int.TryParse(num, out int numero))
                return numero.ToString();
            else
            {
                long count = 0;
                long numerador = 1;
                count = (num.Length - num.IndexOf(",") - 1);
                if (count > 3)
                {
                    string nume = num;
                    num = "";
                    for (int i = 0; i < (nume.IndexOf(',') + 3); i++)
                        num += nume[i];
                    count = (num.Length - num.IndexOf(",") - 1);
                }
                count = Convert.ToInt64(Math.Pow((10), (count)));
                numerador = Convert.ToInt64(num.Replace(",", ""));
                double may = numerador;
                if (count > may)
                    may = count;
                for (int i = 2; i < may; i++)
                {
                    while (numerador % i == 0 && count % i == 0)
                    {
                        numerador = numerador / i;
                        count = count / i;
                        if (count > numerador)
                            may = count;
                        else
                            may = numerador;
                    }
                }
                num = Convert.ToString(numerador) + "/" + Convert.ToString(count);
                return num;
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
        private string Multiplicar(string numA, string numB)
        {
            SepararFraccion(out int numeA, out int denA, numA);
            SepararFraccion(out int numeB, out int denB, numB);
            numeA *= numeB;
            denA *= denB;
            return Simplificar(numeA, denA);
        }        
        private string Sumar(string numA, string numB)
        {
            SepararFraccion(out int numeA, out int denA, numA);
            SepararFraccion(out int numeB, out int denB, numB);
            numeA *= denB;
            numeB *= denA;
            denA *= denB;
            numeA += numeB;
            return Simplificar(numeA, denA);
        }
        private string Restar(string numA, string numB)
        {
            SepararFraccion(out int numeA, out int denA, numA);
            SepararFraccion(out int numeB, out int denB, numB);
            numeA *= denB;
            numeB *= denA;
            denA *= denB;
            numeA -= numeB;
            return Simplificar(numeA, denA);
        }
        private bool HayNegativos(object[] vector)
        {
            for (int i = 1; i < tamX; i++)
            {
                string nM = vector[i].ToString();
                if (fracToDecim(nM) < 0)
                    return true;
            }
            return false;
        }
        private object[,] InsertarM(object[,] matriz, object[] vector)
        {
            object[,] matrizA = new object[tamX,tamY];
            CopiarMatriz(matriz, matrizA);
            for (int i = 1; i < tamX; i++)
            {
                if (vector[i].ToString() != "0")
                    if (matrizA[i, 1].ToString() == "0")
                        if (vector[i].ToString() == "1")
                            matrizA[i, 1] = "M";
                        else if (vector[i].ToString() == "-1")
                            matrizA[i, 1] = "-M";
                        else
                            matrizA[i, 1] = vector[i].ToString() + "M";
                    else if (vector[i].ToString() == "1")
                        matrizA[i, 1] = matrizA[i, 1].ToString() + "+M";
                    else if (vector[i].ToString() == "-1")
                        matrizA[i, 1] = matrizA[i, 1].ToString() + "-M";
                    else if (fracToDecim(vectorZ[i].ToString()) > 0)
                        matrizA[i, 1] = matrizA[i, 1].ToString() + "+" + vector[i].ToString() + "M";
                    else
                        matrizA[i, 1] = matrizA[i, 1].ToString() + vector[i].ToString() + "M";
            }
            return matrizA;
        }
        private void CopiarVector(object[] vecA, object[] vecB)
        {
            for (int i = 0; i < tamX; i++)
                vecB[i] = vecA[i];
        }

        #region Codigo fallido

        //private void SepararConM(out string numero, out string numeroM, string valor)
        //{
        //    numero = "0";
        //    numeroM = "0";
        //    //if (valor=="+M" || valor=="M")

        //    int i = 0;
        //    bool paso = true;
        //    int numeroI = 0;
        //    string valorCapturado;
        //    string signo = "+";
        //    if (valor[0] == '-')
        //    {
        //        signo = "-";
        //        i++;
        //    }
        //    else if (valor[0] == '+')
        //    {
        //        signo = "+";
        //        i++;
        //    }
        //    while (paso && i < valor.Length)
        //    {
        //        paso = int.TryParse(valor[i].ToString(), out int val);
        //        if (paso)
        //        {
        //            numeroI = numeroI * 10 + val;
        //            i++;
        //        }
        //    }
        //    valorCapturado = numeroI.ToString();
        //    if (i < valor.Length)
        //    {
        //        if (valor[i].ToString() == "/")
        //        {
        //            i++;
        //            paso = i < valor.Length;
        //            numeroI = 0;
        //            while (paso && i < valor.Length)
        //            {
        //                paso = int.TryParse(valor[i].ToString(), out int val);
        //                if (paso)
        //                {
        //                    numeroI = numeroI * 10 + val;
        //                    i++;
        //                }
        //            }
        //            valorCapturado += "/" + numeroI;
        //        }//Verificamos si tiene Fraccion
        //        if (!valor.Contains("M"))
        //            numero = valorCapturado;
        //        else if (valor[i].ToString() == "M")
        //            numeroM = valorCapturado;
        //        else
        //        {
        //            if (signo == "-")
        //                numero = "-" + valorCapturado;
        //            else
        //                numero = valorCapturado;
        //            if (valor[i] == '-')
        //                signo = "-";
        //            else
        //                signo = "+";
        //            i++;
        //            paso = true;
        //            numeroI = 0;
        //            while (paso && i < valor.Length)
        //            {
        //                paso = int.TryParse(valor[i].ToString(), out int val);
        //                if (paso)
        //                {
        //                    numeroI = numeroI * 10 + val;
        //                    i++;
        //                }
        //            }
        //            valorCapturado = numeroI.ToString();
        //            if (i < valor.Length)
        //            {
        //                if (valor[i].ToString() == "/")
        //                {
        //                    i++;
        //                    paso = true;
        //                    numeroI = 0;
        //                    while (paso && i < valor.Length)
        //                    {
        //                        paso = int.TryParse(valor[i].ToString(), out int val);
        //                        if (paso)
        //                        {
        //                            numeroI = numeroI * 10 + val;
        //                            i++;
        //                        }
        //                    }
        //                    valorCapturado += "/" + numeroI;
        //                }
        //            }
        //            if (signo == "-")
        //                numeroM = "-" + valorCapturado;
        //            else
        //                numeroM = valorCapturado;
        //        }
        //    }
        //    else
        //    {
        //        if (signo == "-")
        //            numero = numeroI.ToString();
        //        else
        //            numero = signo + numeroI;
        //    }
        //}


        //private string RestarIteraciones(string valorA, string filaPibote, string valorX)
        //{
        //    SepararConM(out string nA, out string mA, valorA);
        //    SepararConM(out string nP, out string mP, filaPibote);
        //    SepararConM(out string nX, out string mX, valorX);
        //    if (nP[0] == '-')
        //        nP = nP.Replace("-", "");
        //    else if (nP[0] == '+')
        //        nP = nP.Replace("+", "-");
        //    else
        //        nP = "-" + nP;

        //    nX = Multiplicar(nX, nP);
        //    mX = Multiplicar(mX, nP);
        //    nA = Sumar(nA, nX);
        //    mA = Sumar(mA, mX);
        //    if (nA != "0" && mA != "0")
        //        if (mA[0] == '-')
        //            return nA + mA + "M";
        //        else
        //            return nA + "+" + mA + "M";
        //    else if (nA != "0")
        //        return nA;
        //    else if (mA != "0")
        //        return mA + "M";
        //    else
        //        return "0";
        //}

        //private void SepararConM(out string numero, out string numeroM, string valor)
        //{
        //    string signo = "+";
        //    int i = 0;
        //    int numeroI = 0;
        //    bool paso = true;
        //    if (valor[0] == '-')
        //    {
        //        signo = "-";
        //        i++;
        //    }
        //    while (paso && i < valor.Length)
        //    {
        //        paso = int.TryParse(valor[i].ToString(), out int val);
        //        if (paso)
        //        {
        //            numeroI = numeroI * 10 + val;
        //            i++;
        //        }
        //    }//Se obtiene el primer valor
        //    numero = numeroI.ToString();
        //    if (i < valor.Length)
        //    {
        //        if (valor[i] == '/')
        //        {
        //            i++;
        //            paso = true;
        //            numeroI = 0;
        //            while (paso && i < valor.Length)
        //            {
        //                paso = int.TryParse(valor[i].ToString(), out int val);
        //                if (paso)
        //                {
        //                    numeroI = numeroI * 10 + val;
        //                    i++;
        //                }
        //            }//Se obtiene el denominador del primer valor
        //            numero += "/" + numeroI;
        //        }
        //    }
        //    if (signo == "-")
        //        numero = "-" + numero;
        //    numeroM = "0";
        //    if (i < valor.Length)
        //        if (valor[i] == '+' || valor[i] == '-')
        //        {
        //            signo = "+";
        //            if (valor[i] == '-')
        //                signo = "-";
        //            i++;
        //            paso = true;
        //            numeroI = 0;
        //            while (paso && i < valor.Length)
        //            {
        //                paso = int.TryParse(valor[i].ToString(), out int val);
        //                if (paso)
        //                {
        //                    numeroI = numeroI * 10 + val;
        //                    i++;
        //                }
        //            }//Se obtiene el primer valor
        //            numeroM = signo + numeroI.ToString();
        //            if (i < valor.Length)
        //            {
        //                if (valor[i] == '/')
        //                {
        //                    i++;
        //                    paso = true;
        //                    numeroI = 0;
        //                    while (paso && i < valor.Length)
        //                    {
        //                        paso = int.TryParse(valor[i].ToString(), out int val);
        //                        if (paso)
        //                        {
        //                            numeroI = numeroI * 10 + val;
        //                            i++;
        //                        }
        //                    }//Se obtiene el denominador del primer valor
        //                    numeroM += "/" + numeroI;
        //                }
        //            }
        //        }
        //        else if (valor[i] == 'M')
        //        {
        //            numeroM = numero;
        //            numero = "0";
        //        }
        //}
        //private void SepararValores(out double valA, out double valB, string valores)
        //{
        //    if (valores == "0")
        //    {
        //        valA = 0;
        //        valB = 0;
        //    }
        //    else
        //    {
        //        int i = valores.IndexOf("M") - 1;
        //        valA = 0;
        //        valB = 0;
        //        bool paso = i >= 0;
        //        string signo = "+";
        //        string signo2 = "+";
        //        while (paso && i >= 0)
        //        {
        //            paso = int.TryParse(valores[i].ToString(), out int valInt);
        //            if (paso)
        //                valB = valB * 10 + valInt;
        //            i--;
        //        }
        //        if (valores[i + 1]=='/')
        //        {
        //            paso = true;
        //            double numera = 0;
        //            while (paso && i >= 0)
        //            {
        //                paso = int.TryParse(valores[i].ToString(), out int valInt);
        //                if (paso)
        //                    numera = numera * 10 + valInt;
        //                i--;
        //            }
        //            valB = numera / valB;
        //        }
        //        if (valB == 0)
        //            valB = 1;
        //        i++;
        //        if (i >= 0)
        //        {
        //            if (valores[i] == '-')
        //                signo2 = "-";
        //            i--;
        //            paso = true;
        //            if (i >= 0)
        //            {
        //                while (paso && i >= 0)
        //                {
        //                    paso = int.TryParse(valores[i].ToString(), out int valInt);
        //                    if (paso)
        //                        valA = valA * 10 + valInt;
        //                    i--;
        //                }
        //                if (valores[i + 1] == '/')
        //                {
        //                    paso = true;
        //                    double numera = 0;
        //                    while (paso && i >= 0)
        //                    {
        //                        paso = int.TryParse(valores[i].ToString(), out int valInt);
        //                        if (paso)
        //                            numera = numera * 10 + valInt;
        //                        i--;
        //                    }
        //                    valB = numera / valA;
        //                }
        //                if (valores[0] == '-')
        //                    signo = "-";
        //            }
        //            else
        //                valA = 0;
        //        }
        //        valB *= Convert.ToInt32(signo2 + "1");
        //        valA *= Convert.ToInt32(signo + "1");
        //    }
        //}
        //private string RestarPenalizadas(string ValActual, string bas, string FactorAEliminar)
        //{
        //    SepararValores(out double numA, out double numB, ValActual);
        //    SepararValores(out double numC, out double numD, FactorAEliminar);
        //    double numBase = fracToDecim(bas);
        //    numC *= -numBase;
        //    numD *= -numBase;
        //    numC += numA;
        //    numD += numB;
        //    if (numC == 0 && numD == 0)
        //        return "0";
        //    else if (numD == 0 && numC != 0)
        //        return decimTofrac(numC.ToString());
        //    else if (numC == 0 && numD != 0)
        //        return decimTofrac(numD.ToString()) + "M";
        //    else if (numC == 0 && numD != 0)
        //        return decimTofrac(numD.ToString()) + "M";
        //    else if (numD > 0)
        //        return decimTofrac(numC.ToString()) + "+" + decimTofrac(numD.ToString()) + "M";
        //    else
        //        return decimTofrac(numC.ToString()) + decimTofrac(numD.ToString()) + "M";
        //}
        //private double RestarNumeros(string ValActual, string bas, string FactorAEliminar)
        //{
        //    double numA = fracToDecim(ValActual);
        //    double numB = fracToDecim(FactorAEliminar) * fracToDecim(bas);
        //    return numA - numB;
        //}


        // paso 5
        //        bool Igual = false;
        //        bool reIteracion = false;
        //        int valX = 0;
        //        int PuntoReferencia = 0;
        //        int cantidadAvance = 0;
        //            for (int i = 1; i <= cx; i++)
        //            {
        //                object[,] itera = new object[tamX, tamY];
        //        CopiarMatriz(iteraciones.mostrarUltimo(), itera);
        //        int posX = 0;
        //        int posY = 0;
        //        string pibote = "0";
        //        double aux = 0;
        //        double valMinX = 100000;
        //                for (int k = 1; k <= cx; k++)
        //                {
        //                    string num;
        //        SepararConM(out num, out string a, itera[k, 1].ToString());
        //                    if (valMinX > fracToDecim(a) && fracToDecim(a) != 0)
        //                    {
        //                        valMinX = fracToDecim(a);
        //        posX = k;
        //                        aux = fracToDecim(num);
        //    }
        //                    else if (valMinX== fracToDecim(a))
        //                    {
        //                        if (aux > fracToDecim(a) && fracToDecim(a) != 0)
        //                        {
        //                            valMinX = fracToDecim(a);
        //    posX = k;
        //                            aux = fracToDecim(num);
        //}
        //                    }
        //                }//Encontramos la Columna pibote  //bien
        //                double valMinY = 1000;
        //                if (reIteracion && valX == i)
        //                {
        //                    posY = PuntoReferencia;
        //                    pibote = itera[posX, posY].ToString();
        //Igual = false;
        //                }
        //                else
        //                    for (int k = 2; k<tamY; k++)
        //                    {
        //                        double calculo = fracToDecim(itera[tamX - 1, k].ToString()) / fracToDecim(itera[posX, k].ToString());
        //                        if (calculo<valMinY && calculo> 0)
        //                        {
        //                            valMinY = calculo;
        //                            pibote = itera[posX, k].ToString();
        //posY = k;

        //                        }
        //                        else if (calculo == valMinY)
        //                        {
        //                            valX = i;
        //                            PuntoReferencia = k;
        //                            Igual = true;
        //                        }
        //                    }//Encontramos la Fila pibote    //bien
        //                if (Igual)
        //                    cantidadAvance++;
        //                for (int k = 1; k<tamX; k++)  
        //                    itera[k, posY] = Dividir(itera[k, posY].ToString(), pibote.ToString());
        //itera[posX, posY] = 1;//Volvemos a uno el pivote   
        //                for (int k = 1; k<tamY; k++)
        //                {
        //                    string FactorAEliminar = itera[posX, k].ToString();
        //                    if (k != posY)
        //                        for (int j = 1; j<tamX; j++)
        //                              itera[j, k] = RestarIteraciones(itera[j, k].ToString(), itera[j, posY].ToString(), FactorAEliminar);
        //                }
        //                itera[0, posY] = itera[posX, 0];
        //                iteraciones.insertatFin(itera, posX, posY);
        //                if (HayNegativos(itera) && i == cx && Igual)
        //                {
        //                    for (int k = 0; k<cantidadAvance; k++)
        //                    {
        //                        iteraciones.EliminarUltimo();
        //                    }
        //                    i = valX - 1;
        //                     reIteracion = true;
        //                }
        //            }

        //public void paso4()
        //{
        //    for (int i = 1 + cx; i < tamX - 1; i++)
        //    {
        //        object[,] itera = new object[tamX, tamY];
        //        CopiarMatriz(iteracionesSM.mostrarUltimo(), itera);
        //        if (itera[i, 0].ToString().Contains("A"))
        //        {
        //            int posX = i;
        //            int posY = 2;
        //            while (itera[posX, posY].ToString() != "1")
        //                posY++;
        //            for (int k = 1; k < tamX - 1; k++)
        //            {
        //                if (itera[k, posY].ToString() != "0")
        //                {
        //                    if (int.TryParse(itera[k, 1].ToString(), out int valInt))
        //                        #region Cuando son solo numeros enteros
        //                        if (valInt == 0)
        //                            if (itera[k, posY].ToString() == "1")
        //                                itera[k, 1] = "-M";
        //                            else if (itera[k, posY].ToString() == "-1")
        //                                itera[k, 1] = "+M";
        //                            else
        //                                itera[k, 1] = itera[k, posY].ToString() + "M";
        //                        else if (itera[k, posY].ToString() == "1")
        //                            itera[k, 1] = valInt.ToString() + "-M";
        //                        else if (itera[k, posY].ToString() == "-1")
        //                            itera[k, 1] = valInt.ToString() + "+M";
        //                        else if (Convert.ToInt32(itera[k, posY]) > 0)
        //                            itera[k, 1] = valInt.ToString() + "-" + itera[k, posY].ToString() + "M";
        //                        else
        //                            itera[k, 1] = valInt.ToString() + itera[k, posY].ToString().Replace("-", "+") + "M";
        //                    #endregion
        //                    else
        //                    {
        //                        int num = 0;
        //                        int num2 = 0;
        //                        int posNum = itera[k, 1].ToString().IndexOf('M') - 1;
        //                        bool cambio = true;
        //                        while (posNum > -1 && cambio)
        //                        {
        //                            cambio = int.TryParse(itera[k, 1].ToString()[posNum].ToString(), out valInt);
        //                            if (int.TryParse(itera[k, 1].ToString()[posNum].ToString(), out valInt))
        //                            {
        //                                num = num * 10 + valInt;
        //                                posNum--;
        //                            }
        //                        }
        //                        if (posNum == -1)
        //                            posNum = 0;
        //                        string signo = "+";
        //                        if (itera[k, 1].ToString()[posNum].ToString() == "-")
        //                            signo = "-";
        //                        if (num == 0)
        //                            num = 1;
        //                        int ii = 0;
        //                        string signo2 = "+";
        //                        if (itera[k, 1].ToString()[ii].ToString() == "-")
        //                        {
        //                            signo2 = "-";
        //                            ii++;
        //                        }
        //                        while (int.TryParse(itera[k, 1].ToString()[ii].ToString(), out valInt))
        //                        {
        //                            num2 = num2 * 10 + valInt;
        //                            ii++;
        //                        }
        //                        num2 *= Convert.ToInt16(signo2 + "1");
        //                        string numero = num2.ToString();
        //                        if (num2 == 0)
        //                            numero = "";
        //                        if ((Convert.ToInt64(itera[k, posY]) - num * Convert.ToInt64(signo + "1")) != 0)
        //                            if (Convert.ToInt64(itera[k, posY]) * -1 + num * Convert.ToInt64(signo + "1") > 0)
        //                                itera[k, 1] = numero + "+" + (Convert.ToInt64(itera[k, posY]) * -1 + num * Convert.ToInt64(signo + "1")).ToString() + "M";
        //                            else
        //                                itera[k, 1] = numero + (Convert.ToInt64(itera[k, posY]) * -1 + num * Convert.ToInt64(signo + "1")).ToString() + "M";
        //                        else
        //                            if (numero == "")
        //                            itera[k, 1] = 0;
        //                        else
        //                            itera[k, 1] = num2;
        //                    }
        //                }
        //            }
        //            if (!itera[tamX - 1, 1].ToString().Contains("M"))
        //                itera[tamX - 1, 1] = "-" + itera[tamX - 1, posY].ToString() + "M";
        //            else
        //            {
        //                int ii = 1;
        //                int num2 = 0;
        //                bool paso = true;
        //                while (paso && ii < itera[tamX - 1, 1].ToString().Length)
        //                {
        //                    paso = int.TryParse(itera[tamX - 1, 1].ToString()[ii].ToString(), out int valInt);
        //                    if (paso)
        //                    {
        //                        num2 = num2 * 10 + valInt;
        //                        ii++;
        //                    }
        //                }
        //                itera[tamX - 1, 1] = "-" + (num2 + Convert.ToInt16(itera[tamX - 1, posY])).ToString() + "M";
        //            }
        //            iteraciones.insertatFin(itera, posX, posY);
        //        }
        //    }
        #endregion
    }
}
