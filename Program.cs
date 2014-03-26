using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GrafAtmero
{
    class Program
    {
        static int max = 0, elozoMax = 0, next = -1;
        const int N = 100;
        /*static int[,] G={{0,2,5,0,0,0,0,0,0,0,0},
                           {2,0,0,0,1,0,0,0,0,0,3},
                           {5,0,0,0,3,3,0,4,0,0,0},
                           {0,0,0,0,2,2,1,0,0,0,0},
                           {0,1,3,2,0,0,0,2,0,4,2},
                           {0,0,3,2,0,0,2,0,0,0,0},
                           {0,0,0,1,0,2,0,3,0,0,0},
                           {0,0,4,0,2,0,3,0,5,0,0},
                           {0,0,0,0,0,0,0,5,0,3,0},
                           {0,0,0,0,4,0,0,0,3,0,3},
                           {0,3,0,0,2,0,0,0,0,3,0}};*/
        static int[,] G;

        static int[] SzCs, Tav;

        public static void generateGraph(int p)
        {
            Random random = new Random();
            int randomnumber = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    randomnumber = random.Next(0, 100);
                    if (randomnumber >= 100 - p)
                    {
                        randomnumber = random.Next(1, 100);
                        G[i, j] = randomnumber;
                        G[j, i] = randomnumber;
                    }
                }
            }

        }

        public static void kiir()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(G[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void Dijkstra(int[,] G, int n, int cs)
        {
            int i, j, m, im = 0;
            max = 0;
            for (i = 0; i < n; i++)
            {
                SzCs[i] = 1;
                if (i != cs)
                    Tav[i] = int.MaxValue - 1;
            }
            Tav[cs] = 0;

            for (i = 0; i < n - 2; i++)
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
                    if ((G[im, j] > 0) && (SzCs[j] == 1) && (Tav[im] + G[im, j] < Tav[j]))
                    {
                        Tav[j] = Tav[im] + G[im, j];
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
            G = new int[N, N];
            SzCs = new int[N];
            Tav = new int[N];
            Stopwatch sw = new Stopwatch();
            generateGraph(75);
            //kiir();
            Console.WriteLine("Indulócsúcs: " + 0);
            sw.Start();
            Dijkstra(G, N, 5);
            for (i = 0; i < N; i++)
            {
                if (Tav[i] > max && Tav[i] != int.MaxValue - 1)
                {
                    max = Tav[i];
                    next = i;
                }
            }
            for (int j = 0; j < N; j++)
                Console.Write(Tav[j] + " ");
            Console.WriteLine();
            Console.WriteLine("Következő csúcs: " + next + " előző maximum  " + elozoMax + " következő csúcs távolsága  " + max);
            Console.WriteLine("----------");
            do
            {
                elozoMax = max;
                Dijkstra(G, N, next);
                for (i = 0; i < N; i++)
                {
                    if (Tav[i] > max && Tav[i] != int.MaxValue - 1)
                    {
                        max = Tav[i];
                        next = i;
                    }
                }
                for (int j = 0; j < N; j++)
                    Console.Write(Tav[j] + " ");
                Console.WriteLine();
                Console.WriteLine("Következő csúcs: " + next + " előző maximum  " + elozoMax + " következő csúcs távolsága  " + max);
                Console.WriteLine("----------");
            } while (max > elozoMax);
            sw.Stop();
            Console.WriteLine("Eltelt idő: " + sw.Elapsed);
            Console.ReadLine();
        }
    }
}
