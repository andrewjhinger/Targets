using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace Targets
{
    public class TargetGame
    {
        // Private delegates
        private Target.TargetDrawDelegate _targetDraw;

        // Private class variables
        private ArrayList _targets = new ArrayList();
        private Random _random = new Random();
        private int _defaultTargetSize = 40;

        // Private class backing properties
        private Size _boardSize;
        private int _numberOfTargets = 0;

        // Public properties
        public Size BoardSize { set { _boardSize = value; } }
        public int NumberOfTargets { get { return _numberOfTargets; } set { _numberOfTargets = value; } }

        // Constructor
        public TargetGame(Size boardSize, int numberOfTargets = 1)
        {
            // Initialize game settings
            _boardSize = boardSize;
            _numberOfTargets = numberOfTargets;
        }

        // Private methods
        private void AddTarget()
        {
            // Create target
            Target target = new Target(
                location: new Point(_random.Next(_boardSize.Width - _defaultTargetSize),
                    _random.Next(_boardSize.Height - _defaultTargetSize)),
                dimensions: new Size(_defaultTargetSize, _defaultTargetSize),
                fillColor: Color.Blue,
                textColor: Color.White,
                startingCountDown: 10);


            // Add an event handler to CountExpired event for each target
            target.CountExpired += new EventHandler(CountExpiredEventHandler);

            // Add target drawing method to our delegate list using shortcut syntax
            _targetDraw += target.Draw;

            // Add target
            _targets.Add(target);
        }

        private void RemoveTarget(Target target)
        {
            if (target != null)
            {
                // Remove event delegate handler for CountExpired event for this target
                target.CountExpired -= this.CountExpiredEventHandler;

                // Remove draw delegate from our delegate list
                _targetDraw -= target.Draw;

                // Remove target from ArrayList
                _targets.Remove(target);
            }
        }

        // Protected event handlers
        protected virtual void CountExpiredEventHandler(object sender, EventArgs e)
        {
            // Remove target
            Target target = sender as Target;
            if (target != null) RemoveTarget(target);
        }


        // Public events and delegate
        public event EventHandler Hit;

        public void Select(Point location)
        {
            // Loop through each balloon in ArrayList to see which was selected
            foreach (Target target in _targets)
            {
                if (target.Hit(location))
                {
                    OnHit(target, EventArgs.Empty);
                    RemoveTarget(target);
                    break;
                }
            }
        }

        public void Reset(int numberOfTargets = 1)
        {
            // Reset targets
            _numberOfTargets = numberOfTargets;
            while (_targets.Count > 0) RemoveTarget(_targets[0] as Target);
        }

        // Protected event handlers
        protected virtual void OnHit(object sender, EventArgs e)
        {
            if (Hit != null) Hit(sender, e);
        }

        // Public methods
        public void Update(Graphics graphics)
        {
            // Add targets
            if (_targets.Count == 0)
                while (_targets.Count < _numberOfTargets)
                    AddTarget();

            // Call delegate to redraw all current targets
            _targetDraw(_boardSize, graphics);
        }
    }
}