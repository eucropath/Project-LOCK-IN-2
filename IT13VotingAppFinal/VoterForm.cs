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
    public partial class VoterForm : Form
    {
        private ComboBox cmbFilterProgram;
        private Label lblFilter; // Add this field

        public VoterForm()
        {
            InitializeComponent();
            dgvVoters.SelectionChanged += dgvVoters_SelectionChanged;
            this.Load += VoterForm_Load;
            this.Resize += VoterForm_Resize;
        }

        private static bool _isShowingMessage = false;

        private void ShowMessage(string message, string title = "Notice")
        {
            if (_isShowingMessage) return;
            try
            {
                _isShowingMessage = true;
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isShowingMessage = false;
            }
        }

        private void VoterForm_Resize(object sender, EventArgs e)
        {
            PositionControls();
        }

        private void PositionControls()
        {
            int centerX = this.ClientSize.Width / 2;

            // === Title ===
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 20;

            // === Filter controls ABOVE DataGridView ===
            int filterTop = label1.Bottom + 20;

            // Only position filter controls if they exist
            if (lblFilter != null && cmbFilterProgram != null)
            {
                lblFilter.Left = 50;
                lblFilter.Top = filterTop + 3;
                cmbFilterProgram.Left = lblFilter.Right + 10;
                cmbFilterProgram.Top = filterTop;
            }

            // === DataGridView directly under Filter ===
            dgvVoters.Left = 50;
            dgvVoters.Top = (cmbFilterProgram != null) ? cmbFilterProgram.Bottom + 10 : filterTop + 30;
            dgvVoters.Width = this.ClientSize.Width - 100;
            dgvVoters.Height = this.ClientSize.Height / 3;

            // === Inputs under DataGridView ===
            int inputTop = dgvVoters.Bottom + 30;

            label2.Left = centerX - 200;
            label2.Top = inputTop;
            txtFirstName.Left = centerX;
            txtFirstName.Top = inputTop;

            label3.Left = centerX - 200;
            label3.Top = inputTop + 50;
            txtLastName.Left = centerX;
            txtLastName.Top = inputTop + 50;

            // === Buttons row at bottom ===
            int buttonsTop = txtLastName.Bottom + 50;
            int spacing = 140;

            btnAdd.Location = new Point(centerX - (spacing * 2), buttonsTop);
            btnUpdate.Location = new Point(centerX - spacing, buttonsTop);
            btnDelete.Location = new Point(centerX, buttonsTop);
            btnRefresh.Location = new Point(centerX + spacing, buttonsTop);
            btnClear.Location = new Point(centerX - btnClear.Width - 10, buttonsTop + 60);
            button1.Location = new Point(btnClear.Right + 20, btnClear.Top);
        }

        private void LoadVoters()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllVoters");
            dgvVoters.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                ShowMessage("Please fill all fields.");
                return;
            }

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_AddVoter",
                   new MySqlParameter("@in_fn", firstName),
                   new MySqlParameter("@in_ln", lastName)
                );
                LoadVoters();
                ClearFields();
                ShowMessage("Voter added successfully.");
            }
            catch (Exception ex)
            {
                ShowMessage("Error adding voter: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count == 0)
            {
                ShowMessage("Select a voter to update.");
                return;
            }

            var row = dgvVoters.SelectedRows[0];
            string id = row.Cells["VoterId"].Value.ToString();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_UpdateVoter",
                   new MySqlParameter("@in_id", id),
                   new MySqlParameter("@in_fn", firstName),
                   new MySqlParameter("@in_ln", lastName)
                );
                LoadVoters();
                ClearFields();
                ShowMessage("Voter updated successfully.");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating voter: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count == 0)
            {
                ShowMessage("Select a voter to delete.");
                return;
            }

            var row = dgvVoters.SelectedRows[0];
            string id = row.Cells["VoterId"].Value.ToString();

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_DeleteVoter",
                    new MySqlParameter("@in_id", id));
                LoadVoters();
                ClearFields();
                ShowMessage("Voter deleted successfully.");
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting voter: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadVoters();
            cmbFilterProgram.SelectedIndex = 0; // Reset filter to "All"
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }

        private void dgvVoters_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count > 0)
            {
                var row = dgvVoters.SelectedRows[0];
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
            }
        }

        private void VoterForm_Load(object sender, EventArgs e)
        {
            // === FORM SETTINGS ===
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 650);

            // === BACKGROUND ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // Re-parent all labels to the PictureBox
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl)
                    lbl.Parent = pictureBox1;
            }

            // === TITLE ===
            label1.Text = "Voter Management";
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Transparent;
            label1.AutoSize = true;

            label2.Text = "First Name";
            label2.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.BackColor = Color.Transparent;
            label2.Parent = pictureBox1;
            label2.AutoSize = true;

            label3.Text = "Last Name";
            label3.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.BackColor = Color.Transparent;
            label3.AutoSize = true;

            // Email label and textbox hidden
            label4.Visible = false;
            txtEmail.Visible = false;

            // === TEXTBOXES ===
            foreach (TextBox txt in new[] { txtFirstName, txtLastName })
            {
                txt.Font = new Font("Segoe UI", 10);
                txt.Width = 180;
            }

            // === BUTTONS ===
            StyleButton(btnAdd, "Add", Color.MediumSeaGreen, Color.White);
            StyleButton(btnUpdate, "Update", Color.SteelBlue, Color.White);
            StyleButton(btnDelete, "Delete", Color.IndianRed, Color.White);
            StyleButton(btnRefresh, "Refresh", Color.Orange, Color.White);
            StyleButton(btnClear, "Clear", Color.Gray, Color.White);
            StyleButton(button1, "Close", Color.DarkRed, Color.White);

            // === DATAGRIDVIEW ===
            dgvVoters.BackgroundColor = Color.White;
            dgvVoters.GridColor = Color.LightGray;
            dgvVoters.DefaultCellStyle.BackColor = Color.White;
            dgvVoters.DefaultCellStyle.ForeColor = Color.Black;
            dgvVoters.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvVoters.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvVoters.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVoters.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvVoters.EnableHeadersVisualStyles = false;
            dgvVoters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // === FILTER LABEL (CREATE FIRST) ===
            lblFilter = new Label();
            lblFilter.Text = "Filter by Program:";
            lblFilter.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFilter.ForeColor = Color.White;
            lblFilter.BackColor = Color.Transparent;
            lblFilter.AutoSize = true;
            lblFilter.Parent = pictureBox1;

            // === FILTER COMBOBOX (CREATE SECOND) ===
            cmbFilterProgram = new ComboBox();
            cmbFilterProgram.Name = "cmbFilterProgram";
            cmbFilterProgram.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterProgram.Items.Add("All");
            cmbFilterProgram.Items.Add("CS");
            cmbFilterProgram.Items.Add("IT");
            cmbFilterProgram.SelectedIndex = 0;
            cmbFilterProgram.Width = 150;
            cmbFilterProgram.Font = new Font("Segoe UI", 10);
            cmbFilterProgram.SelectedIndexChanged += cmbFilterProgram_SelectedIndexChanged;

            // Add controls to form
            this.Controls.Add(cmbFilterProgram);

            PositionControls();
            LoadVoters();
        }

        private void cmbFilterProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterProgram.SelectedItem == null) return;

            string selectedProgram = cmbFilterProgram.SelectedItem.ToString();

            try
            {
                if (selectedProgram == "All")
                {
                    LoadVoters(); // Use existing LoadVoters method
                }
                else
                {
                    // Try stored procedure first
                    try
                    {
                        var dt = DataAccess.ExecuteProcedureToDataTable(
                            "sp_GetVotersByProgram",
                            new MySqlParameter("@Program", selectedProgram)
                        );
                        dgvVoters.DataSource = dt;
                    }
                    catch
                    {
                        // If stored procedure doesn't exist, use DataView filtering
                        var allData = DataAccess.ExecuteProcedureToDataTable("sp_GetAllVoters");
                        DataView dv = allData.DefaultView;
                        dv.RowFilter = $"Program = '{selectedProgram}'";
                        dgvVoters.DataSource = dv.ToTable();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error filtering voters: " + ex.Message);
            }
        }

        private void dgvVoters_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtVoterNumber_TextChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }

        private void StyleButton(Button btn, string text, Color backColor, Color foreColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(120, 40);

            btn.MouseEnter += (s, e) => { btn.BackColor = ControlPaint.Dark(backColor); };
            btn.MouseLeave += (s, e) => { btn.BackColor = backColor; };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}