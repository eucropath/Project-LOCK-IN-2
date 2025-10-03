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
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
            Load += ResultsForm_Load;
            this.Resize += (s, e) => CenterControls();

            cmbPositions.SelectedIndexChanged += cmbPositions_SelectedIndexChanged;
            btnRefresh.Click += btnRefresh_Click;
        }
        private void LoadPositions()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllPositions");
            cmbPositions.DataSource = dt;
            cmbPositions.DisplayMember = "Position";
            cmbPositions.ValueMember = "Position";
            cmbPositions.SelectedIndexChanged += cmbPositions_SelectedIndexChanged;
        }
        private void LoadResults(string position = "")
        {
            DataTable dt;
            if (string.IsNullOrEmpty(position))
                dt = DataAccess.ExecuteProcedureToDataTable("sp_GetResults");
            else
                dt = DataAccess.ExecuteProcedureToDataTable("sp_GetResultsByPosition",
                    new MySql.Data.MySqlClient.MySqlParameter("@in_position", position));

            dgvResults.DataSource = dt;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.Columns["VoteCount"].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // 🔴 Hide CandidateId column if it exists
            if (dgvResults.Columns.Contains("CandidateId"))
            {
                dgvResults.Columns["CandidateId"].Visible = false;
            }

            // Show winner
            if (dt.Rows.Count > 0)
            {
                var maxVotes = dt.AsEnumerable().Max(r => r.Field<Int64>("VoteCount"));
                var winnerRows = dt.AsEnumerable()
                    .Where(r => r.Field<Int64>("VoteCount") == maxVotes)
                    .Select(r => r.Field<string>("Candidate"));
                lblWinner.Text = $"Winner: {string.Join(", ", winnerRows)} ({maxVotes} votes)";
            }
            else
            {
                lblWinner.Text = "Winner: N/A";
            }

            // Show total votes
            var totalVotes = dt.AsEnumerable().Sum(r => r.Field<Int64>("VoteCount"));
            lblTotalVotes.Text = $"Total Votes: {totalVotes}";
        }
        private void ResultsForm_Load(object sender, EventArgs e)
        {
            // === FORM SETTINGS ===
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(950, 600);

            // === BACKGROUND ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // === TITLE ===
            label1.Text = "Election Results";
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Transparent;
            label1.AutoSize = true;
            label1.Parent = pictureBox1;

            // === LABELS ===
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = Color.White;
                    lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lbl.Parent = pictureBox1;
                }
            }

            // === BUTTONS ===
            StyleButton(btnRefresh, "Refresh", Color.SteelBlue, Color.White);

            StyleButton(button1, "Back", Color.IndianRed, Color.White);
            ApplyRoundedCorners(button1, 10);
            button1.Resize += (s, ev) => ApplyRoundedCorners(button1, 10);


            // === COMBOBOX ===
            cmbPositions.Font = new Font("Segoe UI", 10);
            cmbPositions.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPositions.Width = 200;

            // === DATAGRIDVIEW ===
            dgvResults.BackgroundColor = Color.White;
            dgvResults.GridColor = Color.LightGray;
            dgvResults.DefaultCellStyle.BackColor = Color.White;
            dgvResults.DefaultCellStyle.ForeColor = Color.Black;
            dgvResults.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            ApplyRoundedCorners(btnRefresh, 10);
            btnRefresh.Resize += (s, ev) => ApplyRoundedCorners(btnRefresh, 10);

            ApplyRoundedCorners(cmbPositions, 10);
            cmbPositions.Resize += (s, ev) => ApplyRoundedCorners(cmbPositions, 10);



            LoadPositions();
            LoadResults();
            CenterControls();
        }


        private void ApplyRoundedCorners(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;

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
        private void StyleButton(Button btn, string text, Color backColor, Color foreColor)
        {
            btn.Text = text;
            btn.Width = 120;    
            btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
        }
        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            // Title
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 20;

            // DataGridView
            dgvResults.Width = this.ClientSize.Width - 120;
            dgvResults.Left = (this.ClientSize.Width - dgvResults.Width) / 2;
            dgvResults.Top = label1.Bottom + 20;
            dgvResults.Height = this.ClientSize.Height / 3;

            // ComboBox
            cmbPositions.Left = centerX - (cmbPositions.Width / 2);
            cmbPositions.Top = dgvResults.Bottom + 15;

            // Winner label
            lblWinner.Left = centerX - 200;
            lblWinner.Top = cmbPositions.Bottom + 20;

            // Total votes label
            lblTotalVotes.Left = centerX - 200;
            lblTotalVotes.Top = lblWinner.Bottom + 10;

            // Refresh button
            btnRefresh.Left = centerX - (btnRefresh.Width / 2);
            btnRefresh.Top = lblTotalVotes.Bottom + 20;

            // Back button (below Refresh)
            button1.Left = centerX - (button1.Width / 2);
            button1.Top = btnRefresh.Bottom + 20;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Get the currently selected position from the ComboBox
            var pos = cmbPositions.SelectedValue?.ToString() ?? "";

            // Reload the results for the selected position
            LoadResults(pos);
        }
        private void cmbPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pos = cmbPositions.SelectedValue?.ToString() ?? "";
            LoadResults(pos);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalVotes_Click(object sender, EventArgs e)
        {

        }

        private void lblWinner_Click(object sender, EventArgs e)
        {

        }

        private void cmbPositions_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {

        }

        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}

