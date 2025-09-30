using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT13VotingAppFinal
{
    public partial class VoterDashboardcs : Form
    {
        public VoterDashboardcs()
        {
            InitializeComponent();

            // Make the form consistent
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);

            this.Load += VoterDashboardcs_Load;
            this.Resize += VoterDashboardcs_Resize;
        }

        private void VoterDashboardcs_Resize(object sender, EventArgs e)
        {
            CenterControls(); // recenter whenever form resizes
        }


        private void VoterDashboardcs_Load(object sender, EventArgs e)
        {
            SetupBackground();
            StyleControls();
            CenterControls();

            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
        }

        private void SetupBackground()
        {
            // Make the PictureBox fill the whole form
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // Send the background behind all other controls
            pictureBox1.SendToBack();
        }

        private void StyleControls()
        {
            // Title
            label1.Text = "Voter Dashboard";
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.AutoSize = true;

            // Common button style
            foreach (Button btn in new[] { btnVoting, btnResults, btnLogout })
            {
                btn.Width = 120;
                btn.Height = 40;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
            }

            // Specific colors
            btnVoting.BackColor = Color.FromArgb(0, 123, 255);   // Blue
            btnResults.BackColor = Color.FromArgb(108, 117, 125); // Gray
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);    // Red
        }

        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            // Title positioning
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 60;

            // Buttons positioning
            int buttonsTop = 200;
            int totalWidth = (btnVoting.Width + btnResults.Width + btnLogout.Width) + 40; // spacing
            int buttonsLeft = centerX - (totalWidth / 2);

            btnVoting.Left = buttonsLeft;
            btnVoting.Top = buttonsTop;

            btnResults.Left = btnVoting.Right + 20;
            btnResults.Top = buttonsTop;

            btnLogout.Left = btnResults.Right + 20;
            btnLogout.Top = buttonsTop;
        }

        private void btnVoting_Click(object sender, EventArgs e)
        {
            var votingForm = new VotingForm(this);
            votingForm.Show();
            this.Hide();
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            var resultsForm = new ResultsForm();
            resultsForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
