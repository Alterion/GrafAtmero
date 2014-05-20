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
        const int N = 10, radius = 50;
        static double[,] dist = new double[N,N];
        static int[] SzCs;
        static double[] Tav;
        static int[,] M = new int[N, N];

        static List<Pont> l = new List<Pont>();

        public static void generateGraph(int p)
        {
            l.Clear();
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

        public static void erdosRenyi(int p)
        {
            M = new int[N, N];
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
                        M[i, j] = randomnumber;
                        M[j, i] = randomnumber;
                    }
                }
            }

        }

        public static void kiir()
        {
            Console.WriteLine();
            Console.WriteLine("Következő csúcs: " + l[next].x + "  " + l[next].y + " előző maximum  " + elozoMax.ToString("0.00") + " következő csúcs távolsága  " + max.ToString("0.00"));
            Console.WriteLine("----------");
        }

        public static void Dijkstra(int n, int cs, int mod) //mod = 0 ha tejles mod = 1 ha erdős
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
                    if (mod == 0)
                    {
                        double tavolsag = Math.Sqrt((Math.Abs(l[im].x - l[j].x) * Math.Abs(l[im].x - l[j].x)) + (Math.Abs(l[im].y - l[j].y) * Math.Abs(l[im].y - l[j].y)));
                        if ((SzCs[j] == 1) && (Tav[im] + tavolsag < Tav[j]))
                            Tav[j] = Tav[im] + tavolsag;
                    }
                    if(mod == 1)
                    {
                        if ((M[im,j] > 0) && (SzCs[j] == 1) && (Tav[im] + M[im,j] < Tav[j]))
                            Tav[j] = Tav[im] + M[im, j];
                    }
                }
            }
        }

        public static void Floyd(int n, int mod) //mod = 0 ha tejles mod = 1 ha erdős
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
                        if (mod == 0)
                        {
                            dist[i, j] = Math.Sqrt((Math.Abs(l[i].x - l[j].x) * Math.Abs(l[i].x - l[j].x)) + (Math.Abs(l[i].y - l[j].y) * Math.Abs(l[i].y - l[j].y)));
                        }
                        if (mod == 1)
                        {
                            dist[i, j] = M[i, j];
                        }
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
            double osszDijkstra = 0, osszFloyd = 0;
            SzCs = new int[N];
            Tav = new double[N];
            Random r = new Random();
            Stopwatch sw = new Stopwatch();
            for (int x = 0; x < 100; x++)
            {
                max = 0;
                elozoMax = 0;
                Console.WriteLine();
                generateGraph(0);      //0-négyzet, 1-kör   
                erdosRenyi(75);
                sw.Start();
                kezdoCsucs = r.Next(0, N);
                Console.WriteLine("Indulócsúcs: " + l[kezdoCsucs].x + "   " + l[kezdoCsucs].y);
                Dijkstra(N, kezdoCsucs, 0);
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
                    Dijkstra(N, next, 0);
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
                Console.WriteLine("Dijkstra eredmény: " + max.ToString("0.0000"));
                Console.WriteLine("Dijkstra alatt eltelt idő: " + sw.Elapsed);
                osszDijkstra += max;
                max = 0;
                sw.Start();
                Floyd(N, 0);
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
                Console.WriteLine("Floyd-Warshall eredmény: " + max.ToString("0.0000"));
                Console.WriteLine("Floyd-Warshall alatt eltelt idő: " + sw.Elapsed);
                osszFloyd += max;
            }
            Console.WriteLine("Dijkstra összesen: " + (osszDijkstra / 100).ToString("0.0000"));
            Console.WriteLine("Floyd-Warshall összesen: " + (osszFloyd / 100).ToString("0.0000"));
            Console.ReadLine();
        }
    }
}
