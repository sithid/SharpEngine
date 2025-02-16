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
    }
} 