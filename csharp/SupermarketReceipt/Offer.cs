using System.Collections.Generic;

namespace SupermarketReceipt
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount
    }

    public class Offer
    {
        public Product Product { get; set; }

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            Product = product;
        }

        public SpecialOfferType OfferType { get; }
        public double Argument { get; }

        public Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog)
        {
            var p = Product;
            var quantity = productQuantities[p];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(p);
            Discount discount = null;
            var x = 1;
            if (OfferType == SpecialOfferType.ThreeForTwo)
            {
                x = 3;
            }
            else if (OfferType == SpecialOfferType.TwoForAmount)
            {
                x = 2;
                if (quantityAsInt >= 2)
                {
                    var total = Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
                    var discountN = unitPrice * quantity - total;
                    discount = new Discount(p, "2 for " + Argument, -discountN);
                }
            }

            if (OfferType == SpecialOfferType.FiveForAmount) x = 5;
            var numberOfXs = quantityAsInt / x;
            if (OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
            {
                var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                discount = new Discount(p, "3 for 2", -discountAmount);
            }

            if (OfferType == SpecialOfferType.TenPercentDiscount) discount = new Discount(p, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
            if (OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
            {
                var discountTotal = unitPrice * quantity - (Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                discount = new Discount(p, x + " for " + Argument, -discountTotal);
            }

            return discount;
        }
    }
}