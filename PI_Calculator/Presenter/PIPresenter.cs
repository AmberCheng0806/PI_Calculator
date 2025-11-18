using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
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

        public SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(0);

        public AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

        public CancellationTokenSource CancellationTokenSource { get; set; }

        //private Channel<int> channel = Channel.CreateUnbounded<int>();


        public PIPresenter(IPIView pIView)
        {
            View = pIView;

        }
        public void SendMissionRequest(int sampleSize)
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
            View.AddMissionResponse(pIModel);
            SemaphoreSlim.Release();
            //AutoResetEvent.Set();
            //channel.Writer.TryWrite(sampleSize);
        }


        public Task StartMission()
        {
            CancellationTokenSource = new CancellationTokenSource();

            return Task.Run(() =>
             {
                 while (!CancellationTokenSource.Token.IsCancellationRequested)
                 {
                     //AutoResetEvent.WaitOne()
                     SemaphoreSlim.Wait();
                     if (queue.TryDequeue(out int sampleSize))
                     {
                         Task.Run(async () =>
                         {
                             var model = pIModels.GetValueOrDefault(sampleSize);
                             PIMission pIMission = new PIMission(sampleSize, model.CancellationTokenSource.Token);
                             var result = await pIMission.Calculate();
                             if (model != null)
                             {
                                 model.Time = result.Item1;
                                 model.Value = result.Item2;
                                 model.IsCalcelEnabled = false;
                                 cache.Add(model);
                             }
                         });

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

        public void StopMission()
        {
            View.ChangeEnable();
            CancellationTokenSource.Cancel();
        }

        public void DeleteSampleSize(int SampleSize)
        {
            pIModels.TryRemove(SampleSize, out _);
        }
    }
}
