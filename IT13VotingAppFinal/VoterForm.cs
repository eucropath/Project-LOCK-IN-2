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

            dgvVoters.SelectionChanged += dgvVoters_SelectionChanged;

            this.Load += VoterForm_Load;
            this.Resize += VoterForm_Resize;
        }

        private static bool _isShowingMessage = false;

        private void ShowMessage(string message, string title = "Notice")
        {
            if (_isShowingMessage) return; // prevent duplicates
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

    // === DataGridView directly under Title ===
    dgvVoters.Left = 50;
    dgvVoters.Top = label1.Bottom + 30;
    dgvVoters.Width = this.ClientSize.Width - 100;
    dgvVoters.Height = this.ClientSize.Height / 3;

    // === Inputs under DataGridView ===
    int inputTop = dgvVoters.Bottom + 30;

    label2.Left = centerX - 200; label2.Top = inputTop;
    txtFirstName.Left = centerX; txtFirstName.Top = inputTop;

    label3.Left = centerX - 200; label3.Top = inputTop + 50;
    txtLastName.Left = centerX; txtLastName.Top = inputTop + 50;

    label4.Left = centerX - 200; label4.Top = inputTop + 100;
    txtEmail.Left = centerX; txtEmail.Top = inputTop + 100;

    // === Buttons row at bottom ===
    int buttonsTop = txtEmail.Bottom + 50;
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
              ShowMessage("Please fill all fields.");
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

            // Get the Id from the selected row in the DataGridView
            var row = dgvVoters.SelectedRows[0];
            string id = row.Cells["VoterId"].Value.ToString(); // Use the actual column name

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
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
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

            // ✅ Re-parent all labels to the PictureBox
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

            label4.Text = "Email";
            label4.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label4.ForeColor = Color.White;
label4.BackColor = Color.Transparent;
            label4.AutoSize = true;


            // === TEXTBOXES ===
            foreach (TextBox txt in new[] { txtFirstName, txtLastName, txtEmail })
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
            MakeRounded(button1, 15);

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

              MakeRounded(btnAdd, 15);
    MakeRounded(btnUpdate, 15);
    MakeRounded(btnDelete, 15);
    MakeRounded(btnRefresh, 15);
    MakeRounded(btnClear, 15);

    MakeRounded(txtFirstName, 10);
    MakeRounded(txtLastName, 10);
    MakeRounded(txtEmail, 10);

            PositionControls();
            LoadVoters();
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
            button1.Left = btnClear.Right + 20; button1.Top = buttonsTop;

            // DataGridView (below everything)
            dgvVoters.Left = 50;
            dgvVoters.Top = buttonsTop + 80;
            dgvVoters.Width = this.ClientSize.Width - 100;
            dgvVoters.Height = this.ClientSize.Height - dgvVoters.Top - 50;
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

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

