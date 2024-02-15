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
        private void colorsRevertMove()
        {
            var lastMove = movesHistory[movesHistory.Count - 1];
            var playerOverwritten = (int)(lastMove / 100);
            var numberOfBox = lastMove % 100;

            if(lastMove != -1)
            {
                var box = (PictureBox)GetControlByName(boxesPanel, "box" + numberOfBox);

                //MessageBox.Show(box.Tag.ToString());
                //MessageBox.Show(playerOverwritten.ToString());
                if (playerOverwritten == 1)
                {
                    box.Image = Properties.Resources.box_color1;
                    box.Tag = "Colors-0";
                }
                if (playerOverwritten == 2)
                {
                    box.Image = Properties.Resources.box_color2;
                    box.Tag = "Colors-1";
                }
                if (playerOverwritten == 3)
                {
                    box.Image = Properties.Resources.box_color3;
                    box.Tag = "Colors-2";
                }
                if (playerOverwritten == 4)
                {
                    box.Image = Properties.Resources.box_color4;
                    box.Tag = "Colors-3";
                }
                if (playerOverwritten == 5)
                {
                    box.Image = Properties.Resources.box_color5;
                    box.Tag = "Colors-4";
                }

                if (playerOverwritten == 0)
                {
                    box.Image = Properties.Resources.box_normal;
                    box.Tag = "";
                }
                else
                    playerPoints[playerOverwritten - 1] += 1;
            }
                    



            //assess if reverting move will change round for current player or not
            if (currentPlayer - 1 >= 0)
                currentPlayer--;
            else
            {
                currentPlayer = playerCount - 1;

                if (currentRound > 0)
                    currentRound -= 1;

            }

            //assess if reverting move will change round for player before current or not
            if (currentPlayer - 1 >= 0)
                lastMovePlayer--;
            else
                lastMovePlayer = playerCount - 1;

            //if miss then dont doo
            if (lastMove != -1)
            {
                playerPoints[currentPlayer] -= 1;
            }

        }

        private void colorsMissButton()
        {
            var points = 0;

            movesHistory.Add(-1);
            lastMovePlayer = currentPlayer;

            if (currentPlayer + 1 < playerCount)
                currentPlayer++;
            else
            {
                currentPlayer = 0;
                if (currentRound <= rounds)
                {
                    currentRound++;
                }
                if (currentRound > rounds)
                {
                    gameOverPanel.Visible = true;
                    winOutcomes();
                }
            }

            updateScoreBoard();
        }

        private void updateColorsIcons()
        {
            if(colorsMode)
            {
                for(int i=0;i<5;i++)
                {
                    var icon = GetControlByName(basicGameTabAlignmentPanel, "colorPlayer" + i);

                    if (i < playerCount)
                        icon.Visible = true;
                    else
                        icon.Visible = false;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    var icon = GetControlByName(basicGameTabAlignmentPanel, "colorPlayer" + i);
                    icon.Visible = false;
                }
            }
        }

        private void resetColorTags()
        {
            for(int i=0;i<25;i++)
            {
                var box = GetControlByName(boxesPanel, "box" + i);
                box.Tag = "";
            }
        }
        private void colorsBoxClick(object sender)
        {
            if (gameOverPanel.Visible)
                return;

            if (currentRound <= rounds)
            {
                PictureBox box = (PictureBox)sender;
                var indexOfBox = int.Parse(box.Name.Substring(3, (box.Name.Length - 3)));
                int OverWritePlayer = 0;
                //detele point for opponent
                for (int i = 0; i < playerCount; i++)
                {
                    if (box.Tag.ToString() == "Colors-" + i)
                    {
                        playerPoints[i] -= 1;
                        OverWritePlayer = i * 100 + 100;
                    }

                }

                if (currentPlayer == 0)
                {
                    box.Image = Properties.Resources.box_color1;
                    box.Tag = "Colors-0";
                }    
                if (currentPlayer == 1)
                {
                    box.Image = Properties.Resources.box_color2;
                    box.Tag = "Colors-1";
                }
                if (currentPlayer == 2)
                {
                    box.Image = Properties.Resources.box_color3;
                    box.Tag = "Colors-2";
                }
                if (currentPlayer == 3)
                {
                    box.Image = Properties.Resources.box_color4;
                    box.Tag = "Colors-3";
                }
                if (currentPlayer == 4)
                {
                    box.Image = Properties.Resources.box_color5;
                    box.Tag = "Colors-4";
                }

                movesHistory.Add(OverWritePlayer + indexOfBox);
                lastMovePlayer = currentPlayer;

                playerPoints[currentPlayer] += 1;

                if (currentPlayer + 1 < playerCount)
                    currentPlayer++;
                else
                {
                    currentPlayer = 0;
                    if (currentRound <= rounds)
                    {
                        currentRound++;
                    }
                    if (currentRound > rounds)
                    {
                        gameOverPanel.Visible = true;
                        winOutcomes();
                        //showScoreBoard();
                    }
                }

                updateScoreBoard();
            }

            
        }

        private void startGameColors()
        {
            int originalSize, newSize;

            makeControlsInvisible();
            boxesPanel.Visible = true;
            revertMoveButton.Visible = true;
            memoryRememberSchemeLabel.Visible = false;

            if (playerCount >= 1)
            {
                resetGame();
                originalSize = boxesPanel.Width;
                updateBoardLook(boxesPanel);
                newSize = boxesPanel.Width;
                scaleBoxesPanel(newSize, originalSize);
                updateScoreBoard();
                hideScoreBoard();
                resetColorTags();
                updateColorsIcons();
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }
    }
}


//resetGame();
//randomizeBombs();
//sapperDetectBoxes();
//updateSapperKit();
//hideScoreBoard();
//tabsControl.SelectedTab = playOneTab;