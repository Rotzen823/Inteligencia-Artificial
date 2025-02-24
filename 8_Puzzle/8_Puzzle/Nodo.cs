using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    public class Nodo : IComparable<Nodo>
    {
        private string state;
        private Nodo father;
        private static int corToInd(int i, int j)
        {
            return i * 3 + j;
        }

        private static Tuple<int, int> indToCor(int i)
        {
            return Tuple.Create(i / 3, i % 3);
        }

        public Nodo(string state)
        {
            this.state = state;
            this.father = null;
        }

        public string getState()
        {
            return state;
        }

        public bool sameState(string state)
        {
            return this.state.Equals(state);
        }

        public List<Nodo> generateSucesores()
        {
            List<Nodo> sucesores = new List<Nodo>();
            int[] dx = new int[] { 0, 1, 0, -1 };
            int[] dy = new int[] { -1, 0, 1, 0 };

            int x, y, indEmpty;
            indEmpty = getEmptyCell();
            (x, y) = indToCor(indEmpty);

            for (int i = 0; i < dx.Length; i++)
            {
                int a = x + dx[i], b = y + dy[i];
                if (a < 0 || a >= 3 || b < 0 || b >= 3)
                {
                    continue;
                }

                int ind = corToInd(a, b);
                StringBuilder newSate = new StringBuilder(state);

                // Swap manual
                char temp = newSate[indEmpty];
                newSate[indEmpty] = newSate[ind];
                newSate[ind] = temp;

                sucesores.Add(new Nodo(newSate.ToString()));
            }

            return sucesores;
        }

        public int getEmptyCell()
        {
            for(int i = 0; i < state.Length; i++)
            {
                if (state[i] == '0')
                {
                    return i;
                }
            }

            throw new Exception("There is no empty cell in this state table");
        }

        public void imprimirEstado()
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Console.Write(state[i * 3 + j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public Nodo getFather()
        {
            return father;
        }

        public void setFather(Nodo father)
        {
            this.father = father;
        }

        public int CompareTo(Nodo other)
        {
            return state.CompareTo(other.getState());
        }
    }
}
