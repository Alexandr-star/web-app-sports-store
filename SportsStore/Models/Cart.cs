using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(Product product, int quality) {
            CartLine cartLine = lineCollection
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();
            if (cartLine == null)
            {
                lineCollection.Add(new CartLine {
                    Product = product,
                    Quality = quality
                });
            }
            else
            {
                cartLine.Quality += quality;
            }
        }
        public virtual void RemoveLine(Product product) =>
            lineCollection.RemoveAll(line => line.Product.ProductId == product.ProductId);
        public virtual decimal ComputeTotalValue() =>
            lineCollection.Sum(element => element.Product.Price * element.Quality);
        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quality { get; set; }

    }
}