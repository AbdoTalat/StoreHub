namespace StoreHub.Models.DTOs
{
    public class CartToReturnDTO
    {
        public DateTime CartDate { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<CartItemstoReturnDTO> cartItemstoReturnDTOs { get; set; } = Enumerable.Empty<CartItemstoReturnDTO>();
    }

    public class CartItemstoReturnDTO
    {
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public ProductsData ProductsDataCart { get; set; }
    }
}
