namespace MudEngine.Models
{
    public class Item : GameObject
    {
        public string ShortDescription { get; set; }
        public bool IsPickable { get; set; } = true;
        public decimal Weight { get; set; }
        public int StackSize { get; set; } = 1;
        public int MaxStack { get; set; } = 1;

        public Item(string name, string description, string shortDescription, decimal weight = 1) 
            : base(name, description)
        {
            ShortDescription = shortDescription;
            Weight = weight;
        }

        public virtual string GetExamineDescription()
        {
            var desc = $"{Description}\nWeight: {Weight}";
            if (MaxStack > 1)
                desc += $"\nStack size: {StackSize}/{MaxStack}";
            return desc;
        }
    }
} 