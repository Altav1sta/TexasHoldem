using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static DateTime lastInfoDisplayTime;

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            InitGame();
            StartGame();

            var isFinished = false;

            while (!isFinished)
            {
                foreach (var playerId in players)
                {
                    Thread.Sleep(100);
                    
                    var info = game.GetInfo(playerId);

                    Console.WriteLine($"-- {playerId} requested info --");
                    
                    Print(info);
                    lastInfoDisplayTime = DateTime.UtcNow;
                    
                    Console.WriteLine();

                    if (info.AllowedActions == null) continue;
                    
                    var action = GetUserAction(info.AllowedActions);

                    Console.WriteLine();
                    Console.WriteLine($"Processing action {action.Key} {action.Value} by player {playerId}...");
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
            
            game.Start(players);

            Console.WriteLine();
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
        }

        private static void Print(GameInfo info)
        {
            var newRecords = info.History
                                 .Where(x => x.Key >= lastInfoDisplayTime)
                                 .OrderBy(x => x.Key)
                                 .ToList();
            
            foreach (var record in newRecords)
            {
                Console.WriteLine($"Round: {record.Value.Round} Street: {record.Value.Street}");
                Console.WriteLine("\t Pots:");
                
                foreach (var pot in record.Value.Pots)
                {
                    Console.WriteLine($"\t \t Pot: {pot.Value} Participants: ({string.Join(", ", pot.Players)})");
                }

                Console.WriteLine("\t Players:");
                
                foreach (var player in record.Value.Players.OrderBy(x => record.Value.Players.IndexOf(x)))
                {
                    var cardsString = string.Join(' ', player.Cards ?? Array.Empty<Card>());
                    var buttonString = player == record.Value.Players[record.Value.Players.Dealer]
                                           ? " (B)"
                                           : string.Empty;
                    
                    Console.Write($"\t \t Seat: {record.Value.Players.IndexOf(player) + 1}");
                    Console.Write($"\t Cards: [{cardsString}]");
                    Console.Write($"\t Hole: {player.HasHoleCards}");
                    Console.Write($"\t Bet: {player.CurrentBet}");
                    Console.WriteLine($"\t Name: {player.Id}{buttonString}");
                }

                Console.WriteLine($"\t Board: [{string.Join(' ', record.Value.Board.Distinct())}]");
                Console.Write($"\t Player: {record.Value.PlayerAction.PlayerId} ");
                Console.WriteLine($"Move: {record.Value.PlayerAction.Type} {record.Value.PlayerAction.Value}");
            }

            if (info.AllowedActions != null && info.AllowedActions.Count > 0)
            {
                Console.WriteLine($"Allowed actions: [{string.Join("] [", info.AllowedActions.Select(x => GetAllowedActionString(x)))}]");
                return;
            }

            if (newRecords.Count == 0)
            {
                Console.WriteLine("No recent events");
            }
        }

        private static KeyValuePair<ActionType, int?> GetUserAction(IReadOnlyCollection<AllowedAction> allowedActions)
        {
            while (true)
            {
                Console.WriteLine("Enter the name and value for your action separated with whitespace (not case-sensitive):");
                
                var answer = Console.ReadLine();
                var action = answer.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var formattedTypeString = action[0].ToUpper().First() + action[0].Substring(1);

                if (action.Length > 0 && action.Length <= 2 && 
                    Enum.TryParse<ActionType>(formattedTypeString, out var type) &&
                    allowedActions.Select(x => x.Type).Contains(type))
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

        private static string GetAllowedActionString(AllowedAction action)
        {
            var rangeString = string.Empty;

            if (action.Type == ActionType.Call) rangeString += $" {action.Min}";
            if (action.Type == ActionType.Raise) rangeString += $" {action.Min}-{action.Max}";

            return action.Type + rangeString;
        }
    }
}