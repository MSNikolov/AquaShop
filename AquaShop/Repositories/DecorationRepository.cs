using AquaShop.Models.Decorations.Contracts;
using AquaShop.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AquaShop.Repositories
{
    public class DecorationRepository : IRepository<IDecoration>
    {
        private ICollection<IDecoration> models;

        public DecorationRepository()
        {
            this.models = new List<IDecoration>();
        }

        public IReadOnlyCollection<IDecoration> Models => this.models as IReadOnlyList<IDecoration>;

        public void Add(IDecoration model)
        {
            this.models.Add(model);
        }

        public IDecoration FindByType(string type)
        {
            return this.models.FirstOrDefault(m => m.GetType().Name == type);
        }

        public bool Remove(IDecoration model)
        {
            if (this.models.Contains(model))
            {
                this.models.Remove(model);

                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
