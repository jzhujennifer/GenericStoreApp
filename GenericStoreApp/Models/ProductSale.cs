namespace GenericStoreApp.Models
{
    public class ProductSale
    {
        //This Class holds the Quantity of each product in the order. &&  IS the Join Table Between Product and Order
        public int ProductSaleID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
