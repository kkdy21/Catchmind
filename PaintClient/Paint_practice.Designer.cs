
namespace drawingtest
{
    partial class Paint_practice
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Allclearbtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pen_btn = new System.Windows.Forms.Button();
            this.palette = new System.Windows.Forms.Button();
            this.curColor_picture = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Erase_btn = new System.Windows.Forms.Button();
            this.rectangle_btn = new System.Windows.Forms.Button();
            this.Circle_btn = new System.Windows.Forms.Button();
            this.gamestart_btn = new System.Windows.Forms.Button();
            this.roomtalk_richtext = new System.Windows.Forms.RichTextBox();
            this.draw_textbox = new System.Windows.Forms.TextBox();
            this.msgsend_btn = new System.Windows.Forms.Button();
            this.SaveImage_btn = new System.Windows.Forms.Button();
            this.answerkey_textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.score_btn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.curColor_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // Allclearbtn
            // 
            this.Allclearbtn.Location = new System.Drawing.Point(714, 486);
            this.Allclearbtn.Name = "Allclearbtn";
            this.Allclearbtn.Size = new System.Drawing.Size(155, 57);
            this.Allclearbtn.TabIndex = 0;
            this.Allclearbtn.Text = "Allclear";
            this.Allclearbtn.UseVisualStyleBackColor = true;
            this.Allclearbtn.Click += new System.EventHandler(this.Allclearbtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(12, 79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(679, 529);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // pen_btn
            // 
            this.pen_btn.BackColor = System.Drawing.SystemColors.Window;
            this.pen_btn.Image = global::drawingtest.Properties.Resources.pen;
            this.pen_btn.Location = new System.Drawing.Point(716, 81);
            this.pen_btn.Name = "pen_btn";
            this.pen_btn.Size = new System.Drawing.Size(66, 55);
            this.pen_btn.TabIndex = 4;
            this.pen_btn.UseVisualStyleBackColor = false;
            this.pen_btn.Click += new System.EventHandler(this.pen_btn_Click);
            // 
            // palette
            // 
            this.palette.BackColor = System.Drawing.SystemColors.Window;
            this.palette.Image = global::drawingtest.Properties.Resources.paintcolor;
            this.palette.Location = new System.Drawing.Point(716, 16);
            this.palette.Name = "palette";
            this.palette.Size = new System.Drawing.Size(66, 55);
            this.palette.TabIndex = 5;
            this.palette.UseVisualStyleBackColor = false;
            // 
            // curColor_picture
            // 
            this.curColor_picture.Location = new System.Drawing.Point(788, 16);
            this.curColor_picture.Name = "curColor_picture";
            this.curColor_picture.Size = new System.Drawing.Size(64, 55);
            this.curColor_picture.TabIndex = 6;
            this.curColor_picture.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboBox1.Location = new System.Drawing.Point(749, 325);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Erase_btn
            // 
            this.Erase_btn.BackColor = System.Drawing.SystemColors.Window;
            this.Erase_btn.Image = global::drawingtest.Properties.Resources.eraser;
            this.Erase_btn.Location = new System.Drawing.Point(716, 142);
            this.Erase_btn.Name = "Erase_btn";
            this.Erase_btn.Size = new System.Drawing.Size(66, 55);
            this.Erase_btn.TabIndex = 9;
            this.Erase_btn.UseVisualStyleBackColor = false;
            this.Erase_btn.Click += new System.EventHandler(this.Erase_btn_Click);
            // 
            // rectangle_btn
            // 
            this.rectangle_btn.BackColor = System.Drawing.SystemColors.Window;
            this.rectangle_btn.Image = global::drawingtest.Properties.Resources.rectangle;
            this.rectangle_btn.Location = new System.Drawing.Point(716, 203);
            this.rectangle_btn.Name = "rectangle_btn";
            this.rectangle_btn.Size = new System.Drawing.Size(66, 55);
            this.rectangle_btn.TabIndex = 10;
            this.rectangle_btn.UseVisualStyleBackColor = false;
            this.rectangle_btn.Click += new System.EventHandler(this.rectangle_btn_Click);
            // 
            // Circle_btn
            // 
            this.Circle_btn.BackColor = System.Drawing.SystemColors.Window;
            this.Circle_btn.Image = global::drawingtest.Properties.Resources.circle;
            this.Circle_btn.Location = new System.Drawing.Point(715, 264);
            this.Circle_btn.Name = "Circle_btn";
            this.Circle_btn.Size = new System.Drawing.Size(67, 55);
            this.Circle_btn.TabIndex = 11;
            this.Circle_btn.UseVisualStyleBackColor = false;
            this.Circle_btn.Click += new System.EventHandler(this.Circle_btn_Click);
            // 
            // gamestart_btn
            // 
            this.gamestart_btn.Location = new System.Drawing.Point(715, 549);
            this.gamestart_btn.Name = "gamestart_btn";
            this.gamestart_btn.Size = new System.Drawing.Size(154, 58);
            this.gamestart_btn.TabIndex = 12;
            this.gamestart_btn.Text = "게임시작";
            this.gamestart_btn.UseVisualStyleBackColor = true;
            this.gamestart_btn.Click += new System.EventHandler(this.gamestart_btn_Click);
            // 
            // roomtalk_richtext
            // 
            this.roomtalk_richtext.Location = new System.Drawing.Point(876, 203);
            this.roomtalk_richtext.Name = "roomtalk_richtext";
            this.roomtalk_richtext.Size = new System.Drawing.Size(517, 375);
            this.roomtalk_richtext.TabIndex = 13;
            this.roomtalk_richtext.Text = "";
            // 
            // draw_textbox
            // 
            this.draw_textbox.Location = new System.Drawing.Point(876, 585);
            this.draw_textbox.Name = "draw_textbox";
            this.draw_textbox.Size = new System.Drawing.Size(436, 23);
            this.draw_textbox.TabIndex = 14;
            // 
            // msgsend_btn
            // 
            this.msgsend_btn.Location = new System.Drawing.Point(1318, 584);
            this.msgsend_btn.Name = "msgsend_btn";
            this.msgsend_btn.Size = new System.Drawing.Size(75, 23);
            this.msgsend_btn.TabIndex = 15;
            this.msgsend_btn.Text = "전송";
            this.msgsend_btn.UseVisualStyleBackColor = true;
            // 
            // SaveImage_btn
            // 
            this.SaveImage_btn.BackColor = System.Drawing.SystemColors.Window;
            this.SaveImage_btn.Image = global::drawingtest.Properties.Resources.save;
            this.SaveImage_btn.Location = new System.Drawing.Point(1137, 81);
            this.SaveImage_btn.Name = "SaveImage_btn";
            this.SaveImage_btn.Size = new System.Drawing.Size(154, 55);
            this.SaveImage_btn.TabIndex = 16;
            this.SaveImage_btn.UseVisualStyleBackColor = false;
            this.SaveImage_btn.Click += new System.EventHandler(this.SaveImage_btn_Click);
            // 
            // answerkey_textbox
            // 
            this.answerkey_textbox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.answerkey_textbox.Location = new System.Drawing.Point(294, 28);
            this.answerkey_textbox.Name = "answerkey_textbox";
            this.answerkey_textbox.Size = new System.Drawing.Size(165, 29);
            this.answerkey_textbox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(200, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 30);
            this.label1.TabIndex = 19;
            this.label1.Text = "제시어 :";
            // 
            // score_btn
            // 
            this.score_btn.Location = new System.Drawing.Point(1137, 17);
            this.score_btn.Name = "score_btn";
            this.score_btn.Size = new System.Drawing.Size(154, 57);
            this.score_btn.TabIndex = 20;
            this.score_btn.Text = "점수확인";
            this.score_btn.UseVisualStyleBackColor = true;
            this.score_btn.Click += new System.EventHandler(this.score_btn_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(876, 16);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(255, 175);
            this.listView1.TabIndex = 21;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(716, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "굵기";
            // 
            // Paint_practice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 619);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.score_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.answerkey_textbox);
            this.Controls.Add(this.SaveImage_btn);
            this.Controls.Add(this.msgsend_btn);
            this.Controls.Add(this.draw_textbox);
            this.Controls.Add(this.roomtalk_richtext);
            this.Controls.Add(this.gamestart_btn);
            this.Controls.Add(this.Circle_btn);
            this.Controls.Add(this.rectangle_btn);
            this.Controls.Add(this.Erase_btn);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.curColor_picture);
            this.Controls.Add(this.palette);
            this.Controls.Add(this.pen_btn);
            this.Controls.Add(this.Allclearbtn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Paint_practice";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.curColor_picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Allclearbtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button pen_btn;
        private System.Windows.Forms.Button palette;
        private System.Windows.Forms.PictureBox curColor_picture;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Erase_btn;
        private System.Windows.Forms.Button rectangle_btn;
        private System.Windows.Forms.Button Circle_btn;
        private System.Windows.Forms.Button gamestart_btn;
        private System.Windows.Forms.RichTextBox roomtalk_richtext;
        private System.Windows.Forms.TextBox draw_textbox;
        private System.Windows.Forms.Button msgsend_btn;
        private System.Windows.Forms.Button SaveImage_btn;
        private System.Windows.Forms.TextBox answerkey_textbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button score_btn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label2;
    }
}

