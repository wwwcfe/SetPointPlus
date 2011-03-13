namespace SetPointPlus
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.deviceCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.applyButton = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.deleteCheckBox = new System.Windows.Forms.CheckBox();
			this.restartCheckBox = new System.Windows.Forms.CheckBox();
			this.optionGroupBox = new System.Windows.Forms.GroupBox();
			this.restoreButton = new System.Windows.Forms.Button();
			this.optionGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// deviceCheckedListBox
			// 
			this.deviceCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.deviceCheckedListBox.CheckOnClick = true;
			this.deviceCheckedListBox.FormattingEnabled = true;
			this.deviceCheckedListBox.Location = new System.Drawing.Point(14, 24);
			this.deviceCheckedListBox.Name = "deviceCheckedListBox";
			this.deviceCheckedListBox.Size = new System.Drawing.Size(277, 158);
			this.deviceCheckedListBox.Sorted = true;
			this.deviceCheckedListBox.TabIndex = 1;
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.applyButton.Location = new System.Drawing.Point(216, 267);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(75, 23);
			this.applyButton.TabIndex = 3;
			this.applyButton.Text = "適用(&A)";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(12, 9);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(134, 12);
			this.label.TabIndex = 0;
			this.label.Text = "インストール済みデバイス(&I):";
			// 
			// deleteCheckBox
			// 
			this.deleteCheckBox.AutoSize = true;
			this.deleteCheckBox.Checked = true;
			this.deleteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.deleteCheckBox.Location = new System.Drawing.Point(6, 18);
			this.deleteCheckBox.Name = "deleteCheckBox";
			this.deleteCheckBox.Size = new System.Drawing.Size(171, 16);
			this.deleteCheckBox.TabIndex = 0;
			this.deleteCheckBox.Text = "ユーザー設定ファイルを削除(&D)";
			this.deleteCheckBox.UseVisualStyleBackColor = true;
			// 
			// restartCheckBox
			// 
			this.restartCheckBox.AutoSize = true;
			this.restartCheckBox.Checked = true;
			this.restartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.restartCheckBox.Location = new System.Drawing.Point(6, 40);
			this.restartCheckBox.Name = "restartCheckBox";
			this.restartCheckBox.Size = new System.Drawing.Size(132, 16);
			this.restartCheckBox.TabIndex = 1;
			this.restartCheckBox.Text = "SetPoint を再起動(&R)";
			this.restartCheckBox.UseVisualStyleBackColor = true;
			// 
			// optionGroupBox
			// 
			this.optionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.optionGroupBox.Controls.Add(this.deleteCheckBox);
			this.optionGroupBox.Controls.Add(this.restartCheckBox);
			this.optionGroupBox.Location = new System.Drawing.Point(14, 195);
			this.optionGroupBox.Name = "optionGroupBox";
			this.optionGroupBox.Size = new System.Drawing.Size(277, 66);
			this.optionGroupBox.TabIndex = 2;
			this.optionGroupBox.TabStop = false;
			this.optionGroupBox.Text = "オプション";
			// 
			// restoreButton
			// 
			this.restoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.restoreButton.Location = new System.Drawing.Point(14, 267);
			this.restoreButton.Name = "restoreButton";
			this.restoreButton.Size = new System.Drawing.Size(75, 23);
			this.restoreButton.TabIndex = 4;
			this.restoreButton.Text = "復元(&T)";
			this.restoreButton.UseVisualStyleBackColor = true;
			this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(307, 301);
			this.Controls.Add(this.restoreButton);
			this.Controls.Add(this.optionGroupBox);
			this.Controls.Add(this.label);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.deviceCheckedListBox);
			this.HelpButton = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(323, 339);
			this.Name = "MainForm";
			this.Text = "SetPointPlus";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.optionGroupBox.ResumeLayout(false);
			this.optionGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox deviceCheckedListBox;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.CheckBox deleteCheckBox;
		private System.Windows.Forms.CheckBox restartCheckBox;
		private System.Windows.Forms.GroupBox optionGroupBox;
		private System.Windows.Forms.Button restoreButton;
	}
}

