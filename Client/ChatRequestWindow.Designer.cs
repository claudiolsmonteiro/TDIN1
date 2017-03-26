namespace Client
{
    partial class ChatRequestWindow
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
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.DenyButton = new System.Windows.Forms.Button();
            this.ChatTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AcceptButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(37, 207);
            this.ConfirmButton.Name = "AcceptButton";
            this.ConfirmButton.Size = new System.Drawing.Size(75, 23);
            this.ConfirmButton.TabIndex = 0;
            this.ConfirmButton.Text = "Accept";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.AcceptRequest);

            // 
            // DenyButton
            // 
            this.DenyButton.Location = new System.Drawing.Point(168, 207);
            this.DenyButton.Name = "DenyButton";
            this.DenyButton.Size = new System.Drawing.Size(75, 23);
            this.DenyButton.TabIndex = 1;
            this.DenyButton.Text = "Deny";
            this.DenyButton.UseVisualStyleBackColor = true;
            this.DenyButton.Click += new System.EventHandler(this.DenyRequest);
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(37, 45);
            this.ChatTextBox.Multiline = true;
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.ReadOnly = true;
            this.ChatTextBox.Size = new System.Drawing.Size(206, 125);
            this.ChatTextBox.TabIndex = 2;
            // 
            // ChatRequestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.DenyButton);
            this.Controls.Add(this.ConfirmButton);
            this.Name = "ChatRequestWindow";
            this.Text = "ChatRequestWindow";
            this.ResumeLayout(false);
            this.FormClosing += (ChatRequestWindow_Closed);
            //this.Load += new System.EventHandler(this.ChatReqWindow_Load);
            //this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button DenyButton;
        private System.Windows.Forms.TextBox ChatTextBox;
    }
}