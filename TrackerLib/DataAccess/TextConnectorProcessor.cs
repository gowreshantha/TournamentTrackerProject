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

            foreach(var line in lines)
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

        public static void SaveToTournamentFile(this List<TournamentModel> tournaments, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(TournamentModel tournamentModel in tournaments)
            {
                lines.Add($"{tournamentModel.Id},{tournamentModel.TournamentName},{tournamentModel.EntryFee}," +
                    $"{ConvertTeamsListToIDs(tournamentModel.EnteredTeams)},{ConvertPrizesListToIDs(tournamentModel.Prizes)}");
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
    }
}
