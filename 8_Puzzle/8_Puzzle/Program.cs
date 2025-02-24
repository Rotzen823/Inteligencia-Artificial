using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    public class Program
    {
        static void Main(string[] args)
        {
            string initialState = "586207134";
            //string initialState = "102345678";
            string finalState = "012345678";

            SearchTree tree = new SearchTree(initialState, finalState);
            tree.getSolution(0);
            tree.getSolution(1);
            tree.getSolution(2);
            tree.getSolution(3);
        }
    }
}
