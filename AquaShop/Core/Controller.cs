using AquaShop.Core.Contracts;
using AquaShop.Models.Aquariums;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations;
using AquaShop.Models.Fish;
using AquaShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaShop.Core
{
    public class Controller : IController
    {
        private DecorationRepository decorations;
        private ICollection<IAquarium> aquariums;

        public Controller()
        {
            this.decorations = new DecorationRepository();
            this.aquariums = new List<IAquarium>();
        }

        public string AddAquarium(string aquariumType, string aquariumName)
        {
            if (aquariumType != "FreshwaterAquarium" && aquariumType != "SaltwaterAquarium")
            {
                throw new InvalidOperationException("Invalid aquarium type.");
            }

            if (aquariumType == "FreshwaterAquarium")
            {
                this.aquariums.Add(new FreshwaterAquarium(aquariumName));
            }

            if (aquariumType == "SaltwaterAquarium")
            {
                this.aquariums.Add(new SaltwaterAquarium(aquariumName));
            }

            return $"Successfully added { aquariumType}.";
        }

        public string AddDecoration(string decorationType)
        {
            if (decorationType != "Ornament" && decorationType != "Plant")
            {
                throw new InvalidOperationException("Invalid decoration type.");
            }

            if (decorationType == "Ornament")
            {
                this.decorations.Add(new Ornament());
            }

            if (decorationType == "Plant")
            {
                this.decorations.Add(new Plant());
            }

            return $"Successfully added {decorationType}.";
        }

        public string AddFish(string aquariumName, string fishType, string fishName, string fishSpecies, decimal price)
        {
            if (fishType != "FreshwaterFish" && fishType != "SaltwaterFish")
            {
                throw new InvalidOperationException("Invalid fish type.");
            }

            var aquarium = this.aquariums.Single(a => a.Name == aquariumName);

            if (aquarium.GetType().Name == "FreshwaterAquarium" &&
                fishType == "SaltwaterFish" ||
                aquarium.GetType().Name == "SaltwaterAquarium" &&
                fishType == "FreshwaterFish")
            {
                return "Water not suitable.";
            }

            else
            {
                if (fishType == "FreshwaterFish")
                {
                    aquarium.AddFish(new FreshwaterFish(fishName, fishSpecies, price));
                }

                if (fishType == "SaltwaterFish")
                {
                    aquarium.AddFish(new SaltwaterFish(fishName, fishSpecies, price));
                }

                return $"Successfully added {fishType} to {aquariumName}.";
            }
        }

        public string CalculateValue(string aquariumName)
        {
            var aquarium = this.aquariums.Single(a => a.Name == aquariumName);

            var value = aquarium.Fish.Sum(f => f.Price) + aquarium.Decorations.Sum(d => d.Price);

            return $"The value of Aquarium {aquariumName} is {value:f2}.";
        }

        public string FeedFish(string aquariumName)
        {
            var aquarium = this.aquariums.Single(a => a.Name == aquariumName);

            aquarium.Feed();

            return $"Fish fed: {aquarium.Fish.Count}";
        }

        public string InsertDecoration(string aquariumName, string decorationType)
        {
            if (!this.decorations.Models.Any(m => m.GetType().Name == decorationType))
            {
                throw new InvalidOperationException($"There isn't a decoration of type {decorationType}.");
            }

            var decor = this.decorations.Models.First(d => d.GetType().Name == decorationType);

            var aquarium = this.aquariums.Single(a => a.Name == aquariumName);

            aquarium.AddDecoration(decor);

            this.decorations.Remove(decor);

            return $"Successfully added {decorationType} to {aquariumName}.";
        }

        public string Report()
        {
            var sb = new StringBuilder();

            foreach (var aquarium in this.aquariums)
            {
                sb.AppendLine(aquarium.GetInfo());
            }

            return sb.ToString().Trim();
        }
    }
}
