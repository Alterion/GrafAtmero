using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafAtmero
{
    class Program
    {
        static int max = 0, elozoMax = 0, next = -1;
        const int N = 11;
        static int[,] G = {{0,2,5,0,0,0,0,0,0,0,0},
                           {2,0,0,0,1,0,0,0,0,0,3},
                           {5,0,0,0,3,3,0,4,0,0,0},
                           {0,0,0,0,2,2,1,0,0,0,0},
                           {0,1,3,2,0,0,0,2,0,4,2},
                           {0,0,3,2,0,0,2,0,0,0,0},
                           {0,0,0,1,0,2,0,3,0,0,0},
                           {0,0,4,0,2,0,3,0,5,0,0},
                           {0,0,0,0,0,0,0,5,0,3,0},
                           {0,0,0,0,4,0,0,0,3,0,3},
                           {0,3,0,0,2,0,0,0,0,3,0}};

        static int[] SzCs = new int[N], Tav = new int[N];

        public static void Dijkstra(int[,] G, int n, int cs)
        {
            int i, j, m, im = 0;
            max = 0;
            for (i = 0; i < n; i++)
            {
                SzCs[i] = 1;
                if(i != cs)
                    Tav[i] = int.MaxValue - 1;
            }
            Tav[cs] = 0;

            for (i = 0; i < n - 1; i++)
            {
                m = int.MaxValue;
                for (j = 0; j < n; j++)
                {
                    if (SzCs[j] == 1)
                    {
                        if (Tav[j] < m)
                        {
                            im = j;
                            m = Tav[j];
                        }
                    }
                }
                SzCs[im] = 0;

                for (j = 0; j < n; j++)
                {
                    if ((G[im,j] > 0) && (SzCs[j] == 1) && (Tav[im] + G[im,j] < Tav[j]))
                    {
                        Tav[j] = Tav[im] + G[im,j];
                        /*if (Tav[j] > max)
                        {
                            max = Tav[j];
                            next = j;
                        }*/
                    }

                }
            }
        }

        static void Main(string[] args)
        {
            int i;
            Dijkstra(G, N, 6);
            for (i = 0; i < N; i++)
            {
                if (Tav[i] > max)
                {
                    max = Tav[i];
                    next = i;
                }
            }
            Console.WriteLine(next);
            /*for (int j = 0; j < N;j++ )
                Console.WriteLine(Tav[j]);*/
            do
            {
                elozoMax = max;
                Dijkstra(G, N, next);
                for (i = 0; i < N; i++)
                {
                    if (Tav[i] > max)
                    {
                        max = Tav[i];
                        next = i;
                    }
                }       
                Console.WriteLine(next + "   " + elozoMax + "   " + max);
            } while (Math.Abs(max - elozoMax) > elozoMax * 0.05);
            Console.ReadLine();
        }
    }
}
