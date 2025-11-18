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
        public ObservableCollection<PIModel> Models { get; set; }

        public ICommand AddItemCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand CalcelCommand { get; set; }

        public int SampleSize { get; set; }

        private Timer Timer { get; set; }

        private IPIPresenter pIPresenter { get; set; }

        public bool IsStopped { get; set; } = true;

        [DependsOn(nameof(IsStopped))]
        public string StopBtnContent => IsStopped ? "Stop" : "Restart";


        public PIViewModel()
        {
            Models = new ObservableCollection<PIModel>();
            pIPresenter = new PIPresenter(this);
            pIPresenter.StartMission();
            Timer = new Timer(state => Application.Current.Dispatcher.Invoke(() => pIPresenter.FetchCompleteMissions()), null, 0, 1000);
            AddItemCommand = new RelayCommand<int>(pIPresenter.SendMissionRequest);
            StopCommand = new RelayCommand(() =>
            {
                if (IsStopped) { pIPresenter.StartMission(); IsStopped = false; }
                else { pIPresenter.StopMission(); }
            });
            DeleteCommand = new RelayCommand<PIModel>(DeleteMissionResponse);
            CalcelCommand = new RelayCommand<PIModel>(CancelMissionResponse);
        }


        public void AddMissionResponse(PIModel result)
        {
            var model = Models.FirstOrDefault(x => x.SampleSize == result.SampleSize && x.IsCalcelEnabled == false);
            if (model != null) { model.Time = result.Time; model.Value = result.Value; }
            else { Models.Add(result); }
        }

        public void DeleteMissionResponse(PIModel result)
        {
            Models.Remove(result);
            pIPresenter.DeleteSampleSize(result.SampleSize);
        }

        public void CancelMissionResponse(PIModel result)
        {
            result.CancellationTokenSource.Cancel();
            result.IsCalcelEnabled = false;
            pIPresenter.DeleteSampleSize(result.SampleSize);
        }

        public void ShowAlert()
        {
            MessageBox.Show("Sample Size已存在");
        }

        public void ChangeEnable()
        {
            IsStopped = !IsStopped;
        }
    }

}
