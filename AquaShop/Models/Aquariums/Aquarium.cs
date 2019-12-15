using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaShop.Models.Aquariums
{
    public abstract class Aquarium : IAquarium
    {
        private string name;
        private int capacity;
        private ICollection<IDecoration> decorations;
        private ICollection<IFish> fish;

        public Aquarium(string name, int capacity)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.decorations = new List<IDecoration>();
            this.fish = new List<IFish>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Aquarium name cannot be null or empty.");
                }

                this.name = value;
            }
        }

        public int Capacity
        {
            get
            {
                return this.capacity;
            }

            private set
            {
                this.capacity = value;
            }
        }

        public int Comfort => this.decorations.Sum(d => d.Comfort);

        public ICollection<IDecoration> Decorations => this.decorations;

        public ICollection<IFish> Fish => this.fish;

        public void AddDecoration(IDecoration decoration)
        {
            this.Decorations.Add(decoration);
        }

        public void AddFish(IFish fish)
        {
            if (this.Fish.Count >= this.Capacity)
            {
                throw new InvalidOperationException("Not enough capacity.");
            }

            var aquType = this.GetType().Name;

            var fishType = fish.GetType().Name;

            if (aquType == "FreshwaterAquarium" && fishType == "SaltwaterFish" || aquType == "SaltwaterAquarium" && fishType == "FreshwaterFish")
            {
                throw new InvalidOperationException("Not enough capacity.");
            }

            this.Fish.Add(fish);

        }

        public void Feed()
        {
            foreach (var fish in this.Fish)
            {
                fish.Eat();
            }
        }

        public string GetInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{this.Name} ({this.GetType().Name}):");

            if (this.Fish.Count == 0)
            {
                sb.AppendLine("Fish: none");
            }

            else
            {
                var fishes = this.Fish.Select(f => f.Name).ToList();

                sb.AppendLine($"Fish: {string.Join(", ", fishes)}");
            }

            sb.AppendLine($"Decorations: {this.Decorations.Count}");

            sb.AppendLine($"Comfort: {this.Comfort}");

            return sb.ToString().Trim();
        }

        public bool RemoveFish(IFish fish)
        {
            if (this.Fish.Contains(fish))
            {
                this.Fish.Remove(fish);

                return true;
            }

            else
            {
                return false;
            }
        }
    }


//    •	Name - string
//o   If the name is null or whitespace, throw an ArgumentException with message: "Aquarium name cannot be null or empty."
//o All names are unique
//•	Capacity -  int
//o   The number of Fish аn Aquarium can have
//•	Decorations - ICollection<IDecoration>
//•	Fish - ICollection<IFish>
//•	Comfort - calculated property, which returns int
//o   How is it calculated: The sum of each decoration’s comfort in the Aquarium
//Behavior
//void AddFish(IFish fish)
//Adds a Fish in the Aquarium if there is capacity for it and if the water is suitable for the Fish, otherwise throw an InvalidOperationException with message "Not enough capacity.";
//bool RemoveFish(IFish fish)
//Removes a Fish from the Aquarium.Returns true if the Fish is removed successfully, otherwise - false.
//void AddDecoration(IDecoration decoration)
//Adds a Decoration in the Aquarium.
//void Feed()
//The Feed() method feeds all fish, calls their Eat() method.
//string GetInfo()
//Returns a string with information about the Aquarium in the format below.If the Aquarium doesn't have fish, print "none" instead.
//"{aquariumName} ({aquariumType}):
//Fish: { fishName1}, {fishName2
//}, {fishName3} (…) / none
//Decorations: {decorationsCount}
//Comfort: {aquariumComfort}"
//Constructor
//An Aquarium should take the following values upon initialization: 
//string name, int capacity

}
