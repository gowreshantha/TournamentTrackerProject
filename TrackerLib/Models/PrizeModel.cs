﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    /// <summary>
    /// Represents what the prize is for the given place.
    /// </summary>
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The numeric identifier for the place 
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// The friendly name for the place 
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// The fixed amount this place earns or zero if it is not used
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// The number that represents the percentage of the overall take or
        /// zero if it is not used. The percentage is a fraction of 1 (so 0.5 for 50%)
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeNumber , string placeName, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;
            PlaceNumber = int.Parse(placeNumber);
            PrizeAmount = decimal.Parse(prizeAmount);
            PrizePercentage = double.Parse(prizePercentage);

        }

        public PrizeModel(string id, string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            Id = int.Parse(id);
            PlaceName = placeName;
            PlaceNumber = int.Parse(placeNumber);
            PrizeAmount = decimal.Parse(prizeAmount);
            PrizePercentage = double.Parse(prizePercentage);

        }
    }
}
