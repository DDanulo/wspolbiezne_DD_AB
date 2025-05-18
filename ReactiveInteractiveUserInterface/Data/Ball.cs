//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    public class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, int tickMs = 10)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
            _tickMs = Math.Max(1, tickMs);
            _thread = new Thread(Run) { IsBackground = true, Name = $"Ball-{Guid.NewGuid()}" };
            _thread.Start();
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        public IVector Position { set; get; }
        #endregion IBall

        #region private
        private readonly Thread _thread;
        private volatile bool _alive = true;   // thread-life flag
        private readonly int _tickMs;         // sleep per step (e.g. 10 ms)

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }
        private void Run()
        {
            var sw = Stopwatch.StartNew();
            while (_alive)
            {
                double dt = sw.Elapsed.TotalSeconds;
                sw.Restart();
                Move(new Vector(Velocity.x * dt, Velocity.y * dt));
                Thread.Sleep(_tickMs);
            }
        }
        internal void Move(Vector delta) //, Dimensions dims)
        {
            Position = new Vector(Position.x + delta.x, Position.y + delta.y);
            RaiseNewPositionChangeNotification();
        }
        public void Dispose()          // let DataImplementation call this
        {
            _alive = false;
            _thread.Join();
        }
        #endregion private
    }
}