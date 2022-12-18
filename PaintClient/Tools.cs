using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace drawingtest
{
    enum Art_tools : int
    {
        PEN = 0,
        ERASER,
        FIGURE,
        CIRCLE,
        PAINT,
        DEFAULT
    }

    public partial class Paint_practice
    {

        private void SelectTool(int tool)
        {
            switch (tool)
            {
                case (int)Art_tools.PEN:
                    curTool = (int)Art_tools.PEN;
                    pictureBox1.Cursor = new Cursor("PenCursor_small.cur");
                    break;
                case (int)Art_tools.ERASER:
                    curTool = (int)Art_tools.ERASER;
                    pictureBox1.Cursor = new Cursor("EraserCursor.cur");
                    break;
                case (int)Art_tools.FIGURE:
                    curTool = (int)Art_tools.FIGURE;
                    pictureBox1.Cursor = new Cursor("ShapesCursor.cur");
                    break;
                case (int)Art_tools.CIRCLE:
                    curTool = (int)Art_tools.CIRCLE;
                    pictureBox1.Cursor = new Cursor("ShapesCursor.cur");
                    break;
                case (int)Art_tools.PAINT:
                    curTool = (int)Art_tools.PAINT;
                    break;
            }
        }

        private void pen_btn_Click(object sender, EventArgs e)
        {
            SelectTool((int)Art_tools.PEN);
        }

        private void Erase_btn_Click(object sender, EventArgs e)
        {
            SelectTool((int)Art_tools.ERASER);

        }

        private void rectangle_btn_Click(object sender, EventArgs e)
        {
            SelectTool((int)Art_tools.FIGURE);
        }

        private void Circle_btn_Click(object sender, EventArgs e)
        {
            SelectTool((int)Art_tools.CIRCLE);

        }

        private void Allclearbtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            drawimage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            curTool = (int)Art_tools.PAINT;

            DrawSave(curColor, curWidth, mouse_pos, mouse_pos, curTool);
        }
        private void palette_select(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                curColor = colorDialog.Color;
                curColor_picture.BackColor = curColor;
            }
        }

        private void SaveImage_btn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:";
            saveFileDialog.FileName = "paint";
            saveFileDialog.DefaultExt = "jpg";
            saveFileDialog.Filter = "PNG file(*.png)|*.png|Bitmap file(*.bmp)|*.bmp|JPEG file(*.jpg)|*.jpg";

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("그려진 이미지가 없습니다.");
                return;
            }


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
                                    
        }
    }
}
