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

        public void AddSpecialOffer(SpecialOfferType offerType, Product product, double argument)
        {
            if (offerType == SpecialOfferType.TenPercentDiscount)
            {
                _offers.Add(new PercentageOffer(product, argument));
            }
            else if (offerType == SpecialOfferType.FiveForAmount)
            {
                _offers.Add(new XforAmountOffer(product, 5, argument));
            }
            else if (offerType == SpecialOfferType.TwoForAmount)
            {
                _offers.Add(new XforAmountOffer(product, 2, argument));
            }
            else if (offerType == SpecialOfferType.ThreeForTwo)
            {
                _offers.Add(new XforYOffer(product, 3, 2));
            }
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