namespace Lab5_entry
	{
	partial class Entry
		{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose (bool disposing)
			{
			if ( disposing && ( components != null ) )
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
		private void InitializeComponent ()
			{
			this.previewButton = new System.Windows.Forms.Button();
			this.messageBox = new System.Windows.Forms.TextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.colorButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// previewButton
			// 
			this.previewButton.Location = new System.Drawing.Point(12, 221);
			this.previewButton.Name = "previewButton";
			this.previewButton.Size = new System.Drawing.Size(125, 31);
			this.previewButton.TabIndex = 0;
			this.previewButton.Text = "Preview Image";
			this.previewButton.UseVisualStyleBackColor = true;
			this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
			// 
			// messageBox
			// 
			this.messageBox.Location = new System.Drawing.Point(12, 12);
			this.messageBox.Multiline = true;
			this.messageBox.Name = "messageBox";
			this.messageBox.Size = new System.Drawing.Size(408, 203);
			this.messageBox.TabIndex = 1;
			// 
			// sendButton
			// 
			this.sendButton.Location = new System.Drawing.Point(345, 221);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(75, 31);
			this.sendButton.TabIndex = 2;
			this.sendButton.Text = "Send";
			this.sendButton.UseVisualStyleBackColor = true;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(191, 221);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(102, 31);
			this.colorButton.TabIndex = 3;
			this.colorButton.Text = "Set BG Color";
			this.colorButton.UseVisualStyleBackColor = true;
			this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// Entry
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 260);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.messageBox);
			this.Controls.Add(this.previewButton);
			this.Location = new System.Drawing.Point(100, 40);
			this.MaximizeBox = false;
			this.Name = "Entry";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Entry page";
			this.Load += new System.EventHandler(this.Entry_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.Button previewButton;
		private System.Windows.Forms.TextBox messageBox;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Button colorButton;
		}
	}

