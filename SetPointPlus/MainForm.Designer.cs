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
			this.insLabel = new System.Windows.Forms.Label();
			this.deleteCheckBox = new System.Windows.Forms.CheckBox();
			this.restartCheckBox = new System.Windows.Forms.CheckBox();
			this.optionGroupBox = new System.Windows.Forms.GroupBox();
			this.restoreButton = new System.Windows.Forms.Button();
			this.targetLabel = new System.Windows.Forms.Label();
			this.comboBox = new System.Windows.Forms.ComboBox();
			this.optionGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// deviceCheckedListBox
			// 
			resources.ApplyResources(this.deviceCheckedListBox, "deviceCheckedListBox");
			this.deviceCheckedListBox.CheckOnClick = true;
			this.deviceCheckedListBox.FormattingEnabled = true;
			this.deviceCheckedListBox.Name = "deviceCheckedListBox";
			this.deviceCheckedListBox.Sorted = true;
			// 
			// applyButton
			// 
			resources.ApplyResources(this.applyButton, "applyButton");
			this.applyButton.Name = "applyButton";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// insLabel
			// 
			resources.ApplyResources(this.insLabel, "insLabel");
			this.insLabel.Name = "insLabel";
			// 
			// deleteCheckBox
			// 
			resources.ApplyResources(this.deleteCheckBox, "deleteCheckBox");
			this.deleteCheckBox.Checked = true;
			this.deleteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.deleteCheckBox.Name = "deleteCheckBox";
			this.deleteCheckBox.UseVisualStyleBackColor = true;
			// 
			// restartCheckBox
			// 
			resources.ApplyResources(this.restartCheckBox, "restartCheckBox");
			this.restartCheckBox.Checked = true;
			this.restartCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.restartCheckBox.Name = "restartCheckBox";
			this.restartCheckBox.UseVisualStyleBackColor = true;
			// 
			// optionGroupBox
			// 
			resources.ApplyResources(this.optionGroupBox, "optionGroupBox");
			this.optionGroupBox.Controls.Add(this.deleteCheckBox);
			this.optionGroupBox.Controls.Add(this.restartCheckBox);
			this.optionGroupBox.Name = "optionGroupBox";
			this.optionGroupBox.TabStop = false;
			// 
			// restoreButton
			// 
			resources.ApplyResources(this.restoreButton, "restoreButton");
			this.restoreButton.Name = "restoreButton";
			this.restoreButton.UseVisualStyleBackColor = true;
			this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
			// 
			// targetLabel
			// 
			resources.ApplyResources(this.targetLabel, "targetLabel");
			this.targetLabel.Name = "targetLabel";
			// 
			// comboBox
			// 
			resources.ApplyResources(this.comboBox, "comboBox");
			this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox.FormattingEnabled = true;
			this.comboBox.Name = "comboBox";
			this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboBox);
			this.Controls.Add(this.targetLabel);
			this.Controls.Add(this.restoreButton);
			this.Controls.Add(this.optionGroupBox);
			this.Controls.Add(this.insLabel);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.deviceCheckedListBox);
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
		private System.Windows.Forms.Label insLabel;
		private System.Windows.Forms.CheckBox deleteCheckBox;
		private System.Windows.Forms.CheckBox restartCheckBox;
		private System.Windows.Forms.GroupBox optionGroupBox;
		private System.Windows.Forms.Button restoreButton;
		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.ComboBox comboBox;
	}
}

