using PI_Calculator.Presenter;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static PI_Calculator.Contract.PIMissionContract;

namespace PI_Calculator
{
    [AddINotifyPropertyChangedInterface]
    public class PIViewModel : IPIView
    {
        public ObservableCollection<PIModel>? Models { get; set; }

        public ICommand? AddItemCommand { get; set; }

        public int SampleSize { get; set; }

        private Timer? Timer { get; set; }

        private IPIPresenter? pIPresenter { get; set; }

        public string? Time { get; set; }
        public PIViewModel()
        {
            Models = new ObservableCollection<PIModel>();
            pIPresenter = new PIPresenter(this);
            pIPresenter.StartMission();
            Timer = new Timer(state => Application.Current.Dispatcher.Invoke(() => pIPresenter.FetchCompleteMissions()), null, 0, 1000);
            AddItemCommand = new RelayCommand<int>(pIPresenter.SendMissionRequest);
        }


        public void AddMissionResponse(PIModel result)
        {
            Models.Add(result);
        }

        public void RefreshUI(string time)
        {
            Time = time;
        }

        public void ShowAlert()
        {
            MessageBox.Show("Sample Size已存在");
        }
    }

}
