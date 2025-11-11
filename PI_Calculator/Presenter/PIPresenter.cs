using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using static PI_Calculator.Contract.PIMissionContract;

namespace PI_Calculator.Presenter
{
    internal class PIPresenter : IPIPresenter
    {
        public IPIView View { get; set; }
        public ConcurrentDictionary<int, PIModel> pIModels { get; set; } = new ConcurrentDictionary<int, PIModel>();
        public ConcurrentQueue<int> queue { get; set; } = new ConcurrentQueue<int>();
        public ConcurrentBag<PIModel> cache { get; set; } = new ConcurrentBag<PIModel>();
        public PIPresenter(IPIView pIView)
        {
            View = pIView;
        }
        public async void SendMissionRequest(int sampleSize)
        {
            if (pIModels.ContainsKey(sampleSize))
            {
                View.ShowAlert();
                return;
            }
            PIModel pIModel = new PIModel();
            pIModel.SampleSize = sampleSize;
            pIModels.TryAdd(sampleSize, pIModel);
            queue.Enqueue(sampleSize);
        }


        public void StartMission()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (queue.TryDequeue(out int sampleSize))
                    {
                        PIMission pIMission = new PIMission(sampleSize);
                        var result = await pIMission.Calculate();
                        var model = pIModels.GetValueOrDefault(sampleSize);
                        if (model != null)
                        {
                            model.Time = result.Item1;
                            model.Value = result.Item2;
                            cache.Add(model);
                        }
                    }
                }
            });
        }

        public void FetchCompleteMissions()
        {
            while (cache.Count > 0)
            {
                if (cache.TryTake(out PIModel model))
                    View.AddMissionResponse(model);
            }
        }

        public void StoptMission()
        {
            throw new NotImplementedException();
        }
    }
}
