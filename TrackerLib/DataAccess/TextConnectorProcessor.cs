using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLib.Models;

//Load the Text File
//COnvert the text to List<PrizeModel>
//Find the max ID
//Add the new record with the new ID (ma + 1)
//Convert the prizes to List<string>
//Save the List<string> to the text file

namespace TrackerLib.DataAccess.TextHelper
{
    public static class TextConnectorProcessor
    {
        public static string GetFullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if(!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> prizeModels = new List<PrizeModel>();
            foreach(var line in lines)
            {
                string[] row = line.Split(',');
                PrizeModel prizeModel = new PrizeModel(row[0], row[1], row[2], row[3], row[4]);
                prizeModels.Add(prizeModel);
            }
            return prizeModels;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> personModels = new List<PersonModel>();
            foreach (var line in lines)
            {
                string[] row = line.Split(',');
                PersonModel personModel = new PersonModel(row[0], row[1], row[2], row[3], row[4]);
                personModels.Add(personModel);
            }
            return personModels;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string personFile)
        {
            List<TeamModel> teamModels = new List<TeamModel>();
            List<PersonModel> persons = personFile.GetFullFilePath().LoadFile().ConvertToPersonModels();
            foreach (var line in lines)
            {
                string[] row = line.Split(',');
                TeamModel teamModel = new TeamModel();

                teamModel.Id = int.Parse(row[0]);
                teamModel.TeamName = row[1];

                string[] personIds = row[2].Split('|');
                foreach(var id in personIds)
                {
                    teamModel.TeamMembers.Add(persons.Where(x => x.Id == int.Parse(id)).First());
                }
                teamModels.Add(teamModel);
            }
            return teamModels;
        }

        public static List<TournamentModel> ConvertToTournamentModel(this List<string> lines, string personFile, string teamFile, string prizeFile)
        {
            List<TournamentModel> tournamentModels = new List<TournamentModel>();
            List<TeamModel> teamModels = teamFile.GetFullFilePath().LoadFile().ConvertToTeamModels(personFile);
            List<PrizeModel> prizeModels = prizeFile.GetFullFilePath().LoadFile().ConvertToPrizeModels();
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.GetFullFilePath().LoadFile().ConvertToMatchupModels();

            foreach (var line in lines)
            {
                string[] row = line.Split(',');
                TournamentModel tournamentModel = new TournamentModel();

                tournamentModel.Id = int.Parse(row[0]);
                tournamentModel.TournamentName = row[1];
                tournamentModel.EntryFee = int.Parse(row[2]);

                string[] teamIds = row[3].Split('|');
                foreach(var id in teamIds)
                {
                    tournamentModel.EnteredTeams.Add(teamModels.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] prizeIds = row[4].Split('|');
                foreach(var id in prizeIds)
                {
                    tournamentModel.Prizes.Add(prizeModels.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] rounds = row[5].Split('|');
                List<MatchupModel> ms = new List<MatchupModel>();
                foreach(string round in rounds)
                {
                    string[] msText = round.Split('^');
                    foreach(string matchupModelTextId in msText)
                    {
                        ms.Add(matchups.Where(x => x.Id == int.Parse(matchupModelTextId)).First());
                    }
                    tournamentModel.Rounds.Add(ms);
                }

                tournamentModels.Add(tournamentModel);

            }
            return tournamentModels;
        }



        public static void SaveToPrizeFile(this List<PrizeModel> prizes, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(PrizeModel prizeModel in prizes)
            {
                lines.Add($"{prizeModel.Id},{prizeModel.PlaceNumber},{ prizeModel.PlaceName},{prizeModel.PrizeAmount},{prizeModel.PrizePercentage}");
            }
            File.WriteAllLines(fileName.GetFullFilePath(), lines);
        }

        public static void SaveToPersonFile(this List<PersonModel> persons, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(PersonModel personModel in persons)
            {
                lines.Add($"{personModel.Id},{personModel.FirstName},{personModel.LastName},{personModel.Email},{personModel.CellPhone}");
            }
            File.WriteAllLines(fileName.GetFullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> teams, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(TeamModel teamModel in teams)
            {
                lines.Add($"{teamModel.Id},{teamModel.TeamName},{ConvertPeronsListToIDs(teamModel.TeamMembers)}");
            }
            File.WriteAllLines(fileName.GetFullFilePath(), lines);
        }

        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            //Loop through each Round
            //Loop through each Matchup
            //Get the id for the new mathcup and save the record
            //Loop through each Entry, get the id, and save it

            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel matchup in round)
                {
                    //Load all of the matchups from file
                    //Get the top id and add one
                    //Store the id
                    //Save the matchup record
                    matchup.SaveMatchupToFile(matchupFile, matchupEntryFile);            
                }
            }
        }

        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            List<MatchupEntryModel> matchupEntryModels = new List<MatchupEntryModel>();
            foreach (var line in lines)
            {
                string[] row = line.Split(',');
                MatchupEntryModel matchupEntryModel = new MatchupEntryModel();
                matchupEntryModel.Id = int.Parse(row[0]);
                if (row[1].Length == 0)
                {
                    matchupEntryModel.TeamCompeting = null;
                }
                else
                {
                    matchupEntryModel.TeamCompeting = LookupTeamById(int.Parse(row[1]));
                }
                matchupEntryModel.Score = double.Parse(row[2]);

                int parentId = 0;
                if (int.TryParse(row[3], out parentId))
                {
                    matchupEntryModel.ParentMatchup = LookupMatchupById(parentId);
                }
                else
                {
                    matchupEntryModel.ParentMatchup = null;
                }
                

                matchupEntryModels.Add(matchupEntryModel);
            }
            return matchupEntryModels;
        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.GetFullFilePath().LoadFile().ConvertToMatchupEntryModels();
            foreach (string id in ids)
            {
                output.Add(entries.Where(x => x.Id == int.Parse(id)).First());
            }
            return output;
        }

        private static TeamModel LookupTeamById(int id)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.GetFullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PersonFile);
            return teams.Where(x => x.Id == id).First();
        }

        private static MatchupModel LookupMatchupById(int id)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.GetFullFilePath().LoadFile().ConvertToMatchupModels();
            return matchups.Where(x => x.Id == id).First();
        }
        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            List<MatchupModel> matchupModels = new List<MatchupModel>();
            foreach (var line in lines)
            {
                string[] row = line.Split(',');
                MatchupModel matchupModel = new MatchupModel();
                matchupModel.Id = int.Parse(row[0]);
                matchupModel.Entries = ConvertStringToMatchupEntryModels(row[1]);
                matchupModel.Winner = LookupTeamById(int.Parse(row[2]));
                matchupModel.MatchupRound = int.Parse(row[3]);
                matchupModels.Add(matchupModel);
            }
            return matchupModels;
        }

        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.GetFullFilePath().LoadFile().ConvertToMatchupModels();
            int currentId = 1;
            if(matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            matchup.Id = currentId;

            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }

            //save to file
            List<string> lines = new List<string>();
            foreach(MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }

                lines.Add($"{m.Id},{ConvertMatchupEnrtyListToIDs(m.Entries)},{winner},{m.MatchupRound}");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.GetFullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntryModel entry, string matchupEntryFile)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.GetFullFilePath().LoadFile().ConvertToMatchupEntryModels();
            int currentId = 1;
            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            entry.Id = currentId;
            entries.Add(entry);

            List<string> lines = new List<string>();
            foreach (MatchupEntryModel e in entries)
            {
                string parent = "";
                if(e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if(e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{e.Id},{teamCompeting},{e.Score},{parent}");
            }
            File.WriteAllLines(GlobalConfig.MatchupEntryFile.GetFullFilePath(), lines);
        }
        public static void SaveToTournamentFile(this List<TournamentModel> tournaments, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(TournamentModel tournamentModel in tournaments)
            {
                lines.Add($@"{tournamentModel.Id},
                             {tournamentModel.TournamentName},
                             {tournamentModel.EntryFee},
                             {ConvertTeamsListToIDs(tournamentModel.EnteredTeams)},
                             {ConvertPrizesListToIDs(tournamentModel.Prizes)},
                             {ConvertRoundListToIDs(tournamentModel.Rounds)}");
            }
            File.WriteAllLines(fileName.GetFullFilePath(), lines);
        }

        private static string ConvertPeronsListToIDs(List<PersonModel> model)
        {
            string output = "";
            if(model.Count == 0)
            {
                return "";
            }
            foreach(PersonModel person in model)
            {
                output += $"{person.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertTeamsListToIDs(List<TeamModel> models)
        {
            string output = "";
            if(models.Count == 0)
            {
                return "";
            }
            foreach(TeamModel team in models)
            {
                output += $"{team.Id}|";
            }

            output = output.Substring(0, output.Length -1);
            return output;
        }
        private static string ConvertMatchupEnrtyListToIDs(List<MatchupEntryModel> models)
        {
            string output = "";
            if (models.Count == 0)
            {
                return "";
            }
            foreach (MatchupEntryModel matchupEntryModel in models)
            {
                output += $"{matchupEntryModel.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;

        }

        private static string ConvertPrizesListToIDs(List<PrizeModel> models)
        {
            string output = "";
            if (models.Count == 0)
            {
                return "";
            }
            foreach (PrizeModel prize in models)
            {
                output += $"{prize.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;

        }

        private static string ConvertRoundListToIDs(List<List<MatchupModel>> Rounds)
        {
            // Rounds - id^id^id|id^id^id|id^id^id
            string output = "";
            if (Rounds.Count == 0)
            {
                return "";
            }
            foreach (List<MatchupModel> m in Rounds)
            {
                output += $"{ConvertMatchupListToString(m)}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;

        }

        private static string ConvertMatchupListToString(List<MatchupModel> models)
        {
            string output = "";
            if (models.Count == 0)
            {
                return "";
            }
            foreach (MatchupModel matchupModel in models)
            {
                output += $"{matchupModel.Id}^";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
