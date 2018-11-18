using System;
using System.Threading;

class Program
{
    static void Main()
    {

        ManualResetEvent gate = new ManualResetEvent(false);

        Console.Write("Ingrese la cantidad de jugadores: ");

        int jugadores = int.Parse(Console.ReadLine());

        int numberOfThreads = jugadores, pending = numberOfThreads;

        Thread[] threads = new Thread[numberOfThreads];

        ParameterizedThreadStart work = name =>
        {
            Console.WriteLine("Jugador {0} se acerca a la ronda", name);
            if (Interlocked.Decrement(ref pending) == 0)
            {

                Console.WriteLine("Suena la musica y arranca el juego!");
                gate.Set();
                Console.WriteLine("Se para la musica, todos a las sillas!");

            }
            else gate.WaitOne();
            
            Race();
            
            Console.WriteLine("{0} Toma una silla", name);
        };
        
        for (int i = 0; i < numberOfThreads; i++)
        {
            threads[i] = new Thread(work);
            threads[i].Start(i);
        }
        for (int i = 0; i < numberOfThreads; i++)
        {
            threads[i].Join();
        }
        Console.WriteLine("Juego Finalizado");
        Console.ReadLine();
    }

    static readonly Random rand = new Random();
    
    static void Race()
    {
        int time;
        lock (rand)
        {
            time = rand.Next(500, 1000);
        }
        Thread.Sleep(time);
    }
    
}
