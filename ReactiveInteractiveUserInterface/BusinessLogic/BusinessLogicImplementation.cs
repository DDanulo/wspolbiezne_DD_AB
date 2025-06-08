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
using System.Security.AccessControl;
using System.Xml.Linq;
using TP.ConcurrentProgramming.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
    {
        #region ctor


        internal BusinessLogicImplementation(Dimensions dims, UnderneathLayerAPI? underneathLayer = null)
        {
            _dims = dims;

            layerBellow = underneathLayer ?? UnderneathLayerAPI.Create(dims);

        }

        #endregion ctor

        #region BusinessLogicAbstractAPI

        public override void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
            layerBellow.Dispose();
            _log.Dispose();
            Stop();
            Disposed = true;
        }

        public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));
            layerBellow.Start(numberOfBalls, (startVec, rawBall) =>
            {
                lock (_sync) _balls.Add(rawBall);
                
                upperLayerHandler(new Position(startVec.x, startVec.y), new Ball(rawBall));         
            });
            
            _physicsTimer = new Timer(DoPhysics, null,
                                      TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
        }   
        public void Stop()
        {
            _physicsTimer?.Dispose();
        }

        

        private void DoPhysics(object? _)
        {
            lock (_sync)
            {
                ResolveBorderCollisions();
                ResolveBallCollisions();
                
            }
        }

        private void ResolveBorderCollisions()
        {
            double r = _dims.BallDiameter / 2.0;

            foreach (var b in _balls)
            {
                var p = b.Position;
                var v = b.Velocity;

                if (p.x - r < 0) { p = new Vector(r, p.y); v = new Vector(Math.Abs(v.x), v.y); }
                if (p.x + r > _dims.TableWidth) { p = new Vector(_dims.TableWidth - r, p.y); v = new Vector(-Math.Abs(v.x), v.y); }

                if (p.y - r < 0) { p = new Vector(p.x, r); v = new Vector(v.x, Math.Abs(v.y)); }
                if (p.y + r > _dims.TableHeight) { p = new Vector(p.x, _dims.TableHeight - r); v = new Vector(v.x, -Math.Abs(v.y)); }

                b.Position = p;
                b.Velocity = v;
            }
        }

        private void ResolveBallCollisions()
        {
            double r2 = _dims.BallDiameter;              
            double r2Sq = r2 * r2;

            for (int i = 0; i < _balls.Count - 1; ++i)
                for (int j = i + 1; j < _balls.Count; ++j)
                {
                    var a = _balls[i];
                    var b = _balls[j];

                    var dx = b.Position.x - a.Position.x;
                    var dy = b.Position.y - a.Position.y;
                    var distSq = dx * dx + dy * dy;

                    if (distSq >= r2Sq) continue;
                    
                    _log.Log(i, _balls[i].Position, _balls[i].Velocity);
                    _log.Log(j, _balls[j].Position, _balls[j].Velocity);


                    var dist = Math.Sqrt(distSq);
                    if (dist == 0) dist = 0.01;               
                    var overlap = 0.5 * (r2 - dist);
                    var nx = dx / dist; var ny = dy / dist;
                    a.Position = new Vector(a.Position.x - nx * overlap, a.Position.y - ny * overlap);
                    b.Position = new Vector(b.Position.x + nx * overlap, b.Position.y + ny * overlap);

                    var va = a.Velocity;
                    var vb = b.Velocity;

                    var van = va.x * nx + va.y * ny;
                    var vbn = vb.x * nx + vb.y * ny;

                    var vaNew = new Vector(va.x + (vbn - van) * nx,
                                           va.y + (vbn - van) * ny);
                    var vbNew = new Vector(vb.x + (van - vbn) * nx,
                                           vb.y + (van - vbn) * ny);

                    a.Velocity = vaNew;
                    b.Velocity = vbNew;
                }
        }
        #endregion BusinessLogicAbstractAPI

        #region private
        
        private bool Disposed = false;
        private readonly Dimensions _dims;
        private readonly UnderneathLayerAPI layerBellow;
        private readonly List<Data.IBall> _balls = new();
        private readonly object _sync = new();
        private readonly DiagnosticsLogger _log = new(Path.Combine(AppContext.BaseDirectory, $"{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffffffZ")}_balls.log"));
        private Timer _colourTimer;
        private double _hue;
        private readonly record struct Vector(double x, double y) : IVector;

        private Timer? _physicsTimer;
        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}