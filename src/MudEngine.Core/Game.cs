using System;
using System.Collections.Generic;
using MudEngine.Models;

namespace MudEngine
{
    public class Game
    {
        private Dictionary<string, Player> Players { get; set; } = new Dictionary<string, Player>();
        private Dictionary<string, Room> Rooms { get; set; } = new Dictionary<string, Room>();

        public Game()
        {
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            // Create some sample rooms
            var entrance = new Room("Entrance Hall", "A grand entrance hall with marble floors.");
            var corridor = new Room("Corridor", "A long, dimly lit corridor.");
            var library = new Room("Library", "A vast room filled with ancient books.");

            // Connect the rooms
            entrance.AddExit("north", corridor);
            corridor.AddExit("south", entrance);
            corridor.AddExit("east", library);
            library.AddExit("west", corridor);

            // Store rooms
            Rooms[entrance.Id] = entrance;
            Rooms[corridor.Id] = corridor;
            Rooms[library.Id] = library;
        }

        public Player CreatePlayer(string name)
        {
            var player = new Player(name, $"A player named {name}");
            player.CurrentRoom = Rooms.Values.First(); // Start at the first room
            Players[player.Id] = player;
            return player;
        }

        public string ProcessCommand(Player player, string command)
        {
            var parts = command.ToLower().Split(' ');
            switch (parts[0])
            {
                case "look":
                    return Look(player);
                case "go":
                case "move":
                    if (parts.Length < 2)
                        return "Go where?";
                    return Move(player, parts[1]);
                default:
                    return "Unknown command.";
            }
        }

        private string Look(Player player)
        {
            var room = player.CurrentRoom;
            var description = $"{AnsiColors.Green}{room.Name}{AnsiColors.Reset}\n{room.Description}\n\n{AnsiColors.Yellow}Exits: {string.Join(", ", room.Exits.Keys)}{AnsiColors.Reset}";
            return description;
        }

        private string Move(Player player, string direction)
        {
            if (player.Move(direction))
                return $"{AnsiColors.Cyan}You move {direction}.{AnsiColors.Reset}\n\n{Look(player)}";
            return $"{AnsiColors.Red}You cannot go {direction}.{AnsiColors.Reset}";
        }
    }
} 