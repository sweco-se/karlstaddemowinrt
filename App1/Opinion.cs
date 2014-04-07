using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App1
{
    public class Opinion
       : ViewModelBase
    {
        public Guid Id { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private string _desc;
        public string Description
        {
            get { return _desc; }
            set { _desc = value; RaisePropertyChanged(() => Description); }
        }

        private double _x;
        public double X
        {
            get { return _x; }
            set { _x = value; RaisePropertyChanged(() => X); }
        }

        private double _y;
        public double Y
        {
            get { return _y; }
            set { _y = value; RaisePropertyChanged(() => Y); }
        }

        public ICommand RemoveCommad { get; set; }

        public Opinion()
        {
            Id = Guid.NewGuid();
        }
    }
}
