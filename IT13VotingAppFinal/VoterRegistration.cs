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
            private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;


            label1.Left = centerX - (label1.Width / 2);
            label1.Top = 40;

            int startY = 130;
            int spacingY = 60;

       
            label2.Left = centerX - 200;
            label2.Top = startY;
            txtFirstName.Left = centerX;
            txtFirstName.Top = startY - 5;


            label4.Left = centerX - 200;
            label4.Top = startY + spacingY;
            txtLastName.Left = centerX;
            txtLastName.Top = startY + spacingY - 5;


            label3.Left = centerX - 200;
            label3.Top = startY + (spacingY * 2);
            txtEmail.Left = centerX;
            txtEmail.Top = startY + (spacingY * 2) - 5;


            label5.Left = centerX - 200;
            label5.Top = startY + (spacingY * 3);
            txtUsername.Left = centerX;
            txtUsername.Top = startY + (spacingY * 3) - 5;


            label6.Left = centerX - 200;
            label6.Top = startY + (spacingY * 4);
            txtPassword.Left = centerX;
            txtPassword.Top = startY + (spacingY * 4) - 5;

            
            int buttonsTop = startY + (spacingY * 5) + 20;
            int totalWidth = (button1.Width + btnRegister.Width + btnExit.Width) + 40;
            int buttonsLeft = centerX - (totalWidth / 2);

            button1.Left = buttonsLeft;
            button1.Top = buttonsTop;

            btnRegister.Left = button1.Right + 20;
            btnRegister.Top = buttonsTop;

            btnExit.Left = btnRegister.Right + 20;
            btnExit.Top = buttonsTop;

     
            lblStatus.Left = centerX - (lblStatus.Width / 2);
            lblStatus.Top = buttonsTop + 70;
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
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
                
            {
                lblStatus.Text = "Please fill in all fields.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
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
                int rowsAffected = DataAccess.ExecuteProcedureNonQuery("sp_RegisterVoter",
                    new MySqlParameter("p_FirstName", txtFirstName.Text.Trim()),
                    new MySqlParameter("p_LastName", txtLastName.Text.Trim()),
                    new MySqlParameter("p_Email", txtEmail.Text.Trim()),
                    new MySqlParameter("p_Username", txtUsername.Text.Trim()),
                    new MySqlParameter("p_PasswordHash", passwordHash));
                    new MySqlParameter("@Role", "Voter");

                if (rowsAffected > 0)
                {
                    //added
                    MessageBox.Show("Registration successful! Now please log in.",
                                    "Success",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);//Added
                    lblStatus.Text = "Registration successful! You can now log in.";
                    this.Hide();
                    var loginForm = new LoginForm();
                    loginForm.Show();
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    txtFirstName.Text = txtLastName.Text = txtEmail.Text = "";
                }
                else
                {
                    lblStatus.Text = "Registration failed.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry
                    lblStatus.Text = "Email address is already registered.";
                else
                    lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
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

            this.Text = "Voter Registration";// ALL forms must have this brosssss
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600); // fixed window size

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


            MakeRounded(txtFirstName, 15);
            MakeRounded(txtLastName, 15);
            MakeRounded(txtEmail, 15);
            MakeRounded(txtUsername, 15);
            MakeRounded(txtPassword, 15);

            MakeRounded(button1, 20);
            MakeRounded(btnRegister, 20);
            MakeRounded(btnExit, 20);

                



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
        private void StyleTextBox(TextBox txt, int x, int y)
        {
            txt.Size = new Size(200, 30);
            txt.Font = new Font("Segoe UI", 12);
            txt.Location = new Point(x, y);

            if (pictureBox1 != null) txt.Parent = pictureBox1;
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
        private void StyleButton(Button btn, string text, int x, int y, Color foreColor, Color backColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.ForeColor = foreColor;
            btn.BackColor = backColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(100, 40);
            btn.Location = new Point(x, y);
            btn.Cursor = Cursors.Hand;
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

        }

       
    }
        }

