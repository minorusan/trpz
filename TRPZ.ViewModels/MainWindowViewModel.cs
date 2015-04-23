using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TRPZ.DAL;
using TRPZ.ViewModels.Annotations;

namespace TRPZ.ViewModels
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private MySimpleRepository repository;
        private ObservableCollection<Holdings> holdings;

        public MainWindowViewModel()
        {
            this.repository = new MySimpleRepository();
            this.InitViewModel();
        }

        public ObservableCollection<Holdings> Holdings {
            get
            {
                return this.holdings;
            }
            set
            {
                this.holdings = value;
                this.OnPropertyChanged("Holdings");
            }
        }

        private void InitViewModel()
        {
            this.holdings = new ObservableCollection<Holdings>(this.repository.GetHoldings());
            this.holdings.ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
