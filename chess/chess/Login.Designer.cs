namespace chess
{
    partial class Login
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
            this.btn_login = new System.Windows.Forms.Button();
            this.txt_mk = new System.Windows.Forms.TextBox();
            this.txt_tk = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_login = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(510, 291);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(110, 60);
            this.btn_login.TabIndex = 11;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click_1);
            // 
            // txt_mk
            // 
            this.txt_mk.Location = new System.Drawing.Point(308, 227);
            this.txt_mk.Name = "txt_mk";
            this.txt_mk.Size = new System.Drawing.Size(303, 22);
            this.txt_mk.TabIndex = 10;
            // 
            // txt_tk
            // 
            this.txt_tk.Location = new System.Drawing.Point(308, 178);
            this.txt_tk.Name = "txt_tk";
            this.txt_tk.Size = new System.Drawing.Size(303, 22);
            this.txt_tk.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(183, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mật khẩu";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(180, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 52);
            this.label1.TabIndex = 7;
            this.label1.Text = "Tài khoản";
            // 
            // lbl_login
            // 
            this.lbl_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbl_login.Location = new System.Drawing.Point(355, 99);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(228, 57);
            this.lbl_login.TabIndex = 6;
            this.lbl_login.Text = "CỜ VUA ONLINE ";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.txt_mk);
            this.Controls.Add(this.txt_tk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_login);
            this.Name = "Login";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox txt_mk;
        private System.Windows.Forms.TextBox txt_tk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_login;
    }
}

