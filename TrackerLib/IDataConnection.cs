using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
    }
}
