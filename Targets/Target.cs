using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Targets
{
    public class Target
    {
        // Public events and delegate
        public event EventHandler CountExpired;
        public delegate void TargetDrawDelegate(Size boardSize, Graphics graphics);

        // Private class variables
        private Point _location;
        private Size _dimensions;
        private Color _fillColor;
        private Color _textColor;
        private int _countDown;
        private int _lastTickCount;

        // Constructor
        public Target(Point location, Size dimensions, Color fillColor, Color textColor, int startingCountDown = 10)
        {
            _location = location;
            _dimensions = dimensions;
            _fillColor = fillColor;
            _textColor = textColor;
            _countDown = startingCountDown;
            _lastTickCount = System.Environment.TickCount;
        }

        // Public methods
        public void Draw(Size boardSize, Graphics graphics)
        {
            // See if countdown has expired
            if (_countDown > 0)
            {
                if (System.Environment.TickCount - _lastTickCount > 1000)
                {
                    _countDown--;
                    _lastTickCount = System.Environment.TickCount;
                }

                // Draw target
                using (SolidBrush brush = new SolidBrush(_fillColor))
                    graphics.FillEllipse(brush, new Rectangle(_location, _dimensions));

                // Draw countdown
                RectangleF boundingRectangle = new RectangleF(_location.X, _location.Y, _dimensions.Width, _dimensions.Height);
                using (Font font = new Font("Arial", 12, FontStyle.Bold))
                using (StringFormat stringFormat = new StringFormat())
                using (SolidBrush brush = new SolidBrush(_textColor))
                {
                    // Align text horizontally and vertically
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    graphics.DrawString(_countDown.ToString(), font, brush, boundingRectangle, stringFormat);
                }
            }
            else
                // Raise event
                OnCountExpired(EventArgs.Empty);
        }

        public bool Hit(Point location)
        {
            return new Rectangle(_location, _dimensions).Contains(location);
        }

        // Protected event handlers
        protected virtual void OnCountExpired(EventArgs e)
        {
            if (CountExpired != null) CountExpired(this, e);
        }
    }
}