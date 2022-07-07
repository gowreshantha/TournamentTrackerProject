using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;

namespace TrackerLib.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        TeamModel CreateTeam(TeamModel model);
        void CreateTournament(TournamentModel model);
        List<PersonModel> GetAllPersons();
        List<TeamModel> GetAllTeams();
    }
}
