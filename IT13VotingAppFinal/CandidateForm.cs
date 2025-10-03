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
    public partial class CandidateForm : Form
    {
        public CandidateForm()
        {
            InitializeComponent();
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            dgvCandidates.SelectionChanged += dgvCandidates_SelectionChanged;

            this.Load += CandidateForm_Load;
            this.Resize += (s, e) => CenterControls();

            LoadCandidates();
        }
        private void LoadCandidates()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllCandidates");
            dgvCandidates.DataSource = dt;
        }


        private void dgvCandidates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCandidates.SelectedRows.Count > 0)
            {
                var row = dgvCandidates.SelectedRows[0];
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtPosition.Text = row.Cells["Position"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string position = txtPosition.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(position))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_AddCandidate",
                   new MySqlParameter("@FirstName", firstName),
                   new MySqlParameter("@LastName", lastName),
                   new MySqlParameter("@Position", position)
                );
                LoadCandidates();
                ClearFields();
                MessageBox.Show("Candidate added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding candidate: " + ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCandidates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a candidate to update.");
                return;
            }

            var row = dgvCandidates.SelectedRows[0];
            string candidateId = row.Cells["CandidateID"].Value.ToString();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string position = txtPosition.Text.Trim();

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_UpdateCandidate",
                   new MySqlParameter("@in_id", candidateId),
                   new MySqlParameter("@in_fn", firstName),
                   new MySqlParameter("@in_ln", lastName),
                   new MySqlParameter("@in_position", position)
                );
                LoadCandidates();
                ClearFields();
                MessageBox.Show("Candidate updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating candidate: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCandidates.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a candidate to delete.");
                return;
            }

            var row = dgvCandidates.SelectedRows[0];
            string candidateID = row.Cells["CandidateID"].Value.ToString();

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_DeleteCandidate",
                    new MySqlParameter("@p_CandidateID", candidateID)
                );
                LoadCandidates();
                ClearFields();
                MessageBox.Show("Candidate deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting candidate: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPosition.Text = "";
        }

        private void CandidateForm_Load(object sender, EventArgs e)
        {
            // === FORM SETTINGS ===
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(950, 600);

            // === BACKGROUND ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // === TITLE ===
            label1.Text = "Candidates Management";
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
                    lbl.ForeColor = Color.  White;
                    lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lbl.Parent = pictureBox1;
                }
            }

            // === TEXTBOXES ===
            foreach (TextBox txt in new[] { txtFirstName, txtLastName, txtPosition })
            {
                txt.Font = new Font("Segoe UI", 10);
                txt.Width = 180;
            }

            // === BUTTONS ===
            StyleButton(btnAdd, "Add", Color.MediumSeaGreen, Color.White);
            StyleButton(btnUpdate, "Update", Color.SteelBlue, Color.White);
            StyleButton(btnDelete, "Delete", Color.IndianRed, Color.White);
            StyleButton(Close, "Close", Color.DarkRed, Color.White);

            // === DATAGRIDVIEW ===
            dgvCandidates.BackgroundColor = Color.White;
            dgvCandidates.GridColor = Color.LightGray;
            dgvCandidates.DefaultCellStyle.BackColor = Color.White;
            dgvCandidates.DefaultCellStyle.ForeColor = Color.Black;
            dgvCandidates.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvCandidates.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvCandidates.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCandidates.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCandidates.EnableHeadersVisualStyles = false;
            dgvCandidates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            MakeRounded(btnAdd, 15);
            MakeRounded(btnUpdate, 15);
            MakeRounded(btnDelete, 15);
            MakeRounded(Close, 15);

            MakeRounded(txtFirstName, 10);
            MakeRounded(txtLastName, 10);
            MakeRounded(txtPosition, 10);


            CenterControls();
            LoadCandidates();
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

            // === Title ===
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 20;

            // === DataGridView ===
            dgvCandidates.Width = this.ClientSize.Width - 100;
            dgvCandidates.Left = (this.ClientSize.Width - dgvCandidates.Width) / 2;
            dgvCandidates.Top = label1.Bottom + 20;
            dgvCandidates.Height = this.ClientSize.Height / 3;

            // === Input Fields ===
            int inputTop = dgvCandidates.Bottom + 20;

            label2.Left = centerX - 200; label2.Top = inputTop;
            txtFirstName.Left = centerX; txtFirstName.Top = inputTop - 5;

            label3.Left = centerX - 200; label3.Top = inputTop + 40;
            txtLastName.Left = centerX; txtLastName.Top = inputTop + 35;

            label5.Left = centerX - 200; label5.Top = inputTop + 80;
            txtPosition.Left = centerX; txtPosition.Top = inputTop + 75;

            // === Buttons ===
            int buttonsTop = inputTop + 130;
            int totalWidth = (btnAdd.Width + btnUpdate.Width + btnDelete.Width + Close.Width) + (20 * 3);
            int buttonsLeft = centerX - (totalWidth / 2);

            btnAdd.Left = buttonsLeft; btnAdd.Top = buttonsTop;
            btnUpdate.Left = btnAdd.Right + 20; btnUpdate.Top = buttonsTop;
            btnDelete.Left = btnUpdate.Right + 20; btnDelete.Top = buttonsTop;
            Close.Left = btnDelete.Right + 20; Close.Top = buttonsTop;
        }
        private void dgvCandidates_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCandidates.SelectedRows.Count > 0)
            {
                var row = dgvCandidates.SelectedRows[0];
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtPosition.Text = row.Cells["Position"].Value.ToString();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPosition_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
