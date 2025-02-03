using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Diagnostico
{
    public class Arbol
    {
        private Nodo root;

        public Arbol()
        {
            this.root = null;
        }

        public bool vacio()
        {
            return root == null;
        }

        public void agregarNodo(string nombreNodo)
        {
            if (vacio())
            {
                root = new Nodo(nombreNodo);
            }
            else
            {
                agregarNodo(root, new Nodo(nombreNodo));
            }
        }

        public void agregarNodo(Nodo raiz, Nodo nodo)
        {
            if (raiz.getNombre().CompareTo(nodo.getNombre()) == -1)
            {
                if (raiz.getNodoIzq() == null)
                {
                    raiz.setNodoIzq(nodo);
                }
                else
                {
                    agregarNodo(raiz.getNodoIzq(), nodo);
                }
            }
            else
            {
                if (raiz.getNodoDer() == null)
                {
                    raiz.setNodoDer(nodo);
                }
                else
                {
                    agregarNodo(raiz.getNodoDer(), nodo);
                }
            }
        }

        public bool buscarNodo(string nombre)
        {
            if (vacio())
            {
                return false;
            }


        }

        public bool buscarNodo(Nodo raiz, string nombre)
        {
            if(raiz == null)
            {
                return false;
            }

            if(raiz.getNombre() == nombre)
            {
                return true;
            }

            if(raiz.getNombre().CompareTo(nombre) == -1)
            {
                return buscarNodo(raiz.getNodoIzq(), nombre);
            }

            return buscarNodo(raiz.getNodoDer(), nombre);
        }

    }
}
