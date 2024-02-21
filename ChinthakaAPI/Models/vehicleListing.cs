namespace ChinthakaAPI.Models
{
    public class vehicleListing
    {
        public string from { get; set; }

        public string to { get; set; }

        public List<listings> listings { get; set; }
    }

    public class listings
    {
        public string name { get; set; }
        public double pricePerPassenger { get; set; }

        public vehicleType vehicleType { get; set; }
    }

    public class vehicleType
    {
        public string name { get; set; }
        
        public int maxPassengers { get; set; }
    }
}
