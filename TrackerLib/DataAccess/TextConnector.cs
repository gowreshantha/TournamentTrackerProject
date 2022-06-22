using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;
using TrackerLib.DataAccess.TextHelper;
using System.Linq;

namespace TrackerLib.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizeFile = "PrizeModels.csv";

        // TODO - Implement method to save
        /// <summary>
        /// Saves a new prize to the text file
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {

            //Load the Text File
            //COnvert the text to List<PrizeModel>
            List<PrizeModel> prizes = PrizeFile.GetFullFilePath().LoadFile().ConvertToPrizeModels();

            //Find the max ID
            int currentId = 1;
            if(prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            //Add the new record with the new ID (ma + 1)
            prizes.Add(model);

            //Convert the prizes to List<string>
            //Save the List<string> to the text file
            prizes.SaveToPrizeFile(PrizeFile);

            return model;
        }
    }
}
