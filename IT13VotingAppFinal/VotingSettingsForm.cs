using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace IT13VotingAppFinal
{
    public partial class VotingSettingsForm : Form
    {
        public VotingSettingsForm()
        {
            InitializeComponent();
            
        }

        private void VotingSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Voting Settings";
            this.BackColor = ColorTranslator.FromHtml("#F2F5FA");
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(550, 380);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // ✅ Ensure the PictureBox is at the back
            if (pictureBox1 != null)
            {
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.SendToBack();
            }

            // ✅ Now bring all other controls to front
            lblTitle.BringToFront();
            lblCurrentSetting.BringToFront();
            lblResultsTime.BringToFront();
            dtpResultsAvailable.BringToFront();
            btnSave.BringToFront();
            btnCancel.BringToFront();

            MakeLabelTransparent(lblTitle);
            MakeLabelTransparent(lblCurrentSetting);
            MakeLabelTransparent(lblResultsTime);


            LoadCurrentSettings();
        }

        private void MakeLabelTransparent(Label lbl)
        {
            lbl.BackColor = Color.Transparent;
            lbl.Parent = pictureBox1; // important: put label "on" the PictureBox
        }

        private void MakeRounded(Control ctrl, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, ctrl.Width - radius, 0);
            path.AddArc(new Rectangle(ctrl.Width - radius, 0, radius, radius), -90, 90);
            path.AddLine(ctrl.Width, radius, ctrl.Width, ctrl.Height - radius);
            path.AddArc(new Rectangle(ctrl.Width - radius, ctrl.Height - radius, radius, radius), 0, 90);
            path.AddLine(ctrl.Width - radius, ctrl.Height, radius, ctrl.Height);
            path.AddArc(new Rectangle(0, ctrl.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            ctrl.Region = new Region(path);
        }

        private void LoadCurrentSettings()
        {
            try
            {
                // Safety check: create table if missing
                EnsureVotingSettingsTable();

                DataTable dt = DataAccess.ExecuteProcedureToDataTable("sp_AreResultsAvailable");

                if (dt.Rows.Count > 0 && dt.Rows[0]["ResultsAvailableTime"] != DBNull.Value)
                {
                    DateTime currentTime = Convert.ToDateTime(dt.Rows[0]["ResultsAvailableTime"]);
                    dtpResultsAvailable.Value = currentTime;

                    bool isAvailable = Convert.ToBoolean(dt.Rows[0]["IsAvailable"]);

                    if (isAvailable)
                    {
                        lblCurrentSetting.Text = $"✓ Results are currently AVAILABLE (since {currentTime:MMM dd, yyyy h:mm tt})";
                        lblCurrentSetting.ForeColor = Color.LightGreen;
                    }
                    else
                    {
                        lblCurrentSetting.Text = $"⏱ Results will be available on: {currentTime:MMM dd, yyyy h:mm tt}";
                        lblCurrentSetting.ForeColor = Color.Gold;
                    }
                }
                else
                {
                    lblCurrentSetting.Text = "No availability time set yet.";
                    lblCurrentSetting.ForeColor = Color.LightGray;
                    dtpResultsAvailable.Value = DateTime.Now.AddHours(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureVotingSettingsTable()
        {
            try
            {
                string createTable = @"
                    CREATE TABLE IF NOT EXISTS voting_settings (
                        SettingID INT PRIMARY KEY AUTO_INCREMENT,
                        ResultsAvailableTime DATETIME NOT NULL DEFAULT NOW()
                    );
                ";
                DataAccess.ExecuteNonQuery(createTable);

                string insertRow = @"
                    INSERT INTO voting_settings (SettingID, ResultsAvailableTime)
                    SELECT 1, NOW() + INTERVAL 1 HOUR
                    WHERE NOT EXISTS (SELECT 1 FROM voting_settings WHERE SettingID = 1);
                ";
                DataAccess.ExecuteNonQuery(insertRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database structure check failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpResultsAvailable_ValueChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time validation or preview here
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedTime = dtpResultsAvailable.Value;

                if (selectedTime < DateTime.Now)
                {
                    var result = MessageBox.Show(
                        "The selected time is in the past. Results will be immediately available. Continue?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return;
                }

                string query = "UPDATE voting_settings SET ResultsAvailableTime = @ResultsTime WHERE SettingID = 1";
                int rowsAffected = DataAccess.ExecuteNonQuery(query,
                    new MySqlParameter("@ResultsTime", selectedTime));

                if (rowsAffected > 0)
                {
                    MessageBox.Show(
                        $"Settings saved!\nResults will be available on:\n{selectedTime:MMMM dd, yyyy 'at' hh:mm tt}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No changes were made.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblCurrentSetting_Click(object sender, EventArgs e)
        {

        }

        private void lblResultsTime_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
