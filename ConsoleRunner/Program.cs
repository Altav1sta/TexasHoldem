using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Engine;
using Engine.Enums;
using Engine.Objects;

namespace ConsoleRunner
{
    internal class Program
    {
        private const int PlayersCount = 2;
        private const int InitialStack = 1500;
        private const int SmallBlind = 10;

        private static Game game;
        private static HashSet<string> players = new HashSet<string>();
        private static DateTime lastRequestTime;

        private static void Main(string[] args)
        {
            InitGame();
            StartGame();

            var isFinished = false;

            while (!isFinished)
            {
                foreach (var playerId in players)
                {
                    Thread.Sleep(100);
                    
                    lastRequestTime = DateTime.UtcNow;
                    var info = game.GetInfo(playerId);

                    Console.WriteLine($"-- {playerId} requested info --");
                    Print(info);
                    Console.WriteLine();

                    if (info.AllowedActions == null) continue;
                    
                    var action = GetUserAction(info.AllowedActions);

                    Console.WriteLine();
                    Console.WriteLine($"Processsing action {action.Key}{action.Value} by player {playerId}...");
                    Console.WriteLine();
                }
            }
            
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

        private static void Print(GameInfo info)
        {
            foreach (var record in info.History.Where(x => x.Key >= lastRequestTime).OrderBy(x => x.Key))
            {
                Console.WriteLine($"Round: {record.Value.Round} Street: {record.Value.Street}");
                Console.WriteLine("\t Pots:");
                Console.WriteLine($"\t \t Main: {record.Value.MainPot}");
                
                foreach (var pot in record.Value.PartialPots)
                {
                    Console.WriteLine($"\t \t Partial: {pot.Value} ({string.Join(", ", pot.Players)})");
                }

                Console.WriteLine("\t Players:");
                
                foreach (var player in record.Value.Players)
                {
                    var cardsString = string.Join(' ', player.Cards ?? Array.Empty<Card>());
                    var buttonString = player == record.Value.Players[record.Value.Players.Dealer]
                                           ? " (B)"
                                           : string.Empty;
                    Console.WriteLine($"\t \t {player.Id}{buttonString}: [{cardsString}]");
                }

                Console.WriteLine($"\t Board: [{string.Join(' ', record.Value.Board as IEnumerable<Card>)}]");
                Console.WriteLine($"\t {record.Value.PlayerAction.PlayerId} made {record.Value.PlayerAction.Type} {record.Value.PlayerAction.Value}");
            }

            Console.WriteLine($"Allowed actions: [{string.Join("] [", info.AllowedActions ?? Array.Empty<ActionType>())}]");
        }

        private static KeyValuePair<ActionType, int?> GetUserAction(IReadOnlyCollection<ActionType> allowedActions)
        {
            while (true)
            {
                Console.WriteLine("Enter the name and value for your action separated with whitespace (not case-sensitive):");
                
                var answer = Console.ReadLine();
                var action = answer.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var formattedTypeString = action[0].ToUpper().First() + action[0].Substring(1);

                if (action.Length > 0 && action.Length <= 2 && 
                    Enum.TryParse<ActionType>(formattedTypeString, out var type) &&
                    allowedActions.Contains(type))
                {
                    if (action.Length == 1)
                    {
                        return new KeyValuePair<ActionType, int?>(type, null);
                    }
                    
                    if (int.TryParse(action[1], out var value))
                    {
                        return new KeyValuePair<ActionType, int?>(type, value);
                    }
                }

                Console.WriteLine($"Input data '{answer}' is incorrect. Try again please.");
            }
        }
    }
}