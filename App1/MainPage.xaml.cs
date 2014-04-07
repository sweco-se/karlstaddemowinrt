using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var point = e.GetPosition(canv);
            var el = new Ellipse
            {
                Width = 25,
                Height = 25,
                Stroke = new SolidColorBrush(Colors.Pink),
                StrokeThickness = 1,
                Fill = new SolidColorBrush(Colors.Green),
                DataContext = new Opinion { Title = "Test", Description="Beskrivnign"}
            };
            el.Tapped += el_Tapped;
            canv.Children.Add(el);
            el.SetValue(Canvas.LeftProperty, point.X -12.5);
            el.SetValue(Canvas.TopProperty, point.Y - 12.5);
        }

        void el_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            var fe = ((FrameworkElement)sender);
            create.DataContext = fe.DataContext;
            create.SetValue(Canvas.LeftProperty, fe.GetValue(Canvas.LeftProperty));
            create.SetValue(Canvas.TopProperty, fe.GetValue(Canvas.TopProperty));
            create.IsOpen = true;
        }
    }

    public class Opinion 
        : ViewModelBase
    {
        public Guid Id { get; set; }
        
        private string _title;
        
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }

        private string _desc;

        public string Description
        {
            get { return _desc; }
            set { _desc = value; RaisePropertyChanged("Description"); }
        }

        public ICommand RemoveCommad 
        {
            get 
            {
                return new RelayCommand<Canvas>((canvas) => {
                    var ellipse = canvas.Children.FirstOrDefault(c => c is Ellipse && ((Opinion)((Ellipse)c).DataContext).Id == Id);
                    var popup = (Popup)canvas.Children.FirstOrDefault(c => c is Popup);
                    canvas.Children.Remove(ellipse);
                    popup.IsOpen = false;
                }); 
            }
        }


        public Opinion()
        {
            Id = Guid.NewGuid();
        }
    }
}
