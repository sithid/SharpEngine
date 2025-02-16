using System;
using System.Collections.Generic;

namespace MudEngine.Models
{
    public abstract class GameObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, GameObject> Contents { get; set; } = new Dictionary<string, GameObject>();

        public GameObject(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual bool AddItem(GameObject item)
        {
            if (item != null)
            {
                Contents[item.Id] = item;
                return true;
            }
            return false;
        }

        public virtual GameObject RemoveItem(string itemId)
        {
            if (Contents.TryGetValue(itemId, out GameObject item))
            {
                Contents.Remove(itemId);
                return item;
            }
            return null;
        }
    }
} 