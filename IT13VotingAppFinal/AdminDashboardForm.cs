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
    public partial class AdminDashboardForm : Form
    {
        public AdminDashboardForm()
        {
            InitializeComponent();
            btnVoters.Click += btnVoters_Click;
            btnCandidates.Click += btnCandidates_Click;
            btnResults.Click += btnResults_Click;
            btnLogout.Click += btnLogout_Click;
            this.Load += AdminDashboardForm_Load;
            this.Resize += AdminDashboardForm_Resize;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btnVoters_Click(object sender, EventArgs e)
        {
            var vf = new VoterForm();
            vf.ShowDialog();
        }
        private void btnCandidates_Click(object sender, EventArgs e)
        {
            var candidateForm = new CandidateForm();
            candidateForm.ShowDialog();
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

        private void btnVoting_Click_1(object sender, EventArgs e)
        {

        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {  // === FORM SETTINGS ===
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);

            // === BACKGROUND ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();


            label1.Text = "Admin Dashboard";
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.ForeColor = Color.LightBlue;
            label1.BackColor = Color.Transparent;
            label1.AutoSize = true;

            // Important: set the parent to PictureBox so it inherits transparency
            label1.Parent = pictureBox1;
            label1.BringToFront();

            // === BUTTONS (fixed left) ===
            StyleButton(btnVoters, "Voters", Color.MediumSeaGreen, Color.White);
            StyleButton(btnCandidates, "Candidates", Color.SteelBlue, Color.White);
            StyleButton(btnResults, "Results", Color.Orange, Color.White);
            StyleButton(btnLogout, "Logout", Color.IndianRed, Color.White);

            PositionControls();
        }
        private void PositionControls()
        {
            int paddingLeft = 20;
            int spacing = 20;
            int currentY = 120; // start below the title

            // Title at top center
            label1.Left = (this.ClientSize.Width / 2) - (label1.Width / 2);
            label1.Top = 30;

            // Buttons fixed left
            btnVoters.Location = new Point(paddingLeft, currentY);
            currentY += btnVoters.Height + spacing;

            btnCandidates.Location = new Point(paddingLeft, currentY);
            currentY += btnCandidates.Height + spacing;

            btnResults.Location = new Point(paddingLeft, currentY);
            currentY += btnResults.Height + spacing;

            btnLogout.Location = new Point(paddingLeft, currentY);
        }
        private void StyleButton(Button btn, string text, Color backColor, Color foreColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(150, 40);

            // Hover effect
            btn.MouseEnter += (s, e) => { btn.BackColor = ControlPaint.Dark(backColor); };
            btn.MouseLeave += (s, e) => { btn.BackColor = backColor; };

            // Fix to top-left, so it won’t move
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void AdminDashboardForm_Resize(object sender, EventArgs e)
        {
            label1.Left = (this.ClientSize.Width / 2) - (label1.Width / 2);
        }
        // === METHOD: Center controls dynamically ===
        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Title at top center
            label1.Location = new Point(centerX - (label1.Width / 2), 30);

            // Buttons stacked vertically, centered
            int buttonStartY = centerY - 100;
            btnVoters.Location = new Point(centerX - (btnVoters.Width / 2), buttonStartY);
            btnCandidates.Location = new Point(centerX - (btnCandidates.Width / 2), buttonStartY + 60);
            btnResults.Location = new Point(centerX - (btnResults.Width / 2), buttonStartY + 120);
            btnLogout.Location = new Point(centerX - (btnLogout.Width / 2), buttonStartY + 180);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
