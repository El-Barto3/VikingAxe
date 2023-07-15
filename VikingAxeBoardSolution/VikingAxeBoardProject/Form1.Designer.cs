
namespace VikingAxeBoardProject
{
    partial class VikingAxeProject
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VikingAxeProject));
            this.tabsControl = new System.Windows.Forms.TabControl();
            this.mainTab = new System.Windows.Forms.TabPage();
            this.startButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.playTab = new System.Windows.Forms.TabPage();
            this.calibrateTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.boardCalibrationButton = new System.Windows.Forms.Button();
            this.playersSettingButton = new System.Windows.Forms.Button();
            this.playersTab = new System.Windows.Forms.TabPage();
            this.savePlayersButton = new System.Windows.Forms.Button();
            this.playerNameTextBox4 = new System.Windows.Forms.TextBox();
            this.playerNameTextBox3 = new System.Windows.Forms.TextBox();
            this.playerNameTextBox2 = new System.Windows.Forms.TextBox();
            this.playerNameTextBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.boardTab = new System.Windows.Forms.TabPage();
            this.subSizeBoardButtonY = new System.Windows.Forms.Button();
            this.addSizeBoardButtonY = new System.Windows.Forms.Button();
            this.subSizeBoardButtonX = new System.Windows.Forms.Button();
            this.addSizeBoardButtonX = new System.Windows.Forms.Button();
            this.previewBoardButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.saveBoardButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.previewBoardTab = new System.Windows.Forms.TabPage();
            this.previewBoardImage = new System.Windows.Forms.PictureBox();
            this.tabsControl.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.calibrateTab.SuspendLayout();
            this.playersTab.SuspendLayout();
            this.boardTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.previewBoardTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewBoardImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tabsControl
            // 
            this.tabsControl.Controls.Add(this.mainTab);
            this.tabsControl.Controls.Add(this.playTab);
            this.tabsControl.Controls.Add(this.calibrateTab);
            this.tabsControl.Controls.Add(this.playersTab);
            this.tabsControl.Controls.Add(this.boardTab);
            this.tabsControl.Controls.Add(this.previewBoardTab);
            this.tabsControl.Location = new System.Drawing.Point(0, -2);
            this.tabsControl.Name = "tabsControl";
            this.tabsControl.SelectedIndex = 0;
            this.tabsControl.Size = new System.Drawing.Size(1920, 1080);
            this.tabsControl.TabIndex = 4;
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.startButton);
            this.mainTab.Controls.Add(this.exitButton);
            this.mainTab.Controls.Add(this.calibrateButton);
            this.mainTab.Location = new System.Drawing.Point(4, 29);
            this.mainTab.Name = "mainTab";
            this.mainTab.Size = new System.Drawing.Size(1912, 1047);
            this.mainTab.TabIndex = 2;
            this.mainTab.Text = "Main";
            this.mainTab.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(859, 316);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(120, 55);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Graj";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(859, 472);
            this.exitButton.Name = "exitButton";
            this.exitButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.exitButton.Size = new System.Drawing.Size(120, 55);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "Wyjdź";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(859, 394);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(120, 55);
            this.calibrateButton.TabIndex = 5;
            this.calibrateButton.Text = "Kalibruj";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // playTab
            // 
            this.playTab.Location = new System.Drawing.Point(4, 29);
            this.playTab.Name = "playTab";
            this.playTab.Padding = new System.Windows.Forms.Padding(3);
            this.playTab.Size = new System.Drawing.Size(1912, 1047);
            this.playTab.TabIndex = 0;
            this.playTab.Text = "Play";
            this.playTab.UseVisualStyleBackColor = true;
            // 
            // calibrateTab
            // 
            this.calibrateTab.Controls.Add(this.label1);
            this.calibrateTab.Controls.Add(this.boardCalibrationButton);
            this.calibrateTab.Controls.Add(this.playersSettingButton);
            this.calibrateTab.Location = new System.Drawing.Point(4, 29);
            this.calibrateTab.Name = "calibrateTab";
            this.calibrateTab.Padding = new System.Windows.Forms.Padding(3);
            this.calibrateTab.Size = new System.Drawing.Size(1912, 1047);
            this.calibrateTab.TabIndex = 1;
            this.calibrateTab.Text = "Calibrate";
            this.calibrateTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(869, 376);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ustawienia";
            // 
            // boardCalibrationButton
            // 
            this.boardCalibrationButton.Location = new System.Drawing.Point(863, 542);
            this.boardCalibrationButton.Name = "boardCalibrationButton";
            this.boardCalibrationButton.Size = new System.Drawing.Size(140, 65);
            this.boardCalibrationButton.TabIndex = 1;
            this.boardCalibrationButton.Text = "Tarcza";
            this.boardCalibrationButton.UseVisualStyleBackColor = true;
            this.boardCalibrationButton.Click += new System.EventHandler(this.boardCalibrationButton_Click);
            // 
            // playersSettingButton
            // 
            this.playersSettingButton.Location = new System.Drawing.Point(863, 442);
            this.playersSettingButton.Name = "playersSettingButton";
            this.playersSettingButton.Size = new System.Drawing.Size(140, 65);
            this.playersSettingButton.TabIndex = 0;
            this.playersSettingButton.Text = "Gracze";
            this.playersSettingButton.UseVisualStyleBackColor = true;
            this.playersSettingButton.Click += new System.EventHandler(this.playersSettingButton_Click);
            // 
            // playersTab
            // 
            this.playersTab.Controls.Add(this.savePlayersButton);
            this.playersTab.Controls.Add(this.playerNameTextBox4);
            this.playersTab.Controls.Add(this.playerNameTextBox3);
            this.playersTab.Controls.Add(this.playerNameTextBox2);
            this.playersTab.Controls.Add(this.playerNameTextBox1);
            this.playersTab.Controls.Add(this.label6);
            this.playersTab.Controls.Add(this.label5);
            this.playersTab.Controls.Add(this.label4);
            this.playersTab.Controls.Add(this.label3);
            this.playersTab.Controls.Add(this.label2);
            this.playersTab.Location = new System.Drawing.Point(4, 29);
            this.playersTab.Name = "playersTab";
            this.playersTab.Size = new System.Drawing.Size(1912, 1047);
            this.playersTab.TabIndex = 3;
            this.playersTab.Text = "Players";
            // 
            // savePlayersButton
            // 
            this.savePlayersButton.Location = new System.Drawing.Point(935, 569);
            this.savePlayersButton.Name = "savePlayersButton";
            this.savePlayersButton.Size = new System.Drawing.Size(87, 35);
            this.savePlayersButton.TabIndex = 12;
            this.savePlayersButton.Text = "Zapisz";
            this.savePlayersButton.UseVisualStyleBackColor = true;
            // 
            // playerNameTextBox4
            // 
            this.playerNameTextBox4.Location = new System.Drawing.Point(889, 519);
            this.playerNameTextBox4.Name = "playerNameTextBox4";
            this.playerNameTextBox4.Size = new System.Drawing.Size(185, 26);
            this.playerNameTextBox4.TabIndex = 11;
            // 
            // playerNameTextBox3
            // 
            this.playerNameTextBox3.Location = new System.Drawing.Point(889, 475);
            this.playerNameTextBox3.Name = "playerNameTextBox3";
            this.playerNameTextBox3.Size = new System.Drawing.Size(185, 26);
            this.playerNameTextBox3.TabIndex = 10;
            // 
            // playerNameTextBox2
            // 
            this.playerNameTextBox2.Location = new System.Drawing.Point(889, 433);
            this.playerNameTextBox2.Name = "playerNameTextBox2";
            this.playerNameTextBox2.Size = new System.Drawing.Size(185, 26);
            this.playerNameTextBox2.TabIndex = 9;
            // 
            // playerNameTextBox1
            // 
            this.playerNameTextBox1.Location = new System.Drawing.Point(889, 393);
            this.playerNameTextBox1.Name = "playerNameTextBox1";
            this.playerNameTextBox1.Size = new System.Drawing.Size(185, 26);
            this.playerNameTextBox1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(767, 522);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "Gracz #4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(767, 436);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Gracz #2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(767, 478);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Gracz #3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(767, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Gracz #1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(895, 306);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Gracze";
            // 
            // boardTab
            // 
            this.boardTab.Controls.Add(this.subSizeBoardButtonY);
            this.boardTab.Controls.Add(this.addSizeBoardButtonY);
            this.boardTab.Controls.Add(this.subSizeBoardButtonX);
            this.boardTab.Controls.Add(this.addSizeBoardButtonX);
            this.boardTab.Controls.Add(this.previewBoardButton);
            this.boardTab.Controls.Add(this.pictureBox1);
            this.boardTab.Controls.Add(this.textBox6);
            this.boardTab.Controls.Add(this.label12);
            this.boardTab.Controls.Add(this.textBox4);
            this.boardTab.Controls.Add(this.label10);
            this.boardTab.Controls.Add(this.saveBoardButton);
            this.boardTab.Controls.Add(this.label8);
            this.boardTab.Controls.Add(this.textBox2);
            this.boardTab.Controls.Add(this.textBox1);
            this.boardTab.Controls.Add(this.label7);
            this.boardTab.Location = new System.Drawing.Point(4, 29);
            this.boardTab.Name = "boardTab";
            this.boardTab.Size = new System.Drawing.Size(1912, 1047);
            this.boardTab.TabIndex = 4;
            this.boardTab.Text = "Board";
            this.boardTab.UseVisualStyleBackColor = true;
            // 
            // subSizeBoardButtonY
            // 
            this.subSizeBoardButtonY.Location = new System.Drawing.Point(723, 374);
            this.subSizeBoardButtonY.Name = "subSizeBoardButtonY";
            this.subSizeBoardButtonY.Size = new System.Drawing.Size(49, 35);
            this.subSizeBoardButtonY.TabIndex = 25;
            this.subSizeBoardButtonY.Text = "-";
            this.subSizeBoardButtonY.UseVisualStyleBackColor = true;
            // 
            // addSizeBoardButtonY
            // 
            this.addSizeBoardButtonY.Location = new System.Drawing.Point(668, 374);
            this.addSizeBoardButtonY.Name = "addSizeBoardButtonY";
            this.addSizeBoardButtonY.Size = new System.Drawing.Size(49, 35);
            this.addSizeBoardButtonY.TabIndex = 24;
            this.addSizeBoardButtonY.Text = "+";
            this.addSizeBoardButtonY.UseVisualStyleBackColor = true;
            // 
            // subSizeBoardButtonX
            // 
            this.subSizeBoardButtonX.Location = new System.Drawing.Point(723, 324);
            this.subSizeBoardButtonX.Name = "subSizeBoardButtonX";
            this.subSizeBoardButtonX.Size = new System.Drawing.Size(49, 35);
            this.subSizeBoardButtonX.TabIndex = 23;
            this.subSizeBoardButtonX.Text = "-";
            this.subSizeBoardButtonX.UseVisualStyleBackColor = true;
            // 
            // addSizeBoardButtonX
            // 
            this.addSizeBoardButtonX.Location = new System.Drawing.Point(668, 324);
            this.addSizeBoardButtonX.Name = "addSizeBoardButtonX";
            this.addSizeBoardButtonX.Size = new System.Drawing.Size(49, 35);
            this.addSizeBoardButtonX.TabIndex = 22;
            this.addSizeBoardButtonX.Text = "+";
            this.addSizeBoardButtonX.UseVisualStyleBackColor = true;
            // 
            // previewBoardButton
            // 
            this.previewBoardButton.Location = new System.Drawing.Point(651, 451);
            this.previewBoardButton.Name = "previewBoardButton";
            this.previewBoardButton.Size = new System.Drawing.Size(87, 35);
            this.previewBoardButton.TabIndex = 21;
            this.previewBoardButton.Text = "Podgląd";
            this.previewBoardButton.UseVisualStyleBackColor = true;
            this.previewBoardButton.Click += new System.EventHandler(this.previewBoardButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(966, 273);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(350, 326);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(570, 378);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(70, 26);
            this.textBox6.TabIndex = 19;
            this.textBox6.Text = "950";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(512, 381);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 20);
            this.label12.TabIndex = 18;
            this.label12.Text = "Y";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(570, 328);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(70, 26);
            this.textBox4.TabIndex = 15;
            this.textBox4.Text = "950";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(512, 331);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 20);
            this.label10.TabIndex = 14;
            this.label10.Text = "X";
            // 
            // saveBoardButton
            // 
            this.saveBoardButton.Location = new System.Drawing.Point(1229, 682);
            this.saveBoardButton.Name = "saveBoardButton";
            this.saveBoardButton.Size = new System.Drawing.Size(87, 35);
            this.saveBoardButton.TabIndex = 13;
            this.saveBoardButton.Text = "Zapisz";
            this.saveBoardButton.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(646, 273);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 20);
            this.label8.TabIndex = 3;
            this.label8.Text = "x";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(668, 270);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 26);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "100";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(570, 270);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 26);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(496, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Wymiary";
            // 
            // previewBoardTab
            // 
            this.previewBoardTab.Controls.Add(this.previewBoardImage);
            this.previewBoardTab.Location = new System.Drawing.Point(4, 29);
            this.previewBoardTab.Name = "previewBoardTab";
            this.previewBoardTab.Size = new System.Drawing.Size(1912, 1047);
            this.previewBoardTab.TabIndex = 5;
            this.previewBoardTab.Text = "Preview Board";
            this.previewBoardTab.UseVisualStyleBackColor = true;
            // 
            // previewBoardImage
            // 
            this.previewBoardImage.BackColor = System.Drawing.Color.BlueViolet;
            this.previewBoardImage.Image = ((System.Drawing.Image)(resources.GetObject("previewBoardImage.Image")));
            this.previewBoardImage.Location = new System.Drawing.Point(412, 20);
            this.previewBoardImage.Margin = new System.Windows.Forms.Padding(0);
            this.previewBoardImage.Name = "previewBoardImage";
            this.previewBoardImage.Size = new System.Drawing.Size(1000, 1000);
            this.previewBoardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previewBoardImage.TabIndex = 21;
            this.previewBoardImage.TabStop = false;
            this.previewBoardImage.Click += new System.EventHandler(this.previewBoardImage_Click);
            // 
            // VikingAxeProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.ClientSize = new System.Drawing.Size(1917, 1080);
            this.Controls.Add(this.tabsControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "VikingAxeProject";
            this.Text = "Viking Axe Board";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VikingAxeProject_KeyDown);
            this.tabsControl.ResumeLayout(false);
            this.mainTab.ResumeLayout(false);
            this.calibrateTab.ResumeLayout(false);
            this.calibrateTab.PerformLayout();
            this.playersTab.ResumeLayout(false);
            this.playersTab.PerformLayout();
            this.boardTab.ResumeLayout(false);
            this.boardTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.previewBoardTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewBoardImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabsControl;
        private System.Windows.Forms.TabPage playTab;
        private System.Windows.Forms.TabPage calibrateTab;
        private System.Windows.Forms.TabPage mainTab;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button boardCalibrationButton;
        private System.Windows.Forms.Button playersSettingButton;
        private System.Windows.Forms.TabPage playersTab;
        private System.Windows.Forms.TabPage boardTab;
        private System.Windows.Forms.Button savePlayersButton;
        private System.Windows.Forms.TextBox playerNameTextBox4;
        private System.Windows.Forms.TextBox playerNameTextBox3;
        private System.Windows.Forms.TextBox playerNameTextBox2;
        private System.Windows.Forms.TextBox playerNameTextBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button saveBoardButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button subSizeBoardButtonY;
        private System.Windows.Forms.Button addSizeBoardButtonY;
        private System.Windows.Forms.Button subSizeBoardButtonX;
        private System.Windows.Forms.Button addSizeBoardButtonX;
        private System.Windows.Forms.Button previewBoardButton;
        private System.Windows.Forms.TabPage previewBoardTab;
        private System.Windows.Forms.PictureBox previewBoardImage;
    }
}

