namespace Lab01.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? OrderName { get; set; }
        public DateTime OrderPlaced { get; set; }
        public DateTime EstimatedCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPickedUp { get; set; }
        public int NumDumplings { get; set; }
        public int NumTeas { get; set; }
    }

    public class NewOrder
    {
        public string? OrderName { get; set; }
        public DateTime EstimatedCompleted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPickedUp { get; set; }
        public int NumDumplings { get; set; }
        public int NumTeas { get; set; }
    }
}