﻿namespace Dice_and_Combat_Engine
{
    partial class MainForm
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
            this.playerNameTextBox = new System.Windows.Forms.TextBox();
            this.playerHPTextBox = new System.Windows.Forms.TextBox();
            this.playerHPLabel = new System.Windows.Forms.Label();
            this.playerRaceClassTextBox = new System.Windows.Forms.TextBox();
            this.playerABTextBox = new System.Windows.Forms.TextBox();
            this.playerABLabel = new System.Windows.Forms.Label();
            this.playerACTextBox = new System.Windows.Forms.TextBox();
            this.playerACLabel = new System.Windows.Forms.Label();
            this.creatureNameTextBox = new System.Windows.Forms.TextBox();
            this.creatureHPLabel = new System.Windows.Forms.Label();
            this.creatureABLabel = new System.Windows.Forms.Label();
            this.creatureACLabel = new System.Windows.Forms.Label();
            this.creatureHPTextBox = new System.Windows.Forms.TextBox();
            this.creatureABTextBox = new System.Windows.Forms.TextBox();
            this.creatureACTextBox = new System.Windows.Forms.TextBox();
            this.feedbackList = new System.Windows.Forms.ListBox();
            this.playerImgBox = new System.Windows.Forms.PictureBox();
            this.creatureImgBox = new System.Windows.Forms.PictureBox();
            this.roomNameLbl = new System.Windows.Forms.Label();
            this.commandTxtBox = new System.Windows.Forms.TextBox();
            this.instructionLbl = new System.Windows.Forms.Label();
            this.actionBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.playerImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.creatureImgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // playerNameTextBox
            // 
            this.playerNameTextBox.Enabled = false;
            this.playerNameTextBox.Location = new System.Drawing.Point(12, 97);
            this.playerNameTextBox.Name = "playerNameTextBox";
            this.playerNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.playerNameTextBox.TabIndex = 4;
            this.playerNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerHPTextBox
            // 
            this.playerHPTextBox.Enabled = false;
            this.playerHPTextBox.Location = new System.Drawing.Point(40, 139);
            this.playerHPTextBox.Name = "playerHPTextBox";
            this.playerHPTextBox.Size = new System.Drawing.Size(35, 20);
            this.playerHPTextBox.TabIndex = 5;
            this.playerHPTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerHPLabel
            // 
            this.playerHPLabel.AutoSize = true;
            this.playerHPLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.playerHPLabel.Location = new System.Drawing.Point(9, 142);
            this.playerHPLabel.Name = "playerHPLabel";
            this.playerHPLabel.Size = new System.Drawing.Size(25, 13);
            this.playerHPLabel.TabIndex = 6;
            this.playerHPLabel.Text = "HP:";
            // 
            // playerRaceClassTextBox
            // 
            this.playerRaceClassTextBox.Enabled = false;
            this.playerRaceClassTextBox.Location = new System.Drawing.Point(12, 119);
            this.playerRaceClassTextBox.Name = "playerRaceClassTextBox";
            this.playerRaceClassTextBox.Size = new System.Drawing.Size(100, 20);
            this.playerRaceClassTextBox.TabIndex = 7;
            this.playerRaceClassTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerABTextBox
            // 
            this.playerABTextBox.Enabled = false;
            this.playerABTextBox.Location = new System.Drawing.Point(40, 161);
            this.playerABTextBox.Name = "playerABTextBox";
            this.playerABTextBox.Size = new System.Drawing.Size(35, 20);
            this.playerABTextBox.TabIndex = 8;
            this.playerABTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerABLabel
            // 
            this.playerABLabel.AutoSize = true;
            this.playerABLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.playerABLabel.Location = new System.Drawing.Point(9, 164);
            this.playerABLabel.Name = "playerABLabel";
            this.playerABLabel.Size = new System.Drawing.Size(24, 13);
            this.playerABLabel.TabIndex = 9;
            this.playerABLabel.Text = "AB:";
            // 
            // playerACTextBox
            // 
            this.playerACTextBox.Enabled = false;
            this.playerACTextBox.Location = new System.Drawing.Point(40, 183);
            this.playerACTextBox.Name = "playerACTextBox";
            this.playerACTextBox.Size = new System.Drawing.Size(35, 20);
            this.playerACTextBox.TabIndex = 10;
            this.playerACTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // playerACLabel
            // 
            this.playerACLabel.AutoSize = true;
            this.playerACLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.playerACLabel.Location = new System.Drawing.Point(9, 186);
            this.playerACLabel.Name = "playerACLabel";
            this.playerACLabel.Size = new System.Drawing.Size(24, 13);
            this.playerACLabel.TabIndex = 11;
            this.playerACLabel.Text = "AC:";
            // 
            // creatureNameTextBox
            // 
            this.creatureNameTextBox.Enabled = false;
            this.creatureNameTextBox.Location = new System.Drawing.Point(697, 97);
            this.creatureNameTextBox.Name = "creatureNameTextBox";
            this.creatureNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.creatureNameTextBox.TabIndex = 12;
            this.creatureNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // creatureHPLabel
            // 
            this.creatureHPLabel.AutoSize = true;
            this.creatureHPLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.creatureHPLabel.Location = new System.Drawing.Point(765, 122);
            this.creatureHPLabel.Name = "creatureHPLabel";
            this.creatureHPLabel.Size = new System.Drawing.Size(25, 13);
            this.creatureHPLabel.TabIndex = 13;
            this.creatureHPLabel.Text = ":HP";
            // 
            // creatureABLabel
            // 
            this.creatureABLabel.AutoSize = true;
            this.creatureABLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.creatureABLabel.Location = new System.Drawing.Point(766, 141);
            this.creatureABLabel.Name = "creatureABLabel";
            this.creatureABLabel.Size = new System.Drawing.Size(24, 13);
            this.creatureABLabel.TabIndex = 14;
            this.creatureABLabel.Text = ":AB";
            // 
            // creatureACLabel
            // 
            this.creatureACLabel.AutoSize = true;
            this.creatureACLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.creatureACLabel.Location = new System.Drawing.Point(766, 163);
            this.creatureACLabel.Name = "creatureACLabel";
            this.creatureACLabel.Size = new System.Drawing.Size(24, 13);
            this.creatureACLabel.TabIndex = 15;
            this.creatureACLabel.Text = ":AC";
            // 
            // creatureHPTextBox
            // 
            this.creatureHPTextBox.Enabled = false;
            this.creatureHPTextBox.Location = new System.Drawing.Point(725, 119);
            this.creatureHPTextBox.Name = "creatureHPTextBox";
            this.creatureHPTextBox.Size = new System.Drawing.Size(35, 20);
            this.creatureHPTextBox.TabIndex = 16;
            this.creatureHPTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // creatureABTextBox
            // 
            this.creatureABTextBox.Enabled = false;
            this.creatureABTextBox.Location = new System.Drawing.Point(725, 141);
            this.creatureABTextBox.Name = "creatureABTextBox";
            this.creatureABTextBox.Size = new System.Drawing.Size(35, 20);
            this.creatureABTextBox.TabIndex = 17;
            this.creatureABTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // creatureACTextBox
            // 
            this.creatureACTextBox.Enabled = false;
            this.creatureACTextBox.Location = new System.Drawing.Point(725, 163);
            this.creatureACTextBox.Name = "creatureACTextBox";
            this.creatureACTextBox.Size = new System.Drawing.Size(35, 20);
            this.creatureACTextBox.TabIndex = 18;
            this.creatureACTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // feedbackList
            // 
            this.feedbackList.FormattingEnabled = true;
            this.feedbackList.Location = new System.Drawing.Point(162, 55);
            this.feedbackList.Name = "feedbackList";
            this.feedbackList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.feedbackList.Size = new System.Drawing.Size(486, 303);
            this.feedbackList.TabIndex = 19;
            // 
            // playerImgBox
            // 
            this.playerImgBox.Location = new System.Drawing.Point(12, 12);
            this.playerImgBox.Name = "playerImgBox";
            this.playerImgBox.Size = new System.Drawing.Size(75, 75);
            this.playerImgBox.TabIndex = 20;
            this.playerImgBox.TabStop = false;
            // 
            // creatureImgBox
            // 
            this.creatureImgBox.Location = new System.Drawing.Point(722, 12);
            this.creatureImgBox.Name = "creatureImgBox";
            this.creatureImgBox.Size = new System.Drawing.Size(75, 75);
            this.creatureImgBox.TabIndex = 21;
            this.creatureImgBox.TabStop = false;
            // 
            // roomNameLbl
            // 
            this.roomNameLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roomNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomNameLbl.Location = new System.Drawing.Point(239, 9);
            this.roomNameLbl.Name = "roomNameLbl";
            this.roomNameLbl.Size = new System.Drawing.Size(331, 43);
            this.roomNameLbl.TabIndex = 23;
            this.roomNameLbl.Text = "Room Name";
            this.roomNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commandTxtBox
            // 
            this.commandTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandTxtBox.Location = new System.Drawing.Point(279, 400);
            this.commandTxtBox.Name = "commandTxtBox";
            this.commandTxtBox.Size = new System.Drawing.Size(331, 26);
            this.commandTxtBox.TabIndex = 24;
            // 
            // instructionLbl
            // 
            this.instructionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionLbl.Location = new System.Drawing.Point(342, 373);
            this.instructionLbl.Name = "instructionLbl";
            this.instructionLbl.Size = new System.Drawing.Size(206, 24);
            this.instructionLbl.TabIndex = 25;
            this.instructionLbl.Text = "Enter Command Below";
            this.instructionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // actionBtn
            // 
            this.actionBtn.Location = new System.Drawing.Point(198, 400);
            this.actionBtn.Name = "actionBtn";
            this.actionBtn.Size = new System.Drawing.Size(75, 26);
            this.actionBtn.TabIndex = 26;
            this.actionBtn.Text = "Do Action";
            this.actionBtn.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Dice_and_Combat_Engine.Properties.Resources.backgroundBlur;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(809, 438);
            this.Controls.Add(this.actionBtn);
            this.Controls.Add(this.instructionLbl);
            this.Controls.Add(this.commandTxtBox);
            this.Controls.Add(this.roomNameLbl);
            this.Controls.Add(this.creatureImgBox);
            this.Controls.Add(this.playerImgBox);
            this.Controls.Add(this.feedbackList);
            this.Controls.Add(this.creatureACTextBox);
            this.Controls.Add(this.creatureABTextBox);
            this.Controls.Add(this.creatureHPTextBox);
            this.Controls.Add(this.creatureACLabel);
            this.Controls.Add(this.creatureABLabel);
            this.Controls.Add(this.creatureHPLabel);
            this.Controls.Add(this.creatureNameTextBox);
            this.Controls.Add(this.playerACLabel);
            this.Controls.Add(this.playerACTextBox);
            this.Controls.Add(this.playerABLabel);
            this.Controls.Add(this.playerABTextBox);
            this.Controls.Add(this.playerRaceClassTextBox);
            this.Controls.Add(this.playerHPLabel);
            this.Controls.Add(this.playerHPTextBox);
            this.Controls.Add(this.playerNameTextBox);
            this.Name = "MainForm";
            this.Text = "Dice and Combat Engine - Fight";
            ((System.ComponentModel.ISupportInitialize)(this.playerImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.creatureImgBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox playerNameTextBox;
        private System.Windows.Forms.TextBox playerHPTextBox;
        private System.Windows.Forms.Label playerHPLabel;
        private System.Windows.Forms.TextBox playerRaceClassTextBox;
        private System.Windows.Forms.TextBox playerABTextBox;
        private System.Windows.Forms.Label playerABLabel;
        private System.Windows.Forms.TextBox playerACTextBox;
        private System.Windows.Forms.Label playerACLabel;
        private System.Windows.Forms.TextBox creatureNameTextBox;
        private System.Windows.Forms.Label creatureHPLabel;
        private System.Windows.Forms.Label creatureABLabel;
        private System.Windows.Forms.Label creatureACLabel;
        private System.Windows.Forms.TextBox creatureHPTextBox;
        private System.Windows.Forms.TextBox creatureABTextBox;
        private System.Windows.Forms.TextBox creatureACTextBox;
        private System.Windows.Forms.ListBox feedbackList;
        private System.Windows.Forms.PictureBox playerImgBox;
        private System.Windows.Forms.PictureBox creatureImgBox;
        private System.Windows.Forms.Label roomNameLbl;
        private System.Windows.Forms.TextBox commandTxtBox;
        private System.Windows.Forms.Label instructionLbl;
        private System.Windows.Forms.Button actionBtn;
    }
}

