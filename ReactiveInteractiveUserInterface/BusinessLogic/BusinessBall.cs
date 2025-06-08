//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using static TP.ConcurrentProgramming.Data.Ball;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal class Ball : IBall
    {
        private readonly Data.Ball _dal;
        public Ball(Data.IBall ball)
        {
            _dal = (Data.Ball)ball;
            _dal.NewPositionNotification += RaisePositionChangeEvent;
            _dal.NewColourNotification += (_, c) => ColourChangedNotification?.Invoke(this, c);
        }

        #region IBall


        public event EventHandler<IPosition>? NewPositionNotification;
        public event EventHandler<Data.Rgb> ColourChangedNotification;

        //public Rgb Colour => _dal.Colour;

        Data.Rgb IBall.Colour => _dal.Colour;
        #endregion IBall

        #region private

        private void RaisePositionChangeEvent(object? sender, Data.IVector e)
        {
            NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
        }

        #endregion private
    }
}