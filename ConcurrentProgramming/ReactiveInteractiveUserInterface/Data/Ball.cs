//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        public Vector Position { private set; get; }
        #endregion IBall

        #region private


        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Move(Vector delta, Dimensions dims)
        {
            double r = dims.BallDiameter / 2.0;

            double newX = Position.x + delta.x;
            double newY = Position.y + delta.y;

            newX = Math.Max(Math.Min(newX, dims.TableWidth - r), r);
            newY = Math.Max(Math.Min(newY, dims.TableHeight - r), r);
            Position = new Vector(newX, newY);
            RaiseNewPositionChangeNotification();
        }

        #endregion private
    }
}