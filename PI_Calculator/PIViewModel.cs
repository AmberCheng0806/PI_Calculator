using PI_Calculator.Presenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PI_Calculator.Contract.PIMissionContract;

namespace PI_Calculator
{
    public class PIViewModel : INotifyPropertyChanged, IPIView
    {
        public ObservableCollection<PIModel> Models { get; set; } = new ObservableCollection<PIModel>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddItemCommand { get; set; }

        public int SampleSize { get; set; }

        private IPIPresenter pIPresenter { get; set; }
        public PIViewModel()
        {
            pIPresenter = new PIPresenter(this);
            AddItemCommand = new RelayCommand<int>(pIPresenter.AddMissionRequest);
        }
        //private async void AddMission(int input)
        //{
        //    if (Models.Any(x => x.SampleSize == SampleSize))
        //    {
        //        return;
        //    }
        //    PIModel pIModel = new PIModel();
        //    pIModel.SampleSize = input;
        //    PIMission pIMission = new PIMission(SampleSize);
        //    var result = await pIMission.Calculate();
        //    pIModel.Time = result.Item1;
        //    pIModel.Value = result.Item2;
        //    Models.Add(pIModel);
        //}

        public void AddMissionResponse(PIModel result)
        {
            Models.Add(result);
        }
    }

}
