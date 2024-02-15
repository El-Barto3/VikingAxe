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
using System.IO;
using System.Threading;
using System.Drawing.Text;
using System.Reflection;
using System.Globalization;

namespace VikingAxeBoardProject
{
    partial class VikingAxeProjectForm
    {
        private void scaleAttackBoard(int newSize, int originalSize)
        {
            double scale = newSize * 1.0 / originalSize * 1.0;
            Point location;
            for (int i = 0; i <= 15; i++)
            {
                PictureBox box = (PictureBox)GetControlByName(attackModePanel, ("shieldAttackPB" + i));
                box.Width = (int)Math.Round(box.Width * scale);
                box.Height = (int)Math.Round(box.Height * scale);
                location = new Point((int)Math.Round(box.Location.X * scale), (int)Math.Round(box.Location.Y * scale));
                box.Location = location;
                box.Image = Properties.Resources.shield_point1;
                box.Tag = "AttackPoint-1";
            }
        }


        private void startGameAttack()
        {
            int originalSize, newSize;
            // startGameOne();
            makeControlsInvisible();
            attackModePanel.Visible = true;
            revertMoveButton.Visible = true;
            if (playerCount >= 1)
            {
                resetGame();
                // updateBoardLook(playBoardImage, playBoardImagePanel);
                originalSize = attackModePanel.Width;
                updateBoardLook(attackModePanel);
                newSize = attackModePanel.Width;
                scaleAttackBoard(newSize, originalSize);
                updateScoreBoard();
                hideScoreBoard();
                attackMovesHistory.Clear();
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }


    }
}