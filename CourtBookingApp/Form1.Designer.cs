namespace CourtBookingApp
{
    partial class Form1
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
            this.lbUsername = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.lbNotification = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.cbTime = new System.Windows.Forms.ComboBox();
            this.lbPeriod = new System.Windows.Forms.Label();
            this.cbPeriod = new System.Windows.Forms.ComboBox();
            this.lbNote = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.lbDelay = new System.Windows.Forms.Label();
            this.tbDelay = new System.Windows.Forms.TextBox();
            this.lbMs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new System.Drawing.Point(40, 59);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(55, 13);
            this.lbUsername.TabIndex = 0;
            this.lbUsername.Text = "Username";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(226, 283);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(36, 100);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(53, 13);
            this.lbPassword.TabIndex = 2;
            this.lbPassword.Text = "Password";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Location = new System.Drawing.Point(37, 169);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(58, 13);
            this.lbTime.TabIndex = 3;
            this.lbTime.Text = "Court Time";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(142, 56);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(159, 20);
            this.tbUserName.TabIndex = 4;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(142, 97);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(159, 20);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // tbTime
            // 
            this.tbTime.Location = new System.Drawing.Point(307, 167);
            this.tbTime.Name = "tbTime";
            this.tbTime.Size = new System.Drawing.Size(94, 20);
            this.tbTime.TabIndex = 6;
            this.tbTime.Visible = false;
            // 
            // lbNotification
            // 
            this.lbNotification.AutoSize = true;
            this.lbNotification.Location = new System.Drawing.Point(139, 258);
            this.lbNotification.Name = "lbNotification";
            this.lbNotification.Size = new System.Drawing.Size(66, 13);
            this.lbNotification.TabIndex = 7;
            this.lbNotification.Text = "Will run after";
            this.lbNotification.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(142, 283);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // cbTime
            // 
            this.cbTime.FormattingEnabled = true;
            this.cbTime.Location = new System.Drawing.Point(142, 166);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(159, 21);
            this.cbTime.TabIndex = 9;
            // 
            // lbPeriod
            // 
            this.lbPeriod.AutoSize = true;
            this.lbPeriod.Location = new System.Drawing.Point(37, 137);
            this.lbPeriod.Name = "lbPeriod";
            this.lbPeriod.Size = new System.Drawing.Size(65, 13);
            this.lbPeriod.TabIndex = 10;
            this.lbPeriod.Text = "Court Period";
            // 
            // cbPeriod
            // 
            this.cbPeriod.FormattingEnabled = true;
            this.cbPeriod.Location = new System.Drawing.Point(142, 134);
            this.cbPeriod.Name = "cbPeriod";
            this.cbPeriod.Size = new System.Drawing.Size(159, 21);
            this.cbPeriod.TabIndex = 11;
            // 
            // lbNote
            // 
            this.lbNote.AutoSize = true;
            this.lbNote.Location = new System.Drawing.Point(36, 200);
            this.lbNote.Name = "lbNote";
            this.lbNote.Size = new System.Drawing.Size(60, 13);
            this.lbNote.TabIndex = 12;
            this.lbNote.Text = "Notification";
            // 
            // cbName
            // 
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(142, 197);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(159, 21);
            this.cbName.TabIndex = 13;
            // 
            // lbDelay
            // 
            this.lbDelay.AutoSize = true;
            this.lbDelay.Location = new System.Drawing.Point(36, 233);
            this.lbDelay.Name = "lbDelay";
            this.lbDelay.Size = new System.Drawing.Size(95, 13);
            this.lbDelay.TabIndex = 14;
            this.lbDelay.Text = "Delay for clock diff";
            // 
            // tbDelay
            // 
            this.tbDelay.Location = new System.Drawing.Point(142, 229);
            this.tbDelay.Name = "tbDelay";
            this.tbDelay.Size = new System.Drawing.Size(159, 20);
            this.tbDelay.TabIndex = 15;
            // 
            // lbMs
            // 
            this.lbMs.AutoSize = true;
            this.lbMs.Location = new System.Drawing.Point(308, 235);
            this.lbMs.Name = "lbMs";
            this.lbMs.Size = new System.Drawing.Size(20, 13);
            this.lbMs.TabIndex = 16;
            this.lbMs.Text = "ms";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 429);
            this.Controls.Add(this.lbMs);
            this.Controls.Add(this.tbDelay);
            this.Controls.Add(this.lbDelay);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.lbNote);
            this.Controls.Add(this.cbPeriod);
            this.Controls.Add(this.lbPeriod);
            this.Controls.Add(this.cbTime);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lbNotification);
            this.Controls.Add(this.tbTime);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbUsername);
            this.Name = "Form1";
            this.Text = "Court Booking Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Label lbNotification;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cbTime;
        private System.Windows.Forms.Label lbPeriod;
        private System.Windows.Forms.ComboBox cbPeriod;
        private System.Windows.Forms.Label lbNote;
        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.Label lbDelay;
        private System.Windows.Forms.TextBox tbDelay;
        private System.Windows.Forms.Label lbMs;
    }
}

