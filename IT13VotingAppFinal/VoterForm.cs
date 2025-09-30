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
        public VoterForm()
        {
            InitializeComponent();
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnClear.Click += btnClear_Click;

            dgvVoters.SelectionChanged += dgvVoters_SelectionChanged;

            LoadVoters();
        }
        private void LoadVoters()
        {
            var dt = DataAccess.ExecuteProcedureToDataTable("sp_GetAllVoters"); // you need sp_GetAllVoters

            dgvVoters.DataSource = dt;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_AddVoter",
                    
                   new MySqlParameter("@in_fn", txtFirstName.Text.Trim()),
                   new MySqlParameter("@in_ln", txtLastName.Text.Trim()),
                   new MySqlParameter("@in_email", txtEmail.Text.Trim())
                   );
                 LoadVoters();
                ClearFields();
                MessageBox.Show("Voter added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding voter: " + ex.Message);
            }
        }
        

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a voter to update.");
                return;
            }

            // Get the Id from the selected row in the DataGridView
            var row = dgvVoters.SelectedRows[0];
            string id = row.Cells["VoterId"].Value.ToString(); 
            
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_UpdateVoter",
                   new MySqlParameter("@in_id", id),
                 
                   new MySqlParameter("@in_fn", firstName),
                   new MySqlParameter("@in_ln", lastName),
                   new MySqlParameter("@in_email", email));
                LoadVoters();
                ClearFields();
                MessageBox.Show("Voter updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating voter: " + ex.Message);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a voter to delete.");
                return;
            }

            // Get the Id from the selected row in the DataGridView
            var row = dgvVoters.SelectedRows[0];
            string id = row.Cells["VoterId"].Value.ToString(); // Use the actual column name

            try
            {
                DataAccess.ExecuteProcedureNonQuery("sp_DeleteVoter",
                    new MySqlParameter("@in_id", id));
                LoadVoters();
                ClearFields();
                MessageBox.Show("Voter deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting voter: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadVoters();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
        
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
        }
        private void dgvVoters_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVoters.SelectedRows.Count > 0)
            {
                var row = dgvVoters.SelectedRows[0];
            
                ;
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        private void VoterForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvVoters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
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

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtVoterNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void ArrangeLayout()
        {
            // Panel for inputs
            FlowLayoutPanel inputPanel = new FlowLayoutPanel();
            inputPanel.Dock = DockStyle.Top;
            inputPanel.Height = 70;
            inputPanel.Padding = new Padding(10);
            inputPanel.FlowDirection = FlowDirection.LeftToRight;
            inputPanel.WrapContents = false;

            // Add fields in row
            inputPanel.Controls.Add(label2);
           
            inputPanel.Controls.Add(label3);
            inputPanel.Controls.Add(txtFirstName);
            inputPanel.Controls.Add(label4);
            inputPanel.Controls.Add(txtLastName);
            inputPanel.Controls.Add(this.Controls["labelEmail"]);
            inputPanel.Controls.Add(txtEmail);

            this.Controls.Add(inputPanel);

            // Panel for buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 70;
            buttonPanel.Padding = new Padding(10);
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.WrapContents = false;

            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete, btnRefresh, btnClear });

            this.Controls.Add(buttonPanel);
        }
        private void StyleControls()
        {
            // Title
          
            // Labels
            foreach (Label lbl in new[] { label2, label3, label4 })
            {
                lbl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                lbl.AutoSize = true;
            }

            // Email label (make sure label4 is Email in Designer)
            label2.Text = "Voter Number:";
            label3.Text = "First Name:";
            label4.Text = "Last Name:";
            // Add a label for Email if missing
            Label lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Name = "labelEmail"
            };
            this.Controls.Add(lblEmail);

            // Textboxes
            foreach (TextBox txt in new[] { txtFirstName, txtLastName, txtEmail })
            {
                txt.Font = new Font("Segoe UI", 10);
                txt.Width = 180;
            }

            // Buttons styling
            foreach (Button btn in new[] { btnAdd, btnUpdate, btnDelete, btnRefresh, btnClear })
            {
                btn.Width = 120;
                btn.Height = 40;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
            }

            btnAdd.BackColor = Color.FromArgb(0, 123, 255);      // Blue
            btnUpdate.BackColor = Color.FromArgb(40, 167, 69);   // Green
            btnDelete.BackColor = Color.FromArgb(220, 53, 69);   // Red
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125); // Gray
            btnClear.BackColor = Color.FromArgb(255, 193, 7);    // Yellow

            // DataGridView styling
            dgvVoters.BackgroundColor = Color.White;
            dgvVoters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVoters.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvVoters.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvVoters.EnableHeadersVisualStyles = false;
            dgvVoters.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvVoters.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dgvVoters.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvVoters.Dock = DockStyle.Top;
            dgvVoters.Height = this.ClientSize.Height / 2;
        }
        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            // Title
         

            // Input fields (stacked vertically, like screenshot)
            int inputTop = 80;

            label2.Left = centerX - 200; label2.Top = inputTop;
       

            label3.Left = centerX - 200; label3.Top = inputTop + 50;
            txtFirstName.Left = centerX; txtFirstName.Top = inputTop + 45;

            label4.Left = centerX - 200; label4.Top = inputTop + 100;
            txtLastName.Left = centerX; txtLastName.Top = inputTop + 95;

            var lblEmail = (Label)this.Controls["labelEmail"];
            lblEmail.Left = centerX - 200; lblEmail.Top = inputTop + 150;
            txtEmail.Left = centerX; txtEmail.Top = inputTop + 145;

            // Buttons row
            int buttonsTop = inputTop + 210;
            int totalWidth = (btnAdd.Width + btnUpdate.Width + btnDelete.Width + btnRefresh.Width + btnClear.Width) + (20 * 4);
            int buttonsLeft = centerX - (totalWidth / 2);

            btnAdd.Left = buttonsLeft; btnAdd.Top = buttonsTop;
            btnUpdate.Left = btnAdd.Right + 20; btnUpdate.Top = buttonsTop;
            btnDelete.Left = btnUpdate.Right + 20; btnDelete.Top = buttonsTop;
            btnRefresh.Left = btnDelete.Right + 20; btnRefresh.Top = buttonsTop;
            btnClear.Left = btnRefresh.Right + 20; btnClear.Top = buttonsTop;

            // DataGridView (below everything)
            dgvVoters.Left = 50;
            dgvVoters.Top = buttonsTop + 80;
            dgvVoters.Width = this.ClientSize.Width - 100;
            dgvVoters.Height = this.ClientSize.Height - dgvVoters.Top - 50;
        }

    }
}

