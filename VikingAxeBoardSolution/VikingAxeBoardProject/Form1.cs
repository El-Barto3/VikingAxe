using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VikingAxeBoardProject
{
    public partial class VikingAxeProject : Form
    {
        public VikingAxeProject()
        {
            InitializeComponent();
            WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                            (int)(pixel & 0x0000FF00) >> 8,
                            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
        

        private void VikingAxeProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (tabsControl.SelectedTab == mainTab)
                    this.Close();
                else
                    tabsControl.SelectedTab = mainTab;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = playTab;
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = calibrateTab;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (tabsControl.SelectedTab == mainTab)
                this.Close();
            else
                tabsControl.SelectedTab = mainTab;
        }

        private void playersSettingButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = playersTab;
        }

        private void previewBoardButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = previewBoardTab;
        }

        private void previewBoardImage_Click(object sender, EventArgs e)
        {

            Color colorOfClicked = GetPixelColor(MousePosition.X * 3 / 2, MousePosition.Y * 3 / 2);
            //tuuuu.Location = this.PointToClient(Cursor.Position);
            int centerX = previewBoardImage.Width / 2 + previewBoardImage.Location.X - 5,
                centerY = previewBoardImage.Width / 2 + previewBoardImage.Location.Y - 5;

            double vectorX = centerX - this.PointToClient(Cursor.Position).X,
                vectorY = centerY - this.PointToClient(Cursor.Position).Y;

            //MessageBox.Show(string.Format("X: {0} Y: {1}", vectorX, vectorY));

            double lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            double sixPointRadius =  (double)(previewBoardImage.Width) / 2.0 * 18 / 122;
            double fourPointRadius = (double)(previewBoardImage.Width) / 2.0 * 44 / 122;
            double threePointRadius = (double)(previewBoardImage.Width) / 2.0 * 70 / 122;
            double twoPointRadius = (double)(previewBoardImage.Width) / 2.0 * 96 / 122;
            double onePointRadius = (double)(previewBoardImage.Width) / 2.0 * 1;

            if (lengthFromCenter <= sixPointRadius && colorOfClicked != Color.Black)
                MessageBox.Show("6!");
            else if(lengthFromCenter <= fourPointRadius) 
                MessageBox.Show("4!");
            else if (lengthFromCenter <= threePointRadius)
                MessageBox.Show("3!");
            else if (lengthFromCenter <= twoPointRadius)
                MessageBox.Show("2!");
            else if (lengthFromCenter <= onePointRadius)
                MessageBox.Show("1!");



        }

        private void gameOneButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = playOneTab;
        }

        private void gameTwoButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = playTwoTab;
        }

        private void minimalizeSettingButton_Click(object sender, EventArgs e)
        {
            maximalizeSettingsButton.Visible = true;
            boardSettingsGroupBox.Width = 100;
            boardSettingsGroupBox.Height = 60;
        }

        private void maximalizeSettingsButton_Click(object sender, EventArgs e)
        {
            maximalizeSettingsButton.Visible = false;
            boardSettingsGroupBox.Width = 590;
            boardSettingsGroupBox.Height = 360;
        }

        private void boardCalibrationButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = previewBoardTab;
        }
        int boardSize = 500;
        int boardPositionX = 
        private void widthBoardTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}