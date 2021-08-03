using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        private readonly List<IOffer> _offers;

        public Teller(SupermarketCatalog catalog)
        {
            _offers = new List<IOffer>();
            _catalog = catalog;
        }

        public void AddSpecialOffer(IOffer offer)
        {
            _offers.Add(offer);
        }

        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            foreach (var pq in productQuantities)
            {
                var p = pq.Product;
                var quantity = pq.Quantity;
                var unitPrice = _catalog.GetUnitPrice(p);
                var price = quantity * unitPrice;
                receipt.AddProduct(p, quantity, unitPrice, price);
            }

            foreach (var offer in _offers)
            {
                var discount = offer.GetDiscount(theCart.GetProductQuantities(), _catalog);
                if (discount != null)
                    receipt.AddDiscount(discount);
            }

            return receipt;
        }
    }
}