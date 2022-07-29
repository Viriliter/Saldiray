extern alias merged;
using merged::Newtonsoft.Json;
using merged::Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace FornoxGUI.Elevation
{
    class GetElevation
    {
        static string key;
        static string heights;
        static string samples;

        private string getKey { get { return key; } }
        private string getHeight { get { return heights; } }
        private string getSamples { get { return samples; } }

        private static void setKey(string value){ key = value; }
        private static void setHeight(string value) { heights = value; }
        private static void setSamples(string value) { samples = value; }

        public static void GetElevationPlane(string points)
        {
            /*
            * Finds the altitude values of the specified points using Bing REST Services.
            */
            setKey("o2L9tTtDNKuDjJCIBc96~p_WU7aLpI2GKIK-3w3CLRQ~AgRJ4Hvl6yk3dt7HabLkO_ruoRFkV24c-a06bMsFxaB11DD94gvHQ9ZEOyk5NxLW");
            setHeight("sealevel");
            setSamples("100");
            string elevationUrl = string.Format("http://dev.virtualearth.net/REST/v1/Elevation/Bounds?bounds={0}&rows=31&cols=31&heights={1}&key={2}"
                                      , points, heights, key);

            /*Polyline Elevation
            * string elevationUrl = string.Format("http://dev.virtualearth.net/REST/v1/Elevation/Polyline?points={0}&heights={1}&samples={2}&key={3}"
            *                    , points, heights, samples, key);
            */

            //Get Web Request
            var request = System.Net.WebRequest.Create(elevationUrl);
            //Defines Content Type of Request
            request.ContentType = "application/json; charset=utf-8";
            if (points != string.Empty)
            {
                using (System.Net.WebResponse jsonResponse = request.GetResponse())
                {
                    StreamReader streamReader = new StreamReader(jsonResponse.GetResponseStream());
                    String responseData = streamReader.ReadToEnd();

                    dynamic jData = JsonConvert.DeserializeObject(responseData);
                    String statusCode = jData.statusCode;
                    if (statusCode == "200")
                    {
                        //Converts Elevation to Json Array:
                        JArray elevationData = jData.resourceSets[0].resources[0].elevations;
                        //Converts Json Array to Array:
                        MainWindow.elevationArray = elevationData.Select(jv => (int)jv).ToArray();

                        for (int pos = 0; pos < elevationData.Count; pos++)
                        {
                            var val = elevationData.ElementAt(pos);
                            int val2 = (int)MainWindow.elevationArray.ElementAt(pos);
                        }
                    }
                    else
                    {
                        throw new Exception("Error: Remote Server (" + statusCode.ToString() + ")");
                    }
                }
            }
            else
            {
                    throw new Exception("Error: No Legible Waypoint for elevation plot)");
            }
        }

        public static double GetElevationPoint(string point)
        {

            try
            {

                /*
                * Finds the altitude value of the specified point using Bing REST Services.
                */
                setKey("o2L9tTtDNKuDjJCIBc96~p_WU7aLpI2GKIK-3w3CLRQ~AgRJ4Hvl6yk3dt7HabLkO_ruoRFkV24c-a06bMsFxaB11DD94gvHQ9ZEOyk5NxLW");
                setHeight("sealevel");
                setSamples("1");

                string elevationUrl = string.Format("http://dev.virtualearth.net/REST/v1/Elevation/List?points={0}&heights={1}&key={2}"
                                          , point, heights, key);

                //Get Web Request
                var request = System.Net.WebRequest.Create(elevationUrl);
                //Defines Content Type of Request
                request.ContentType = "application/json; charset=utf-8";

                if (point != string.Empty)
                {
                    using (System.Net.WebResponse jsonResponse = request.GetResponse())
                    {

                        StreamReader streamReader = new StreamReader(jsonResponse.GetResponseStream());
                        String responseData = streamReader.ReadToEnd();

                        dynamic jData = JsonConvert.DeserializeObject(responseData);
                        String statusCode = jData.statusCode;

                        if (statusCode == "200")
                        {
                            //Converts Elevation to Json Array:
                            JArray elevationData = jData.resourceSets[0].resources[0].elevations;
                            //Converts Json Array to Array:
                            double elevation = elevationData.Select(jv => (double)jv).ToArray()[0];
                            return elevation;
                        }
                        else
                        {
                            double elevation = -1;
                            return elevation;
                            throw new Exception("Error: Remote Server (" + statusCode.ToString() + ")");
                        }
                    }
                }
                else
                {
                    double elevation = -1;
                    return elevation;
                    throw new Exception("Error: No Legible Altitude Value");
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }


        /*
        public static double[] LinearRange(double min,double max,int range)
        {
            if (max > min)
            {
                double increment = (max - min) / (range - 1);
                double[] linearNumbers = new double[range];
                for (int pos = 0; pos < range; pos++)
                {
                    linearNumbers[pos] = min + increment;
                }
                return linearNumbers;
            }
            else throw new Exception("Error: Max-Min value is negative");
        }
        */

        public static Location[] LinearRange(Location loc1, Location loc2, int range)
        {
            Location[] array_loc = new Location[range];
            double[] array_lat = new double[range];
            double[] array_long = new double[range];
            double increment_lat = Math.Abs(loc1.Latitude-loc2.Latitude)/( range-1);
            double increment_long = Math.Abs(loc1.Longitude-loc2.Longitude)/(range-1);
            for(int i =0; i<range; i++)
            {
                array_lat[i] = loc1.Latitude + increment_lat;
                array_long[i] = loc1.Longitude + increment_long;
                if (loc1.Latitude<loc2.Latitude || loc1.Latitude == loc2.Latitude)
                {
                    array_lat[i] = loc1.Latitude + increment_lat*i;
                }
                else
                {
                    array_lat[i] = loc2.Latitude + increment_lat * i;
                }
                if (loc1.Longitude < loc2.Longitude || loc1.Longitude == loc2.Longitude)
                {
                    array_long[i] = loc1.Longitude + increment_long * i;
                }
                else
                {
                    array_long[i] = loc2.Longitude + increment_long * i;
                }
                array_loc[i] = new Location(array_lat[i], array_long[i]);
            }
            return array_loc;
        }

        /*    public static string PointCompression(Location[] points)
        {
            string compressedValue = "";
            int resolution = 300;
            Location[,] array = new Location[resolution,resolution];
            Location[] row = LinearRange(points[0], points[1], resolution);
            double increment_lat = (points[3].Latitude - points[0].Latitude)/(resolution-1);
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    array[i, j] = new Location((row[j].Latitude + increment_lat * j), row[i].Longitude);
                }
            }

            Location[,] temp_array = array;

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    array[i, j].Latitude= (array[i, j].Latitude) * 100000;
                    array[i, j].Longitude = (array[i, j].Longitude) * 100000;
                    
                }
            }

            for (int i =1; i < resolution; i++)
            {
                for (int j = 1; j < resolution; j++)
                {
                    if (temp_array[i-1, j-1].Longitude - array[i, j].Longitude > 18000000)
                    {
                        array[i, j].Longitude = temp_array[i-1, j-1].Longitude - array[i, j].Longitude - 36000000;
                    }
                    else if (temp_array[i - 1, j - 1].Longitude - array[i, j].Longitude < -18000000)
                    {
                        array[i, j].Longitude = temp_array[i-1, j-1].Longitude - array[i, j].Longitude + 36000000;
                    }
                    else
                    {
                        array[i, j].Longitude = temp_array[i - 1, j - 1].Longitude - array[i, j].Longitude;
                    }

                    array[i, j].Latitude = temp_array[i - 1, j - 1].Latitude - array[i, j].Latitude;

                    if (array[i, j].Latitude < 0)
                    {
                        array[i, j].Latitude = Math.Abs(array[i, j].Latitude); array[i, j].Latitude = array[i, j].Latitude - 1;
                    }
                    if (array[i, j].Longitude < 0)
                    {
                        array[i, j].Longitude = Math.Abs(array[i, j].Longitude); array[i, j].Latitude = array[i, j].Latitude - 1;
                    }     
                }
            }

            for(int i = 1; i < resolution; i++)
            {
                for (int j = 1; j < resolution; j++)
                {
                    array[i, j].Latitude = array[i, j].Latitude * 2;
                    array[i, j].Longitude = array[i, j].Longitude * 2;
                }
            }

            int[] array_1 = new int[resolution];

            for (int i = 1; i < resolution; i++)
            {
                for (int j = 1; j < resolution; j++)
                {
                    //((latitude + longitude) * (latitude + longitude + 1) / 2) + latitude
                    array_1[i] = (int) ((array[i, j].Latitude+ array[i, j].Longitude)*(array[i, j].Latitude + array[i, j].Longitude + 1)/2 + array[i, j].Latitude);
                }
            }

            int rem;
            int num;
            for (int i = 1; i < resolution; i++)
            {
                num = (int)(array_1[i] / (32));
                while (num /) { }

                Math.DivRem( ,32,out rem);
                if (rem == 0) { break; }
            }
            Microsoft.
            return compressedValue;
        }*/

        public static string encodePoints(Location[] points)
        {
            int resolution = 10;
            //Location[,] array = new Location[resolution,resolution];
            List<Location> array = new List<Location>();
            Location[] row = LinearRange(points[0], points[1], resolution);
            double increment_lat = (points[3].Latitude - points[0].Latitude)/(resolution-1);

            int pos = 0;

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    //array[i, j] = new Location((row[j].Latitude + increment_lat * j), row[i].Longitude);
                    array.Add(new Location((row[j].Latitude + increment_lat * j), row[i].Longitude)); ;
                    pos++;
                }
            }

            var latitude = 0;
            var longitude = 0;
            var result = new List<String>();
            //var l;

            String[] code = new String[] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P",
            "Q","R","S","T","U","V","W","X","Y","Z","a","b","c","d","e","f","g","h","i","j","k","l","m","n",
            "o","p","q","r","s","t","u","v","w","x","y","z","0","1","2","3","4","5","6","7","8","9","_","-"};

            foreach (var point in array)
            {

                // step 2
                var newLatitude = (int) Math.Round(point.Latitude * 100000);
                var newLongitude = (int) Math.Round(point.Longitude * 100000);

                // step 3
                var dy = newLatitude - (int)latitude;
                var dx = newLongitude - (int)longitude;
                latitude = newLatitude;
                longitude = newLongitude;

                // step 4 and 5
                dy = (dy << 1) ^ (dy >> 31);
                dx = (dx << 1) ^ (dx >> 31);

                // step 6
                var index = ((dy + dx) * (dy + dx + 1) / 2) + dy;


                while (index > 0)
                {

                    // step 7
                    var rem = index & 31;
                    index = (index - rem) / 32;

                    // step 8
                    if (index > 0) rem += 32;

                    // step 9
                    result.Add(code[rem]);
                }
            }
            MessageBox.Show(string.Join(string.Empty, result.ToArray()));
            // step 10
            return string.Join(string.Empty, result.ToArray());
        }

        public static void asdf(Location[] points)
        {
            setKey("o2L9tTtDNKuDjJCIBc96~p_WU7aLpI2GKIK-3w3CLRQ~AgRJ4Hvl6yk3dt7HabLkO_ruoRFkV24c-a06bMsFxaB11DD94gvHQ9ZEOyk5NxLW");
            setHeight("sealevel");
            setSamples("100");
            string pointsstring = encodePoints(points);
            
            string elevationUrl = string.Format("http://dev.virtualearth.net/REST/v1/Elevation/Polyline?points={0}&samples={1}&heights={2}&key={3}"
                                      , pointsstring,samples, heights, key);

            /*Polyline Elevation
            * string elevationUrl = string.Format("http://dev.virtualearth.net/REST/v1/Elevation/Polyline?points={0}&heights={1}&samples={2}&key={3}"
            *                    , points, heights, samples, key);
            */
            TextWriter tw = new StreamWriter(@"\Users\ASUS\Desktop\TrialUAVasdf.txt");tw.WriteLine(elevationUrl);tw.Close();
            //Get Web Request
            var request = System.Net.WebRequest.Create(elevationUrl);
            //Defines Content Type of Request
            request.ContentType = "application/json; charset=utf-8";
            /*if (points != "")
            {*/
                using (System.Net.WebResponse jsonResponse = request.GetResponse())
                {
                    StreamReader streamReader = new StreamReader(jsonResponse.GetResponseStream());
                    String responseData = streamReader.ReadToEnd();

                    dynamic jData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                    String statusCode = jData.statusCode;
                    if (statusCode == "200")
                    {
                        //Converts Elevation to Json Array:
                        JArray elevationData = jData.resourceSets[0].resources[0].elevations;
                        //Converts Json Array to Array:
                        MainWindow.elevationArray = elevationData.Select(jv => (int)jv).ToArray();

                        for (int pos = 0; pos < elevationData.Count; pos++)
                        {
                            var val = elevationData.ElementAt(pos);
                            int val2 = (int)MainWindow.elevationArray.ElementAt(pos);
                        }
                    }
                    else
                    {
                        throw new Exception("Error: Remote Server (" + statusCode.ToString() + ")");
                    }
                }
            /*}
            else
            {
                throw new Exception("Error: No Legible Waypoint for elevation plot)");
            }*/
        }
    }
}
