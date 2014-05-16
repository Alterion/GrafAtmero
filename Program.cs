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
        const int N = 100, radius = 50;
        static double[,] dist = new double[N,N];
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
            Console.WriteLine();
            Console.WriteLine("Következő csúcs: " + l[next].x + "  " + l[next].y + " előző maximum  " + elozoMax.ToString("0.00") + " következő csúcs távolsága  " + max.ToString("0.00"));
            Console.WriteLine("----------");
        }

        public static void Dijkstra(int n, int cs)
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
                }
            }
        }

        public static void Floyd(int n )
        {
            int i, j;

            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        dist[i, j] = 0;
                    }
                    else
                    {
                        dist[i, j] = Math.Sqrt((Math.Abs(l[i].x - l[j].x) * Math.Abs(l[i].x - l[j].x)) + (Math.Abs(l[i].y - l[j].y) * Math.Abs(l[i].y - l[j].y)));
                    }
                }
 
            for (int k = 0; k < n; k++)
            {
                for (i = 0; i < n; i++)
                    for (j = 0; j < n; j++)
                    {
                        if ((dist[i, k] * dist[k, j] != 0) && (i != j))
                            if ((dist[i, k] + dist[k, j] < dist[i, j]) || (dist[i, j] == 0))
                                dist[i, j] = dist[i, k] + dist[k, j];
                    }                
            }
        }

        static void Main(string[] args)
        {
            int i ,j;
            SzCs = new int[N];
            Tav = new double[N];
            Random r = new Random();
            Stopwatch sw = new Stopwatch();            
            generateGraph(0);      //0-négyzet, 1-kör              
            sw.Start();
            kezdoCsucs = r.Next(0, N);
            Console.WriteLine("Indulócsúcs: " + l[kezdoCsucs].x + "   " + l[kezdoCsucs].y);
            Dijkstra(N, kezdoCsucs);
            for (i = 0; i < N; i++)
            {
                if (Tav[i] > max && Tav[i] != int.MaxValue - 1)
                {
                    max = Tav[i];
                    next = i;
                }
            }
            kiir();
            do
            {
                elozoMax = max;
                Dijkstra(N, next);
                for (i = 0; i < N; i++)
                {
                    if (Tav[i] > max && Tav[i] != int.MaxValue - 1)
                    {
                        max = Tav[i];
                        next = i;
                    }
                }
                kiir();
            } while (max > elozoMax);
            sw.Stop();
            Console.WriteLine("Dijkstra eredmény: " + max);
            Console.WriteLine("Dijkstra alatt eltelt idő: " + sw.Elapsed);

            max = 0;
            sw.Start();
            Floyd(N);
            for (i = 0; i < N; i++)
            {
                for (j = i + 1; j < N; j++)
                {
                    if (Math.Abs(dist[i, i] - dist[i, j]) > max)
                    {
                        max = Math.Abs(dist[i, i] - dist[i, j]);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("Floyd-Warshall eredmény: " + max);
            Console.WriteLine("Floyd-Warshall alatt eltelt idő: " + sw.Elapsed);
            Console.ReadLine();
        }
    }
}
