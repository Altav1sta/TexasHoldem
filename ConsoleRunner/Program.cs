using System;
using System.Collections.Generic;
using Engine;

namespace ConsoleRunner
{
    internal class Program
    {
        private const int PlayersCount = 2;
        private const int InitialStack = 1500;
        private const int SmallBlind = 10;

        private static Game game;

        private static void Main(string[] args)
        {
            InitGame();
            StartGame();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void InitGame()
        {
            Console.WriteLine("Game initializing...");
            
            var blindStructure = new BlindStructure(SmallBlind);
            var gameSettings = new GameSettings(InitialStack, blindStructure);
            
            game = new Game(gameSettings);

            Console.WriteLine();
        }

        private static void StartGame()
        {
            Console.WriteLine("Starting the game...");
            
            SetPlayers();
        }
        
        private static void SetPlayers()
        {
            var players = new HashSet<string>();
            
            for (var i = 0; i < PlayersCount; i++)
            {
                Console.Write($"Enter the name of {i + 1} player: ");
                
                var name = Console.ReadLine();

                if (players.Contains(name))
                {
                    Console.WriteLine($"The name {name} is already registered!");
                    i--;
                    continue;
                }

                players.Add(name);
            }
            
            game.Start(players);

            Console.WriteLine();
        }
    }
}