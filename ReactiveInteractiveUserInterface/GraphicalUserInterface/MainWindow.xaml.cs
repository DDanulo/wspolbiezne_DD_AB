//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Windows;
using TP.ConcurrentProgramming.BusinessLogic;
using TP.ConcurrentProgramming.Data;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView
{
    /// <summary>
    /// View implementation
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double w = BoardBorder.ActualWidth - BoardBorder.BorderThickness.Left;   
            double h = BoardBorder.ActualHeight - BoardBorder.BorderThickness.Right;  
            double ballD = 20.0;                  

            var dims = new Dimensions(ballD, h, w);
            var logic = BusinessLogicAbstractAPI.CreateBusinessLogicLayer(dims);
            var model = ModelAbstractApi.CreateModel(dims);
            DataContext = new MainWindowViewModel(model);
            Loaded -= MainWindow_Loaded;          
        }

        /// <summary>
        /// Raises the <seealso cref="System.Windows.Window.Closed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                viewModel.Dispose();
            base.OnClosed(e);
        }
    }
}