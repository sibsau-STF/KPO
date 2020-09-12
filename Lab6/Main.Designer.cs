namespace Lab6
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
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.userBox = new System.Windows.Forms.TextBox();
			this.serialBox = new System.Windows.Forms.TextBox();
			this.vendorBox = new System.Windows.Forms.TextBox();
			this.createdBox = new System.Windows.Forms.TextBox();
			this.untillBox = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// createKey
			// 
			this.createKey.Location = new System.Drawing.Point(12, 12);
			this.createKey.Name = "createKey";
			this.createKey.Size = new System.Drawing.Size(97, 35);
			this.createKey.TabIndex = 0;
			this.createKey.Text = "Create key";
			this.createKey.UseVisualStyleBackColor = true;
			this.createKey.Click += new System.EventHandler(this.createKey_Click);
			// 
			// logBox
			// 
			this.logBox.Location = new System.Drawing.Point(8, 335);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ReadOnly = true;
			this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logBox.Size = new System.Drawing.Size(522, 216);
			this.logBox.TabIndex = 1;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 53);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(97, 33);
			this.button1.TabIndex = 2;
			this.button1.Text = "Select key";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(307, 238);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(223, 91);
			this.textBox1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 315);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Logs";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(304, 218);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 17);
			this.label2.TabIndex = 5;
			this.label2.Text = "Flash drives";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox1.Location = new System.Drawing.Point(3, 3);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(95, 22);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "Funciton1";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Enabled = false;
			this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox2.Location = new System.Drawing.Point(3, 31);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(95, 22);
			this.checkBox2.TabIndex = 7;
			this.checkBox2.Text = "Function2";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Enabled = false;
			this.checkBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox3.Location = new System.Drawing.Point(3, 59);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(95, 22);
			this.checkBox3.TabIndex = 8;
			this.checkBox3.Text = "Function3";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(264, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 17);
			this.label3.TabIndex = 9;
			this.label3.Text = "User";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(264, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 17);
			this.label4.TabIndex = 10;
			this.label4.Text = "Serial number";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(264, 101);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(54, 17);
			this.label5.TabIndex = 11;
			this.label5.Text = "Vendor";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(264, 141);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 17);
			this.label6.TabIndex = 12;
			this.label6.Text = "Created day";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(264, 181);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(66, 17);
			this.label7.TabIndex = 13;
			this.label7.Text = "Untill day";
			// 
			// userBox
			// 
			this.userBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.userBox.Location = new System.Drawing.Point(367, 16);
			this.userBox.Name = "userBox";
			this.userBox.ReadOnly = true;
			this.userBox.Size = new System.Drawing.Size(163, 22);
			this.userBox.TabIndex = 14;
			this.userBox.TabStop = false;
			// 
			// serialBox
			// 
			this.serialBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.serialBox.Location = new System.Drawing.Point(367, 56);
			this.serialBox.Name = "serialBox";
			this.serialBox.ReadOnly = true;
			this.serialBox.Size = new System.Drawing.Size(163, 22);
			this.serialBox.TabIndex = 15;
			this.serialBox.TabStop = false;
			// 
			// vendorBox
			// 
			this.vendorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.vendorBox.Location = new System.Drawing.Point(367, 96);
			this.vendorBox.Name = "vendorBox";
			this.vendorBox.ReadOnly = true;
			this.vendorBox.Size = new System.Drawing.Size(163, 22);
			this.vendorBox.TabIndex = 16;
			this.vendorBox.TabStop = false;
			// 
			// createdBox
			// 
			this.createdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.createdBox.Location = new System.Drawing.Point(367, 136);
			this.createdBox.Name = "createdBox";
			this.createdBox.ReadOnly = true;
			this.createdBox.Size = new System.Drawing.Size(163, 22);
			this.createdBox.TabIndex = 17;
			this.createdBox.TabStop = false;
			// 
			// untillBox
			// 
			this.untillBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.untillBox.Location = new System.Drawing.Point(367, 176);
			this.untillBox.Name = "untillBox";
			this.untillBox.ReadOnly = true;
			this.untillBox.Size = new System.Drawing.Size(163, 22);
			this.untillBox.TabIndex = 18;
			this.untillBox.TabStop = false;
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button2.Location = new System.Drawing.Point(115, 12);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(143, 74);
			this.button2.TabIndex = 19;
			this.button2.Text = "Validate key";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.checkBox1);
			this.flowLayoutPanel1.Controls.Add(this.checkBox2);
			this.flowLayoutPanel1.Controls.Add(this.checkBox3);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 104);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(115, 94);
			this.flowLayoutPanel1.TabIndex = 20;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 563);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.untillBox);
			this.Controls.Add(this.createdBox);
			this.Controls.Add(this.vendorBox);
			this.Controls.Add(this.serialBox);
			this.Controls.Add(this.userBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.logBox);
			this.Controls.Add(this.createKey);
			this.Name = "Main";
			this.Text = "Main";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
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
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox userBox;
		private System.Windows.Forms.TextBox serialBox;
		private System.Windows.Forms.TextBox vendorBox;
		private System.Windows.Forms.TextBox createdBox;
		private System.Windows.Forms.TextBox untillBox;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		}
	}