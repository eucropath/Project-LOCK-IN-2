using MySql.Data.MySqlClient;
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
    public partial class VotingForm : Form
    {
        private Form voterDashboard;
        private int voterID;
        private string voterProgram;
        private bool isFormLoaded = false;

        // Cache data to avoid repeated DB calls
        private DataTable votedCandidatesCache;
        private Dictionary<string, DataTable> candidatesByPositionCache = new Dictionary<string, DataTable>();

        private readonly List<string> votingHierarchy = new List<string>
        {
            "President",
            "Vice President",
            "Secretary",
            "Treasurer",
            "Auditor",
            "Business Manager",
            "CS PIO - 1st",
            "CS PIO - 2nd",
            "IT PIO - 1st",
            "IT PIO - 2nd"
        };

        public VotingForm(Form dashboard, int voterId)
        {
            InitializeComponent();
            voterDashboard = dashboard;
            voterID = voterId;

            this.Load += VotingForm_Load;
        }

        private void InitializeVotedCandidatesListBox()
        {
            // Style the listBox1 from designer
            listBox1.Location = new Point(20, 100);
            listBox1.Size = new Size(220, 420);
            listBox1.ForeColor = Color.White;
            listBox1.BackColor = Color.FromArgb(15, 25, 50); // dark background to match theme
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 65;
            listBox1.SelectionMode = SelectionMode.None;
            listBox1.Parent = pictureBox1;
            listBox1.BringToFront();

            listBox1.DrawItem += ListBox1_DrawItem;

            // Custom drawing for list items
            listBox1.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();

                string item = listBox1.Items[e.Index].ToString();

                if (item == "YOU VOTED:")
                {
                    using (Font headerFont = new Font("Segoe UI", 12, FontStyle.Bold))
                    using (Brush headerBrush = new SolidBrush(Color.White))
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        e.Graphics.DrawString(item, headerFont, headerBrush, e.Bounds, sf);
                    }
                }
                else if (item.StartsWith("─"))
                {
                    using (Brush summaryBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                    using (Font summaryFont = new Font("Segoe UI", 9, FontStyle.Bold))
                    {
                        e.Graphics.DrawString(item, summaryFont, summaryBrush, e.Bounds.X + 5, e.Bounds.Y + 5);
                    }
                }
                else if (item == "No votes cast yet")
                {
                    using (Brush noBrush = new SolidBrush(Color.FromArgb(200, 200, 200)))
                    using (Font noFont = new Font("Segoe UI", 9, FontStyle.Italic))
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        e.Graphics.DrawString(item, noFont, noBrush, e.Bounds, sf);
                    }
                }
                else
                {
                    // Draw vote item with checkmark
                    string[] parts = item.Split('|');
                    if (parts.Length == 2)
                    {
                        // Background for vote item
                        using (Brush bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                        {
                            Rectangle bgRect = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 5, e.Bounds.Width - 10, e.Bounds.Height - 10);
                            e.Graphics.FillRectangle(bgBrush, bgRect);
                        }

                        // Checkmark
                        using (Font checkFont = new Font("Segoe UI", 14, FontStyle.Bold))
                        using (Brush checkBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                        {
                            e.Graphics.DrawString("✓", checkFont, checkBrush, e.Bounds.X + 10, e.Bounds.Y + 10);
                        }

                        // Position
                        using (Font posFont = new Font("Segoe UI", 8, FontStyle.Bold))
                        using (Brush posBrush = new SolidBrush(Color.FromArgb(100, 181, 246)))
                        {
                            e.Graphics.DrawString(parts[0], posFont, posBrush, e.Bounds.X + 35, e.Bounds.Y + 10);
                        }

                        // Candidate name
                        using (Font nameFont = new Font("Segoe UI", 8, FontStyle.Regular)) 
                        using (Brush nameBrush = new SolidBrush(Color.White))
                        {
                            Rectangle nameRect = new Rectangle(e.Bounds.X + 35, e.Bounds.Y + 28, e.Bounds.Width - 45, 35);
                            e.Graphics.DrawString(parts[1], nameFont, nameBrush, nameRect);
                        }
                    }
                }

                e.DrawFocusRectangle();
            };

            listBox1.BringToFront();
        }

        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            string item = listBox1.Items[e.Index].ToString();
            Rectangle rect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 4, e.Bounds.Width - 12, e.Bounds.Height - 8);

            // Default background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(25, 40, 80)))
            {
                e.Graphics.FillRectangle(bgBrush, rect);
            }

            // Border for separation
            using (Pen borderPen = new Pen(Color.FromArgb(45, 60, 110)))
            {
                e.Graphics.DrawRectangle(borderPen, rect);
            }

            // Different text categories
            if (item == "YOU VOTED:")
            {
                using (Font headerFont = new Font("Segoe UI Semibold", 11, FontStyle.Bold))
                using (Brush headerBrush = new SolidBrush(Color.FromArgb(100, 181, 246)))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(item, headerFont, headerBrush, e.Bounds, sf);
                }
            }
            else if (item.StartsWith("─"))
            {
                using (Font sumFont = new Font("Segoe UI", 9, FontStyle.Italic))
                using (Brush sumBrush = new SolidBrush(Color.FromArgb(180, 200, 255)))
                {
                    e.Graphics.DrawString(item.Replace("─", "").Trim(), sumFont, sumBrush, e.Bounds.X + 10, e.Bounds.Y + 20);
                }
            }
            else if (item == "No votes cast yet")
            {
                using (Font noFont = new Font("Segoe UI", 9, FontStyle.Italic))
                using (Brush noBrush = new SolidBrush(Color.Gray))
                {
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(item, noFont, noBrush, e.Bounds, sf);
                }
            }
            else
            {
                // Regular vote entries — format: Position|CandidateName
                string[] parts = item.Split('|');
                if (parts.Length == 2)
                {
                    string position = parts[0];
                    string name = parts[1];

                    // Checkmark
                    using (Font checkFont = new Font("Segoe UI", 13, FontStyle.Bold))
                    using (Brush checkBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                    {
                        e.Graphics.DrawString("✓", checkFont, checkBrush, rect.X + 8, rect.Y + 10);
                    }

                    // Position title
                    using (Font posFont = new Font("Segoe UI Semibold", 9, FontStyle.Bold))
                    using (Brush posBrush = new SolidBrush(Color.FromArgb(120, 170, 255)))
                    {
                        e.Graphics.DrawString(position, posFont, posBrush, rect.X + 30, rect.Y + 10);
                    }

                    // Candidate name
                    using (Font nameFont = new Font("Segoe UI", 9, FontStyle.Regular))
                    using (Brush nameBrush = new SolidBrush(Color.White))
                    {
                        e.Graphics.DrawString(name, nameFont, nameBrush, rect.X + 30, rect.Y + 32);
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        private void LoadVotedCandidates()
        {
            listBox1.BeginUpdate(); // OPTIMIZATION: Suspend updates

            listBox1.Items.Clear();
            listBox1.Items.Add("YOU VOTED:");

            try
            {
                // Use cached data if available
                if (votedCandidatesCache == null)
                {
                    votedCandidatesCache = DataAccess.ExecuteProcedureToDataTable("sp_GetVoterReceipt",
                        new MySqlParameter("@in_voterid", voterID));
                }

                if (votedCandidatesCache.Rows.Count == 0)
                {
                    listBox1.Items.Add("No votes cast yet");
                }
                else
                {
                    // Add each vote from the DataTable
                    foreach (DataRow row in votedCandidatesCache.Rows)
                    {
                        string position = row["Position"].ToString();
                        string candidateName = row["CandidateName"].ToString();
                        listBox1.Items.Add($"{position}|{candidateName}");
                    }

                    // Add summary
                    listBox1.Items.Add($"─────────────\nTotal: {votedCandidatesCache.Rows.Count} vote(s)");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Error loading votes");
            }
            finally
            {
                listBox1.EndUpdate(); // OPTIMIZATION: Resume updates
            }
        }

        private void LoadVoterProgram()
        {
            try
            {
                var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetVoterByID",
                    new MySqlParameter("@in_voterid", voterID));

                if (dt.Rows.Count > 0)
                {
                    voterProgram = dt.Rows[0]["Program"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading voter program: " + ex.Message);
                voterProgram = "";
            }
        }

        private void ShowVotingSummary()
        {
            try
            {
                // Use cached data
                if (votedCandidatesCache == null || votedCandidatesCache.Rows.Count == 0)
                {
                    MessageBox.Show("No votes found.");
                    return;
                }

                StringBuilder receipt = new StringBuilder();
                receipt.AppendLine("========================================");
                receipt.AppendLine("         VOTING RECEIPT");
                receipt.AppendLine("========================================");
                receipt.AppendLine();
                receipt.AppendLine($"Voter ID: {voterID}");
                receipt.AppendLine($"Date: {DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt")}");
                receipt.AppendLine();
                receipt.AppendLine("YOUR VOTES:");
                receipt.AppendLine("----------------------------------------");

                foreach (DataRow row in votedCandidatesCache.Rows)
                {
                    string position = row["Position"].ToString();
                    string candidateName = row["CandidateName"].ToString();
                    receipt.AppendLine($"• {position}: {candidateName}");
                }

                receipt.AppendLine("========================================");
                receipt.AppendLine("Thank you for participating!");

                MessageBox.Show(receipt.ToString(), "Voting Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating receipt: " + ex.Message);
            }
        }

        private string GetNextPositionToVote()
        {
            try
            {
                foreach (string position in votingHierarchy)
                {
                    if ((position.StartsWith("CS PIO") && voterProgram != "CS") ||
                        (position.StartsWith("IT PIO") && voterProgram != "IT"))
                        continue;

                    string basePosition = position.Contains(" - ") ? position.Substring(0, position.IndexOf(" - ")) : position;

                    var dtCheck = DataAccess.ExecuteProcedureToDataTable("sp_CheckIfAlreadyVoted",
                        new MySqlParameter("@in_voterid", voterID),
                        new MySqlParameter("@in_position", basePosition));

                    int voteCount = 0;
                    if (dtCheck.Rows.Count > 0)
                        voteCount = Convert.ToInt32(dtCheck.Rows[0]["VoteCount"]);

                    if (position.Contains(" - 1st"))
                    {
                        if (voteCount < 1)
                            return position;
                    }
                    else if (position.Contains(" - 2nd"))
                    {
                        if (voteCount < 2)
                            return position;
                    }
                    else
                    {
                        if (voteCount == 0)
                            return position;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking voting progress: " + ex.Message);
                return null;
            }
        }

        private void btnCastVote_Click(object sender, EventArgs e)
        {
            if (!isFormLoaded)
                return;

            if (cmbCandidates.DataSource == null || cmbCandidates.Items.Count == 0)
            {
                return;
            }

            if (cmbCandidates.SelectedValue == null || cmbCandidates.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a candidate to cast your vote.", "No Candidate Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int candidateId = Convert.ToInt32(cmbCandidates.SelectedValue);
                string selectedPosition = cmbPositions.SelectedValue.ToString();

                string nextPosition = GetNextPositionToVote();

                if (nextPosition == null)
                {
                    MessageBox.Show("You have already completed all your votes!");
                    return;
                }

                DataAccess.ExecuteProcedureNonQuery("sp_CastVote",
                    new MySqlParameter("@in_voterid", voterID),
                    new MySqlParameter("@in_candidateid", candidateId),
                    new MySqlParameter("@in_position", selectedPosition));

                MessageBox.Show($"Vote cast successfully for {selectedPosition}!");

                // OPTIMIZATION: Clear cache to force refresh
                votedCandidatesCache = null;
                LoadVotedCandidates();

                string newNextPosition = GetNextPositionToVote();
                if (newNextPosition != null)
                {
                    cmbPositions.SelectedValue = newNextPosition;
                }
                else
                {
                    ShowVotingSummary();
                    MessageBox.Show("You have completed all your votes! Thank you for participating.",
                                  "Voting Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCastVote.Enabled = false;
                }
            }
            catch (MySqlException mex)
            {
                if (mex.Message.Contains("already voted for this position"))
                {
                    MessageBox.Show("You have already voted for this position!");
                }
                else
                {
                    MessageBox.Show("Database error: " + mex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadPositions()
        {
            cmbPositions.Enabled = false;

            DataTable filteredDt = new DataTable();
            filteredDt.Columns.Add("Position", typeof(string));

            filteredDt.Rows.Add("President");
            filteredDt.Rows.Add("Vice President");
            filteredDt.Rows.Add("Secretary");
            filteredDt.Rows.Add("Treasurer");
            filteredDt.Rows.Add("Auditor");
            filteredDt.Rows.Add("Business Manager");

            if (voterProgram == "CS")
            {
                filteredDt.Rows.Add("CS PIO - 1st");
                filteredDt.Rows.Add("CS PIO - 2nd");
            }
            else if (voterProgram == "IT")
            {
                filteredDt.Rows.Add("IT PIO - 1st");
                filteredDt.Rows.Add("IT PIO - 2nd");
            }

            cmbPositions.DataSource = filteredDt;
            cmbPositions.DisplayMember = "Position";
            cmbPositions.ValueMember = "Position";

            string nextPosition = GetNextPositionToVote();
            if (nextPosition != null)
            {
                cmbPositions.SelectedValue = nextPosition;
            }
        }

        private void cmbPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPositions.SelectedValue == null || !isFormLoaded)
                return;

            string selectedPosition = cmbPositions.SelectedValue.ToString();
            string basePosition = selectedPosition.Contains(" - ") ?
                selectedPosition.Substring(0, selectedPosition.IndexOf(" - ")) : selectedPosition;

            string nextPosition = GetNextPositionToVote();
            if (nextPosition == null)
            {
                label2.Text = "✓ All votes completed!";
                label2.ForeColor = Color.LightGreen;
                btnCastVote.Enabled = false;
            }
            else
            {
                label2.Text = $"Now voting for: {selectedPosition}";
                label2.ForeColor = Color.LightGreen;
                btnCastVote.Enabled = true;
            }

            // OPTIMIZATION: Cache candidates by position
            DataTable dt;
            if (!candidatesByPositionCache.ContainsKey(basePosition))
            {
                dt = DataAccess.ExecuteProcedureToDataTable("sp_GetCandidatesByPosition",
                    new MySqlParameter("@in_position", basePosition));
                candidatesByPositionCache[basePosition] = dt;
            }
            else
            {
                dt = candidatesByPositionCache[basePosition];
            }

            dgvCandidates.DataSource = dt;

            if (dgvCandidates.Columns.Contains("CandidateID"))
            {
                dgvCandidates.Columns["CandidateID"].Visible = false;
            }

            if (dgvCandidates.Columns.Contains("FullName"))
            {
                dgvCandidates.Columns["FullName"].Visible = false;
            }

            DataTable dtCombo = dt.Copy();
            if (!dtCombo.Columns.Contains("FullName"))
            {
                dtCombo.Columns.Add("FullName", typeof(string));
            }
            foreach (DataRow row in dtCombo.Rows)
            {
                row["FullName"] = row["FirstName"] + " " + row["LastName"];
            }
            cmbCandidates.DataSource = dtCombo;
            cmbCandidates.DisplayMember = "FullName";
            cmbCandidates.ValueMember = "CandidateId";
        }

        private void VotingForm_Load(object sender, EventArgs e)
        {
            // OPTIMIZATION: Suspend layout during load
            this.SuspendLayout();

            dgvCandidates.Visible = false;

            this.Text = "Voting Form";
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);

            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // Load voter program first (needed for everything else)
            LoadVoterProgram();

            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label1.Text = "Voting Form - " + voterProgram + " Program";
            label1.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.AutoSize = true;

            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            StyleLabel(label2, "");

            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            StyleLabel(label3, "Current Position:");

            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            StyleLabel(label4, "Candidate:");

            StyleComboBox(cmbPositions);
            StyleComboBox(cmbCandidates);
            StyleDataGridView(dgvCandidates);
            StyleButton(btnCastVote, "Cast Vote", Color.FromArgb(0, 123, 255), Color.White);
            StyleButton(button1, "Back", Color.FromArgb(220, 53, 69), Color.White);

            // Initialize listBox1 (from designer)
            InitializeVotedCandidatesListBox();

            CenterControls();
            this.Resize += (s, ev) => CenterControls();

            // OPTIMIZATION: Load positions first
            LoadPositions();

            // Load voted candidates asynchronously
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    votedCandidatesCache = DataAccess.ExecuteProcedureToDataTable("sp_GetVoterReceipt",
                        new MySqlParameter("@in_voterid", voterID));

                    this.Invoke((MethodInvoker)delegate
                    {
                        LoadVotedCandidates();
                    });
                }
                catch { }
            });

            // Add event handler AFTER everything is loaded
            cmbPositions.SelectedIndexChanged += cmbPositions_SelectedIndexChanged;

            // OPTIMIZATION: Resume layout
            this.ResumeLayout();
            isFormLoaded = true;
        }

        private void StyleLabel(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;
        }

        private void StyleComboBox(ComboBox cmb)
        {
            cmb.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            cmb.Width = 250;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.BackgroundColor = Color.FromArgb(250, 250, 255);
            dgv.GridColor = Color.FromArgb(220, 230, 245);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 70, 150);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 70, 150);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 200, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 255);
            dgv.RowTemplate.Height = 35;
            dgv.RowHeadersVisible = false;
            dgv.ScrollBars = ScrollBars.Vertical;

            dgv.Paint += (s, e) =>
            {
                var rect = dgv.ClientRectangle;
                int radius = 10;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();
                    dgv.Region = new Region(path);
                }
            };
        }

        private void StyleButton(Button btn, string text, Color backColor, Color foreColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(140, 40);
        }

        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 30;

            int currentY = 120;
            int labelX = centerX - 200;
            int inputX = centerX;

            label2.Left = centerX - (label2.Width / 2);
            label2.Top = currentY;
            currentY += 40;

            label3.Left = labelX;
            label3.Top = currentY;
            cmbPositions.Left = inputX;
            cmbPositions.Top = currentY - 3;
            currentY += 50;

            label4.Left = labelX;
            label4.Top = currentY;
            cmbCandidates.Left = inputX;
            cmbCandidates.Top = currentY - 3;
            currentY += 70;

            btnCastVote.Left = centerX - (btnCastVote.Width + 20);
            btnCastVote.Top = currentY;

            button1.Left = centerX + 20;
            button1.Top = currentY;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            voterDashboard.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void dgvCandidates_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void cmbPositions_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void cmbCandidates_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) { }
        private void label5_Click_2(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}