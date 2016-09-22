namespace Dice_and_Combat_Engine
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
            this.attackButton = new System.Windows.Forms.Button();
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
            this.playerInventoryBox = new System.Windows.Forms.GroupBox();
            this.playerInventoryList = new System.Windows.Forms.ListBox();
            this.roomNameLbl = new System.Windows.Forms.Label();
            this.previousBtn = new System.Windows.Forms.Button();
            this.nextBtn = new System.Windows.Forms.Button();
            this.useItemBtn = new System.Windows.Forms.Button();
            this.creatureListBox = new System.Windows.Forms.GroupBox();
            this.creatureList = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.playerImgBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.creatureImgBox)).BeginInit();
            this.playerInventoryBox.SuspendLayout();
            this.creatureListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // attackButton
            // 
            this.attackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attackButton.Location = new System.Drawing.Point(294, 382);
            this.attackButton.Name = "attackButton";
            this.attackButton.Size = new System.Drawing.Size(220, 33);
            this.attackButton.TabIndex = 0;
            this.attackButton.Text = "&Attack";
            this.attackButton.UseVisualStyleBackColor = true;
            this.attackButton.Click += new System.EventHandler(this.attackButton_Click);
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
            this.feedbackList.Location = new System.Drawing.Point(239, 119);
            this.feedbackList.Name = "feedbackList";
            this.feedbackList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.feedbackList.Size = new System.Drawing.Size(331, 251);
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
            // playerInventoryBox
            // 
            this.playerInventoryBox.Controls.Add(this.playerInventoryList);
            this.playerInventoryBox.Location = new System.Drawing.Point(12, 209);
            this.playerInventoryBox.Name = "playerInventoryBox";
            this.playerInventoryBox.Size = new System.Drawing.Size(142, 217);
            this.playerInventoryBox.TabIndex = 22;
            this.playerInventoryBox.TabStop = false;
            this.playerInventoryBox.Text = "Inventory";
            // 
            // playerInventoryList
            // 
            this.playerInventoryList.FormattingEnabled = true;
            this.playerInventoryList.Location = new System.Drawing.Point(7, 20);
            this.playerInventoryList.Name = "playerInventoryList";
            this.playerInventoryList.Size = new System.Drawing.Size(129, 186);
            this.playerInventoryList.TabIndex = 0;
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
            // previousBtn
            // 
            this.previousBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.previousBtn.Enabled = false;
            this.previousBtn.Image = global::Dice_and_Combat_Engine.Properties.Resources.leftArrow;
            this.previousBtn.Location = new System.Drawing.Point(306, 55);
            this.previousBtn.Name = "previousBtn";
            this.previousBtn.Size = new System.Drawing.Size(66, 32);
            this.previousBtn.TabIndex = 24;
            this.previousBtn.UseVisualStyleBackColor = false;
            this.previousBtn.Click += new System.EventHandler(this.previousBtn_Click);
            // 
            // nextBtn
            // 
            this.nextBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.nextBtn.Enabled = false;
            this.nextBtn.Image = global::Dice_and_Combat_Engine.Properties.Resources.rightArrow;
            this.nextBtn.Location = new System.Drawing.Point(437, 55);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(66, 32);
            this.nextBtn.TabIndex = 25;
            this.nextBtn.UseVisualStyleBackColor = false;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // useItemBtn
            // 
            this.useItemBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useItemBtn.Location = new System.Drawing.Point(160, 395);
            this.useItemBtn.Name = "useItemBtn";
            this.useItemBtn.Size = new System.Drawing.Size(56, 31);
            this.useItemBtn.TabIndex = 26;
            this.useItemBtn.Text = "&Use";
            this.useItemBtn.UseVisualStyleBackColor = true;
            this.useItemBtn.Click += new System.EventHandler(this.useItemBtn_Click);
            // 
            // creatureListBox
            // 
            this.creatureListBox.Controls.Add(this.creatureList);
            this.creatureListBox.Location = new System.Drawing.Point(655, 209);
            this.creatureListBox.Name = "creatureListBox";
            this.creatureListBox.Size = new System.Drawing.Size(142, 217);
            this.creatureListBox.TabIndex = 23;
            this.creatureListBox.TabStop = false;
            this.creatureListBox.Text = "Creatures in Room";
            // 
            // creatureList
            // 
            this.creatureList.FormattingEnabled = true;
            this.creatureList.Location = new System.Drawing.Point(7, 20);
            this.creatureList.Name = "creatureList";
            this.creatureList.Size = new System.Drawing.Size(129, 186);
            this.creatureList.TabIndex = 0;
            this.creatureList.SelectedIndexChanged += new System.EventHandler(this.creatureList_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.attackButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Dice_and_Combat_Engine.Properties.Resources.backgroundBlurFight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(809, 438);
            this.Controls.Add(this.creatureListBox);
            this.Controls.Add(this.useItemBtn);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.previousBtn);
            this.Controls.Add(this.roomNameLbl);
            this.Controls.Add(this.playerInventoryBox);
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
            this.Controls.Add(this.attackButton);
            this.Name = "MainForm";
            this.Text = "Dice and Combat Engine - Fight";
            ((System.ComponentModel.ISupportInitialize)(this.playerImgBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.creatureImgBox)).EndInit();
            this.playerInventoryBox.ResumeLayout(false);
            this.creatureListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button attackButton;
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
        private System.Windows.Forms.GroupBox playerInventoryBox;
        private System.Windows.Forms.ListBox playerInventoryList;
        private System.Windows.Forms.Label roomNameLbl;
        private System.Windows.Forms.Button previousBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button useItemBtn;
        private System.Windows.Forms.GroupBox creatureListBox;
        private System.Windows.Forms.ListBox creatureList;
    }
}

