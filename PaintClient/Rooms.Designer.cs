
namespace drawingtest
{
    partial class Rooms
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
            this.refresh_btn = new System.Windows.Forms.Button();
            this.waitroomtalk_richtext = new System.Windows.Forms.RichTextBox();
            this.makeroom_btn = new System.Windows.Forms.Button();
            this.waitroommsg_textbox = new System.Windows.Forms.TextBox();
            this.waitroommsgsend_btn = new System.Windows.Forms.Button();
            this.waitroominfo_listbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // refresh_btn
            // 
            this.refresh_btn.Location = new System.Drawing.Point(21, 25);
            this.refresh_btn.Name = "refresh_btn";
            this.refresh_btn.Size = new System.Drawing.Size(155, 44);
            this.refresh_btn.TabIndex = 6;
            this.refresh_btn.Text = "새로고침";
            this.refresh_btn.UseVisualStyleBackColor = true;
            this.refresh_btn.Click += new System.EventHandler(this.refresh_btn_Click);
            // 
            // waitroomtalk_richtext
            // 
            this.waitroomtalk_richtext.Location = new System.Drawing.Point(21, 351);
            this.waitroomtalk_richtext.Name = "waitroomtalk_richtext";
            this.waitroomtalk_richtext.Size = new System.Drawing.Size(916, 292);
            this.waitroomtalk_richtext.TabIndex = 8;
            this.waitroomtalk_richtext.Text = "";
            // 
            // makeroom_btn
            // 
            this.makeroom_btn.Location = new System.Drawing.Point(840, 249);
            this.makeroom_btn.Name = "makeroom_btn";
            this.makeroom_btn.Size = new System.Drawing.Size(97, 95);
            this.makeroom_btn.TabIndex = 9;
            this.makeroom_btn.Text = "방만들기";
            this.makeroom_btn.UseVisualStyleBackColor = true;
            this.makeroom_btn.Click += new System.EventHandler(this.makeroom_btn_Click);
            // 
            // waitroommsg_textbox
            // 
            this.waitroommsg_textbox.Location = new System.Drawing.Point(21, 650);
            this.waitroommsg_textbox.Name = "waitroommsg_textbox";
            this.waitroommsg_textbox.Size = new System.Drawing.Size(812, 23);
            this.waitroommsg_textbox.TabIndex = 10;
            this.waitroommsg_textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.waitroommsg_textbox_KeyDown);
            // 
            // waitroommsgsend_btn
            // 
            this.waitroommsgsend_btn.Location = new System.Drawing.Point(839, 648);
            this.waitroommsgsend_btn.Name = "waitroommsgsend_btn";
            this.waitroommsgsend_btn.Size = new System.Drawing.Size(97, 28);
            this.waitroommsgsend_btn.TabIndex = 11;
            this.waitroommsgsend_btn.Text = "전송";
            this.waitroommsgsend_btn.UseVisualStyleBackColor = true;
            this.waitroommsgsend_btn.Click += new System.EventHandler(this.waitroommsgsend_btn_Click);
            // 
            // waitroominfo_listbox
            // 
            this.waitroominfo_listbox.FormattingEnabled = true;
            this.waitroominfo_listbox.ItemHeight = 15;
            this.waitroominfo_listbox.Items.AddRange(new object[] {
            "<접속중인 유저목록>"});
            this.waitroominfo_listbox.Location = new System.Drawing.Point(21, 75);
            this.waitroominfo_listbox.Name = "waitroominfo_listbox";
            this.waitroominfo_listbox.Size = new System.Drawing.Size(155, 259);
            this.waitroominfo_listbox.TabIndex = 12;
            // 
            // Rooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 725);
            this.Controls.Add(this.waitroominfo_listbox);
            this.Controls.Add(this.waitroommsgsend_btn);
            this.Controls.Add(this.waitroommsg_textbox);
            this.Controls.Add(this.makeroom_btn);
            this.Controls.Add(this.waitroomtalk_richtext);
            this.Controls.Add(this.refresh_btn);
            this.Name = "Rooms";
            this.Text = "Rooms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button refresh_btn;
        private System.Windows.Forms.RichTextBox waitroomtalk_richtext;
        private System.Windows.Forms.Button makeroom_btn;
        private System.Windows.Forms.TextBox waitroommsg_textbox;
        private System.Windows.Forms.Button waitroommsgsend_btn;
        private System.Windows.Forms.ListBox waitroominfo_listbox;
    }
}