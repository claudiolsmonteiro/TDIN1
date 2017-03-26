namespace Client
{
    partial class ChatWindow
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
            this.SendButton = new System.Windows.Forms.Button();
            this.MsgBox = new System.Windows.Forms.TextBox();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.SendFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(592, 457);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "SendMsg";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendMessage);
            // 
            // MsgBox
            // 
            this.MsgBox.Location = new System.Drawing.Point(43, 448);
            this.MsgBox.Multiline = true;
            this.MsgBox.Name = "MsgBox";
            this.MsgBox.Size = new System.Drawing.Size(513, 44);
            this.MsgBox.TabIndex = 1;
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(43, 29);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(513, 357);
            this.ChatBox.TabIndex = 2;
            // 
            // SendFileButton
            // 
            this.SendFileButton.Location = new System.Drawing.Point(592, 363);
            this.SendFileButton.Name = "SendFileButton";
            this.SendFileButton.Size = new System.Drawing.Size(75, 23);
            this.SendFileButton.TabIndex = 3;
            this.SendFileButton.Text = "SendFile";
            this.SendFileButton.UseVisualStyleBackColor = true;
            this.SendFileButton.Click += new System.EventHandler(this.SendFile);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 536);
            this.Controls.Add(this.SendFileButton);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.MsgBox);
            this.Controls.Add(this.SendButton);
            this.Name = "ChatWindow";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatWindow_FormClosed);
            this.Load += new System.EventHandler(this.ChatWindow_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox MsgBox;
        private System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.Button SendFileButton;
    }
}