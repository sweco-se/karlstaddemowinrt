using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region AddCommand

        public ICommand AddCommad
        {
            get { return (ICommand)GetValue(AddCommadProperty); }
            set { SetValue(AddCommadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommadProperty =
            DependencyProperty.Register("AddCommad", typeof(ICommand), typeof(MainPage), new PropertyMetadata(null));
        
        #endregion AddCommand

        #region Opinitions

        private static NotifyCollectionChangedEventHandler CollectionChangedHandler;
        
        public ObservableCollection<Opinion> OpionCollection
        {
            get { return (ObservableCollection<Opinion>)GetValue(OpionCollectionProperty); }
            set { SetValue(OpionCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpionCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpionCollectionProperty =
            DependencyProperty.Register("OpionCollection", typeof(ObservableCollection<Opinion>), typeof(MainPage), new PropertyMetadata(null, OnOpionCollectionChanged));


        private static void OnOpionCollectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = (MainPage)sender;
            if (CollectionChangedHandler == null)
            {
                CollectionChangedHandler = (a, b) => {
                    switch (b.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (Opinion opinion in b.NewItems)
                            {
                                var el = new Ellipse
                                {
                                    Width = 25,
                                    Height = 25,
                                    Stroke = new SolidColorBrush(Colors.Pink),
                                    StrokeThickness = 1,
                                    Fill = new SolidColorBrush(Colors.Green),
                                    DataContext = opinion
                                };
                                el.Tapped += page.el_Tapped;
                                page.canv.Children.Add(el);
                                el.SetValue(Canvas.LeftProperty, opinion.X - 12.5);
                                el.SetValue(Canvas.TopProperty, opinion.Y - 12.5);
                            }
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            foreach (Opinion opinion in b.OldItems)
                            {
                                var ellipse = page.canv.Children.FirstOrDefault(c => c is Ellipse && ((Opinion)((Ellipse)c).DataContext).Id == opinion.Id);
                                var popup = (Popup)page.canv.Children.FirstOrDefault(c => c is Popup);
                                page.canv.Children.Remove(ellipse);
                                popup.IsOpen = false;
                            }
                            break;
                    }
            
                };
            }

            if (e.OldValue != null)
                ((INotifyCollectionChanged)e.OldValue).CollectionChanged += CollectionChangedHandler;
            ((INotifyCollectionChanged)e.NewValue).CollectionChanged += CollectionChangedHandler;
        }

        #endregion Opinitions

        public MainPage()
        {
            this.InitializeComponent();

            SetBinding(AddCommadProperty, new Binding
            {
                Path = new PropertyPath("AddCommand"),
                Mode = BindingMode.OneWay
            });

            SetBinding(OpionCollectionProperty, new Binding
            {
                Path = new PropertyPath("Opinions"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var popup = (Popup)canv.Children.FirstOrDefault(c => c is Popup);
            if (!popup.IsOpen)
            {
                var point = e.GetPosition(canv);
                if (AddCommad.CanExecute(point))
                    AddCommad.Execute(point);
            }
            else
            {
                popup.IsOpen = false;
            }
        }

        private void el_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            var fe = ((FrameworkElement)sender);
            create.DataContext = fe.DataContext;
            create.SetValue(Canvas.LeftProperty, fe.GetValue(Canvas.LeftProperty));
            create.SetValue(Canvas.TopProperty, fe.GetValue(Canvas.TopProperty));
            create.IsOpen = true;
        }
    }
}
