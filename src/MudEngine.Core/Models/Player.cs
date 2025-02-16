namespace MudEngine.Models
{
    public class Player : GameObject
    {
        public Room? CurrentRoom { get; set; }

        public Player(string name, string description) : base(name, description)
        {
        }

        public bool Move(string direction)
        {
            direction = direction.ToLower();
            if (CurrentRoom?.Exits.ContainsKey(direction) == true)
            {
                CurrentRoom = CurrentRoom.Exits[direction];
                return true;
            }
            return false;
        }
    }
} 