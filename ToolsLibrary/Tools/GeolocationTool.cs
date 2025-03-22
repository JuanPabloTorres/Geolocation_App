using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

namespace GeolocationAds.Tools
{
    public static class GeolocationTool
    {
        public static async Task<ResponseTool<Location>> GetLocation()
        {
            try
            {
                // Verifica si ya tiene permisos antes de solicitar
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().ConfigureAwait(false);

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>().ConfigureAwait(false);

                    if (status != PermissionStatus.Granted)
                    {
                        return ResponseFactory<Location>.BuildFail("Permission denied for location access.", null);
                    }
                }

                // Obtiene la ubicación con la mejor precisión disponible
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best)).ConfigureAwait(false);

                return location is not null ? ResponseFactory<Location>.BuildSuccess("Location found", location) : ResponseFactory<Location>.BuildFail("Unable to retrieve location", null);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Location>.BuildFail($"Error retrieving location: {ex.Message}", null);
            }
        }

        public static double VincentyFormula4(double lat1, double lon1, double lat2, double lon2)
        {
            // WGS 84 ellipsoid parameters
            const double a = 6378137;         // Semi-major axis (equatorial radius) in meters

            const double b = 6356752.314245;  // Semi-minor axis (polar radius) in meters

            const double f = 1 / 298.257223563; // Flattening

            var phi1 = lat1 * Math.PI / 180.0;

            var phi2 = lat2 * Math.PI / 180.0;

            var lambda1 = lon1 * Math.PI / 180.0;

            var lambda2 = lon2 * Math.PI / 180.0;

            var L = lambda2 - lambda1;

            double tanU1 = (1 - f) * Math.Tan(phi1);

            double cosU1 = 1 / Math.Sqrt(1 + tanU1 * tanU1);

            double sinU1 = tanU1 * cosU1;

            double tanU2 = (1 - f) * Math.Tan(phi2);

            double cosU2 = 1 / Math.Sqrt(1 + tanU2 * tanU2);

            double sinU2 = tanU2 * cosU2;

            double sinLambda, cosLambda;

            double cosSqAlpha = 0; // Initialize

            double cos2SigmaM = 0; // Initialize

            double sinSigma, cosSigma;

            double sigma;

            int iterLimit = 100;

            double lambda = L;

            double prevLambda = lambda; // Initialize prevLambda

            do
            {
                sinLambda = Math.Sin(lambda);

                cosLambda = Math.Cos(lambda);

                double sinSqSigma = (cosU2 * sinLambda) * (cosU2 * sinLambda) + (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda);

                sinSigma = Math.Sqrt(sinSqSigma);

                if (sinSigma == 0) return 0; // Co-incident points

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;

                sigma = Math.Atan2(sinSigma, cosSigma);

                double sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;

                cosSqAlpha = 1 - sinAlpha * sinAlpha;

                if (cosSqAlpha != 0)
                {
                    cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                }

                double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));

                prevLambda = lambda;
                lambda = L + (1 - C) * f * sinAlpha * (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            } while (Math.Abs(lambda - prevLambda) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0) return double.NaN; // Formula failed to converge

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));

            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) - B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));

            double distance = b * A * (sigma - deltaSigma); // Distance in meters

            return distance;
        }
    }
}