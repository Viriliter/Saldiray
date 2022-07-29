using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FornoxGUI;

namespace DraggablePushpin
{
    public class DraggablePin : Pushpin
    {
        private Map _map;
        private bool isDragging = false;
        Location _center;
        Location oldLocation = new Location(0,0);

        public DraggablePin(Map map)
        {
            _map = map;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_map != null)
            {
                _center = _map.Center;
                _map.ViewChangeOnFrame += _map_ViewChangeOnFrame;
                _map.MouseUp += ParentMap_MouseLeftButtonUp;
                _map.MouseMove += ParentMap_MouseMove;
                _map.TouchMove += _map_TouchMove;
                oldLocation = this.Location;
            }

            // Enable Dragging
            this.isDragging = true;

            base.OnMouseLeftButtonDown(e);
        }

        protected void UpdatePoint(Location mouseGeocode)
        {

            //Update pin location
            oldLocation = mouseGeocode;
        }

        protected void changePinArray()
        {
            bool a = false;
            int pos;

            for (pos = 0; pos < MainWindow.pinArray.Count; pos++)
            {
                Location temp = MainWindow.pinArray.ElementAt(pos);
                Console.WriteLine(this.Location.Latitude);
                Console.WriteLine(temp.Latitude);

                if (temp.Latitude == oldLocation.Latitude)
                {
                    MainWindow.pinArray.ElementAt(pos).Latitude = this.Location.Latitude;
                    MainWindow.pinArray.ElementAt(pos).Longitude = this.Location.Longitude;
                    break;
                }
            }
        }

        void _map_TouchMove(object sender, TouchEventArgs e)
        {
            var map = sender as Microsoft.Maps.MapControl.WPF.Map;
            // Check if the user is currently dragging the Pushpin
            if (this.isDragging)
            {
                // If so, the Move the Pushpin to where the Mouse is.
                var mouseMapPosition = e.GetTouchPoint(map);
                var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition.Position);
                this.Location = mouseGeocode;
                changePinArray();
            }
        }

        void _map_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            if (isDragging)
            {
                _map.Center = _center;
            }
        }

        #region "Mouse Event Handler Methods"

        void ParentMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Left Mouse Button released, stop dragging the Pushpin
            if (_map != null)
            {
                _map.ViewChangeOnFrame -= _map_ViewChangeOnFrame;
                _map.MouseUp -= ParentMap_MouseLeftButtonUp;
                _map.MouseMove -= ParentMap_MouseMove;
                _map.TouchMove -= _map_TouchMove;

            }
            changePinArray();
            this.isDragging = false;
            //Fire pinDragged event so that new polygon is drawn
            PinDragged(this, null);
        }

        void ParentMap_MouseMove(object sender, MouseEventArgs e)
        {
            var map = sender as Microsoft.Maps.MapControl.WPF.Map;
            // Check if the user is currently dragging the Pushpin
            if (this.isDragging)
            {
                // If so, the Move the Pushpin to where the Mouse is.
                var mouseMapPosition = e.GetPosition(map);
                var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition);
                this.Location = mouseGeocode;
            }
        }
        #endregion

        //Handler to signal that pin location is changed when left mouse button is released.
        public event EventHandler PinDragged;

    }
}
