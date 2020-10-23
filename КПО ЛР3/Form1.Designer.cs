namespace КПО_ЛР3
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.workersUpDown = new System.Windows.Forms.NumericUpDown();
			this.recBtn = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.dbBox = new System.Windows.Forms.TextBox();
			this.recStopBtn = new System.Windows.Forms.Button();
			this.pathBox = new System.Windows.Forms.TextBox();
			this.openBtn = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.filterCombo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.resCombo = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.workersUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(16, 15);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(633, 438);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(16, 460);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 28);
			this.button1.TabIndex = 1;
			this.button1.Text = "Запустить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(16, 496);
			this.button2.Margin = new System.Windows.Forms.Padding(4);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(100, 28);
			this.button2.TabIndex = 4;
			this.button2.Text = "Остановить";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(16, 532);
			this.button3.Margin = new System.Windows.Forms.Padding(4);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(100, 28);
			this.button3.TabIndex = 5;
			this.button3.Text = "Пауза";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(124, 463);
			this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(160, 24);
			this.comboBox1.TabIndex = 10;
			this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(707, 584);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 17);
			this.label1.TabIndex = 11;
			this.label1.Text = "Потоки";
			// 
			// workersUpDown
			// 
			this.workersUpDown.Location = new System.Drawing.Point(770, 584);
			this.workersUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.workersUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.workersUpDown.Name = "workersUpDown";
			this.workersUpDown.Size = new System.Drawing.Size(63, 22);
			this.workersUpDown.TabIndex = 12;
			this.workersUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// recBtn
			// 
			this.recBtn.Location = new System.Drawing.Point(824, 610);
			this.recBtn.Name = "recBtn";
			this.recBtn.Size = new System.Drawing.Size(106, 33);
			this.recBtn.TabIndex = 13;
			this.recBtn.Text = "Записать";
			this.recBtn.UseVisualStyleBackColor = true;
			this.recBtn.Click += new System.EventHandler(this.recBtn_Click);
			// 
			// dbBox
			// 
			this.dbBox.Location = new System.Drawing.Point(656, 15);
			this.dbBox.Multiline = true;
			this.dbBox.Name = "dbBox";
			this.dbBox.ReadOnly = true;
			this.dbBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dbBox.Size = new System.Drawing.Size(274, 438);
			this.dbBox.TabIndex = 14;
			// 
			// recStopBtn
			// 
			this.recStopBtn.Location = new System.Drawing.Point(720, 610);
			this.recStopBtn.Name = "recStopBtn";
			this.recStopBtn.Size = new System.Drawing.Size(98, 33);
			this.recStopBtn.TabIndex = 15;
			this.recStopBtn.Text = "Остановить";
			this.recStopBtn.UseVisualStyleBackColor = true;
			this.recStopBtn.Click += new System.EventHandler(this.recStopBtn_Click);
			// 
			// pathBox
			// 
			this.pathBox.Location = new System.Drawing.Point(443, 476);
			this.pathBox.Name = "pathBox";
			this.pathBox.ReadOnly = true;
			this.pathBox.Size = new System.Drawing.Size(400, 22);
			this.pathBox.TabIndex = 16;
			// 
			// openBtn
			// 
			this.openBtn.Location = new System.Drawing.Point(850, 472);
			this.openBtn.Name = "openBtn";
			this.openBtn.Size = new System.Drawing.Size(80, 30);
			this.openBtn.TabIndex = 17;
			this.openBtn.Text = "Открыть";
			this.openBtn.UseVisualStyleBackColor = true;
			this.openBtn.Click += new System.EventHandler(this.openBtn_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(704, 553);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 17);
			this.label2.TabIndex = 18;
			this.label2.Text = "Фильтр";
			// 
			// filterCombo
			// 
			this.filterCombo.FormattingEnabled = true;
			this.filterCombo.Location = new System.Drawing.Point(770, 550);
			this.filterCombo.Margin = new System.Windows.Forms.Padding(4);
			this.filterCombo.Name = "filterCombo";
			this.filterCombo.Size = new System.Drawing.Size(160, 24);
			this.filterCombo.TabIndex = 19;
			this.filterCombo.SelectedValueChanged += new System.EventHandler(this.filterCombo_SelectedValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(671, 525);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 17);
			this.label3.TabIndex = 20;
			this.label3.Text = "Разрешение";
			// 
			// resCombo
			// 
			this.resCombo.FormattingEnabled = true;
			this.resCombo.Location = new System.Drawing.Point(769, 518);
			this.resCombo.Margin = new System.Windows.Forms.Padding(4);
			this.resCombo.Name = "resCombo";
			this.resCombo.Size = new System.Drawing.Size(160, 24);
			this.resCombo.TabIndex = 21;
			this.resCombo.SelectedValueChanged += new System.EventHandler(this.resCombo_SelectedValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(942, 653);
			this.Controls.Add(this.resCombo);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.filterCombo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.openBtn);
			this.Controls.Add(this.pathBox);
			this.Controls.Add(this.recStopBtn);
			this.Controls.Add(this.dbBox);
			this.Controls.Add(this.recBtn);
			this.Controls.Add(this.workersUpDown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.workersUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown workersUpDown;
		private System.Windows.Forms.Button recBtn;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.TextBox dbBox;
		private System.Windows.Forms.Button recStopBtn;
		private System.Windows.Forms.TextBox pathBox;
		private System.Windows.Forms.Button openBtn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox filterCombo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox resCombo;
		}
}

