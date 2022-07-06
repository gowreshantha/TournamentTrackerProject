using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLib.Models;

namespace TrackerUI
{
    //Loosely coupled 
    //Note: They dont about each other but they work together
    //for ex CreateTournament and CreatePrize Forms
    public interface IPrizeRequester
    {
        void PrizeComplete(PrizeModel prizeModel);
    }
}
