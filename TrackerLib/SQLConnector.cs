using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib
{
    public class SQLConnector : IDataConnection
    {
        // TODO - Implement method to save
        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;
            return model;
        }
    }
}
