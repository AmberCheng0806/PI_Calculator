using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PI_Calculator
{
    public class PIViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PIModel> Models { get; set; } = new ObservableCollection<PIModel>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddItemCommand { get; set; }

        public int SampleSize { get; set; }

        public PIViewModel()
        {
            AddItemCommand = new RelayCommand<int>(AddMission);
        }
        private async void AddMission(int input)
        {
            if (Models.Any(x => x.SampleSize == SampleSize))
            {
                return;
            }
            PIModel pIModel = new PIModel();
            pIModel.SampleSize = input;
            PIMission pIMission = new PIMission(SampleSize);
            var result = await pIMission.Calculate();
            pIModel.Time = result.Item1;
            pIModel.Value = result.Item2;
            Models.Add(pIModel);
        }
    }

}
