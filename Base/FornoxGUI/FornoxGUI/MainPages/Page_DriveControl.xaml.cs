using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using DraggablePushpin;
using FornoxGUI.ExternalScripts;
using FornoxGUI.SubPages;

namespace FornoxGUI.MainPages
{
    /// <summary>
    /// Page_DriveControl.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_DriveControl : Page
    {
        bool hideValue = true;
        bool switchValue = true;
        bool switchPointerValue = false;
        bool Auto_isEnable = true;
        MapPolyline polygon = new MapPolyline();

        public Page_DriveControl()
        {
            InitializeComponent();
            loadSurveillanceCam();
            loadArtificialHorizon();
            loadHeadingControl();
            loadAirSpeedControl();
            loadNavigator();
            loadTurretPointer();
            LoadBingMap();
        }

        private void loadArtificialHorizon()
        {
            ArtificialHorizontal.Content = new SubPages.Page_ArticificialHorizontal();

        }

        private void loadSurveillanceCam()
        {
            SurveillanceView.Content = new SubPages.Page_SurveillanceCam();
        }

        private void loadTurretCam()
        {
            //WeaponView.Content = new SubPages.Page_WeaponCam();

        }

        private void Btn_HideClick_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (hideValue == true)
            {
                App.Current.Resources["IsGridVisible"] = Visibility.Hidden;
                hideValue = false;
            }
            else
            {
                TurretPointer.Visibility = Visibility.Hidden;
                switchPointerValue = true;
                App.Current.Resources["IsGridVisible"] = Visibility.Visible;
                hideValue = true;
            }
            */
        }

        private void Btn_Switch_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Switches camera views
             */
            //If switchValue is true show surveillance camera view, otherwise show weapon camera view
            if (switchValue == true)
            {
                //WeaponView.Visibility = Visibility.Hidden;
                //SecondFrame.Content = null;
                //SecondFrame.Content = WeaponView;
                //SecondFrame.Refresh();
                SurveillanceView.Visibility = Visibility.Visible;
                loadSurveillanceVideoFeatures();
                switchValue = false;
                App.Current.Resources[""] = false;
            }
            else
            {
                unloadSurveillanceVideoFeatures();
                //SurveillanceView.Visibility = Visibility.Hidden;
                //SecondFrame.Content = SurveillanceView;
                //SecondFrame.Refresh();
                //WeaponView.Visibility = Visibility.Visible;
                switchValue = true;
            }
        }

        private void loadSurveillanceVideoFeatures()
        {
            Grid_GimbalSlider.Visibility = Visibility.Visible;
            Grid_GimbalSlider.Visibility = Visibility.Visible;
        }

        private void unloadSurveillanceVideoFeatures()
        {
            Grid_GimbalSlider.Visibility = Visibility.Hidden;
            Grid_GimbalSlider.Visibility = Visibility.Hidden;
        }

        private void unloadTurretPointer()
        {
            TurretPointer.Visibility = Visibility.Hidden;

        }

        private void loadHeadingControl()
        {
            Heading.Content = new Page_Heading();
        }

        private void loadAirSpeedControl()
        {
            AirSpeed.Content = new SubPages.Page_Airspeed();
        }

        private void loadNavigator()
        {
            Navigator.Content = new SubPages.Page_Sub_VNavigator();
        }

        private void loadTurretPointer()
        {
            //loadTurretCam();
            TurretPointer.Content = new SubPages.Page_Sub_Turret();
        }

        private void Btn_Switch_Pointer_Click(object sender, RoutedEventArgs e)
        {
            if (switchPointerValue == true)
            {
                TurretPointer.Visibility = Visibility.Visible;
                switchPointerValue = false;

            }
            else
            {
                unloadTurretPointer();
                switchPointerValue = true;
            }
        }

        #region

        public void LoadBingMap()
        {
            LoadPinLayer();
            ConnectPins();
        }


        private void LoadPinLayer()
        {
            foreach (Location loc in MainWindow.pinArray)
            {
                Pushpin waypoint = new Pushpin();
                waypoint.Location = loc;
                pinLayer.Children.Add(waypoint);
            }
        }


        private void ConnectPins()
        {
            /*
             * Creates polygon that passes throught the specied waypoints before.
             */
            //Remove Existing Polygons:
            polygonLayer.Children.Remove(polygon);
            polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            polygon.StrokeThickness = 3;
            polygon.StrokeDashOffset = 50;
            polygon.Fill.Opacity = 0.1;
            LocationCollection points = new LocationCollection();
            polygon.Locations = new LocationCollection();
            Location loc;

            for (int pos = 0; pos < MainWindow.pinArray.Count; pos++)
            {
                double lat = MainWindow.pinArray.ElementAt(pos).Latitude;
                double longa = MainWindow.pinArray.ElementAt(pos).Longitude;
                loc = new Location(lat, longa);
                //points.Add(loc);
                polygon.Locations.Add(loc);
            }
            //Connects with First Push Pin:
            if (MainWindow.pinArray.Count <= 1)
            {

            }
            else
            {
                
                if (Page_MissionPlanner.IsReturnHome)
                {
                    loc = new Location(MainWindow.pinArray.ElementAt(0).Latitude, MainWindow.pinArray.ElementAt(0).Longitude);
                    polygon.Locations.Add(loc);
                    polygonLayer.Children.Add(polygon);
                    MeasureDistance();
                }
                else
                {
                    polygonLayer.Children.Remove(polygon);
                    polygonLayer.Children.Add(polygon);
                    MeasureDistance();
                }
            }

        }

        private void MeasureDistance()
        {
            /*
             * Adds measured distance on the Map
             */
            DraggablePin distPin = null;
            distBoxLayer.Children.Remove(distPin);
            distBoxLayer.Children.Clear();

            double totalDist = 0;
            for (int pos = 0; pos < MainWindow.pinArray.Count - 1; pos++)
            {
                double lat1 = MainWindow.pinArray.ElementAt(pos).Latitude;
                double long1 = MainWindow.pinArray.ElementAt(pos).Longitude;
                double lat2 = MainWindow.pinArray.ElementAt(pos + 1).Latitude;
                double long2 = MainWindow.pinArray.ElementAt(pos + 1).Longitude;
                double distance = FindDistance(lat1, long1, lat2, long2);

                MapPolyline[] polygonList = polygonLayer.Children.OfType<MapPolyline>().ToArray();

                var pinLat1 = polygonList.ElementAt(0).Locations.ElementAt(pos).Latitude;
                var pinLong1 = polygonList.ElementAt(0).Locations.ElementAt(pos).Longitude;
                var pinLat2 = polygonList.ElementAt(0).Locations.ElementAt(pos + 1).Latitude;
                var pinLong2 = polygonList.ElementAt(0).Locations.ElementAt(pos + 1).Longitude;

                //Create Rectangular:
                distPin = new DraggablePin(BingMapLayout);
                double x = (pinLat1 + pinLat2) / 2;
                double y = (pinLong1 + pinLong2) / 2;

                //Define Rectangle and Features:
                Rectangle distRect = new Rectangle();
                distRect.Width = 50;
                distRect.Height = 50;
                distRect.Stroke = new SolidColorBrush(Colors.Black);
                distRect.Fill = new SolidColorBrush(Colors.WhiteSmoke);

                ControlTemplate template = (ControlTemplate)this.FindResource("DistBox");
                Pushpin pin = new Pushpin();
                pin.Template = template;
                pin.PositionOrigin = PositionOrigin.BottomLeft;
                pin.Location = new Location(x, y);
                pin.Content = (Math.Round(distance)).ToString() + " m";

                distBoxLayer.Children.Add(pin);
                distBoxLayer.Visibility = System.Windows.Visibility.Visible;

                //Add Distance to Total Distance:
                totalDist += distance;
            }
        }

        public double FindDistance(double lat1, double long1, double lat2, double long2)
        {
            /*
             * Finds the distance between two location in meters.
             */
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat1 / 180 * Math.PI) * Math.Cos(lat2 / 180 * Math.PI)
                        * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);
            //Calculate distance in meters.
            distance = radius * c;
            return distance; // distance in meters
        }

        #endregion

        private void Btn_GimbalControl_Click(object sender, RoutedEventArgs e)
        {
            loadSurveillanceVideoFeatures();
        }

        private void Slider_HorizontalGimbal_Swipe(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.Current.Resources["RotationArtBackground"] = Slider_HorizontalGimbal.Value;
            ArtificialHorizontal.Content = new SubPages.Page_ArticificialHorizontal();
        }

        private void Slider_VerticalGimbal_Swipe(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.Current.Resources["TransformArtBackground"] = Slider_VerticalGimbal.Value;
            App.Current.Resources["TransformPointArtBackground"] = new Point(0.5, 0.5 - 0.25 * Slider_VerticalGimbal.Value / 200);
            ArtificialHorizontal.Content = new SubPages.Page_ArticificialHorizontal();

        }

        private void AutoButton_01_Click(object sender, RoutedEventArgs e)
        {

            if (Auto_isEnable)
            {
                Alarms.CallAutopilotAlarm();
                AutoButton_01.Content = "Disable";
                Auto_isEnable = false;
            }
            else
            {
                AutoButton_01.Content = "Enable";
                Auto_isEnable = true;
            }
        }

        private void AutoButton_02_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoButton_03_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoButton_04_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoButton_05_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoButton_06_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Emergency_Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Start_Engine_Click(object sender, RoutedEventArgs e)
        {
            if (true)
            {/*
                Viewbox startEngineIcon = (Viewbox)App.Current.FindResource(resourceKey: "StartEngine");
                Canvas canvas = (Canvas)startEngineIcon.ElementAt(0).FindName("SVGRoot432");
                Canvas layer4321 = (Canvas)canvas.FindName("layer4321");
                Canvas g8243233 = (Canvas)layer4321.FindName("g8243233");
                Path EngineColor = (Path)layer4321.FindName("EngineColor");

                EngineColor.Fill = Brushes.Green;
             */
            }


        }

        private void Navigator_Navigated(object sender, NavigationEventArgs e)
        {

        }
        private void Btn_Elevation_Click(object sender, RoutedEventArgs e)
        {
            int degree = Int32.Parse(TxtBox_Elevation.Text);
            int limit = checkLimit_Elev(degree);
            if (limit == 0)
            {
                SetTurretElev(degree);
            }
            if (limit == -1)
            {
                degree = -120;
                SetTurretElev(degree);
            }
            if (limit == 1)
            {
                degree = 120;
                SetTurretElev(degree);
            }

        }



        private void Btn_Azimuth_Click(object sender, RoutedEventArgs e)
        {
            int  degree = Int32.Parse(TxtBox_Azimuth.Text);
            int limit = checkLimit_Azim(degree);
            if (limit == 0)
            {
                SetTurretAzim(degree);
            }
            if (limit == -1)
            {
                degree = -120;
                SetTurretAzim(degree);
            }
            if (limit == 1)
            {
                degree = 120;
                SetTurretAzim(degree);
            }

        }

        private void SetTurretElev(int degree) { }
        private void SetTurretAzim(int degree) { }

        private int checkLimit_Azim(int degree)
        {
            if (-120 <= degree && degree <= 120)
            {
                return 0;
            }
            else
            {
                if (-360 < degree) { return -1; }
                else { return 1; }
            }
        }

        private int checkLimit_Elev(int degree)
        {
            if (15 <= degree && degree <= 60)
            {
                return 0;
            }
            else
            {
                if (15 < degree) { return -1; }
                else { return 1; }
            }
        }
    }
}
