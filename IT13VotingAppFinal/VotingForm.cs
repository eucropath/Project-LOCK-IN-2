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

        public VotingForm(Form dashboard, int voterId)
        {
            InitializeComponent();
            voterDashboard = dashboard;
            voterID = voterId;
            LoadPositions();
            LoadCandidates();
            cmbPositions.SelectedIndexChanged += cmbPositions_SelectedIndexChanged;

            this.Load += VotingForm_Load;
            
        }



   

        private void btnCastVote_Click(object sender, EventArgs e)
        {
            if (dgvCandidates.CurrentRow == null)
            {
                MessageBox.Show("Select a candidate.");
                return;
            }
            try
            {
                // Validate voter from database (optional)
                var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetVoterByID",
                    new MySqlParameter("@in_voterid", voterID));

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Voter not found.");
                    return;
                }
                
                int candidateId = Convert.ToInt32(dgvCandidates.CurrentRow.Cells["CandidateID"].Value);
                string position = cmbPositions.SelectedValue.ToString();
                var dtCheck = DataAccess.ExecuteProcedureToDataTable("sp_CheckIfAlreadyVoted",
                           new MySqlParameter("@in_voterid", voterID),
                           new MySqlParameter("@in_position", position));

                if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0]["VoteCount"]) > 0)
                {
                    MessageBox.Show("You have already voted for " + position + "!");
                    return;
                }
                DataAccess.ExecuteProcedureNonQuery("sp_CastVote",
            new MySqlParameter("@in_voterid", voterID),
            new MySqlParameter("@in_candidateid", candidateId));

                MessageBox.Show("Vote cast successfully.");
            }
            catch (MySqlException mex)
            {
                if (mex.Message.Contains("already voted for this position"))
                {
                    MessageBox.Show("You have already voted for this position!");
                }
                MessageBox.Show("Database error: " + mex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void LoadPositions()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllPositions");
            cmbPositions.DataSource = dt;
            cmbPositions.DisplayMember = "Position"; // column name from your table
            cmbPositions.ValueMember = "Position";   // same as DisplayMember if just using text
        }
        private void cmbPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPositions.SelectedValue != null)
            {
                string selectedPosition = cmbPositions.SelectedValue.ToString();
                var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetCandidatesByPosition",
                    new MySqlParameter("@in_position", selectedPosition));
                dgvCandidates.DataSource = dt;

                // Fill the candidate ComboBox with filtered candidates
                dt.Columns.Add("FullName", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["FullName"] = row["FirstName"] + " " + row["LastName"];
                }
                cmbCandidates.DataSource = dt;
                cmbCandidates.DisplayMember = "FullName";
                cmbCandidates.ValueMember = "CandidateId";
            }
        }
        private void LoadCandidates()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllCandidates");
           
            dgvCandidates.DataSource = dt;

            // Fill ComboBox with candidates (showing full name and position)
            cmbCandidates.DataSource = dt;
            cmbCandidates.DisplayMember = "FirstName"; // or use a custom property for full name

            dt.Columns.Add("FullName", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                row["FullName"] = row["FirstName"] + " " + row["LastName"] + " (" + row["Position"] + ")";
            }

            cmbCandidates.DataSource = dt;
            cmbCandidates.DisplayMember = "FullName";
        }


        private void VotingForm_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;

            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;

            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;

            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;

            // === Form Styling ===
            this.Text = "Voting Form";
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);


            // === Title Label ===
            label1.Text = "Voting Form";
            label1.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            label1.ForeColor = Color.White; // dark blue
            label1.AutoSize = true;

            // === Labels ===
            StyleLabel(label2, "");
            StyleLabel(label3, "Position:");
            StyleLabel(label4, "Candidate:");

            // === BACKGROUND ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // === Inputs ===
            StyleComboBox(cmbPositions);
            StyleComboBox(cmbCandidates);

            // === DataGridView ===
            StyleDataGridView(dgvCandidates);

            // === Buttons ===
            StyleButton(btnCastVote, "Cast Vote", Color.FromArgb(0, 123, 255), Color.White); // blue
            StyleButton(button1, "Back", Color.FromArgb(220, 53, 69), Color.White); // red

            ApplyRoundedCorners(btnCastVote, 10);
            ApplyRoundedCorners(button1, 10);
            ApplyRoundedCorners(cmbPositions, 10);
            ApplyRoundedCorners(cmbCandidates, 10);


            // Center everything
            CenterControls();

            // Recenter when resized
            this.Resize += (s, ev) => CenterControls();
        }

        private void StyleLabel(Label lbl, string text)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lbl.ForeColor = Color.White;
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;
        }
        private void StyleTextBox(TextBox txt)
        {
            txt.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            txt.Width = 250;
        }

        private void StyleComboBox(ComboBox cmb)
        {
            cmb.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            cmb.Width = 250;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            dgv.ForeColor = Color.Black;
            dgv.BackgroundColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.Height = 200;
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

            // Title
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 30;

            // DataGridView
            dgvCandidates.Width = this.ClientSize.Width - 100;
            dgvCandidates.Left = (this.ClientSize.Width - dgvCandidates.Width) / 2;
            dgvCandidates.Top = 80;

            int currentY = dgvCandidates.Bottom + 30;
            int labelX = centerX - 200;
            int inputX = centerX;

            // Voter Number
          
            currentY += 50;

            // Position
            label3.Left = labelX;
            label3.Top = currentY;
            cmbPositions.Left = inputX;
            cmbPositions.Top = currentY - 3;
            currentY += 50;

            // Candidate
            label4.Left = labelX;
            label4.Top = currentY;
            cmbCandidates.Left = inputX;
            cmbCandidates.Top = currentY - 3;
            currentY += 70;

            // Buttons
            btnCastVote.Left = centerX - (btnCastVote.Width + 20);
            btnCastVote.Top = currentY;

            button1.Left = centerX + 20;
            button1.Top = currentY;
        }
        private void ApplyLabelStyles()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = Color.LightSkyBlue;
                    lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);

                    
                    lbl.Parent = pictureBox1;
                }
            }
        }
        private void ApplyRoundedCorners(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), -90, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dgvCandidates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbPositions_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void cmbCandidates_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            voterDashboard.Show();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
    }

    


