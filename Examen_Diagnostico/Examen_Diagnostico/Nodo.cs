using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Diagnostico
{
    public class Nodo
    {
        private string nombre;
        private Nodo nodoIzq;
        private Nodo nodoDer;

        public Nodo(string nombre)
        {
            this.nombre = nombre;
            nodoIzq = null;
            nodoDer = null;
        }

        public string getNombre()
        {
            return nombre;
        }

        public Nodo getNodoIzq()
        {
            return nodoIzq;
        }

        public void setNodoIzq(Nodo nodo)
        {
            this.nodoIzq = nodo;
        }

        public Nodo getNodoDer()
        {
            return nodoDer;
        }

        public void setNodoDer(Nodo nodo)
        {
            this.nodoDer = nodo;
        }
    }
}
