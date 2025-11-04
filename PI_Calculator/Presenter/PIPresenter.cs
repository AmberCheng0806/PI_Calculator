using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PI_Calculator.Contract.PIMissionContract;

namespace PI_Calculator.Presenter
{
    internal class PIPresenter : IPIPresenter
    {
        public IPIView View { get; set; }
        public List<PIModel> pIModels { get; set; } = new List<PIModel>();
        public PIPresenter(IPIView pIView)
        {
            View = pIView;
        }
        public async void AddMissionRequest(int sampleSize)
        {
            if (pIModels.Any(x => x.SampleSize == sampleSize))
            {
                return;
            }
            PIModel pIModel = new PIModel();
            pIModel.SampleSize = sampleSize;
            PIMission pIMission = new PIMission(sampleSize);
            var result = await pIMission.Calculate();
            pIModel.Time = result.Item1;
            pIModel.Value = result.Item2;
            pIModels.Add(pIModel);
            View.AddMissionResponse(pIModel);
        }
    }
}
