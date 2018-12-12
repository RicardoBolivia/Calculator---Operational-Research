using System.Windows.Forms;

namespace Investigacion_operativa
{
    class nodo
    {
        public object dato;
        public nodo anterior, siguiente;

        public nodo()
        {
            anterior = null;
            siguiente = null;
            dato = "";
        }
    }

    class nodoGraf
    {
        public object X1;
        public object X2;
        public object LD;
        public nodoGraf anterior, siguiente;

        public nodoGraf()
        {
            anterior = null;
            siguiente = null;
            X1 = "";
            X2 = "";
            LD = "";
        }
    }

    class nodoIteracion
    {
        public object[,] dato;
        public nodoIteracion anterior, siguiente;
        public int posX;
        public int posY;
        public nodoIteracion()
        {
            anterior = null;
            siguiente = null;
        }
    }

    class ListaRestricciones
    {
        nodo primero;
        nodo ultimo;
        int n = 0;

        public int N
        {
            get { return n; }
        }

        public void insertatFin(object elemento)
        {
            nodo nuevo = new nodo();
            nuevo.dato = elemento;
            if (ultimo == null)
            {
                primero = nuevo;
                ultimo = nuevo;
                n++;
            }
            else
            {
                nuevo.anterior = ultimo;
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
                n++;
            }
        }      
        public object mostrarPosicion(int pos)
        {
            if (primero != null)
            {
                if (pos == 0)
                {
                    return primero.dato;
                }
                else
                {
                    nodo q = primero;
                    for (int i = 0; i < pos; i++)
                    {
                        q = q.siguiente;
                    }
                    return q.dato;
                }
            }
            else
                return "ERROR";
        }
        public void EliminarPorPosicion(int ele)
        {
            if (primero != null)
            {
                if (ele == 0)
                {
                    primero = primero.siguiente;
                }
                else
                {
                    nodo q = primero;
                    for (int i = 0; i < ele; i++)
                    {
                        q = q.siguiente;
                    }
                    q.anterior = q.siguiente;
                }
                n--;
            }
        }
        public void editar(object elemento, int pos)
        {
            nodo q = primero;
            for (int i = 0; i < pos; i++)
            {
                q = q.siguiente;
            }
            q.dato = elemento;
        }
        public void mostrar(ListBox ltbSalida)
        {
            nodo q = primero;
            for (int i = 0; i < n; i++)
            {
                ltbSalida.Items.Add(q.dato);
                q = q.siguiente;
            }
        }
    }

    class ListaIteraciones
    {
        nodoIteracion primero;
        nodoIteracion ultimo;
        public int tx = 0;
        public int ty = 0;
        int n = 0;

        public int N
        {
            get { return n; }
        }

        public void insertatFin(object[,] elemento, int pX,int pY)
        {
            nodoIteracion nuevo = new nodoIteracion();
            nuevo.dato = elemento;
            nuevo.posX = pX;
            nuevo.posY = pY;
            if (ultimo == null)
            {
                primero = nuevo;
                ultimo = nuevo;
                n++;
            }
            else
            {
                nuevo.anterior = ultimo;
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
                n++;
            }
        }
        public object[,] mostrarUltimo()
        {
            return ultimo.dato;
        }
        public void mostrarPosicion(DataGridView dgvSalida, int pos)
        {
            if (primero != null)
            {
                nodoIteracion q = primero;
                for (int i = 0; i < pos; i++)
                {
                    q = q.siguiente;
                }
                dgvSalida.RowCount = ty;
                dgvSalida.ColumnCount = tx;
                for (int i = 0; i < tx; i++)
                {
                    for (int j = 0; j < ty; j++)
                    {
                        dgvSalida[i, j].Value = q.dato[i, j];
                    }
                }
            }
        }
        public void EliminarUltimo()
        {
            nodoIteracion q = ultimo.anterior;
            ultimo = q;
            q.siguiente = null;
        }
    }

    class ListaRestriccionesGrafos
    {
        nodoGraf primero;
        nodoGraf ultimo;
        int n = 0;

        public int N
        {
            get { return n; }
        }

        public void insertatFin(object x1, object x2 , object LD)
        {
            nodoGraf nuevo = new nodoGraf();
            nuevo.X1 = x1;
            nuevo.X2 = x2;
            nuevo.LD = LD;
            if (ultimo == null)
            {
                primero = nuevo;
                ultimo = nuevo;
                n++;
            }
            else
            {
                nuevo.anterior = ultimo;
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
                n++;
            }
        }
        public void mostrarPosicion(int pos, out object x1, out object x2, out object LD)
        {
            x1 = 0;
            x2 = 0;
            LD = 0;
            if (primero != null)
            {
                if (pos == 0)
                {
                    x1= primero.X1;
                    x2= primero.X2;
                    LD= primero.LD;
                }
                else
                {
                    nodoGraf q = primero;
                    for (int i = 0; i < pos; i++)
                    {
                        q = q.siguiente;
                    }
                    x1 = q.X1;
                    x2 = q.X2;
                    LD = q.LD;
                }
            }
           // else
            //    return "ERROR";
        }
        public void EliminarPorPosicion(int ele)
        {
            if (primero != null)
            {
                if (ele == 0)
                {
                    primero = primero.siguiente;
                }
                else
                {
                    nodoGraf q = primero;
                    for (int i = 0; i < ele; i++)
                    {
                        q = q.siguiente;
                    }
                    q.anterior = q.siguiente;
                }
                n--;
            }
        }
        public void editar(object x1, object x2, object LD, int pos)
        {
            nodoGraf q = primero;
            for (int i = 0; i < pos; i++)
            {
                q = q.siguiente;
            }
            q.X1 = x1;
            q.X2 = x2;
            q.LD = LD;
        }
        public void mostrar(ListBox ltbSalida)
        {
            nodoGraf q = primero;
            for (int i = 0; i < n; i++)
            {
                ltbSalida.Items.Add( "(" +q.X1 + "," + q.X2 + ")");
                q = q.siguiente;
            }
        }
    }

}
