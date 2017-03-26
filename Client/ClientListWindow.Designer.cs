﻿using System.Windows.Forms;

namespace Client
{
    partial class ClientListWindow
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
            this.LogOut = new System.Windows.Forms.Button();
            this.ClientList = new System.Windows.Forms.ListView();
            this.InitChatButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LogOut
            // 
            this.LogOut.Location = new System.Drawing.Point(289, 332);
            this.LogOut.Name = "LogOut";
            this.LogOut.Size = new System.Drawing.Size(75, 23);
            this.LogOut.TabIndex = 2;
            this.LogOut.Text = "LogOut";
            this.LogOut.UseVisualStyleBackColor = true;
            this.LogOut.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // ClientList
            // 
            this.ClientList.Location = new System.Drawing.Point(1, 0);
            this.ClientList.Name = "ClientList";
            this.ClientList.Size = new System.Drawing.Size(374, 327);
            this.ClientList.TabIndex = 3;
            this.ClientList.UseCompatibleStateImageBehavior = false;
            this.ClientList.SelectedIndexChanged += new System.EventHandler(this.ClientList_SelectedIndexChanged);
            this.ClientList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // initChatButton
            // 
            this.InitChatButton.Location = new System.Drawing.Point(12, 332);
            this.InitChatButton.Name = "initChatButton";
            this.InitChatButton.Size = new System.Drawing.Size(75, 23);
            this.InitChatButton.TabIndex = 4;
            this.InitChatButton.Text = "Chat";
            this.InitChatButton.UseVisualStyleBackColor = true;
            this.InitChatButton.Click += new System.EventHandler(this.InitChatButton_Click);
            // 
            // ClientListWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 368);
            this.Controls.Add(this.InitChatButton);
            this.Controls.Add(this.ClientList);
            this.Controls.Add(this.LogOut);
            this.Name = "ClientListWindow";
            this.Text = "Client List";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.ResumeLayout(false);
            this.FormClosing += ClientWindow_FormClosed;

        }

        #endregion

        private System.Windows.Forms.Button LogOut;
        private System.Windows.Forms.ListView ClientList;
        private Button InitChatButton;
    }
}