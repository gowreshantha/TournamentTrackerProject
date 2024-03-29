﻿using System;
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
        private const string PersonFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";
        private const string TournamentFile = "Tournament.csv";
        private const string MatchupFile = "MatchupModels.csv";
        private const string MatchupEntryFile = "MatchupEntryModels.csv";

        /// <summary>
        /// Saves a new person to the text file
        /// </summary>
        /// <param name="model">The person information</param>
        /// <returns>The person information, including the unique identifier</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            //Load the Text File and COnvert the text to List<PersonModel>
            List<PersonModel> persons = PersonFile.GetFullFilePath().LoadFile().ConvertToPersonModels();

            //Find the max ID and Add the new record with the new ID (max + 1)
            int currentId = 1;
            if(persons.Count > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            persons.Add(model);

            //Convert the peoples to List<string> and Save the List<string> to the text file
            persons.SaveToPersonFile(PersonFile);

            return model;
        }

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

        public TeamModel CreateTeam(TeamModel model)
        {
            //Load the Text File and Convert the text to List<TeamModel>
            List<TeamModel> teams = TeamFile.GetFullFilePath().LoadFile().ConvertToTeamModels(PersonFile);

            //Find the max ID
            int currentId = 1;
            if(teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            //Add the new record with the new
            teams.Add(model);

            //Convert the teams to List<string>
            //Save the List<string> to the text file
            teams.SaveToTeamFile(TeamFile);

            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            //Load the Text File and Convert the text to List<TeamModel>
            List<TournamentModel> tournaments = TournamentFile
                .GetFullFilePath()
                .LoadFile()
                .ConvertToTournamentModel(PersonFile, TeamFile, PrizeFile);

            //Find the max ID
            int currentId = 1;
            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            model.SaveRoundsToFile(MatchupFile, MatchupEntryFile);

            //Add the new record with the new
            tournaments.Add(model);

            //Convert the tournaments to List<string>
            //Save the List<string> to the text file
            tournaments.SaveToTournamentFile(TournamentFile);

        }

        public List<PersonModel> GetAllPersons()
        {
            //Load the Text File and Convert the text to List<PrizeModel>
            return PersonFile.GetFullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetAllTeams()
        {
            return TeamFile.GetFullFilePath().LoadFile().ConvertToTeamModels(PersonFile);
        }

        public List<TournamentModel> GetAllTournaments()
        {
            return TournamentFile
                .GetFullFilePath()
                .LoadFile()
                .ConvertToTournamentModel(PersonFile, TeamFile, PrizeFile);
        }
    }
}
