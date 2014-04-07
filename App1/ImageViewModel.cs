using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace App1
{
    public class ImageViewModel : ViewModelBase
    {
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                RaisePropertyChanged(() => ImageSource);
            }
        }

        public ObservableCollection<Opinion> Opinions { get; set; }

        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new RelayCommand<Guid>((id) => { Opinions.Remove(Opinions.FirstOrDefault(o => o.Id == id)); }));
            }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand<Point>((point) => Opinions.Add(new Opinion { X = point.X, Y = point.Y, RemoveCommad = RemoveCommand })); }
        }

        public ICommand SaveCommad
        {
            get 
            {
                return new RelayCommand(async() => {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("Sparad");
                    await messageDialog.ShowAsync();
                }); 
            }
        }

        public ImageViewModel()
            : this(new Uri("ms-appx:///Assets/wallpaper-711691.jpg", UriKind.Absolute))
        {
        }

        public ImageViewModel(Uri uri)
        {
            ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(uri);
            Opinions = new ObservableCollection<Opinion>();
        }
    }
}
