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
        bool memoryPauseBetweenGames = false;
        int memoryCountPoints = 0;
        List<int> goodBoxes = new List<int>();
        int memoryLength;

        private void memoryBoxClick(object sender)
        {
            if (memoryPauseBetweenGames)
            {
                memoryPauseBetweenGames = false;
                if (memoryLength < 15)
                    memoryLength += 1;
                randomizeGoodBoxes(memoryLength);
                return;
            }

            if (memoryRememberSchemeLabel.Visible)
            {
                foreach (PictureBox boxx in GetControlsByTag(boxesPanel, "Box-ClickIt"))
                    boxx.Image = Properties.Resources.box_normal;

                memoryRememberSchemeLabel.Visible = false;
                return;
            }

            PictureBox box = (PictureBox)sender;

            var points = 0;
            var indexOfBox = int.Parse(box.Name.Substring(3, (box.Name.Length - 3)));

            if (box.Tag.ToString() == "Box-ClickIt")
            {
                points = 1;
                box.Tag = "Box-Good";
                box.Image = Properties.Resources.box_good;
            }
            else if (box.Tag.ToString() == "Box-Normal")
            {
                box.Image = Properties.Resources.box_bad;
                box.Tag = "Box-Bad";
                if (GetControlsByTag(boxesPanel, "Box-Bad").Count() > 2)
                {
                    gameOverPanel.Visible = true;
                    updateMemoryStagePanel();
                }
            }
            else
                return;

            if (memoryLength == GetControlsByTag(boxesPanel, "Box-Good").Count())
            {
                foreach (PictureBox boxx in GetControlsByTag(boxesPanel, "Box-Good"))
                    boxx.Image = Properties.Resources.box_FullGood;

                memoryPauseBetweenGames = true;
            }


            memoryCountPoints += points;
            playerPoints[currentPlayer] += points;
            movesHistory.Add(indexOfBox);

            lastMovePlayer = currentPlayer;

            if (currentPlayer + 1 < playerCount)
                currentPlayer++;
            else
            {
                currentPlayer = 0;
            }

            updateScoreBoard();
            
        }

        private void updateMemoryStagePanel()
        {
            //survivalHealthLabel.Text = survivalHealth.ToString();
            //survivalShotsLabel.Text = survivalShots.ToString();

            winnerLabel.Text = memoryCountPoints.ToString();
            scoreTextAfterGameLabel.Visible = true;
        }

        private void memoryMissButton()
        {
            playerPoints[currentPlayer] += 0;
            movesHistory.Add(-1);

            lastMovePlayer = currentPlayer;

            if (currentPlayer + 1 < playerCount)
                currentPlayer++;
            else
                currentPlayer = 0;

            updateScoreBoard();
        }

        private void memoryRevertMove()
        {
            if(movesHistory.Count > 0)
            {
                

                if (currentPlayer - 1 >= 0)
                    currentPlayer--;
                else
                    currentPlayer = playerCount - 1;

                var indexOfBox = movesHistory[movesHistory.Count - 1];
                if (indexOfBox == -1)
                    return;

                PictureBox box = (PictureBox)GetControlByName(boxesPanel, ("box" + indexOfBox));

                if (box.Tag.ToString() == "Box-Good")
                {

                    if (memoryLength == GetControlsByTag(boxesPanel, "Box-Good").Count())
                    {
                        foreach (PictureBox boxx in GetControlsByTag(boxesPanel, "Box-Good"))
                            boxx.Image = Properties.Resources.box_good;
                        memoryPauseBetweenGames = false;
                    }

                    playerPoints[currentPlayer] -= 1;
                    box.Tag = "Box-ClickIt";
                }
                else 
                    box.Tag = "Box-Normal";
                
                    
                
                box.Image = Properties.Resources.box_normal;

            }
        }

        private void randomizeGoodBoxes(int length)
        {
            //clearing previous goodboxes and claering board to normal
            movesHistory.Clear();
            goodBoxes.Clear();
            for (int i = 0; i <= 24; i++)
            {
                PictureBox box = (PictureBox)GetControlByName(boxesPanel, ("box" + i));
                box.Image = Properties.Resources.box_normal;
                box.Tag = "Box-Normal";
            }

            //making new random boxes and showing them to remember for player
            var rand = new Random();
            int testVar;
            for(int x = 0;x<length; x++)
            {
                do
                {
                    testVar = rand.Next() % 25;
                    if (!goodBoxes.Contains(testVar))
                    {
                        var box = (PictureBox)GetControlByName(boxesPanel, ("box" + testVar));
                        box.Tag = "Box-ClickIt";
                        box.Image = Properties.Resources.box_FullGood;
                        goodBoxes.Add(testVar);
                        break;
                    }
                } while (goodBoxes.Contains(testVar));
            }
            memoryRememberSchemeLabel.Visible = true;

        }

        private void scaleBoxesPanel(int newSize, int originalSize)
        {
            double scale = newSize * 1.0 / originalSize * 1.0;
            Point location;
            for (int i = 0; i <= 24; i++)
            {
                PictureBox box = (PictureBox)GetControlByName(boxesPanel, ("box" + i));
                box.Width = (int)Math.Round(box.Width * scale);
                box.Height = (int)Math.Round(box.Height * scale);
                location = new Point((int)Math.Round(box.Location.X * scale), (int)Math.Round(box.Location.Y * scale));
                box.Location = location;
                box.Image = Properties.Resources.box_normal;
                box.Tag = "Box-Normal";
                box.Visible = true;
            }
            memoryRememberSchemeLabel.Width = (int)(memoryRememberSchemeLabel.Width * scale);
        }


        private void startGameMemory()
        {
            int originalSize, newSize; 
            // startGameOne();
            makeControlsInvisible();
            boxesPanel.Visible = true;
            revertMoveButton.Visible = true;
            memoryTextBannerLabel.Visible = true;
            memoryRememberSchemeLabel.Visible = true;
            memoryPauseBetweenGames = false;
            if (playerCount >= 1)
            {
                resetGame();
                // updateBoardLook(playBoardImage, playBoardImagePanel);
                originalSize = boxesPanel.Width;
                updateBoardLook(boxesPanel);
                newSize = boxesPanel.Width;
                scaleBoxesPanel(newSize, originalSize);
                updateScoreBoard();
                hideScoreBoard();
                memoryCountPoints = 0;
                memoryLength = 5;
                randomizeGoodBoxes(memoryLength);
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }


    }
}