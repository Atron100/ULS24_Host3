namespace ULS24_Host
{
    partial class MainHost
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
            this.Cmd_Exit = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Cmd_Start = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_man = new System.Windows.Forms.TabPage();
            this.Cmd_SaveRaw = new System.Windows.Forms.Button();
            this.Cmd_Capture = new System.Windows.Forms.Button();
            this.Cmd_SaveBmp = new System.Windows.Forms.Button();
            this.singleFramePictureBox = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tab_auto = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.sessionDurationUpDown = new System.Windows.Forms.NumericUpDown();
            this.photoRateUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tab_Params = new System.Windows.Forms.TabPage();
            this.imgIntegrationTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkPipe = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.imgLowGainRadioButton = new System.Windows.Forms.RadioButton();
            this.imgHighGainRadioButton = new System.Windows.Forms.RadioButton();
            this.check_Log = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Cmd_Int = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.imgResolutionComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Cmd_Sens = new System.Windows.Forms.Button();
            this.Cmd_ConnectDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Cmd_File = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Cmd_DevName = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tab_man.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singleFramePictureBox)).BeginInit();
            this.tab_auto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionDurationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.photoRateUpDown)).BeginInit();
            this.tab_Params.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgIntegrationTimeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Cmd_Exit
            // 
            this.Cmd_Exit.Location = new System.Drawing.Point(588, 476);
            this.Cmd_Exit.Name = "Cmd_Exit";
            this.Cmd_Exit.Size = new System.Drawing.Size(136, 47);
            this.Cmd_Exit.TabIndex = 0;
            this.Cmd_Exit.Text = "Exit";
            this.Cmd_Exit.UseVisualStyleBackColor = true;
            this.Cmd_Exit.Click += new System.EventHandler(this.Cmd_Exit_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "device1",
            "device2",
            "device3",
            "device4"});
            this.listBox1.Location = new System.Drawing.Point(12, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(215, 459);
            this.listBox1.TabIndex = 1;
            // 
            // Cmd_Start
            // 
            this.Cmd_Start.Enabled = false;
            this.Cmd_Start.Location = new System.Drawing.Point(234, 386);
            this.Cmd_Start.Name = "Cmd_Start";
            this.Cmd_Start.Size = new System.Drawing.Size(96, 30);
            this.Cmd_Start.TabIndex = 2;
            this.Cmd_Start.Text = "Start Sampling";
            this.Cmd_Start.UseVisualStyleBackColor = true;
            this.Cmd_Start.Click += new System.EventHandler(this.Cmd_Start_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(233, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(334, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_man);
            this.tabControl1.Controls.Add(this.tab_auto);
            this.tabControl1.Controls.Add(this.tab_Params);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(233, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(349, 448);
            this.tabControl1.TabIndex = 0;
            // 
            // tab_man
            // 
            this.tab_man.Controls.Add(this.Cmd_SaveRaw);
            this.tab_man.Controls.Add(this.Cmd_Capture);
            this.tab_man.Controls.Add(this.Cmd_SaveBmp);
            this.tab_man.Controls.Add(this.singleFramePictureBox);
            this.tab_man.Controls.Add(this.label7);
            this.tab_man.Location = new System.Drawing.Point(4, 22);
            this.tab_man.Name = "tab_man";
            this.tab_man.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tab_man.Size = new System.Drawing.Size(341, 422);
            this.tab_man.TabIndex = 0;
            this.tab_man.Text = "Manual";
            this.tab_man.UseVisualStyleBackColor = true;
            // 
            // Cmd_SaveRaw
            // 
            this.Cmd_SaveRaw.Location = new System.Drawing.Point(164, 370);
            this.Cmd_SaveRaw.Name = "Cmd_SaveRaw";
            this.Cmd_SaveRaw.Size = new System.Drawing.Size(80, 38);
            this.Cmd_SaveRaw.TabIndex = 6;
            this.Cmd_SaveRaw.Text = "Save Raw";
            this.Cmd_SaveRaw.UseVisualStyleBackColor = true;
            this.Cmd_SaveRaw.Click += new System.EventHandler(this.Cmd_SaveRaw_Click);
            // 
            // Cmd_Capture
            // 
            this.Cmd_Capture.Enabled = false;
            this.Cmd_Capture.Location = new System.Drawing.Point(9, 370);
            this.Cmd_Capture.Name = "Cmd_Capture";
            this.Cmd_Capture.Size = new System.Drawing.Size(80, 38);
            this.Cmd_Capture.TabIndex = 5;
            this.Cmd_Capture.Text = "Capture";
            this.Cmd_Capture.UseVisualStyleBackColor = true;
            this.Cmd_Capture.Click += new System.EventHandler(this.Cmd_Capture_Click);
            // 
            // Cmd_SaveBmp
            // 
            this.Cmd_SaveBmp.Location = new System.Drawing.Point(250, 370);
            this.Cmd_SaveBmp.Name = "Cmd_SaveBmp";
            this.Cmd_SaveBmp.Size = new System.Drawing.Size(80, 38);
            this.Cmd_SaveBmp.TabIndex = 4;
            this.Cmd_SaveBmp.Text = "Save BMP";
            this.Cmd_SaveBmp.UseVisualStyleBackColor = true;
            this.Cmd_SaveBmp.Click += new System.EventHandler(this.Cmd_Save_Click);
            // 
            // singleFramePictureBox
            // 
            this.singleFramePictureBox.Location = new System.Drawing.Point(6, 6);
            this.singleFramePictureBox.Name = "singleFramePictureBox";
            this.singleFramePictureBox.Size = new System.Drawing.Size(108, 117);
            this.singleFramePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.singleFramePictureBox.TabIndex = 3;
            this.singleFramePictureBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Photo Data:";
            // 
            // tab_auto
            // 
            this.tab_auto.Controls.Add(this.label14);
            this.tab_auto.Controls.Add(this.sessionDurationUpDown);
            this.tab_auto.Controls.Add(this.photoRateUpDown);
            this.tab_auto.Controls.Add(this.label3);
            this.tab_auto.Controls.Add(this.Cmd_Start);
            this.tab_auto.Location = new System.Drawing.Point(4, 22);
            this.tab_auto.Name = "tab_auto";
            this.tab_auto.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tab_auto.Size = new System.Drawing.Size(341, 422);
            this.tab_auto.TabIndex = 1;
            this.tab_auto.Text = "Automatic";
            this.tab_auto.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 41);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Session Duration (sec):";
            // 
            // sessionDurationUpDown
            // 
            this.sessionDurationUpDown.Location = new System.Drawing.Point(126, 39);
            this.sessionDurationUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sessionDurationUpDown.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.sessionDurationUpDown.Name = "sessionDurationUpDown";
            this.sessionDurationUpDown.Size = new System.Drawing.Size(72, 20);
            this.sessionDurationUpDown.TabIndex = 8;
            this.sessionDurationUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // photoRateUpDown
            // 
            this.photoRateUpDown.Location = new System.Drawing.Point(126, 18);
            this.photoRateUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.photoRateUpDown.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
            this.photoRateUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.photoRateUpDown.Name = "photoRateUpDown";
            this.photoRateUpDown.Size = new System.Drawing.Size(72, 20);
            this.photoRateUpDown.TabIndex = 7;
            this.photoRateUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Photo Rate(sec):";
            // 
            // tab_Params
            // 
            this.tab_Params.Controls.Add(this.imgIntegrationTimeUpDown);
            this.tab_Params.Controls.Add(this.checkPipe);
            this.tab_Params.Controls.Add(this.label13);
            this.tab_Params.Controls.Add(this.imgLowGainRadioButton);
            this.tab_Params.Controls.Add(this.imgHighGainRadioButton);
            this.tab_Params.Controls.Add(this.check_Log);
            this.tab_Params.Controls.Add(this.label11);
            this.tab_Params.Controls.Add(this.label9);
            this.tab_Params.Controls.Add(this.Cmd_Int);
            this.tab_Params.Controls.Add(this.label10);
            this.tab_Params.Controls.Add(this.imgResolutionComboBox);
            this.tab_Params.Controls.Add(this.label4);
            this.tab_Params.Controls.Add(this.label8);
            this.tab_Params.Controls.Add(this.textBox2);
            this.tab_Params.Controls.Add(this.Cmd_Sens);
            this.tab_Params.Location = new System.Drawing.Point(4, 22);
            this.tab_Params.Name = "tab_Params";
            this.tab_Params.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tab_Params.Size = new System.Drawing.Size(341, 422);
            this.tab_Params.TabIndex = 2;
            this.tab_Params.Text = "Parameters";
            this.tab_Params.UseVisualStyleBackColor = true;
            // 
            // imgIntegrationTimeUpDown
            // 
            this.imgIntegrationTimeUpDown.Location = new System.Drawing.Point(17, 89);
            this.imgIntegrationTimeUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imgIntegrationTimeUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.imgIntegrationTimeUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.imgIntegrationTimeUpDown.Name = "imgIntegrationTimeUpDown";
            this.imgIntegrationTimeUpDown.Size = new System.Drawing.Size(90, 20);
            this.imgIntegrationTimeUpDown.TabIndex = 14;
            this.imgIntegrationTimeUpDown.ThousandsSeparator = true;
            this.imgIntegrationTimeUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.imgIntegrationTimeUpDown.ValueChanged += new System.EventHandler(this.imgIntegrationTimeUpDown_ValueChanged);
            // 
            // checkPipe
            // 
            this.checkPipe.AutoSize = true;
            this.checkPipe.Enabled = false;
            this.checkPipe.Location = new System.Drawing.Point(18, 361);
            this.checkPipe.Name = "checkPipe";
            this.checkPipe.Size = new System.Drawing.Size(93, 17);
            this.checkPipe.TabIndex = 13;
            this.checkPipe.Text = "Pipeline Mode";
            this.checkPipe.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Gain Control:";
            // 
            // imgLowGainRadioButton
            // 
            this.imgLowGainRadioButton.AutoSize = true;
            this.imgLowGainRadioButton.Checked = true;
            this.imgLowGainRadioButton.Location = new System.Drawing.Point(18, 219);
            this.imgLowGainRadioButton.Name = "imgLowGainRadioButton";
            this.imgLowGainRadioButton.Size = new System.Drawing.Size(70, 17);
            this.imgLowGainRadioButton.TabIndex = 11;
            this.imgLowGainRadioButton.TabStop = true;
            this.imgLowGainRadioButton.Text = "Low Gain";
            this.imgLowGainRadioButton.UseVisualStyleBackColor = true;
            this.imgLowGainRadioButton.CheckedChanged += new System.EventHandler(this.imgLowGainRadioButton_CheckedChanged);
            // 
            // imgHighGainRadioButton
            // 
            this.imgHighGainRadioButton.AutoSize = true;
            this.imgHighGainRadioButton.Location = new System.Drawing.Point(18, 196);
            this.imgHighGainRadioButton.Name = "imgHighGainRadioButton";
            this.imgHighGainRadioButton.Size = new System.Drawing.Size(72, 17);
            this.imgHighGainRadioButton.TabIndex = 10;
            this.imgHighGainRadioButton.TabStop = true;
            this.imgHighGainRadioButton.Text = "High Gain";
            this.imgHighGainRadioButton.UseVisualStyleBackColor = true;
            // 
            // check_Log
            // 
            this.check_Log.AutoSize = true;
            this.check_Log.Enabled = false;
            this.check_Log.Location = new System.Drawing.Point(18, 384);
            this.check_Log.Name = "check_Log";
            this.check_Log.Size = new System.Drawing.Size(80, 17);
            this.check_Log.TabIndex = 9;
            this.check_Log.Text = "Enable Log";
            this.check_Log.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(137, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Integration Time Parameter:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Temperature:";
            // 
            // Cmd_Int
            // 
            this.Cmd_Int.Location = new System.Drawing.Point(208, 77);
            this.Cmd_Int.Name = "Cmd_Int";
            this.Cmd_Int.Size = new System.Drawing.Size(86, 39);
            this.Cmd_Int.TabIndex = 6;
            this.Cmd_Int.Text = "Integration Time";
            this.Cmd_Int.UseVisualStyleBackColor = true;
            this.Cmd_Int.Click += new System.EventHandler(this.Cmd_Int_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(112, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "(us)";
            // 
            // imgResolutionComboBox
            // 
            this.imgResolutionComboBox.FormattingEnabled = true;
            this.imgResolutionComboBox.Items.AddRange(new object[] {
            "12px",
            "24px"});
            this.imgResolutionComboBox.Location = new System.Drawing.Point(18, 139);
            this.imgResolutionComboBox.Name = "imgResolutionComboBox";
            this.imgResolutionComboBox.Size = new System.Drawing.Size(90, 21);
            this.imgResolutionComboBox.TabIndex = 1;
            this.imgResolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.imgResolutionComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Photo Pixel Size:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(112, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "(degree C)";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(18, 33);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 20);
            this.textBox2.TabIndex = 1;
            // 
            // Cmd_Sens
            // 
            this.Cmd_Sens.Enabled = false;
            this.Cmd_Sens.Location = new System.Drawing.Point(208, 23);
            this.Cmd_Sens.Name = "Cmd_Sens";
            this.Cmd_Sens.Size = new System.Drawing.Size(86, 39);
            this.Cmd_Sens.TabIndex = 0;
            this.Cmd_Sens.Text = "Sensor Temp";
            this.Cmd_Sens.UseVisualStyleBackColor = true;
            // 
            // Cmd_ConnectDisconnect
            // 
            this.Cmd_ConnectDisconnect.Location = new System.Drawing.Point(12, 493);
            this.Cmd_ConnectDisconnect.Name = "Cmd_ConnectDisconnect";
            this.Cmd_ConnectDisconnect.Size = new System.Drawing.Size(214, 30);
            this.Cmd_ConnectDisconnect.TabIndex = 7;
            this.Cmd_ConnectDisconnect.Text = "Connect";
            this.Cmd_ConnectDisconnect.UseVisualStyleBackColor = true;
            this.Cmd_ConnectDisconnect.Click += new System.EventHandler(this.Cmd_ConnectDisconnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Device List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Device:";
            // 
            // Cmd_File
            // 
            this.Cmd_File.Location = new System.Drawing.Point(590, 97);
            this.Cmd_File.Name = "Cmd_File";
            this.Cmd_File.Size = new System.Drawing.Size(136, 47);
            this.Cmd_File.TabIndex = 7;
            this.Cmd_File.Text = "File Dialog";
            this.Cmd_File.UseVisualStyleBackColor = true;
            this.Cmd_File.Click += new System.EventHandler(this.Cmd_File_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 535);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "file path: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 535);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "filepathvariable";
            // 
            // Cmd_DevName
            // 
            this.Cmd_DevName.Location = new System.Drawing.Point(590, 150);
            this.Cmd_DevName.Name = "Cmd_DevName";
            this.Cmd_DevName.Size = new System.Drawing.Size(136, 47);
            this.Cmd_DevName.TabIndex = 10;
            this.Cmd_DevName.Text = "Set Device Name";
            this.Cmd_DevName.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(233, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(162, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "Device Trim Data Loaded Status";
            // 
            // MainHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 557);
            this.Controls.Add(this.Cmd_ConnectDisconnect);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Cmd_DevName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Cmd_File);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Cmd_Exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainHost";
            this.Text = "Main Host";
            this.tabControl1.ResumeLayout(false);
            this.tab_man.ResumeLayout(false);
            this.tab_man.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singleFramePictureBox)).EndInit();
            this.tab_auto.ResumeLayout(false);
            this.tab_auto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionDurationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.photoRateUpDown)).EndInit();
            this.tab_Params.ResumeLayout(false);
            this.tab_Params.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgIntegrationTimeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cmd_Exit;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Cmd_Start;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_man;
        private System.Windows.Forms.TabPage tab_auto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox imgResolutionComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Cmd_SaveBmp;
        private System.Windows.Forms.PictureBox singleFramePictureBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Cmd_File;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tab_Params;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button Cmd_Int;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button Cmd_Sens;
        private System.Windows.Forms.Button Cmd_DevName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton imgLowGainRadioButton;
        private System.Windows.Forms.RadioButton imgHighGainRadioButton;
        private System.Windows.Forms.CheckBox check_Log;
        private System.Windows.Forms.Button Cmd_Capture;
        private System.Windows.Forms.CheckBox checkPipe;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button Cmd_SaveRaw;
        private System.Windows.Forms.Button Cmd_ConnectDisconnect;
        private System.Windows.Forms.NumericUpDown imgIntegrationTimeUpDown;
        private System.Windows.Forms.NumericUpDown photoRateUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown sessionDurationUpDown;
    }
}

