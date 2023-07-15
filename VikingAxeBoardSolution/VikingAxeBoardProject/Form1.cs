using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
           // WindowState = FormWindowState.Maximized;
           // FormBorderStyle = FormBorderStyle.None;
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

        private void boardCalibrationButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = boardTab;
        }

        private void previewBoardButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = previewBoardTab;
        }

        private void previewBoardImage_Click(object sender, EventArgs e)
        {
            int centerX, centerY = previewBoardImage.Width / 2;
            centerX = centerY;
            double vectorX = centerX - MousePosition.X;
            double vectorY = centerY - MousePosition.Y;
            double lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            
            if (lengthFromCenter <= (18 / 122) * centerY)
                MessageBox.Show("6!0");
            
            MessageBox.Show(string.Format("X: {0} Y: {1}", (MousePosition.X - previewBoardImage.Location.X), (MousePosition.Y - previewBoardImage.Location.Y)));
            MessageBox.Show(string.Format("X: {0} Y: {1}", MousePosition.X, MousePosition.Y));
            MessageBox.Show(string.Format("X: {0} Y: {1}", previewBoardImage.Location.X, previewBoardImage.Location.Y));
            MessageBox.Show(string.Format("X: {0} X: {1}", previewBoardImage.ClientSize.Width, previewBoardImage.Width));

        }
    }
}