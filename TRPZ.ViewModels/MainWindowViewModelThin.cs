using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Helpers;
using System.Windows;
using System.Windows.Media;
using TRPZ.DAL;
using TRPZ.ViewModels.Annotations;

namespace TRPZ.ViewModels
{
    public class MainWindowViewModelThin : INotifyPropertyChanged
    {
        private MySimpleRepository repository;
        private System.Timers.Timer timer;
        private SolidColorBrush color;
        private string statusMessage;
        private ObservableCollection<Holdings> holdings;

        public MainWindowViewModelThin()
        {
            this.ConnectButtonCommand = new RelayCommand(this.ConnectButtonCommandExecute);
            this.timer = new Timer();
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                this.OnPropertyChanged("BackgroundColor");
            }
        }

        public string StatusMessage
        {
            get
            {
                return this.statusMessage;
            }
            set
            {
                this.statusMessage = value;
                this.OnPropertyChanged("StatusMessage");
            }
        }

        public ObservableCollection<Holdings> Holdings
        {
            get
            {
                return this.holdings;
            }
            set
            {
                this.holdings = value;
                this.BackgroundColor = new SolidColorBrush(Colors.Green);
                this.OnPropertyChanged("Holdings");
            }
        }

        private async Task InitViewModel()
        {
            this.timer.Interval = 2000;
            this.StatusMessage = "Connecting...";
            this.BackgroundColor = new SolidColorBrush(Colors.Orange);
            this.timer.Start();
            this.timer.Elapsed += OnTimerElapsed;
            var result = await new HttpClient().GetStringAsync("http://localhost:59220/Home/GetRequest");
            this.Holdings = (ObservableCollection<Holdings>)Json.Decode(result, typeof(ObservableCollection<Holdings>));
        }

     

        void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.Holdings == null)
            {
                this.StatusMessage = "Failed to connect.";
            }
            else
            {
                this.StatusMessage = "Connection succeful.";             
            }       
        }

        public void ConnectButtonCommandExecute()
        {
            this.InitViewModel();
        }

        public void HelpButtonCommandExecute()
        {
           
        }
        public RelayCommand HelpButtonCommand { get; set; }
        public RelayCommand ConnectButtonCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
