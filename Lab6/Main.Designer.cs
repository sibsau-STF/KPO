﻿namespace Lab6
	{
	partial class Main
		{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
			{
			if ( disposing && ( components != null ) )
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
		private void InitializeComponent ()
			{
			this.createKey = new System.Windows.Forms.Button();
			this.logBox = new System.Windows.Forms.TextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// createKey
			// 
			this.createKey.Location = new System.Drawing.Point(25, 33);
			this.createKey.Name = "createKey";
			this.createKey.Size = new System.Drawing.Size(97, 35);
			this.createKey.TabIndex = 0;
			this.createKey.Text = "Create key";
			this.createKey.UseVisualStyleBackColor = true;
			this.createKey.Click += new System.EventHandler(this.createKey_Click);
			// 
			// logBox
			// 
			this.logBox.Location = new System.Drawing.Point(22, 102);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.Size = new System.Drawing.Size(151, 120);
			this.logBox.TabIndex = 1;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(14, 424);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(99, 33);
			this.button1.TabIndex = 2;
			this.button1.Text = "Select key";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(22, 259);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(151, 139);
			this.textBox1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 82);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Logs";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(22, 239);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 17);
			this.label2.TabIndex = 5;
			this.label2.Text = "Flash drives";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(481, 250);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(92, 21);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "Funciton1";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(481, 277);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(92, 21);
			this.checkBox2.TabIndex = 7;
			this.checkBox2.Text = "Function2";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new System.Drawing.Point(481, 304);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(92, 21);
			this.checkBox3.TabIndex = 8;
			this.checkBox3.Text = "Function3";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(681, 532);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.logBox);
			this.Controls.Add(this.createKey);
			this.Name = "Main";
			this.Text = "Main";
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private System.Windows.Forms.Button createKey;
		private System.Windows.Forms.TextBox logBox;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		}
	}