namespace IT13VotingAppFinal
{
    partial class VotingSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpResultsAvailable = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblResultsTime = new System.Windows.Forms.Label();
            this.lblCurrentSetting = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpResultsAvailable
            // 
            this.dtpResultsAvailable.CustomFormat = "MMMM dd, yyyy - hh:mm tt";
            this.dtpResultsAvailable.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpResultsAvailable.Location = new System.Drawing.Point(20, 150);
            this.dtpResultsAvailable.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtpResultsAvailable.Name = "dtpResultsAvailable";
            this.dtpResultsAvailable.Size = new System.Drawing.Size(440, 20);
            this.dtpResultsAvailable.TabIndex = 0;
            this.dtpResultsAvailable.ValueChanged += new System.EventHandler(this.dtpResultsAvailable_ValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 220);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 40);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(190, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 40);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(173, 30);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Voting Settings";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblResultsTime
            // 
            this.lblResultsTime.AutoSize = true;
            this.lblResultsTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblResultsTime.Location = new System.Drawing.Point(20, 120);
            this.lblResultsTime.Name = "lblResultsTime";
            this.lblResultsTime.Size = new System.Drawing.Size(201, 20);
            this.lblResultsTime.TabIndex = 4;
            this.lblResultsTime.Text = "Results Available Date & Time:";
            this.lblResultsTime.Click += new System.EventHandler(this.lblResultsTime_Click);
            // 
            // lblCurrentSetting
            // 
            this.lblCurrentSetting.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCurrentSetting.ForeColor = System.Drawing.Color.Gray;
            this.lblCurrentSetting.Location = new System.Drawing.Point(20, 60);
            this.lblCurrentSetting.Name = "lblCurrentSetting";
            this.lblCurrentSetting.Size = new System.Drawing.Size(440, 40);
            this.lblCurrentSetting.TabIndex = 5;
            this.lblCurrentSetting.Text = "Loading current settings...";
            this.lblCurrentSetting.Click += new System.EventHandler(this.lblCurrentSetting_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::IT13VotingAppFinal.Properties.Resources.Setting_NEw_Bg;
            this.pictureBox1.Location = new System.Drawing.Point(345, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // VotingSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 350);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblCurrentSetting);
            this.Controls.Add(this.lblResultsTime);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtpResultsAvailable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VotingSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Voting Settings";
            this.Load += new System.EventHandler(this.VotingSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpResultsAvailable;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblResultsTime;
        private System.Windows.Forms.Label lblCurrentSetting;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}