namespace Lab5_image_preview
	{
	partial class ImagePreview
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.panel1.Location = new System.Drawing.Point(50, 50);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1080, 650);
			this.panel1.TabIndex = 0;
			// 
			// ImagePreview
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1182, 753);
			this.Controls.Add(this.panel1);
			this.Location = new System.Drawing.Point(420, 40);
			this.Name = "ImagePreview";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Preview";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImagePreview_FormClosing);
			this.Load += new System.EventHandler(this.ImagePreview_Load);
			this.SizeChanged += new System.EventHandler(this.ImagePreview_SizeChanged);
			this.ResumeLayout(false);

			}

		#endregion

		private System.Windows.Forms.Panel panel1;
		}
	}

