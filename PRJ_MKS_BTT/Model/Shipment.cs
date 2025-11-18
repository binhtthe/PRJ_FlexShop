namespace PRJ_MKS_BTT.Model
{
    public class Shipment
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }

        public string TrackingNo { get; set; }
        public string Courier { get; set; }
        public decimal ShippingFee { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public Order Order { get; set; }
    }
}
