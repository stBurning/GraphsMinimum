using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs {
    class Program {
        

        public static void Main(string[] args) {
            //Пример 1
            double[,] matrix = {
                {0,2,3,4,3,2},
                {2,0,1,6,4,2},
                {3,1,0,1,4,3},
                {4,6,1,0,9,5},
                {3,4,4,9,0,7},
                {2,2,3,5,7,0},
            };
            //Пример 2
            double[,] matrix2 = {
                {0,2,7,0,0},
                {2,0,1,0,0},
                {7,1,0,0,0},
                {0,0,0,0,4},
                {0,0,0,4,0}
            };
            Graph g = new Graph(matrix2);
            double min_sum = Double.MaxValue;
            int vert = 0;
            for (int i = 0; i < g.Vertices.Count; i++) {
                for(int j = i; j < g.Vertices.Count; j++) {
                    var sum = g.SumToOtherVerts(i, j);
                    if( sum < min_sum) {
                        min_sum = sum;
                        vert = i;
                    }
                }
            }
            Console.WriteLine("Вершина №" + vert + "| сумма - " + min_sum);

            Console.ReadKey();
        }
    }
}
