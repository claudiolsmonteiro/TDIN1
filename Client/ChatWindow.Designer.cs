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
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(592, 457);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendMessage);
            // 
            // MessageBox
            // 
            this.MessageBox.Location = new System.Drawing.Point(43, 448);
            this.MessageBox.Multiline = true;
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.Size = new System.Drawing.Size(513, 44);
            this.MessageBox.TabIndex = 1;
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(43, 29);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(513, 357);
            this.ChatBox.TabIndex = 2;
            //this.ChatBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 536);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.SendButton);
            this.Name = "ChatWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.Load += new System.EventHandler(this.ChatWindow_Load);
            this.PerformLayout();
            this.FormClosing += ChatWindow_Closed;

        }

        #endregion

        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox MessageBox;
        private System.Windows.Forms.TextBox ChatBox;
    }
}