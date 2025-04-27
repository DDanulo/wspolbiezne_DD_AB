//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Collections.ObjectModel;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel.MVVMLight;
using ModelIBall = TP.ConcurrentProgramming.Presentation.Model.IBall;

namespace TP.ConcurrentProgramming.Presentation.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        #region ctor

        public MainWindowViewModel(ModelAbstractApi modelLayerAPI)
        {
            ModelLayer = modelLayerAPI ?? throw new ArgumentNullException(nameof(modelLayerAPI));
            Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x));
            StartCommand = new RelayCommand(
                execute: StartFromText,
                canExecute: () => TryParsePositiveInt(BallCountText, out _)
            );
        }

        #endregion ctor

        #region public API

        public void Start(int numberOfBalls)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            ModelLayer.Start(numberOfBalls);
            Observer.Dispose();
        }

        public ObservableCollection<ModelIBall> Balls { get; } = new ObservableCollection<ModelIBall>();

        private string _ballCountText = "5";
        public string BallCountText
        {
            get => _ballCountText;
            set
            {
                if (Set(ref _ballCountText, value))
                {
                    StartCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand StartCommand { get; }
        #endregion public API

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Balls.Clear();
                    Observer.Dispose();
                    ModelLayer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Disposed = true;
            }
        }

        public void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region Helpers
        private void StartFromText()
        {
            if (TryParsePositiveInt(BallCountText, out int n))
                Start(n);
        }

        private static bool TryParsePositiveInt(string? s, out int n) =>
            int.TryParse(s, out n) && n > 0;


        #endregion Helpers

        #region private

        private IDisposable Observer = null;
        private ModelAbstractApi ModelLayer;
        private bool Disposed = false;

        #endregion private
    }
}