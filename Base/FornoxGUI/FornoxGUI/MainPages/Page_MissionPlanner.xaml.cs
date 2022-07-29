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
using System.IO;
using Newtonsoft.Json.Linq;
using FornoxGUI;
using FornoxGUI.SubPages;
using FornoxGUI.ExternalScripts;
using System.ComponentModel;
using System.Threading;

namespace FornoxGUI.MainPages
{
    /// <summary>
    /// Page_MissionPlanner.xaml etkileşim mantığı
    /// </summary>
    public partial class Page_MissionPlanner : Page
    {
        int mode = 1;
        static DraggablePin pin;
        static MapPolyline polygon = new MapPolyline();
        Location Selectedlocation = new Location();
        TextBox Textbox_Alt = new TextBox();
        MapPolygon areaPolygon = new MapPolygon();
        int missionNumber = 1;
        static string record_filename;

        public static bool IsReturnHome;
        public static BackgroundWorker elevationThread = new BackgroundWorker();
        public static BackgroundWorker routeThread = new BackgroundWorker();
        private static MissionReader msReader = new MissionReader();


        public Page_MissionPlanner()
        {
            InitializeComponent();
            //Changes Comma to Dot at Numbers
            #region Change Local

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            #endregion
            //Call updateUGV class to update the UGV locations at the background
            UpdateUGV updateUGV = new UpdateUGV();
            //Task task = Task.Run((Action)updateUGV.updateLocation);
            Page_MissionPlanner.IsReturnHome = false;


        }

        #region

        /// <summary>
        /// Adds the push pin on the to the clicked point and connect with the previous points if exist.
        /// Also stores location information into the pinArray.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PinLocation(object sender, MouseButtonEventArgs e)
        {
            // Disables the Default Mouse Double-Click Action:
            e.Handled = true;

            //Get the Mouse Click Coordinates:
            Point mousePosition = e.GetPosition(BingMapLayout);

            //Convert the Mouse Coordinates to a Location on the Map:
            Location pinLocation = BingMapLayout.ViewportPointToLocation(mousePosition);

            //Creates the Pushpin:
            pin = new DraggablePin(BingMapLayout);

            //Create DraggablePin Handler
            pin.PinDragged += HandleDraggedPin;

            pin.Location = pinLocation;

            //Adds Pin to Array
            MainWindow.pinArray.Add(pinLocation);

            // Adds the Pushpin to the Map:
            pinLayer.Children.Add(pin);

            //Focuses Pin:
            pin.Focus();

            //Set Latitude and Longitude Values
            double lat = pinLocation.Latitude;
            double longa = pinLocation.Longitude;

            //Set Altitude Value
            pinLocation.Altitude = 10;

            //Adds Pin to List:
            AddList(pinLocation);
            ConnectPins();

        }

        /// <summary>
        /// Add location information and other visual elements corresponding to the items to the list.
        /// </summary>
        /// <param name="pinLocation">Location of the pushpin item</param>
        private void AddList(Location pinLocation)
        {
            AddList_PinNo(null, null);
            AddList_Down();
            AddList_Up();
            AddList_Command();
            AddList_Long(pinLocation.Longitude);
            AddList_Lat(pinLocation.Latitude);
            //elevationThread.DoWork += getPointElevation_DoWork;
            //elevationThread.RunWorkerAsync(argument: pinLocation);

            AddList_Alt(getPointElevation(pinLocation));
            AddList_WRad(pinLocation);
        }

        private void ShowDelete(object sender, MouseEventArgs e)
        {
            foreach (Grid grid in List_PinNo.Items.OfType<Grid>())
            {
                grid.Children.OfType<Button>().ElementAt(0).Visibility = Visibility.Visible;
            }
        }

        private void HideDelete(object sender, MouseEventArgs e)
        {
            foreach (Grid grid in List_PinNo.Items.OfType<Grid>())
            {
                grid.Children.OfType<Button>().ElementAt(0).Visibility = Visibility.Hidden;
            }
        }

        private void DeletePin(object sender, RoutedEventArgs e)
        {
            int pos = 0;
            foreach (Grid grid in List_PinNo.Items.OfType<Grid>())
            {
                if (sender == List_PinNo.Items.OfType<Grid>().ElementAt(pos).Children.OfType<Button>().ElementAt(0))
                {
                    RemovePinIndex(pos);
                    break;
                }
                pos++;
            }
            ConnectPins();
        }

        private void RemovePinIndex(int pos)
        {
            MainWindow.pinArray.RemoveAt(pos);
            pinLayer.Children.RemoveAt(pos);
            List_PinNo.Items.RemoveAt(pos);
            List_Up.Items.RemoveAt(pos);
            List_Down.Items.RemoveAt(pos);
            List_Command.Items.RemoveAt(pos);
            List_Lat.Items.RemoveAt(pos);
            List_Long.Items.RemoveAt(pos);
            List_Alt.Items.RemoveAt(pos);
            List_WRad.Items.RemoveAt(pos);
            if (pos>0)
            {
                distBoxLayer.Children.RemoveAt(pos - 1);
            }
        }

        private void AddList_PinNo(object sender, EventArgs e)
        {
            Grid grid = new Grid();
            TextBlock Textblock = new TextBlock();
            Textblock.TextWrapping = System.Windows.TextWrapping.NoWrap;
            Textblock.Text = MainWindow.pinArray.Count.ToString();
            Textblock.Foreground = Brushes.White;
            Textblock.FontWeight = FontWeights.Bold;
            Textblock.Width = 29;
            Textblock.Height = 30;

            Textblock.Background = Brushes.DimGray;
            Textblock.TextAlignment = TextAlignment.Center;
            Textblock.HorizontalAlignment = HorizontalAlignment.Center;
            Textblock.VerticalAlignment = VerticalAlignment.Center;
            List_PinNo.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            grid.Children.Add(Textblock);

            //Show delete button
            Button btn_delete = new Button();
            btn_delete.Height = 29;
            btn_delete.Width = 32;
            btn_delete.Content = this.FindResource("CloseBoxIcon");
            btn_delete.HorizontalContentAlignment = HorizontalAlignment.Center;
            btn_delete.VerticalContentAlignment = VerticalAlignment.Center;
            btn_delete.Background = Brushes.DimGray;
            btn_delete.Visibility = Visibility.Hidden;
            btn_delete.Click += DeletePin;
            grid.Children.Add(btn_delete);
            List_PinNo.Items.Add(grid);
        }

        private void AddList_Down()
        {
            //Creates New Button
            Button button = new Button();
            button.Height = 30;
            button.Width = 45;

            button.Content = this.FindResource("PriorityLowIcon");
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Background = Brushes.DimGray;
            button.BorderThickness = new Thickness(0);
            List_Down.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            button.Click += PriorityDecrease;
            List_Down.Items.Add(button);
        }

        private void AddList_Up()
        {
            //Creates New Button
            Button button = new Button();
            button.Height = 30;
            button.Width = 45;
            button.Content = this.FindResource("PriorityHighIcon");
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Background = Brushes.DimGray;
            button.BorderThickness = new Thickness(0);
            List_Up.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            button.Click += PriorityIncrease;
            List_Up.Items.Add(button);

        }

        private void PriorityIncrease(object sender, RoutedEventArgs e)
        {
            int pos = 0;
            foreach (Grid grid in List_PinNo.Items.OfType<Grid>())
            {
                if (sender == List_Up.Items.OfType<Button>().ElementAt(pos))
                {
                    IncreasePinIndex(pos);
                    break;
                }
                pos++;
            }

        }

        private void PriorityDecrease(object sender, RoutedEventArgs e)
        {
            int pos = 0;
            foreach (Grid grid in List_PinNo.Items.OfType<Grid>())
            {
                if (sender == List_Down.Items.OfType<Button>().ElementAt(pos))
                {
                    break;
                }
                pos++;
            }
            DecreasePinIndex(pos);
        }

        private void IncreasePinIndex(int pos)
        {
            if (pos > 0)
            {
                Location temp_loc = MainWindow.pinArray.ElementAt(pos - 1);
                Location current_loc = MainWindow.pinArray.ElementAt(pos);
                MainWindow.pinArray.Insert(pos - 1, current_loc);
                MainWindow.pinArray.Insert(pos, temp_loc);

                foreach (var element in List_Command.Items.OfType<ComboBox>())
                {
                    if (element == List_Command.Items.GetItemAt(pos))
                    {
                        var element2 = element;
                        List_Command.Items.Remove(element);
                        List_Command.Items.Insert(pos - 1, element2);
                        break;
                    }
                }

                foreach (var element in List_Lat.Items.OfType<TextBlock>())
                {
                    if (element == List_Lat.Items.GetItemAt(pos))
                    {
                        var element2 = element;
                        List_Lat.Items.Remove(element);
                        List_Lat.Items.Insert(pos - 1, element2);
                        break;
                    }
                }

                foreach (var element in List_Long.Items.OfType<TextBlock>())
                {
                    if (element == List_Long.Items.GetItemAt(pos))
                    {
                        var element2 = element;
                        List_Long.Items.Remove(element);
                        List_Long.Items.Insert(pos - 1, element2);
                        break;
                    }
                }

                foreach (var element in List_Alt.Items.OfType<TextBlock>())
                {
                    if (element == List_Alt.Items.GetItemAt(pos))
                    {
                        var element2 = element;
                        List_Alt.Items.Remove(element);
                        List_Alt.Items.Insert(pos - 1, element2);
                        break;
                    }
                }
                
                foreach (var element in List_WRad.Items.OfType<StackPanel>())
                {
                    if (element == List_WRad.Items.GetItemAt(pos))
                    {
                        var element2 = element;
                        List_WRad.Items.Remove(element);
                        List_WRad.Items.Insert(pos - 1, element2);
                        break;
                    }
                }
            }
        }

        private void DecreasePinIndex(int pos)
        {
            foreach (var element in List_Command.Items.OfType<ComboBox>())
            {
                if (element == List_Command.Items.GetItemAt(pos))
                {
                    var element2 = element;
                    List_Command.Items.Remove(element);
                    List_Command.Items.Insert(pos + 1, element2);
                    break;
                }
            }

            foreach (var element in List_Lat.Items.OfType<TextBlock>())
            {
                if (element == List_Lat.Items.GetItemAt(pos))
                {
                    var element2 = element;
                    List_Lat.Items.Remove(element);
                    List_Lat.Items.Insert(pos + 1, element2);
                    break;
                }
            }

            foreach (var element in List_Long.Items.OfType<TextBlock>())
            {
                if (element == List_Long.Items.GetItemAt(pos))
                {
                    var element2 = element;
                    List_Long.Items.Remove(element);
                    List_Long.Items.Insert(pos + 1, element2);
                    break;
                }
            }

            foreach (var element in List_Alt.Items.OfType<StackPanel>())
            {
                if (element == List_Alt.Items.GetItemAt(pos))
                {
                    var element2 = element;
                    List_Alt.Items.Remove(element);
                    List_Alt.Items.Insert(pos + 1, element2);
                    break;
                }
            }

            foreach (var element in List_WRad.Items.OfType<StackPanel>())
            {
                if (element == List_WRad.Items.GetItemAt(pos))
                {
                    var element2 = element;
                    List_WRad.Items.Remove(element);
                    List_WRad.Items.Insert(pos + 1, element2);
                    break;
                }
            }
        }

        private void AddList_Command()
        {
            ComboBox combobox = new ComboBox();
            combobox.Height = 30;
            combobox.Width = 180;
            combobox.Background = Brushes.DimGray;


            combobox.Items.Insert(0, "Pass");
            combobox.Items.Insert(1, "Surveillance");
            combobox.Items.Insert(2, "Arm Weapon");
            //combobox.Items.Insert(3, "Return Home");
            //Set default command to "Pass"
            combobox.SelectedIndex = 0;
            List_Command.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            List_Command.Items.Add(combobox);
        }

        private void AddList_Lat(Double Lat)
        {
            TextBlock Textblock = new TextBlock();
            Textblock.TextWrapping = System.Windows.TextWrapping.NoWrap;
            Textblock.Text = String.Format("{0:F6}", Lat);
            Textblock.FontWeight = FontWeights.Bold;
            Textblock.Foreground = Brushes.White;
            Textblock.Width = 150;
            Textblock.Height = 30;
            Textblock.Background = Brushes.DimGray;
            Textblock.TextAlignment = TextAlignment.Center;
            List_Lat.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            List_Lat.Items.Add(Textblock);
        }

        private void AddList_Long(Double Long)
        {
            TextBlock Textblock = new TextBlock();
            Textblock.TextWrapping = System.Windows.TextWrapping.NoWrap;
            Textblock.Text = String.Format("{0:F6}", Long);
            Textblock.FontWeight = FontWeights.Bold;
            Textblock.Foreground = Brushes.White;
            Textblock.Width = 150;
            Textblock.Height = 30;
            Textblock.Background = Brushes.DimGray;
            Textblock.TextAlignment = TextAlignment.Center;
            List_Long.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            List_Long.Items.Add(Textblock);
        }

        private void AddList_Alt(Double Alt)
        {
            TextBlock Textblock = new TextBlock();
            Textblock.TextWrapping = System.Windows.TextWrapping.NoWrap;
            Textblock.Text = Alt.ToString();
            Textblock.FontWeight = FontWeights.Bold;
            Textblock.Foreground = Brushes.White;
            Textblock.Width = 150;
            Textblock.Height = 30;
            Textblock.Background = Brushes.DimGray;
            Textblock.TextAlignment = TextAlignment.Center;
            List_Alt.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            List_Alt.Items.Add(Textblock);
        }

        private void AddList_WRad(Location pinLocation)
        {

            Selectedlocation = pinLocation;
            StackPanel Stackpanel = new StackPanel();
            Stackpanel.Orientation = Orientation.Horizontal;
            Stackpanel.HorizontalAlignment = HorizontalAlignment.Center;
            Stackpanel.Width = 160;
            Stackpanel.Height = 30;

            TextBox Textbox = new TextBox();

            Textbox.Margin = new Thickness(5, 1, 0, 1);
            Textbox.Width = 80;
            Textbox.Height = 30;
            Textbox.BorderThickness = new Thickness(0);
            Textbox.FontWeight = FontWeights.Bold;
            Textbox.Foreground = Brushes.White;
            Textbox.Background = Brushes.DimGray;
            Textbox.TextWrapping = TextWrapping.NoWrap;
            Textbox.Text = pinLocation.Altitude.ToString();
            Textbox.TextAlignment = TextAlignment.Right;
            Textbox.VerticalContentAlignment = VerticalAlignment.Top;
            List_WRad.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            Textbox.TextChanged += AddList_WRad_TextChanged;

            Stackpanel.Children.Add(Textbox);

            Button Button_up = new Button();
            Button_up.Margin = new Thickness(5, 1, 0, 1);
            Button_up.Width = 25;
            Button_up.FontWeight = FontWeights.Bold;
            Button_up.BorderThickness = new Thickness(0);
            Button_up.Foreground = Brushes.White;
            Button_up.Background = Brushes.DimGray;
            Button_up.Content = "˄";
            Button_up.Click += AddList_WRad_button_up;
            Stackpanel.Children.Add(Button_up);

            Button Button_down = new Button();
            Button_down.Margin = new Thickness(0, 1, 0, 1);
            Button_down.Width = 25;
            Button_down.FontWeight = FontWeights.Bold;
            Button_down.BorderThickness = new Thickness(0);
            Button_down.Foreground = Brushes.White;
            Button_down.Background = Brushes.DimGray;
            Button_down.Content = "˅";
            Button_down.Click += AddList_WRad_button_down;
            Stackpanel.Children.Add(Button_down);

            List_WRad.Items.Add(Stackpanel);
        }

        private void AddList_WRad_TextChanged(object sender, TextChangedEventArgs e)
        {/*
            foreach (Location loc in MainWindow.pinArray)
            { 
                if (loc.Latitude == Selectedlocation.Latitude)
                {
                    TextBox txt = sender as TextBox;
                    if (txt.Text == "")
                    {
                        txt.Text = "10";
                    }
                    loc.Altitude = Convert.ToDouble(txt.Text.ToString());
                    sender = loc.Altitude.ToString();
                }
            }*/
        }

        private void AddList_WRad_button_up(object sender, RoutedEventArgs e)
        {
            bool Isbreaked = false;
            int i = 0;
            foreach (Location loc in MainWindow.pinArray)
            {
                int pos = 0;
                for (pos = 0; pos < List_WRad.Items.OfType<StackPanel>().Count(); pos++)
                {
                    if (sender == List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<Button>().ElementAt(0))
                    {
                        if (loc == MainWindow.pinArray.ElementAt(pos))
                        {
                            loc.Altitude = (Convert.ToDouble(List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<TextBox>().ElementAt(0).Text) + 1);
                            List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<TextBox>().ElementAt(0).Text = loc.Altitude.ToString();
                            Isbreaked = true;
                            break;
                        }
                    }
                }
                if (Isbreaked) { break; }
                i++;
            }
        }

        private void AddList_WRad_button_down(object sender, RoutedEventArgs e)
        {
            bool Isbreaked = false;
            int i = 0;
            foreach (Location loc in MainWindow.pinArray)
            {
                if (loc.Altitude > 0)
                {
                    for (int pos = 0; pos < List_WRad.Items.OfType<StackPanel>().Count(); pos++)
                    {
                        if (sender == List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<Button>().ElementAt(1))
                        {
                            if (loc == MainWindow.pinArray.ElementAt(pos))
                            {
                                loc.Altitude = Convert.ToDouble(List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<TextBox>().ElementAt(0).Text) - 1;
                                List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<TextBox>().ElementAt(0).Text = (loc.Altitude).ToString();
                                Isbreaked = true;
                                break;
                            }
                        }
                    }
                }
                if (Isbreaked) { break; }
                i++;
            }
        }

        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        private void lbx1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(List_WRad, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(List_Alt, typeof(ScrollViewer)) as ScrollViewer;

            ScrollViewer _listboxScrollViewer3 = GetDescendantByType(List_Long, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer4 = GetDescendantByType(List_Lat, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer5 = GetDescendantByType(List_Command, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer6 = GetDescendantByType(List_Down, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer7 = GetDescendantByType(List_Up, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer8 = GetDescendantByType(List_PinNo, typeof(ScrollViewer)) as ScrollViewer;

            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer3.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer4.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer5.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer6.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer7.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer8.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);

        }
        #endregion

        #region

        private void Btn_SwitchView_Click(object sender, RoutedEventArgs e)
        {
            // Switch View of Map:
            if (mode == 1)
            {
                BingMapLayout.Mode = new AerialMode();
                mode = 2;
                return;
            }
            else if (mode == 2)
            {
                BingMapLayout.Mode = new RoadMode();
                mode = 1;
                return;
            }
        }

        private void Btn_ConnectPins_Click(object sender, RoutedEventArgs e)
        {
            ConnectPins();
        }

        public void HandleDraggedPin(object sender, EventArgs e)
        {
            ConnectPins();
        }

        /// <summary>
        /// Draws polygon in order to connect the pushpins
        /// </summary>
        public void ConnectPins()
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
                if (IsReturnHome)
                {
                    loc = new Location(MainWindow.pinArray.ElementAt(0).Latitude, MainWindow.pinArray.ElementAt(0).Longitude);
                    polygon.Locations.Add(loc);
                    polygonLayer.Children.Add(polygon);
                    MeasureDistance();
                    Text_NumPins.Text = MainWindow.pinArray.Count.ToString();
                    //areaCreator(); 
                }
                else
                {
                    polygonLayer.Children.Add(polygon);
                    MeasureDistance();
                    Text_NumPins.Text = MainWindow.pinArray.Count.ToString();
                }
            }
        }

        /// <summary>
        /// Measure the distance between locations and show it on the map in a box.
        /// </summary>
        public void MeasureDistance()
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
            Text_Distance.Text = ((int)totalDist).ToString() + " m";
        }

        /// <summary>
        /// Finds the distance between two points specified by its locations in meter.
        /// </summary>
        /// <param name="lat1">Latitude of first point</param>
        /// <param name="long1">Longatitude of first point</param>
        /// <param name="lat2">Latitude of second point</param>
        /// <param name="long2">Longatitude of second point</param>
        /// <returns></returns>
        public double FindDistance(double lat1, double long1, double lat2, double long2)
        {
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



        private void Btn_GetElevationData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                areaCreator();
                String points = string.Empty;

                points += areaPolygon.Locations[0].Latitude.ToString() + "," + areaPolygon.Locations[0].Longitude.ToString();
                points += ",";
                points += areaPolygon.Locations[3].Latitude.ToString() + "," + areaPolygon.Locations[3].Longitude.ToString();

                /*Location[] points = new Location[4];
                points[0] = areaPolygon.Locations[0];
                points[1] = areaPolygon.Locations[1];
                points[2] = areaPolygon.Locations[2];
                points[3] = areaPolygon.Locations[3];
                */
                //Elevation.GetElevation.asdf(points);


                Elevation.GetElevation.GetElevationPlane(points);

                Expander2.Height = 500;
                ElevationPlot.Content = new Page_Elevation();

            }
            catch (Exception exception)
            {
                string message = "Error: " + exception.Message.ToString();
                MessageBox.Show(message);
            }
        }

        private void getPointElevation_DoWork(object sender, DoWorkEventArgs e)
        {
            Location location = (Location)e.Argument;

            Application.Current.Dispatcher.Invoke((Action)delegate {
                double elevation = getPointElevation(location);
                Console.WriteLine(elevation);
                AddList_Alt(elevation);
            });
            
        }

        private double getPointElevation(Location location)
        {
            /*
             * Gets the altitude value of the location.
             */
            double lat = location.Latitude;
            double longa = location.Longitude;
            string point = lat.ToString() + "," + longa.ToString();
            double elevation = Elevation.GetElevation.GetElevationPoint(point);
            return elevation;
        }

        /// <summary>
        /// Creates an area that encapulates all waypoints.
        /// </summary>
        public void areaCreator()
        {
            /*
             * Creates area that encapsulates the waypoints and draws the area on the Map.
             */
            //Removes Existing Polygon:
            polygonLayer.Children.Remove(areaPolygon);

            double maxLonga = -180;
            double maxLat = -180;
            double minLonga = 180;
            double minLat = 180;

            foreach (var pin in MainWindow.pinArray)
            {
                if (minLonga > pin.Longitude)
                {
                    minLonga = pin.Longitude;
                }
                if (minLat > pin.Latitude)
                {
                    minLat = pin.Latitude;
                }
                if (maxLonga < pin.Longitude)
                {
                    maxLonga = pin.Longitude;
                }
                if (maxLat < pin.Latitude)
                {
                    maxLat = pin.Latitude;
                }
            }
            areaPolygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            areaPolygon.StrokeThickness = 3;

            Location[,] corners = new Location[2, 2];
            corners[0, 0] = new Location(minLat - 0.0002, maxLonga + 0.0002);
            corners[0, 1] = new Location(maxLat + 0.0002, maxLonga + 0.0002);
            corners[1, 0] = new Location(minLat - 0.0002, minLonga - 0.0002);
            corners[1, 1] = new Location(maxLat + 0.0002, minLonga - 0.0002);
            areaPolygon.Locations = new LocationCollection();
            areaPolygon.Locations.Add(corners[1, 0]);
            areaPolygon.Locations.Add(corners[0, 0]);
            areaPolygon.Locations.Add(corners[0, 1]);
            areaPolygon.Locations.Add(corners[1, 1]);

            polygonLayer.Children.Add(areaPolygon);
        }

        #endregion

        /// <summary>
        /// Stores the path plan in the local and also uploads to the vehicle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_UploadPathPlan_Click(object sender, RoutedEventArgs e)
        {
            string path = ((string)App.Current.Resources["PathDirectory"]) + "\\PathPlan.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            string mission_name = "Mission_" + missionNumber.ToString();
            int count = MainWindow.pinArray.Count();
            file.Write("<");
            file.Write("/{0}-{1}/", mission_name, count);
            int pos = 0;
            //for(int j = 0; j < MainWindow.pinArray.Count(); j++)
            //{
            //    MessageBox.Show(MainWindow.pinArray.ElementAt(j).Altitude.ToString());
            //}

            foreach (Location loc in MainWindow.pinArray)
            {
                double lat = Math.Round(loc.Latitude ,6);
                double lng = Math.Round(loc.Longitude,6);
                double alt = Math.Round(loc.Altitude ,6);
                double command = List_Command.Items.OfType<ComboBox>().ElementAt(pos).SelectedIndex;
                double wRadius = Convert.ToDouble(List_WRad.Items.OfType<StackPanel>().ElementAt(pos).Children.OfType<TextBox>().ElementAt(0).Text);
                //double command = MainWindow.pinArray.;
                file.Write("/{0}-{1}-{2}-{3}-{4}/", lat, lng, alt, command, wRadius);
                pos++;
            }
            if (IsReturnHome)
            {
                file.Write("/{0}-{1}-{2}-{3}/",
                            MainWindow.pinArray[0].Latitude, MainWindow.pinArray[0].Longitude, List_Command.Items.OfType<ComboBox>().ElementAt(0).SelectedIndex,
                            Convert.ToDouble(List_WRad.Items.OfType<StackPanel>().ElementAt(0).Children.OfType<TextBox>().ElementAt(0).Text));
            }
            //Close .txt File
            file.Write(">");
            file.Close();
            MessageBox.Show("Path plan is saved.");
            missionNumber++;

            //Send waypoint messages to the UGV
            msReader.sendNavigationPoints();

        }

        /// <summary>
        /// Search pre-created path plan and retrives it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_BrowsePathPlan_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                record_filename = dlg.FileName;
                Btn_BrowsePathPlan.ToolTip = record_filename;
                MessageBox.Show(record_filename, "", MessageBoxButton.OK, MessageBoxImage.Information);

                App.Current.Resources["PathDirectory"] = record_filename.Replace("\\PathPlan.txt", "");
            }
        }

        /// <summary>
        /// Checks whether user added return home action in to the path plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Checked_ReturnHome(object sender, RoutedEventArgs e)
        {
            IsReturnHome = true;
            ConnectPins();
        }

        /// <summary>
        /// Checks whether user did not added return home action in to the path plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnChecked_ReturnHome(object sender, RoutedEventArgs e)
        {
            IsReturnHome = false;
            ConnectPins();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ListBox && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private async Task UpdateRoute()
        {

            if (MainWindow.pinArray.Count >= 2)
            {
                Console.WriteLine("Route is calculating...");

                routeLayer.Children.Clear();

                var startLoc = MainWindow.pinArray.ElementAt(0);
                var endLoc = MainWindow.pinArray.Last();

                var startCoord = LocationToCoordinate(MainWindow.pinArray.ElementAt(0));
                var endCoord = LocationToCoordinate(MainWindow.pinArray.Last());

                //Calculate a route between the start and end pushpin.
                var response = await BingMapsRESTToolkit.ServiceManager.GetResponseAsync(new BingMapsRESTToolkit.RouteRequest()
                {
                    Waypoints = new List<BingMapsRESTToolkit.SimpleWaypoint>()
                    {
                        new BingMapsRESTToolkit.SimpleWaypoint(startCoord),
                        new BingMapsRESTToolkit.SimpleWaypoint(endCoord)
                    },
                    BingMapsKey = ("o2L9tTtDNKuDjJCIBc96~p_WU7aLpI2GKIK-3w3CLRQ~AgRJ4Hvl6yk3dt7HabLkO_ruoRFkV24c-a06bMsFxaB11DD94gvHQ9ZEOyk5NxLW").ToString(),
                    RouteOptions = new BingMapsRESTToolkit.RouteOptions()
                    {
                        RouteAttributes = new List<BingMapsRESTToolkit.RouteAttributeType>
                        {
                            //Be sure to return the route path information so that we can draw the route line.
                            BingMapsRESTToolkit.RouteAttributeType.RoutePath
                        }
                    }
                });

                if (response != null &&
                    response.ResourceSets != null &&
                    response.ResourceSets.Length > 0 &&
                    response.ResourceSets[0].Resources != null &&
                    response.ResourceSets[0].Resources.Length > 0)
                {
                    var route = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Route;

                    //Generate a Polyline from the route path information.
                    var locs = new LocationCollection();

                    for (var i = 0; i < route.RoutePath.Line.Coordinates.Length; i++)
                    {
                        locs.Add(new Location(route.RoutePath.Line.Coordinates[i][0], route.RoutePath.Line.Coordinates[i][1]));
                    }

                    //Delete exisiting polygon layer
                    polygonLayer.Children.Clear();
                    //Delete exisiting pins from pinArray and pin list
                    while(MainWindow.pinArray.Count!=0)
                    {
                        RemovePinIndex(0);
                        Console.WriteLine("pin deleted.");
                        Console.WriteLine(MainWindow.pinArray.Count);
                    }
                    //Add initial waypoints if they are not same as the end point of the route
                    if (locs.ElementAt(0) !=  startLoc)
                    {
                        locs.Insert(0, startLoc);
                    }
                    if (locs.Last() != endLoc)
                    {
                        locs.Insert(locs.Count(), endLoc);
                    }

                    //Add each location on the list
                    foreach (Location loc in locs)
                    {
                        //Creates a new Pushpin:
                        pin = new DraggablePin(BingMapLayout);
                    
                        //Set Altitude Value
                        loc.Altitude = 10;

                        //Set Location Value
                        pin.Location = loc;
                        //double alt = pinLocation.Altitude;

                        //Create DraggablePin Handler
                        pin.PinDragged += HandleDraggedPin;

                        //Adds Pin to Array
                        MainWindow.pinArray.Add(pin.Location);
                        //Add pin to the layer
                        pinLayer.Children.Add(pin);
                        //Add pin to the list
                        AddList(loc);

                        Console.WriteLine("pin added.");
                    }
                    ConnectPins();
                }
            }
        }

        private BingMapsRESTToolkit.Coordinate LocationToCoordinate(Location loc)
        {
            return new BingMapsRESTToolkit.Coordinate(loc.Latitude, loc.Longitude);
        }

        private void Btn_CreateRoute_Click(object sender, RoutedEventArgs e)
        {
            UpdateRoute();
            //UpdateRouteThread();
            //routeThread.DoWork += UpdateRoute_DoWork;
            //routeThread.RunWorkerAsync();
        }

        private void UpdateRouteThread()
        {
            try
            {
                /*ThreadStart poolref = new ThreadStart(UpdateRoute);
                //Create UpdateRoute thread
                Thread RoutingThread = new Thread(poolref);
                //Start the thread
                RoutingThread.Start();
                Console.WriteLine("routing...");*/
            }
            catch (Exception) { }
        }


        private void UpdateRoute_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                UpdateRoute();
                Console.WriteLine("routing...");

            });

        }

        private void Checked_Loop(object sender, RoutedEventArgs e)
        {

        }


        private void Unchecked_Loop(object sender, RoutedEventArgs e)
        {

        }
    }
}

