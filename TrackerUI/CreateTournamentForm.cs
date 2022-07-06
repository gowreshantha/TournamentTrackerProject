using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form , IPrizeRequester, ITeamRequester
    {
        private List<TeamModel> availableTeams = GlobalConfig.Connection.GetAllTeams();
        private List<TeamModel> selectedTeams = new List<TeamModel>();
        private List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamListBox.DataSource = null;
            tournamentTeamListBox.DataSource = selectedTeams;
            tournamentTeamListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";

        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)selectTeamDropDown.SelectedItem;
            if (team != null)
            {
                selectedTeams.Add(team);
                availableTeams.Remove(team);
                WireUpLists();
            }

        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel teamModel = (TeamModel)tournamentTeamListBox.SelectedItem;
            if(teamModel != null)
            {
                availableTeams.Add(teamModel);
                selectedTeams.Remove(teamModel);
                WireUpLists();
            }
            else if (tournamentTeamListBox.Items.Count == 0)
            {
                MessageBox.Show("No Teams in the List to Delete");
            }
            else
            {
                MessageBox.Show("Please select a Team to Delete");
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            //Call the Create Prize form
            CreatePrizeForm createPrizeForm = new CreatePrizeForm(this);
            createPrizeForm.ShowDialog();
        }

        //Get back the PrizeModel from the Prize form
        public void PrizeComplete(PrizeModel prizeModel)
        {
            selectedPrizes.Add(prizeModel);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm createTeamForm = new CreateTeamForm(this);
            createTeamForm.ShowDialog();
        }

        public void TournamentComplete(TeamModel teamModel)
        {
            selectedTeams.Add(teamModel);
            WireUpLists();
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {

            PrizeModel prizeModel = (PrizeModel)prizesListBox.SelectedItem;
            if (prizeModel != null)
            {
                selectedPrizes.Remove(prizeModel);
                WireUpLists();
            }
            else if (prizesListBox.Items.Count == 0)
            {
                MessageBox.Show("No Prizes in the List to Delete");
            }
            else
            {
                MessageBox.Show("Please select a Prize to Delete");
            }
        }
    }
}
