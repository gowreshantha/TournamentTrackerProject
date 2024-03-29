﻿using System;
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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetAllPersons();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
            WireUpLists();
        }

        /// <summary>
        /// To Load the data into Team Select Team Member dropdown & Team Member list box
        /// </summary>
        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            teamMembersListBox.DataSource = null;

            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel person = new PersonModel(
                    firstNameValue.Text,
                    lastNameValue.Text,
                    emailValue.Text,
                    cellphoneValue.Text);

                person = GlobalConfig.Connection.CreatePerson(person);

                selectedTeamMembers.Add(person);
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("This form has invalid information, Please check and it and try again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;

            if(firstNameValue.Text.Length == 0)
            {
                output = false;
            }
            if(lastNameValue.Text.Length == 0)
            {
                output = false;
            }
            if(emailValue.Text.Length == 0)
            {
                output = false;
            }
            if(cellphoneValue.Text.Length == 0)
            {
                output = false;
            }
            return output;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if(person != null)
            {
                selectedTeamMembers.Add(person);
                availableTeamMembers.Remove(person);
                WireUpLists();
            }

        }

        private void deleteSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)teamMembersListBox.SelectedItem;

            if(person != null)
            {
                availableTeamMembers.Add(person);
                selectedTeamMembers.Remove(person);
                WireUpLists();
            }
            else if(teamMembersListBox.Items.Count == 0)
            {
                MessageBox.Show("No Members in the List to Delete");
            }
            else
            {
                MessageBox.Show("Please select a Member to Delete");
            }

        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if(teamNameValue.Text.Length != 0 && teamMembersListBox.Items.Count > 0)
            {
                TeamModel team = new TeamModel(teamNameValue.Text, selectedTeamMembers);
                GlobalConfig.Connection.CreateTeam(team);
                callingForm.TournamentComplete(team);
                this.Close();

            }
            else if(teamNameValue.Text.Length == 0)
            {
                MessageBox.Show("Team Name cannot be Empty");
            }
            else
            {
                MessageBox.Show("Team Members cannot be Empty");
            }
        }

    }
}
