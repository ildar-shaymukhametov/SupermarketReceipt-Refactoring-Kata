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

    public interface IOffer
    {
        Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog);
    }

    public class Offer : IOffer
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

        public virtual Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog)
        {
            if (OfferType == SpecialOfferType.TwoForAmount)
            {
                var p = Product;
                var quantity = productQuantities[p];
                var quantityAsInt = (int)quantity;
                var unitPrice = catalog.GetUnitPrice(p);
                var x = 2;
                if (quantityAsInt >= 2)
                {
                    var total = Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
                    var discountN = unitPrice * quantity - total;
                    return new Discount(p, "2 for " + Argument, -discountN);
                }
                return null;
            }
            if (OfferType == SpecialOfferType.ThreeForTwo)
            {
                var p = Product;
                var quantity = productQuantities[p];
                var quantityAsInt = (int)quantity;
                var unitPrice = catalog.GetUnitPrice(p);
                if (quantityAsInt > 2)
                {
                    var x = 3;
                    var numberOfXs = quantityAsInt / x;
                    var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                    return new Discount(p, "3 for 2", -discountAmount);
                }
                return null;
            }
            if (OfferType == SpecialOfferType.FiveForAmount)
            {
                var p = Product;
                var quantity = productQuantities[p];
                var quantityAsInt = (int)quantity;
                var unitPrice = catalog.GetUnitPrice(p);
                if (quantityAsInt >= 5)
                {
                    var x = 5;
                    var numberOfXs = quantityAsInt / x;
                    var discountTotal = unitPrice * quantity - (Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                    return new Discount(p, x + " for " + Argument, -discountTotal);
                }
                return null;
            }

            return null;
        }

    }

    public class PercentageOffer : IOffer
    {
        private readonly Product product;
        private readonly double percent;

        public PercentageOffer(Product product, double percent)
        {
            this.product = product;
            this.percent = percent;
        }

        public Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog)
        {
            var quantity = productQuantities[product];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(product);
            return new Discount(product, percent + "% off", -quantity * unitPrice * percent / 100.0);
        }
    }
}