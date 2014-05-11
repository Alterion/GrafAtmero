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
        static int next = -1, kezdoCsucs;
        static double max = 0, elozoMax = 0;
        const int N = 10, radius = 10;
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

        static int[] SzCs;
        static double[] Tav;

        static List<Pont> l = new List<Pont>();

        public static void generateGraph(int p)
        {
            Random random = new Random();
            if (p == 0)
            {              
                l.Add(new Pont(0, 0));
                for (int i = 2; i < N; i++)
                {
                    l.Add(new Pont(random.Next(0, 100), random.Next(0, 100)));
                }
                l.Add(new Pont(100, 100));
            }
            if(p == 1)
            {
                l.Add(new Pont(-1 * radius, 0));
                for (int i = 2; i < N; i++)
                {
                    var angle = random.NextDouble() * Math.PI * 2;
                    l.Add(new Pont(Math.Cos(angle) * radius, Math.Sin(angle) * radius));
                }
                l.Add(new Pont(radius, 0));
            }
        }

        public static void kiir()
        {
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(l[i].x + "   " + l[i].y);
            }
        }

        public static void Dijkstra(int[,] G, int n, int cs)
        {
            int i, j, im = 0;
            double m;
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
                    double tavolsag = Math.Sqrt((Math.Abs(l[im].x - l[j].x) * Math.Abs(l[im].x - l[j].x)) + (Math.Abs(l[im].y - l[j].y) * Math.Abs(l[im].y - l[j].y)));
                    if ((SzCs[j] == 1) && (Tav[im] + tavolsag < Tav[j]))
                        Tav[j] = Tav[im] + tavolsag;
                    /*if ((G[im, j] > 0) && (SzCs[j] == 1) && (Tav[im] + G[im, j] < Tav[j]))
                    {
                        Tav[j] = Tav[im] + G[im, j];
                    }*/

                }
            }
        }

        static void Main(string[] args)
        {
            int i;
            //G = new int[N, N];
            SzCs = new int[N];
            Tav = new double[N];
            Random r = new Random();
            Stopwatch sw = new Stopwatch();            
            generateGraph(0);      //0-négyzet, 1-kör         
            //kiir();
            kezdoCsucs = r.Next(0, N);
            Console.WriteLine("Indulócsúcs: " + l[kezdoCsucs].x + "   " + l[kezdoCsucs].y);
            sw.Start();
            Dijkstra(G, N, kezdoCsucs);
            for (i = 0; i < N; i++)
            {
                if (Tav[i] > max && Tav[i] != int.MaxValue - 1)
                {
                    max = Tav[i];
                    next = i;
                }
            }
            /*for (int j = 0; j < N; j++)
                Console.Write(Tav[j] + " ");*/
            Console.WriteLine();
            Console.WriteLine("Következő csúcs: " + l[next].x + "  " + l[next].y + " előző maximum  " + elozoMax.ToString("0.00") + " következő csúcs távolsága  " + max.ToString("0.00"));
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
                /*for (int j = 0; j < N; j++)
                    Console.Write(Tav[j] + " ");*/
                Console.WriteLine();
                Console.WriteLine("Következő csúcs: " + l[next].x + "  " + l[next].y + " előző maximum  " + elozoMax.ToString("0.00") + " következő csúcs távolsága  " + max.ToString("0.00"));
                Console.WriteLine("----------");
            } while (max > elozoMax);
            sw.Stop();
            Console.WriteLine("Eltelt idő: " + sw.Elapsed);
            Console.ReadLine();
        }
    }
}
