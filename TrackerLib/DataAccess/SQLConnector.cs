using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TrackerLib.Models;

namespace TrackerLib.DataAccess
{
    public class SQLConnector : IDataConnection
    {
        private string connectionString = GlobalConfig.GetConnectonString("Tournaments");

        /// <summary>
        /// Saves a new person to the database
        /// </summary>
        /// <param name="model">The person information</param>
        /// <returns>The person information, including the unique identifier</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@@Email", model.Email);
                p.Add("@@CellPhone", model.CellPhone);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {        
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");

                foreach(PersonModel person in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonID", person.Id);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
               SaveTournament(connection, model);
               SaveTournamentEntries(connection, model);
               SaveTournamentPrizes(connection, model);
               SaveTournamentRounds(connection, model);
            }
        }

        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            model.Id = p.Get<int>("@id");
        }

        private void SaveTournamentEntries(IDbConnection connection, TournamentModel model)
        {
            foreach (TeamModel team in model.EnteredTeams)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", team.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }

        }

        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel prizeModel in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", prizeModel.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);

            }
        }

        private void SaveTournamentRounds(IDbConnection connection, TournamentModel model)
        {
            //List<List<MatchupModel>> Rounds 
            //List<MatchupEntryModel> Entries

            //Loop through the rounds
            //Loop through the matchups
            //Save the matchup
            //Loop through the entries and save them

            foreach(List<MatchupModel> round in model.Rounds)
            {
                foreach(MatchupModel matchup in round )
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    matchup.Id = p.Get<int>("@id");

                    foreach(MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId ", matchup.Id);
                       
                        if(entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else
                        {
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }
                        if(entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }

        }

        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> models = null;
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                models = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return models;
        }

        public List<TeamModel> GetAllTeams()
        {
            List<TeamModel> models = null;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                models = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();
                foreach (TeamModel team in models)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return models;
        }
    }
}
