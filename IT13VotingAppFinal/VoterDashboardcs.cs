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
        private System.Windows.Forms.Timer resultsCheckTimer;
        

        private int voterID;
        public VoterDashboardcs(int voterId)
        {
            InitializeComponent();
            voterID = voterId;

            // Make the form consistent
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);

            this.Load += VoterDashboardcs_Load;
            this.Resize += VoterDashboardcs_Resize;

            resultsCheckTimer = new System.Windows.Forms.Timer();
            resultsCheckTimer.Interval = 1000; // Check every second for real-time countdown
            resultsCheckTimer.Tick += ResultsCheckTimer_Tick;
        }

        private void ResultsCheckTimer_Tick(object sender, EventArgs e)
        {
            UpdateResultsButtonState();
        }

        private void UpdateResultsButtonState()
        {
            bool isAvailable = AreResultsAvailable();

            // Button is always enabled now
            btnResults.Enabled = true;

            try
            {
                DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_AreResultsAvailable");
                
                if (!isAvailable)
                {
                    btnResults.BackColor = Color.FromArgb(255, 193, 7); // Orange/Yellow color
                    btnResults.ForeColor = Color.Black;
                    btnResults.Text = "Results\n(Click for Info)";
                    
                    // Update info label
                    if (dt.Rows.Count > 0 && dt.Rows[0]["ResultsAvailableTime"] != DBNull.Value)
                    {
                        DateTime resultsTime = Convert.ToDateTime(dt.Rows[0]["ResultsAvailableTime"]);
                        TimeSpan timeRemaining = resultsTime - DateTime.Now;
                        
                        if (timeRemaining.TotalSeconds > 0)
                        {
                            lblResultsInfo.Text = $"Results available in: {timeRemaining.Days}d {timeRemaining.Hours}h {timeRemaining.Minutes}m";
                            lblResultsInfo.ForeColor = Color.Orange;
                        }
                        else
                        {
                            lblResultsInfo.Text = "Results should be available now - click to refresh";
                            lblResultsInfo.ForeColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        lblResultsInfo.Text = "Results availability time not set";
                        lblResultsInfo.ForeColor = Color.Red;
                    }
                }
                else
                {
                    btnResults.BackColor = Color.FromArgb(40, 167, 69); // Green color
                    btnResults.ForeColor = Color.White;
                    btnResults.Text = "View Results";
                    
                    lblResultsInfo.Text = "Results are now available! Click to view.";
                    lblResultsInfo.ForeColor = Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                lblResultsInfo.Text = "Error checking results status";
                lblResultsInfo.ForeColor = Color.Red;
            }
        }

        private void VoterDashboardcs_Resize(object sender, EventArgs e)
        {
            CenterControls(); // recenter whenever form resizes

        }

        private void UpdateResultsInfoPosition()
        {
            lblResultsInfo.AutoSize = false;
            lblResultsInfo.Dock = DockStyle.Bottom; // 🔥 This locks it to the bottom
            lblResultsInfo.Height = 35; // fixed height for the footer
            lblResultsInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblResultsInfo.BringToFront();
        }


        private void VoterDashboardcs_Load(object sender, EventArgs e)
        {
            SetupBackground();
            StyleControls();
            CenterControls();
            // Check results availability on load
            UpdateResultsButtonState();
            // Start the timer
            resultsCheckTimer.Start();

            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            
            // Set up results info label
            lblResultsInfo.Parent = pictureBox1;
            lblResultsInfo.BackColor = Color.FromArgb(128, 0, 0, 0); // Semi-transparent black background
            lblResultsInfo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblResultsInfo.BringToFront();

            

            lblResultsInfo.AutoSize = false;
            lblResultsInfo.Height = 35;
            lblResultsInfo.Dock = DockStyle.Bottom;
            lblResultsInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblResultsInfo.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            UpdateResultsInfoPosition();

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
            label1.ForeColor = Color.White;
            label1.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            label1.AutoSize = true;
            label1.Parent = pictureBox1;

            // Common button style
            foreach (Button btn in new[] { btnVoting, btnResults, btnLogout })
            {
                btn.Width = 140;
                btn.Height = 45;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;

                // Hover effects
                btn.MouseEnter += (s, e) =>
                {
                    btn.BackColor = ControlPaint.Light(btn.BackColor);
                };
                btn.MouseLeave += (s, e) =>
                {
                    if (btn == btnVoting)
                        btn.BackColor = Color.FromArgb(0, 123, 255);   // Blue
                    else if (btn == btnResults)
                    {
                  
                        UpdateResultsButtonState();
                    }
                    else if (btn == btnLogout)
                        btn.BackColor = Color.FromArgb(220, 53, 69);    // Red
                };
            }

            // Specific colors
            btnVoting.BackColor = Color.FromArgb(0, 123, 255);   // Blue
            btnResults.BackColor = Color.FromArgb(108, 117, 125); // Gray
            btnResults.TextAlign = ContentAlignment.MiddleCenter;
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
            var votingForm = new VotingForm(this, voterID);
            votingForm.Show();
            this.Hide();
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            if (!AreResultsAvailable())
            {
                // Get the results availability time to show when results will be available
                try
                {
                    DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_AreResultsAvailable");
                    
                    string message = "Results are not yet available.";
                    
                    if (dt.Rows.Count > 0 && dt.Rows[0]["ResultsAvailableTime"] != DBNull.Value)
                    {
                        DateTime resultsTime = Convert.ToDateTime(dt.Rows[0]["ResultsAvailableTime"]);
                        TimeSpan timeRemaining = resultsTime - DateTime.Now;
                        
                        if (timeRemaining.TotalSeconds > 0)
                        {
                            message = $"Results are not yet available.\n\n" +
                                     $"Results will be available on:\n" +
                                     $"{resultsTime:MMMM dd, yyyy 'at' h:mm tt}\n\n" +
                                     $"Time remaining: {timeRemaining.Days} days, {timeRemaining.Hours} hours, {timeRemaining.Minutes} minutes";
                        }
                        else
                        {
                            message = "Results should be available now. Please try refreshing or contact the administrator.";
                        }
                    }
                    else
                    {
                        message = "Results are not yet available.\n\nNo specific availability time has been set. Please check back later or contact the administrator.";
                    }
                    
                    MessageBox.Show(message,
                                   "Results Not Available",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Results are not yet available.\n\nError checking availability: " + ex.Message,
                                   "Results Not Available",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                }
                return;
            }

            // Results are available, show the results form
            var resultsForm = new ResultsForm("Voter", this);
            resultsForm.Show();
            this.Hide();
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

        private bool AreResultsAvailable()
        {
            try
            {
                DataTable dt = DataAccess.ExecuteProcedureReader("sp_AreResultsAvailable");

                if (dt.Rows.Count > 0)
                {
                    bool isAvailable = Convert.ToBoolean(dt.Rows[0]["IsAvailable"]);
                    return isAvailable;
                }

                return false; // Default: Results not available
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking results availability: {ex.Message}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
