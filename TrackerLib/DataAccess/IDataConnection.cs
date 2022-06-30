﻿using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;

namespace TrackerLib.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetAllPersons();
        TeamModel CreateTeam(TeamModel model);
    }
}
