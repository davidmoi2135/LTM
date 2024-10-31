namespace chess
{
    partial class MatchGame
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
            this.btn_find = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_find
            // 
            this.btn_find.Location = new System.Drawing.Point(534, 85);
            this.btn_find.Name = "btn_find";
            this.btn_find.Size = new System.Drawing.Size(101, 77);
            this.btn_find.TabIndex = 0;
            this.btn_find.Text = "Find";
            this.btn_find.UseVisualStyleBackColor = true;
            this.btn_find.Click += new System.EventHandler(this.btn_find_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(187, 209);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "label1";
            this.lblStatus.Click += new System.EventHandler(this.label1_Click);
            // 
            // MatchGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btn_find);
            this.Name = "MatchGame";
            this.Text = "MatchGame";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_find;
        private System.Windows.Forms.Label lblStatus;
    }
}