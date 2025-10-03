using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Game : EntityBase
    {
        protected Game() : base() { }

        public Game(
            Guid key,
            string title,
            string description,
            Guid catalogKey
        ) : base(key)
        {
            Title = title;
            Description = description;
            CatalogKey = catalogKey;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid CatalogKey { get; private set; }

        public virtual Catalog Catalog { get; private set; }

        public void SetData(string title, string description)
        {
            Title = title;
            Description = description;

            WasUpdated();
        }
    }
}
