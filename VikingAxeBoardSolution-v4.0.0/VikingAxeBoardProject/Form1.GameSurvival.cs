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
        private int survivalPoints = 0;
        private int survivalShots = 3;
        private int survivalShipsCount = 1;
        private int survivalStage = 1;
        private int survivalHealth = 3;

        private void resetSurvivalGame()
        {
            survivalShots = 3;
            survivalStage = 0;
            survivalPoints = 0;
            survivalShipsCount = 0;
            survivalHealth = 3 * playerCount;
            showControlsByTagName(this, "SurvivalShip", false);
            survivalNextStage();
            resetGame();
        }
        private void startGameSurvival()
        {
            // startGameOne();
            makeControlsInvisible();

            if (playerCount >= 1)
            {
                resetGame();
                updateBoardLook(playBoardImage, playBoardImagePanel);
                updateBoardLook(survivalGamePanel);
                updateScoreBoard();
                hideScoreBoard();
                showControlsByTagName(this, "Survival", true);
                resetSurvivalGame();
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }

        private void survivalNextStage()
        {
            var ship = (PictureBox)GetControlByName(this, "shipSurvivalPictureBox" + (survivalShipsCount + 1));
            Random rand = new Random();

            ship.Width = boardSize / (int)Math.Sqrt(survivalStage + 3) + 1 / 10 * boardSize;
            ship.Height = ship.Width;

            var x = (int)rand.Next(0, boardSize - ship.Width);
            var y = (int)rand.Next(0, boardSize - ship.Height);
            var location = new Point(x, y);
            ship.Location = location;

            if (rand.Next() % 100 > 15)
            {
                ship.Image = Properties.Resources.ship;
                ship.Tag = "NormalShip";
                survivalShots += 1;
            }
            else
            {
                ship.Image = Properties.Resources.ship2;
                ship.Tag = "SilverShip";
                survivalShots += 2;
            }
            survivalShipsCount++;
            survivalStage++;
            ship.Visible = true;

            updateSurvivalStagePanel();
            updateScoreBoard();
        }

        private void updateSurvivalStagePanel()
        {
            survivalHealthLabel.Text = survivalHealth.ToString();
            survivalShotsLabel.Text = survivalShots.ToString();
            winnerLabel.Text = survivalPoints.ToString();
            scoreTextAfterGameLabel.Visible = true;
        }
    }
}