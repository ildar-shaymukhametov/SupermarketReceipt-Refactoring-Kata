using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

namespace SupermarketReceipt.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class SupermarketTest
    {
        private SupermarketCatalog _catalog;
        private Teller _teller;
        private ShoppingCart _theCart;
        private Product _toothbrush;
        private Product _rice;
        private Product _apples;
        private Product _cherryTomatoes;

        public SupermarketTest()
        {
            _catalog = new FakeCatalog();
            _teller = new Teller(_catalog);
            _theCart = new ShoppingCart();

            _toothbrush = new Product("toothbrush", ProductUnit.Each);
            _catalog.AddProduct(_toothbrush, 0.99);
            _rice = new Product("rice", ProductUnit.Each);
            _catalog.AddProduct(_rice, 2.99);
            _apples = new Product("apples", ProductUnit.Kilo);
            _catalog.AddProduct(_apples, 1.99);
            _cherryTomatoes = new Product("cherry tomato box", ProductUnit.Each);
            _catalog.AddProduct(_cherryTomatoes, 0.69);

        }

        [Fact]
        public void an_empty_shopping_cart_should_cost_nothing()
        {
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void one_normal_item()
        {
            _theCart.AddItem(_toothbrush);
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void two_normal_items()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_rice);
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void buy_two_get_one_free()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _teller.AddSpecialOffer(new XforYOffer(_toothbrush, 3, 2));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void buy_five_get_one_free()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _teller.AddSpecialOffer(new XforYOffer(_toothbrush, 3, 2));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void loose_weight_product()
        {
            _theCart.AddItemQuantity(_apples, .5);
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void percent_discount()
        {
            _theCart.AddItem(_rice);
            _teller.AddSpecialOffer(new PercentageOffer(_rice, 10.0));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void xForY_discount()
        {
            _theCart.AddItem(_cherryTomatoes);
            _theCart.AddItem(_cherryTomatoes);
            _teller.AddSpecialOffer(new XforAmountOffer(_cherryTomatoes, 2, .99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void FiveForY_discount()
        {
            _theCart.AddItemQuantity(_apples, 5);
            _teller.AddSpecialOffer(new XforAmountOffer(_apples, 5, 6.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void FiveForY_discount_withSix()
        {
            _theCart.AddItemQuantity(_apples, 6);
            _teller.AddSpecialOffer(new XforAmountOffer(_apples, 5, 6.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void FiveForY_discount_withSixteen()
        {
            _theCart.AddItemQuantity(_apples, 16);
            _teller.AddSpecialOffer(new XforAmountOffer(_apples, 5, 6.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void FiveForY_discount_withFour()
        {
            _theCart.AddItemQuantity(_apples, 4);
            _teller.AddSpecialOffer(new XforAmountOffer(_apples, 5, 6.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public void Bundle_discount()
        {
            _theCart.AddItemQuantity(_toothbrush, 1);
            _theCart.AddItemQuantity(_apples, 2);
            _teller.AddSpecialOffer(new BundleOffer(new List<ProductQuantity>
            {
                new ProductQuantity(_toothbrush, 1),
                new ProductQuantity(_apples, 2)
            }, 1.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }
        
        [Fact]
        public void Bundle_incomplete()
        {
            _theCart.AddItemQuantity(_toothbrush, 2);
            _theCart.AddItemQuantity(_apples, 2);
            _teller.AddSpecialOffer(new BundleOffer(new List<ProductQuantity>
            {
                new ProductQuantity(_toothbrush, 1),
                new ProductQuantity(_apples, 2)
            }, 1.99));
            Receipt receipt = _teller.ChecksOutArticlesFrom(_theCart);
            Approvals.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }
    }
}