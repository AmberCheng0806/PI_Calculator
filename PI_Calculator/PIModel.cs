using PI_Calculator.Presenter;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PI_Calculator.Contract.PIMissionContract;

namespace PI_Calculator
{
    [AddINotifyPropertyChangedInterface]
    public class PIModel
    {
        public int SampleSize { get; set; }
        public long Time { get; set; }
        public double Value { get; set; }

        public bool IsCalcelEnabled { get; set; } = true;

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        public PIModel()
        {
        }
    }

}
