//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation(Dimensions dims)
        {
            _dims = dims;
            _colourTimer = new Timer(UpdateColours,
                             null,
                             TimeSpan.Zero,
                             TimeSpan.FromMilliseconds(50));
        }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));
            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector startingPosition = new(random.Next(100, 400 - 100), random.Next(100, 400 - 100));
                Ball newBall = new(startingPosition, new(random.NextDouble() * 200 - 100, random.NextDouble() * 200 - 100));
                upperLayerHandler(startingPosition, newBall);
                BallsList.Add(newBall);
            }
            isStarted = true;
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    //MoveTimer.Dispose();
                    //BallsList.Clear();
                    foreach (Ball b in BallsList) b.Dispose();
                    BallsList.Clear();
                }
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(nameof(DataImplementation));
        }

        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        private void UpdateColours(object? _)
        {
            if (!isStarted) return;
            const double speedDegPerSec = 45.0;        // full rainbow ≈8 s
            _hue = (_hue + speedDegPerSec * 0.05) % 360;
            Rgb c = HueToRgb(_hue);

            lock (_lock)                               // same lock used for BallsList
            {
                foreach (Ball b in BallsList)          // <- List<Data.Ball>
                    b.Colour = c;                      // raises NewColourNotification
            }
        }

        private static Rgb HueToRgb(double hueDeg)
        {
            // keep hue in [0,360)
            hueDeg = (hueDeg % 360 + 360) % 360;

            double h = hueDeg / 60.0;           
            double c = 1.0;                     
            double x = 1 - Math.Abs(h % 2 - 1);
            double r1 = 0, g1 = 0, b1 = 0;       

            if (h < 1) { r1 = c; g1 = x; b1 = 0; }
            else if (h < 2) { r1 = x; g1 = c; b1 = 0; }
            else if (h < 3) { r1 = 0; g1 = c; b1 = x; }
            else if (h < 4) { r1 = 0; g1 = x; b1 = c; }
            else if (h < 5) { r1 = x; g1 = 0; b1 = c; }
            else { r1 = c; g1 = 0; b1 = x; }

            return new Rgb((byte)(r1 * 255),
                           (byte)(g1 * 255),
                           (byte)(b1 * 255));
        }

        private bool Disposed = false;
        Dimensions _dims;
        private Random RandomGenerator = new();
        private List<Ball> BallsList = [];
        private readonly Timer _colourTimer;
        private double _hue;                 // 0-360°
        private readonly object _lock = new();
        private bool isStarted = false;

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
        {
            returnBallsList(BallsList);
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
        {
            returnNumberOfBalls(BallsList.Count);
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}