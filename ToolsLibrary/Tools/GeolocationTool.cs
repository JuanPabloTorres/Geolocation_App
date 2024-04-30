using ToolsLibrary.Factories;
using ToolsLibrary.Tools;

namespace GeolocationAds.Tools
{
    public static class GeolocationTool
    {
        public static (double lat, double lon) CalculateDestinationPoint(double lat, double lon, double distanceKm, double bearing)
        {
            double EarthRadiusKm = 6371;

            var φ1 = lat * Math.PI / 180;

            var λ1 = lon * Math.PI / 180;

            var θ = bearing * Math.PI / 180;

            var δ = distanceKm / EarthRadiusKm;

            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(δ) + Math.Cos(φ1) * Math.Sin(δ) * Math.Cos(θ));

            var λ2 = λ1 + Math.Atan2(Math.Sin(θ) * Math.Sin(δ) * Math.Cos(φ1), Math.Cos(δ) - Math.Sin(φ1) * Math.Sin(φ2));

            return (φ2 * 180 / Math.PI, λ2 * 180 / Math.PI);
        }

        public static Location CalculateLocation(Location startLocation, double distanceInMeters, double bearingInDegrees)
        {
            double lat1 = startLocation.Latitude * Math.PI / 180.0;

            double lon1 = startLocation.Longitude * Math.PI / 180.0;

            double brng = bearingInDegrees * Math.PI / 180.0;

            double R = 6371000; // Earth's radius in meters

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distanceInMeters / R) + Math.Cos(lat1) * Math.Sin(distanceInMeters / R) * Math.Cos(brng));

            double lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(distanceInMeters / R) * Math.Cos(lat1), Math.Cos(distanceInMeters / R) - Math.Sin(lat1) * Math.Sin(lat2));

            return new Location(lat2 * 180.0 / Math.PI, lon2 * 180.0 / Math.PI);
        }

        public static async Task<ResponseTool<Location>> GetLocation()
        {
            try
            {
                // Solicita permiso para acceder a la ubicación del usuario
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    // Obtiene la ubicación del usuario
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);

                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        return ResponseFactory<Location>.BuildSuccess("Location Found", location);
                    }
                    else
                    {
                        // No se pudo obtener la ubicación del usuario

                        return ResponseFactory<Location>.BuildFail("Location could not found", null);
                    }
                }
                else
                {
                    return ResponseFactory<Location>.BuildFail("Not have permission for access location.", null);
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory<Location>.BuildFail(ex.Message, null);
            }
        }

        public static double Haversineformula(double lat1, double lon1, double lat2, double lon2)
        {
            // Radius of the Earth in kilometers
            const double earthRadius = 6371.0;

            // Convert latitude and longitude values to radians
            var lat1Rad = ToRadians(lat1);

            var lon1Rad = ToRadians(lon1);

            var lat2Rad = ToRadians(lat2);

            var lon2Rad = ToRadians(lon2);

            // Calculate the differences between the latitudes and longitudes
            var deltaLat = lat2Rad - lat1Rad;

            var deltaLon = lon2Rad - lon1Rad;

            // Apply the Haversine formula
            var a = Math.Pow(Math.Sin(deltaLat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(deltaLon / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = earthRadius * c;

            return distance;
        }

        public static bool IsInPerimeter(Location centerLocation, double perimeterRadiusInMeters, Location currentLocation, out double distanceDisplay)
        {
            // Radius of the Earth in meters
            const double earthRadiusMeters = 6371000.0;

            // Convert latitude and longitude values to radians
            var lat1Rad = ToRadians(centerLocation.Latitude);

            var lon1Rad = ToRadians(centerLocation.Longitude);

            var lat2Rad = ToRadians(currentLocation.Latitude);

            var lon2Rad = ToRadians(currentLocation.Longitude);

            // Calculate the differences between the latitudes and longitudes
            var deltaLat = lat2Rad - lat1Rad;

            var deltaLon = lon2Rad - lon1Rad;

            // Apply the Haversine formula
            var a = Math.Pow(Math.Sin(deltaLat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(deltaLon / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = earthRadiusMeters * c;

            distanceDisplay = distance;

            // Check if distance is less than or equal to 1 meter
            return distance <= perimeterRadiusInMeters;
        }

        public static bool IsLocationInsidePerimeter(Location centerLocation, double perimeterRadiusInMeters, Location otherLocation, out double distance)
        {
            //double R = 6371000; // Earth's radius in meters

            double R = 6371000.0; // Earth's radius in meters

            //double lat1 = centerLocation.Latitude * (Math.PI / 180.0);

            //double lon1 = centerLocation.Longitude * (Math.PI / 180.0);

            //double lat2 = otherLocation.Latitude * (Math.PI / 180.0);

            //double lon2 = otherLocation.Longitude * (Math.PI / 180.0);

            //double dLat = lat2 - lat1;

            //double dLon = lon2 - lon1;

            double dLat = (Math.PI / 180) * (otherLocation.Latitude - centerLocation.Latitude);

            double dLon = (Math.PI / 180) * (otherLocation.Longitude - centerLocation.Longitude);

            // convert to radians
            double lat1 = (Math.PI / 180) * (centerLocation.Latitude);

            double lat2 = (Math.PI / 180) * (otherLocation.Latitude);

            //double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(lat1) * Math.Cos(lat2);

            //double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double c = 2 * Math.Asin(Math.Sqrt(a));

            double distanceInMeters = R * c;

            distance = distanceInMeters;

            return distanceInMeters <= perimeterRadiusInMeters;
        }

        public static bool IsPointInsidePolygon((double lat, double lon)[] polygon, double lat, double lon)
        {
            var inside = false;

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                var pi = polygon[i];

                var pj = polygon[j];

                if (((pi.lat <= lat && lat < pj.lat) || (pj.lat <= lat && lat < pi.lat)) && (lon < (pj.lon - pi.lon) * (lat - pi.lat) / (pj.lat - pi.lat) + pi.lon))
                {
                    inside = !inside;
                }
            }
            return inside;
        }

        public static double SphericalLawofCosines(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // in kilometers

            var phi1 = lat1 * Math.PI / 180.0;

            var phi2 = lat2 * Math.PI / 180.0;

            var deltaLambda = (lon2 - lon1) * Math.PI / 180.0;

            var distance = Math.Acos(Math.Sin(phi1) * Math.Sin(phi2) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda)) * earthRadius * 1000;

            return distance;
        }

        public static double VicentyFormula(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // in kilometers

            var phi1 = lat1 * Math.PI / 180.0;

            var phi2 = lat2 * Math.PI / 180.0;

            var lambda1 = lon1 * Math.PI / 180.0;

            var lambda2 = lon2 * Math.PI / 180.0;

            var deltaLambda = Math.Abs(lambda2 - lambda1);

            var numerator = Math.Sqrt(Math.Pow(Math.Cos(phi2) * Math.Sin(deltaLambda), 2) + Math.Pow(Math.Cos(phi1) * Math.Sin(phi2) - Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda), 2));

            var denominator = Math.Sin(phi1) * Math.Sin(phi2) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Cos(deltaLambda);

            var centralAngle = Math.Atan2(numerator, denominator);

            var distance = earthRadius * centralAngle;

            return distance;
        }

        public static double VicentyFormula2(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadius = 6371.0; // in kilometers

            var phi1 = ToRadians(lat1);
            var phi2 = ToRadians(lat2);

            var deltaLambda = ToRadians(lon2 - lon1);

            var a = Math.Pow(Math.Cos(phi2) * Math.Sin(deltaLambda), 2) +
                    Math.Pow(Math.Cos(phi1) * Math.Sin(phi2) -
                             Math.Sin(phi1) * Math.Cos(phi2) *
                             Math.Cos(deltaLambda), 2);

            var b = Math.Sin(phi1) * Math.Sin(phi2) +
                    Math.Cos(phi1) * Math.Cos(phi2) *
                    Math.Cos(deltaLambda);

            var centralAngle = Math.Atan2(Math.Sqrt(a), b);

            var sinSigma = Math.Sin(centralAngle);
            var cosSigma = Math.Cos(centralAngle);

            var cos2SigmaM = Math.Cos(2.0 * centralAngle) - (2.0 * Math.Pow(cosSigma, 2.0));

            var bigB = (EarthRadius / 16.0) * Math.Pow(cosSigma, 2.0) *
                       (4.0 + (EarthRadius * (4.0 - 3.0 * Math.Pow(cosSigma, 2.0))));

            var deltaSigma = bigB * sinSigma *
                             (cos2SigmaM + (bigB / 4.0) *
                              (Math.Cos(3.0 * centralAngle) -
                               (4.0 * Math.Pow(cos2SigmaM, 2.0)) -
                               (bigB / 6.0) * Math.Cos(4.0 * centralAngle) *
                               (4.0 * Math.Pow(cos2SigmaM, 2.0) - 3.0) +
                               (bigB / 8.0) * Math.Cos(5.0 * centralAngle) *
                               (Math.Cos(2.0 * centralAngle) +
                                (2.0 * Math.Pow(cos2SigmaM, 2.0)))));

            return EarthRadius * deltaSigma;
        }

        public static double VincentyFormula3(double lat1, double lon1, double lat2, double lon2)
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

            double sinSigma, cosSigma;

            double sigma;

            double lambda = L;

            double prevLambda;

            int iterLimit = 100;

            double cos2SigmaM;

            double cosSqAlpha;

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

                if (cosSqAlpha == 0)
                {
                    cos2SigmaM = 0; // Equatorial line: cosSqAlpha = 0
                }
                else
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

            //return distance / 1000.0; // Convert to kilometers

            return distance;
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

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}