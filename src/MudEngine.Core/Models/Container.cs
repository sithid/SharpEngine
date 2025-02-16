namespace MudEngine.Models
{
    public class Container : Item
    {
        public decimal MaxContainerWeight { get; set; }
        public bool IsOpen { get; set; }
        
        public Container(string name, string description, string shortDescription, decimal weight = 1) 
            : base(name, description, shortDescription, weight)
        {
            MaxContainerWeight = 50.0m;
            IsOpen = false;
        }

        public override string GetExamineDescription()
        {
            var desc = base.GetExamineDescription();
            desc += $"\nCapacity: {MaxContainerWeight}";
            if (IsOpen)
            {
                if (Contents.Count == 0)
                    desc += "\nIt is empty.";
                else
                {
                    desc += "\nContents:";
                    foreach (var item in Contents.Values)
                    {
                        if (item is Item containedItem)
                            desc += $"\n  {containedItem.ShortDescription}";
                        else
                            desc += $"\n  {item.Name}";
                    }
                }
            }
            else
                desc += "\nIt is closed.";
            return desc;
        }
    }
} 