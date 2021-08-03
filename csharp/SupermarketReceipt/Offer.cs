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

    public class XforAmountOffer : IOffer
    {
        private readonly Product product;
        private readonly int x;
        private readonly double amount;

        public XforAmountOffer(Product product, int x, double amount)
        {
            this.product = product;
            this.x = x;
            this.amount = amount;
        }

        public Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog)
        {
            var p = product;
            var quantity = productQuantities[p];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(p);
            if (quantityAsInt >= x)
            {
                var numberOfXs = quantityAsInt / x;
                var discountTotal = unitPrice * quantity - (amount * numberOfXs + quantityAsInt % x * unitPrice);
                return new Discount(p, x + " for " + amount, -discountTotal);
            }
            return null;
        }
    }

    public class XforYOffer : IOffer
    {
        private readonly Product product;
        private readonly int x;
        private readonly int y;

        public XforYOffer(Product product, int x, int y)
        {
            this.product = product;
            this.x = x;
            this.y = y;
        }

        public Discount GetDiscount(Dictionary<Product, double> productQuantities, SupermarketCatalog catalog)
        {
            var p = product;
            var quantity = productQuantities[p];
            var quantityAsInt = (int)quantity;
            var unitPrice = catalog.GetUnitPrice(p);
            if (quantityAsInt > y)
            {
                var numberOfXs = quantityAsInt / x;
                var discountAmount = quantity * unitPrice - (numberOfXs * y * unitPrice + quantityAsInt % x * unitPrice);
                return new Discount(p, $"{x} for {y}", -discountAmount);
            }
            return null;
        }
    }
}