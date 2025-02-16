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

            // Create some sample items
            var sword = new Item("Sword", "A rusty old sword lies here.", "a rusty old sword", 5);
            var apple = new Item("Apple", "A shiny red apple lies here.", "a shiny red apple", 0.1m) 
            { 
                MaxStack = 10 
            };
            var bag = new Container("Bag", "A leather bag lies here.", "a leather bag", 1);

            // Place items in rooms
            entrance.Contents.Add(sword.Id, sword);
            corridor.Contents.Add(apple.Id, apple);
            library.Contents.Add(bag.Id, bag);

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
                case "help":
                case "?":
                    return Help();
                case "look":
                    return Look(player);
                case "north":
                case "n":
                    return Move(player, "north");
                case "south":
                case "s":
                    return Move(player, "south");
                case "east":
                case "e":
                    return Move(player, "east");
                case "west":
                case "w":
                    return Move(player, "west");
                case "get":
                    if (parts.Length < 2)
                        return "Get what?";
                    return Get(player, parts[1]);
                case "drop":
                    if (parts.Length < 2)
                        return "Drop what?";
                    return Drop(player, parts[1]);
                case "inventory":
                case "inv":
                case "i":
                case "inven":
                    return Inventory(player);
                case "examine":
                case "exam":
                case "ex":
                    if (parts.Length < 2)
                        return "Examine what?";
                    return Examine(player, parts[1]);
                case "open":
                    if (parts.Length < 2)
                        return "Open what?";
                    return Open(player, parts[1]);
                case "close":
                    if (parts.Length < 2)
                        return "Close what?";
                    return Close(player, parts[1]);
                case "put":
                    if (parts.Length < 4) // put apple in bag
                        return "Put what in what?";
                    return Put(player, parts[1], parts[3]);
                default:
                    return "Unknown command. Type 'help' for a list of commands.";
            }
        }

        private string Help()
        {
            var commands = new[]
            {
                ("look", "Look at your surroundings"),
                ("north/n", "Move north"),
                ("south/s", "Move south"),
                ("east/e", "Move east"),
                ("west/w", "Move west"),
                ("get <item>", "Pick up an item"),
                ("drop <item>", "Drop an item"),
                ("inventory/inv/i/inven", "Show your inventory"),
                ("help/?", "Show this help message"),
                ("quit", "Exit the game")
            };

            var helpText = $"{AnsiColors.Green}Available Commands:{AnsiColors.Reset}\n";
            foreach (var (cmd, desc) in commands)
            {
                helpText += $"{AnsiColors.Yellow}{cmd,-25}{AnsiColors.Reset} - {desc}\n";
            }
            return helpText;
        }

        private string Look(Player player)
        {
            var room = player.CurrentRoom;
            var description = $"{AnsiColors.Green}{room.Name}{AnsiColors.Reset}\n{room.Description}\n";

            if (room.Contents.Count > 0)
            {
                var items = string.Join("\n", room.Contents.Values
                    .Where(i => i is Item)
                    .Select(i => i.Description));
                if (!string.IsNullOrEmpty(items))
                    description += $"\n{AnsiColors.Yellow}{items}{AnsiColors.Reset}\n";
            }

            description += $"\n{AnsiColors.Yellow}Exits: {string.Join(", ", room.Exits.Keys)}{AnsiColors.Reset}";

            return description;
        }

        private string Move(Player player, string direction)
        {
            if (player.Move(direction))
                return $"{AnsiColors.Cyan}You move {direction}.{AnsiColors.Reset}\n\n{Look(player)}";
            return $"{AnsiColors.Red}You cannot go {direction}.{AnsiColors.Reset}";
        }

        private string Get(Player player, string itemName)
        {
            var room = player.CurrentRoom;
            
            if (room == null)
                return "You are not in a room.";

            var item = room.Contents.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());

            if (item == null)
                return $"There is no {itemName} here.";

            if (!(item is Item pickupItem))
                return $"You cannot get the {item.Name}.";

            if (!player.AddItem(pickupItem))
                return $"The {item.Name} is too heavy for you to carry.";

            room.RemoveItem(item.Id);
            return $"You get the {item.Name}.";
        }

        private string Drop(Player player, string itemName)
        {
            var item = player.Inventory.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());

            if (item == null)
                return $"You don't have a {itemName}.";

            player.RemoveItem(item.Id);
            
            if (player.CurrentRoom == null)
                return "You are not in a room.";

            player.CurrentRoom.AddItem(item);
            return $"You drop the {item.Name}.";
        }

        private string Inventory(Player player)
        {
            if (player.Inventory.Count == 0)
                return "You are not carrying anything.";

            var inventoryText = $"{AnsiColors.Green}You are carrying:{AnsiColors.Reset}\n";
            foreach (var item in player.Inventory.Values)
            {
                inventoryText += $"{AnsiColors.Yellow}{item.ShortDescription,-25}{AnsiColors.Reset} - Weight: {item.Weight}kg\n";
            }
            
            inventoryText += $"\n{AnsiColors.Cyan}Total Weight: {player.CurrentWeight}/{player.MaxWeight}kg{AnsiColors.Reset}";
            return inventoryText;
        }

        private string Examine(Player player, string itemName)
        {
            // Check inventory first
            GameObject item = player.Inventory.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());
            
            // Then check room
            if (item == null && player.CurrentRoom != null)
                item = player.CurrentRoom.Contents.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());

            if (item == null)
                return $"You don't see {itemName} anywhere.";

            return item is Item examItem ? examItem.GetExamineDescription() : item.Description;
        }

        private string Open(Player player, string containerName)
        {
            var container = FindItem(player, containerName) as Container;
            if (container == null)
                return $"You don't see {containerName} to open.";

            if (!container.IsPickable)
                return $"You can't open that.";

            if (container.IsOpen)
                return $"The {containerName} is already open.";

            container.IsOpen = true;
            return $"You open the {containerName}.";
        }

        private string Close(Player player, string containerName)
        {
            var container = FindItem(player, containerName) as Container;
            if (container == null)
                return $"You don't see {containerName} to close.";

            if (!container.IsOpen)
                return $"The {containerName} is already closed.";

            container.IsOpen = false;
            return $"You close the {containerName}.";
        }

        private string Put(Player player, string itemName, string containerName)
        {
            var item = player.Inventory.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());
            if (item == null)
                return $"You don't have {itemName}.";

            var container = FindItem(player, containerName) as Container;
            if (container == null)
                return $"You don't see {containerName} to put things in.";

            if (!container.IsOpen)
                return $"The {containerName} is closed.";

            player.RemoveItem(item.Id);
            container.AddItem(item);
            return $"You put the {itemName} in the {containerName}.";
        }

        private GameObject FindItem(Player player, string itemName)
        {
            return player.Inventory.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower()) ??
                   player.CurrentRoom.Contents.Values.FirstOrDefault(i => i.Name.ToLower() == itemName.ToLower());
        }
    }
} 