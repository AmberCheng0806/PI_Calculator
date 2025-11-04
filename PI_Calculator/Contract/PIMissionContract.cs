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
        };
        public interface IPIPresenter
        {
            void AddMissionRequest(int sampleSize);
        };
    }
}
