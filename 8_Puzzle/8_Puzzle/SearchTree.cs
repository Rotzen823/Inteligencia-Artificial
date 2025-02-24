using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _8_Puzzle
{
    public class SearchTree
    {
        private Nodo root;
        private string finalState;

        public SearchTree(string root, string finalState)
        {
            this.root = new Nodo(root);
            this.finalState = finalState;
        }

        public Nodo bfs()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HashSet<string> estados = new HashSet<string>();

            Queue<Nodo> cola = new Queue<Nodo>();
            cola.Enqueue(root);
            estados.Add(root.getState());

            while (cola.Count > 0)
            {
                Nodo nodoActual = cola.Dequeue();

                foreach(Nodo child in nodoActual.generateSucesores())
                {
                    if (!estados.Contains(child.getState()))
                    {
                        child.setFather(nodoActual);

                        estados.Add(child.getState());
                        if (child.sameState(finalState))
                        {
                            stopwatch.Stop();
                            generarReporte("BFS", stopwatch.ElapsedMilliseconds, estados.Count);
                            return child;
                        }
                        cola.Enqueue(child);
                    }
                }
            }

            return null;
        }

        public Nodo dfs()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HashSet<string> estados = new HashSet<string>();

            Stack<Nodo> pila = new Stack<Nodo>();
            pila.Push(root);
            estados.Add(root.getState());

            while (pila.Count > 0)
            {
                Nodo nodoActual = pila.Pop();

                foreach (Nodo child in nodoActual.generateSucesores())
                {
                    if (!estados.Contains(child.getState()))
                    {
                        child.setFather(nodoActual);

                        estados.Add(child.getState());
                        if (child.sameState(finalState))
                        {
                            stopwatch.Stop();
                            generarReporte("DFS", stopwatch.ElapsedMilliseconds, estados.Count);
                            return child;
                        }
                        pila.Push(child);
                    }
                }
            }

            return null;
        }

        public Nodo dls()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HashSet<string> estados = new HashSet<string>();
            int limite = 50;

            Stack<(Nodo, int)> pila = new Stack<(Nodo, int)>();
            pila.Push((root, 0));
            estados.Add(root.getState());

            while (pila.Count > 0)
            {
                Nodo nodoActual;
                int profundidad;
                (nodoActual, profundidad) = pila.Pop();

                if(profundidad > limite)
                {
                    continue;
                }

                foreach (Nodo child in nodoActual.generateSucesores())
                {
                    if (!estados.Contains(child.getState()))
                    {
                        child.setFather(nodoActual);

                        estados.Add(child.getState());
                        if (child.sameState(finalState))
                        {
                            stopwatch.Stop();
                            generarReporte("DLS con limite de prfundiad de 50", stopwatch.ElapsedMilliseconds, estados.Count);
                            return child;
                        }
                        pila.Push((child, profundidad + 1));
                    }
                }
            }

            stopwatch.Stop();
            generarReporte("DLS fallido", stopwatch.ElapsedMilliseconds, estados.Count);
            return null;
        }

        public Nodo uniformCost()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HashSet<string> estados = new HashSet<string>();

            SortedSet<(int, Nodo)> cola = new SortedSet<(int, Nodo)>();
            estados.Add(root.getState());
            cola.Add((0, root));

            while(cola.Count > 0)
            {
                Nodo nodoActual = cola.Min.Item2;
                cola.Remove(cola.Min);

                int emptyCell = nodoActual.getEmptyCell();
                foreach (Nodo child in nodoActual.generateSucesores())
                {
                    string childState = child.getState();
                    if (!estados.Contains(childState))
                    {
                        child.setFather(nodoActual);

                        estados.Add(childState);
                        if (child.sameState(finalState))
                        {
                            stopwatch.Stop();
                            generarReporte("Uniform Cost", stopwatch.ElapsedMilliseconds, estados.Count);
                            return child;
                        }
                        cola.Add((childState[emptyCell] - '0', child));
                    }
                }
            }
            return null;
        }

        private void generarReporte(string nombre, long time, int memoria)
        {
            Console.WriteLine($"Reporte del algoritmo {nombre}");
            Console.WriteLine($"Tiempo en encontrar la solucion: {time} ms");
            Console.WriteLine($"Estados visitados para encontrar la solucion: {memoria}");
        }

        //0 = depth, 1 = brief, 2 = priority
        public List<Nodo> getSolution(int option)
        {
            Nodo target = null;

            switch (option)
            {
                case 0: //Depth
                    target = dfs();
                    break;
                case 1:
                    target = bfs();
                    break;
                case 2:
                    target = uniformCost();
                    break;
                case 3:
                    target = dls();
                    break;
                default: return null;
            }

            List<Nodo> solution = new List<Nodo>();
            while(target != null)
            {
                solution.Add(target);
                target = target.getFather();
            }

            solution.Reverse();

            Console.WriteLine($"Numero de pasos para llegar al estado final: {solution.Count}");
            Console.WriteLine();

            return solution;
        }

        public void printSultion(int option)
        {
            List<Nodo> solution = getSolution(option);
            foreach(Nodo node in solution)
            {
                node.imprimirEstado();
            }
        }
    }
}
