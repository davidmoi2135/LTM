namespace chess
{
    partial class ChessBoardForm
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
            this.lblPlayerColor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPlayerColor
            // 
            this.lblPlayerColor.Location = new System.Drawing.Point(12, 23);
            this.lblPlayerColor.Name = "lblPlayerColor";
            this.lblPlayerColor.Size = new System.Drawing.Size(273, 23);
            this.lblPlayerColor.TabIndex = 0;
            this.lblPlayerColor.Text = "label1";
            // 
            // ChessBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblPlayerColor);
            this.Name = "ChessBoardForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.ChessBoardForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerColor;
    }
}