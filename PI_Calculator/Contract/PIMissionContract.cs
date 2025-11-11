using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_Calculator.Contract
{
    public class PIMissionContract
    {
        public interface IPIView
        {
            void AddMissionResponse(PIModel result);
            void RefreshUI(string time);
            void ShowAlert();
        };
        public interface IPIPresenter
        {
            /// <summary>
            /// 啟動執行緒任務，不斷接收來自 <see href="ConcurrentQueue"/> 請求的任務，並在背景執行計算
            /// </summary>
            void StartMission();

            /// <summary>
            /// 傳入指定的 <see cref="sampleSize"/> 發一起一個計算PI的任務
            /// </summary>
            /// <param name="sampleSize"></param>
            void SendMissionRequest(int sampleSize);

            /// <summary>
            /// 取得已經完成PI計算任務的結果
            /// </summary>
            void FetchCompleteMissions();

            /// <summary>
            /// 暫停執行緒任務，停止接收來自 <see href="ConcurrentQueue"/> 請求的任務
            /// </summary>
            void StoptMission();
        };
    }
}
