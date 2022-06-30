using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        public TeamModel()
        {

        }
        public TeamModel(string teamName, List<PersonModel> teamMembers)
        {
            TeamName = teamName;
            TeamMembers = teamMembers;
        }
        public TeamModel(string id, string teamName, List<PersonModel> teamMembers)
        {
            Id = int.Parse(id);
            TeamName = teamName;
            TeamMembers = teamMembers;
        }
    }
}
