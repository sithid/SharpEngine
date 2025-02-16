using System.Collections.Generic;

namespace MudEngine.Models
{
    public class Room : GameObject
    {
        public Dictionary<string, Room> Exits { get; set; } = new Dictionary<string, Room>();

        public Room(string name, string description) : base(name, description)
        {
        }

        public void AddExit(string direction, Room destination)
        {
            Exits[direction.ToLower()] = destination;
        }
    }
} 