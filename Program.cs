using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SillasMusicales
{
    class Program
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int[] auxArray)
        {
            int[] posicionesLibres = auxArray.Select((z, j) => j).Where(i => auxArray[i] == 0).OrderBy(x => Guid.NewGuid()).ToArray();
            lock (syncLock)
            {
                int auxRandom = posicionesLibres[0];
                return auxRandom;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenido al juego de las sillas musicales");
            Console.WriteLine();
            Console.Write("Ingrese la cantidad de jugadores: ");
            
            
                int jugadores = int.Parse(Console.ReadLine());

                int cantidadSillas = jugadores - 1;
                int[] sillas = new int[cantidadSillas];
                Task[] tasks = new Task[jugadores];
                Barrier barrier = new Barrier(jugadores);
                int corteMusica = 0;

                Action<object> actionJugadores = (object identificador) =>
                {
                    int bandera = 0;
                    int banderaSentado = 0;

                    while (true)
                    {
                        if (bandera == 0)
                        {
                            Console.WriteLine("El Jugador {0} se acerca a la ronda.", identificador);
                            bandera = 1;
                            barrier.SignalAndWait();
                           
                        }
                        else
                        {
                            if (corteMusica == 1)
                            {
                                while (corteMusica == 1)
                                {
                                    while (banderaSentado == 0)
                                    {
                                        int posicionSilla = RandomNumber(sillas);
                                        if (Array.Exists(sillas, element => element == 0))
                                        {
                                            if (sillas[posicionSilla] == 0)
                                            {
                                                lock (sillas)
                                                {
                                                    if (sillas[posicionSilla] == 0)
                                                    {
                                                        sillas[posicionSilla] = 1;
                                                        Console.WriteLine("El jugador {0} toma una silla", posicionSilla);
                                                        banderaSentado = 1;
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            banderaSentado = 1;
                                        }
                                    }
                                }
                                banderaSentado = 0;
                                barrier.SignalAndWait();
                            }
                        }
                    }

                };
                
                for (int i = 0; i < jugadores; i++)
                {
                    tasks[i] = Task.Factory.StartNew(actionJugadores, (i + 1));
                }

            while (cantidadSillas >= 1)
            {
                

                    int tiempoMusica = new Random().Next(1, 5);
                    Thread.Sleep(tiempoMusica * 1000);
                    Console.WriteLine("Se detiene la musica! Todos a las sillas!");
                    corteMusica = 1;
                    while (Array.Exists(sillas, element => element == 0))
                    {
                        //Waiting??
                    }
                    
                    corteMusica = 0;
                    jugadores--;
                    cantidadSillas--;
                    sillas = new int[cantidadSillas];
                    barrier.RemoveParticipant();
                    //Remove one barrier participant
                    
                
            }
            Console.Write("Participante Ganador ^");
            Console.ReadLine();
        }
            
    }
}

