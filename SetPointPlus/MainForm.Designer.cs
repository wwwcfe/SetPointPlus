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
			this.deviceCheckedListBox.AccessibleDescription = null;
			this.deviceCheckedListBox.AccessibleName = null;
			resources.ApplyResources(this.deviceCheckedListBox, "deviceCheckedListBox");
			this.deviceCheckedListBox.BackgroundImage = null;
			this.deviceCheckedListBox.CheckOnClick = true;
			this.deviceCheckedListBox.Font = null;
			this.deviceCheckedListBox.FormattingEnabled = true;
			this.deviceCheckedListBox.Name = "deviceCheckedListBox";
			this.deviceCheckedListBox.Sorted = true;
			// 
			// applyButton
			// 
			this.applyButton.AccessibleDescription = null;
			this.applyButton.AccessibleName = null;
			resources.ApplyResources(this.applyButton, "applyButton");
			this.applyButton.BackgroundImage = null;
			this.applyButton.Font = null;
			this.applyButton.Name = "applyButton";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// label
			// 
			this.label.AccessibleDescription = null;
			this.label.AccessibleName = null;
			resources.ApplyResources(this.label, "label");
			this.label.Font = null;
			this.label.Name = "label";
			// 
			// deleteCheckBox
			// 
			this.deleteCheckBox.AccessibleDescription = null;
			this.deleteCheckBox.AccessibleName = null;
			resources.ApplyResources(this.deleteCheckBox, "deleteCheckBox");
			this.deleteCheckBox.BackgroundImage = null;
			this.deleteCheckBox.Checked = true;
			this.deleteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.deleteCheckBox.Font = null;
			this.deleteCheckBox.Name = "deleteCheckBox";
			this.deleteCheckBox.UseVisualStyleBackColor = true;
			// 
			// restartCheckBox
			// 
			this.restartCheckBox.AccessibleDescription = null;
			this.restartCheckBox.AccessibleName = null;
			resources.ApplyResources(this.restartCheckBox, "restartCheckBox");
			this.restartCheckBox.BackgroundImage = null;
			this.restartCheckBox.Checked = true;
			this.restartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.restartCheckBox.Font = null;
			this.restartCheckBox.Name = "restartCheckBox";
			this.restartCheckBox.UseVisualStyleBackColor = true;
			// 
			// optionGroupBox
			// 
			this.optionGroupBox.AccessibleDescription = null;
			this.optionGroupBox.AccessibleName = null;
			resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
			this.optionGroupBox.BackgroundImage = null;
			this.optionGroupBox.Controls.Add(this.deleteCheckBox);
			this.optionGroupBox.Controls.Add(this.restartCheckBox);
			this.optionGroupBox.Font = null;
			this.optionGroupBox.Name = "optionGroupBox";
			this.optionGroupBox.TabStop = false;
			// 
			// restoreButton
			// 
			this.restoreButton.AccessibleDescription = null;
			this.restoreButton.AccessibleName = null;
			resources.ApplyResources(this.restoreButton, "restoreButton");
			this.restoreButton.BackgroundImage = null;
			this.restoreButton.Font = null;
			this.restoreButton.Name = "restoreButton";
			this.restoreButton.UseVisualStyleBackColor = true;
			this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
			// 
			// MainForm
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.restoreButton);
			this.Controls.Add(this.optionGroupBox);
			this.Controls.Add(this.label);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.deviceCheckedListBox);
			this.Font = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
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

