namespace ToolsLibrary.Models
{
    public class CurrentLocation
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public CurrentLocation()
        {
        }

        public CurrentLocation(double latitud, double longitude)
        {
            this.Latitude = latitud;

            this.Longitude = longitude;
        }
    }
}