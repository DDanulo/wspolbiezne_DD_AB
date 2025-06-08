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
            //Colour = new Rgb(0, 0, 255);
            _tickMs = Math.Max(1, tickMs);
            _thread = new Thread(Run) { IsBackground = true, Name = $"Ball-{Guid.NewGuid()}" };
            _thread.Start();
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;
        public event EventHandler<Rgb>? NewColourNotification;

        public IVector Velocity { get; set; }

        public IVector Position { set; get; }
        #endregion IBall

        #region private
        private readonly Thread _thread;
        private volatile bool _alive = true;   // thread-life flag
        private readonly int _tickMs;         // sleep per step (e.g. 10 ms)
                                              //public record struct Rgb(byte R, byte G, byte B);   // tiny value-object
                                              // Data/Ball.cs
        private Rgb _colour = new(0, 0, 255);
        public Rgb Colour
        {
            get => _colour;
            set
            {
                if (_colour.Equals(value)) return;         // ignore duplicates
                _colour = value;
                NewColourNotification?.Invoke(this, _colour);
            }
        }

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