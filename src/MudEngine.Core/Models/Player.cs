namespace MudEngine.Models
{
    public class Player : GameObject
    {
        public Room? CurrentRoom { get; set; }
        public decimal MaxWeight { get; set; } = 100.0m;
        public Dictionary<string, Item> Inventory { get; set; } = new Dictionary<string, Item>();
        public decimal CurrentWeight => Inventory.Values.Sum(i => i.Weight);

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

        public override bool AddItem(GameObject item)
        {
            if (item is Item itemToAdd)
            {
                if (CurrentWeight + itemToAdd.Weight > MaxWeight)
                    return false;
                    
                Inventory[item.Id] = itemToAdd;
                return true;
            }
            return false;
        }

        public override GameObject RemoveItem(string itemId)
        {
            if (Inventory.TryGetValue(itemId, out Item item))
            {
                Inventory.Remove(itemId);
                return item;
            }
            return null;
        }
    }
} 