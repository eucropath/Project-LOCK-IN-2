using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
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
    public partial class VoterRegistration : Form
    {
        public VoterRegistration()
        {
            InitializeComponent();
            this.Load += VoterRegistration_Load;

        }
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbProgram;
        private System.Windows.Forms.ComboBox cmbYearLevel;


        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;
            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 40;

            int startY = 120;
            int spacingY = 60;

            // Arrange labels + textboxes
            (Label lbl, Control ctrl)[] fields = {
        (label2, txtFirstName),
        (label4, txtLastName),
        (label9, cmb),
        (label10, comboBox1),
        (label3, txtEmail),
        (label5, txtUsername),
        (label6, txtPassword)
    };

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].lbl.Left = centerX - 200;
                fields[i].lbl.Top = startY + (spacingY * i);
                fields[i].ctrl.Left = centerX;
                fields[i].ctrl.Top = fields[i].lbl.Top - 5;
            }

            // Position buttons neatly centered below password
            int buttonsTop = fields.Last().lbl.Top + 70;
            int totalWidth = (button1.Width + btnRegister.Width + btnExit.Width) + 40;
            int buttonsLeft = centerX - (totalWidth / 2);

            button1.Left = buttonsLeft;
            button1.Top = buttonsTop;

            btnRegister.Left = button1.Right + 20;
            btnRegister.Top = buttonsTop;

            btnExit.Left = btnRegister.Right + 20;
            btnExit.Top = buttonsTop;

            // Status label below buttons
            lblStatus.Left = centerX - (lblStatus.Width / 2);
            lblStatus.Top = buttonsTop + 60;
        }

        
        private void StyleLabel(Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lbl.ForeColor = ColorTranslator.FromHtml("#0A2E5C"); 
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y);
            if (pictureBox1 != null) lbl.Parent = pictureBox1;

       
            lbl.Parent = pictureBox1;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            // Validate input
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                lblStatus.Text = "First name is required.";
                lblStatus.ForeColor = Color.Red;
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                lblStatus.Text = "Last name is required.";
                lblStatus.ForeColor = Color.Red;
                txtLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblStatus.Text = "Email is required.";
                lblStatus.ForeColor = Color.Red;
                txtEmail.Focus();
                return;
            }

            if (cmb.SelectedIndex < 0)
            {
                lblStatus.Text = "Please select a program.";
                lblStatus.ForeColor = Color.Red;
                cmb.Focus();
                return;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                lblStatus.Text = "Please select a year level.";
                lblStatus.ForeColor = Color.Red;
                comboBox1.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblStatus.Text = "Username is required.";
                lblStatus.ForeColor = Color.Red;
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "Password is required.";
                lblStatus.ForeColor = Color.Red;
                txtPassword.Focus();
                return;
            }

            // === Username length ===
            if (txtUsername.Text.Trim().Length < 4)
            {
                lblStatus.Text = "Username must be at least 4 characters long.";
                lblStatus.ForeColor = Color.Red;
                txtUsername.Focus();
                return;
            }

            // === Email check ===
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                lblStatus.Text = "Please enter a valid email address.";
                lblStatus.ForeColor = Color.Red;
                txtEmail.Focus();
                return;
            }

            // === Password strength ===
            string passwordError;
            if (!ValidatePassword(txtPassword.Text, out passwordError))
            {
                lblStatus.Text = passwordError;
                lblStatus.ForeColor = Color.Red;
                txtPassword.Focus();
                return;
            }
            try
            {
                string passwordHash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(txtPassword.Text.Trim());
                    var hashBytes = sha256.ComputeHash(bytes);
                    passwordHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                string program = cmb.SelectedItem.ToString();

                // Convert "BSCS" or "BSIT" to just "CS" or "IT" for the database
                if (program.Contains("CS"))
                    program = "CS";
                else if (program.Contains("IT"))
                    program = "IT";

                int yearLevel = Convert.ToInt32(comboBox1.SelectedItem.ToString());

                // FIXED: All parameters must be inside the ExecuteProcedureNonQuery call
                int rowsAffected = DataAccess.ExecuteProcedureNonQuery("sp_RegisterVoter",
                    new MySqlParameter("@p_FirstName", txtFirstName.Text.Trim()),
                    new MySqlParameter("@p_LastName", txtLastName.Text.Trim()),
                    new MySqlParameter("@p_Email", txtEmail.Text.Trim()),
                    new MySqlParameter("@p_Program", program),
                    new MySqlParameter("@p_YearLevel", yearLevel),
                    new MySqlParameter("@p_Username", txtUsername.Text.Trim()),
                    new MySqlParameter("@p_PasswordHash", passwordHash)
                ); // ← Notice: closing parenthesis is HERE, after all parameters

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registration successful! Now please log in.",
                                    "Success",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    // Clear fields
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtEmail.Text = "";
                    cmb.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    lblStatus.Text = "";

                    // Go to login
                    this.Hide();
                    var loginForm = new LoginForm();
                    loginForm.Show();
                }
                else
                {
                    lblStatus.Text = "Registration failed. Please try again.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry
                {
                    if (ex.Message.Contains("Username"))
                    {
                        lblStatus.Text = "Username already exists. Please choose another.";
                    }
                    else if (ex.Message.Contains("Email"))
                    {
                        lblStatus.Text = "Email address is already registered.";
                    }
                    else
                    {
                        lblStatus.Text = "Username or email already exists.";
                    }
                }
                else
                {
                    lblStatus.Text = $"Database Error: {ex.Message}";
                }
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
        private bool ValidatePassword(string password, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password cannot be empty.";
                return false;
            }

            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            // Check for at least one lowercase letter
            if (!password.Any(char.IsLower))
            {
                errorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Password must contain at least one number.";
                return false;
            }

            // Optional: Check for special character
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errorMessage = "Password must contain at least one special character.";
                return false;
            }

            return true;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void VoterRegistration_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Load event fired. Program items count: {cmb.Items.Count}");

            this.Text = "Voter Registration";// ALL forms must have this brosssss
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 700); // fixed window size(changed to 700 to fit everything)
            this.AcceptButton = btnRegister;

            // === Background ===
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SendToBack();

            // === Title Label ===
            label1.Text = "Voter Registration";
            label1.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Transparent;
            label1.AutoSize = true;
            label1.Parent = pictureBox1;

            // === Labels ===
            StyleLabel(label2, "First Name:", 0, 0);
            label2.ForeColor = Color.White;
            StyleLabel(label4, "Last Name:", 0, 0);
            label4.ForeColor = Color.White;

            // NEW: Program Label
            StyleLabel(label9, "Program:", 0, 0);
            label9.ForeColor = Color.White;

            // NEW: Year Level Label
            StyleLabel(label10, "Year Level:", 0, 0);
            label10.ForeColor = Color.White;

            StyleLabel(label3, "Email:", 0, 0);
            label3.ForeColor = Color.White; 

            StyleLabel(label5, "Username:", 0, 0);
            label5.ForeColor = Color.White;

            StyleLabel(label6, "Password:", 0, 0);
            label6.ForeColor = Color.White;

            // === Textboxes ===
            StyleTextBox(txtFirstName, 0, 0);
            StyleTextBox(txtLastName, 0, 0);
            StyleTextBox(txtEmail, 0, 0);
            StyleTextBox(txtUsername, 0, 0);
            StyleTextBox(txtPassword, 0, 0);
            txtPassword.PasswordChar = '*';

            // === NEW: ComboBoxes ===
            StyleComboBox(cmb, 0, 0);
            StyleComboBox(comboBox1, 0, 0);

            cmb.Items.Clear();
            cmb.Items.AddRange(new object[] { "BSIT", "BSCS" });
            cmb.SelectedIndex = 0;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "1", "2", "3", "4" });
            comboBox1.SelectedIndex = 0;

            // === Buttons ===
            StyleButton(button1, "Login", 0, 0, ColorTranslator.FromHtml("#0A2E5C"), Color.White);
            StyleButton(btnRegister, "Register", 0, 0, Color.White, ColorTranslator.FromHtml("#0D47A1"));
            StyleButton(btnExit, "Exit", 0, 0, Color.White, Color.FromArgb(231, 76, 60));

            // === Status Label ===
            lblStatus.Text = "";
            lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblStatus.ForeColor = Color.White;
            lblStatus.BackColor = Color.Transparent;
            lblStatus.AutoSize = true;
            lblStatus.Parent = pictureBox1;


            // Center everything
            CenterControls();
            foreach (Control c in this.Controls)
            {
                if (c.Location == new Point(0, 0))
                {
                    System.Diagnostics.Debug.WriteLine($"Control at 0,0 => Name: {c.Name} Type: {c.GetType().Name}");
                }
            }


            // Recenter controls when window resizes
            this.Resize += (s, ev) => CenterControls();
        }



        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void StyleComboBox(ComboBox cmb, int x, int y)
        {
            cmb.Size = new Size(200, 30);
            cmb.Font = new Font("Segoe UI", 12);
            cmb.Location = new Point(x, y);
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Color.White;

            if (pictureBox1 != null) cmb.Parent = pictureBox1;
        }
        private void StyleTextBox(TextBox txt, int x, int y)
        {
            txt.Size = new Size(200, 30);
            txt.Font = new Font("Segoe UI", 12);
            txt.Location = new Point(x, y);

            if (pictureBox1 != null) txt.Parent = pictureBox1;
        }
       
        private void StyleButton(Button btn, string text, int x, int y, Color foreColor, Color backColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.ForeColor = foreColor;
            btn.BackColor = backColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(backColor, 0.1f);  // slightly darker on hover
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.2f);  // darker on click
            btn.FlatAppearance.BorderColor = backColor; // ensure no white edge
            btn.TabStop = false; // remove blue outline on focus
            btn.UseVisualStyleBackColor = false; // prevent system color overlay
            btn.Size = new Size(120, 45);
            btn.Location = new Point(x, y);
            btn.Cursor = Cursors.Hand;
            btn.Parent = pictureBox1;
        }
     

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }


            private void pictureBox1_Click_1(object sender, EventArgs e)
            {
            
            }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(password))
            {
                lblStatus.Text = "";
                return;
            }

            string error;
            if (ValidatePassword(password, out error))
            {
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "✓ Strong password";
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Orange;
                lblStatus.Text = error;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
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
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }

       
    }
        

