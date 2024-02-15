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
        //bool memoryPauseBetweenGames = false;
        //int memoryCountPoints = 0;
        int[] bombsMapList = new int[25];
        List<string> sapperActionHistory = new List<string>();
        //int memoryLength = 4;
        PictureBox animationControl;
        bool animationBanClick = false;


        private void sweeperRevertMove()
        {
            if (animationBanClick)
                return;

            var lastMove = movesHistory[movesHistory.Count - 1];
            var lastAction = sapperActionHistory[sapperActionHistory.Count - 1];

            //back to previous player
            var kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
            do
            {

                if (currentPlayer - 1 >= 0)
                    currentPlayer--;
                else
                    currentPlayer = playerCount - 1;

                kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
            } while (!kit.Visible && lastAction != "DEATH");

            var points = 0;
            if (lastAction == "MISS")
            {
                    points = -lastMove;
            }
            else if (lastAction == "KIT")
            {
                //kit.Visible = true;
                kit.Image = Properties.Resources.pilers_sapper;
                kit.Tag = "Sapper";
                points = -5;
            }
            else if (lastAction == "DEATH")
            {
                kit.Visible = true;
                kit.Image = Properties.Resources.heart_valentine;
                kit.Tag = "Health";
            }
            else if (lastAction == "SAFE")
            {
                points = -bombsMapList[lastMove];
            }

            //if not missed, turn back unreavealed box
            if (lastAction != "MISS")
            {
                PictureBox box = (PictureBox)GetControlByName(boxesPanel, ("box" + lastMove));
                box.Image = Properties.Resources.box_normal;
                box.Visible = true;
                if (box.Tag.ToString() == "Box-Bomb-Revealed")
                    box.Tag = "Box-Bomb";                
                else
                    box.Tag = "Box-Normal";
            }

            playerPoints[currentPlayer] += points;
            sapperActionHistory.RemoveAt(sapperActionHistory.Count - 1);
        }

        private void sweeperMissButton()
        {
            if (animationBanClick)
                return;

            var points = -1;
            if (playerPoints[currentPlayer] == 0)
                points = 0;

            playerPoints[currentPlayer] += points;
            sapperActionHistory.Add("MISS");
            movesHistory.Add(points);
            lastMovePlayer = currentPlayer;

            var kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
            do
            {

                if (currentPlayer + 1 < playerCount)
                    currentPlayer++;
                else
                {
                    currentPlayer = 0;
                }

                kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
            } while (!kit.Visible);
            updateScoreBoard();
        }

        private void updateSapperKit()
        {
            for (int i = 0; i < 5; i++)
            {
                var kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + i);
                kit.Image = Properties.Resources.pilers_sapper;
                if (i < playerCount && sweeperMode)
                {
                    kit.Visible = true;
                    kit.Tag = "Sapper";
                }
                else
                {
                    kit.Visible = false;
                    kit.Tag = "";
                }
            }

            //dont make invisible if new game
            animationControl = null;
        }

        private int sapperAddBombCheck(int bombOrNot)
        {
            if (bombOrNot == -1)
                return -1;            
            else
                return 0;
        }
        private void sapperDetectBoxes()
        {
            int counter = 0;
            for(int i=0;i<bombsMapList.Length;i++)
            {
                counter = 0;
                if(i > 4)
                {
                    counter += sapperAddBombCheck(bombsMapList[i - 5]);
                    if (i % 5 != 0)
                        counter += sapperAddBombCheck(bombsMapList[i - 6]);
                    if (i % 5 != 4)
                        counter += sapperAddBombCheck(bombsMapList[i - 4]);
                }
                if(i < 20)
                {
                    counter += sapperAddBombCheck(bombsMapList[i + 5]);
                    if (i % 5 != 0)
                        counter += sapperAddBombCheck(bombsMapList[i + 4]);
                    if (i % 5 != 4)
                        counter += sapperAddBombCheck(bombsMapList[i + 6]);

                }
                if(i % 5 != 0)
                    counter += sapperAddBombCheck(bombsMapList[i - 1]);
                if (i % 5 != 4)
                    counter += sapperAddBombCheck(bombsMapList[i + 1]);




                // counter = bombsMapList[i - 6] + bombsMapList[i - 5] + bombsMapList[i - 4]
                //         + bombsMapList[i - 1] +        i           bombsMapList[i + 1]
                //         + bombsMapList[i + 4] + bombsMapList[i + 5] + bombsMapList[i + 6];
                if(bombsMapList[i] != -1)
                    bombsMapList[i] = -counter;
            }

        }

        private void randomizeBombs()
        {
            for (int i = 0; i <= 24; i++)
            {
                PictureBox box = (PictureBox)GetControlByName(boxesPanel, ("box" + i));
                box.Image = Properties.Resources.box_normal;
                box.Tag = "Box-Normal";
                box.Visible = true;
                bombsMapList[i] = 0;
            }

            //making new random boxes and showing them to remember for player
            var rand = new Random();
            int testVar;
            int bombsCount = playerCount * 2 - 1;
            if (playerCount == 1)
                bombsCount = 2;
            //give text how much bombs is to detonation
            bombCountLabel.Text = (playerCount * 2 - 1).ToString();

            for (int x = 1; x <= bombsCount; x++)
            {
                do
                {
                    testVar = rand.Next() % 25;
                    if (bombsMapList[testVar] != -1)
                    {
                        var box = (PictureBox)GetControlByName(boxesPanel, ("box" + testVar));
                        box.Tag = "Box-Bomb";
                        bombsMapList[testVar] = -1;
                        break;
                    }
                } while (true);
                
            }
        }

        private void sweeperBoxClick(object sender)
        {
            if(!animationBanClick)
            {
                PictureBox boxxx = (PictureBox)sender;
                var kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
                var indexOfBox = int.Parse(boxxx.Name.Substring(3, (boxxx.Name.Length - 3)));
                var points = 0;

                //MessageBox.Show((GetControlsByTag(basicGameTabAlignmentPanel, "Sapper").Count + GetControlsByTag(basicGameTabAlignmentPanel, "Health").Count).ToString());
                if (boxxx.Tag.ToString() == "")
                    return;

                if (boxxx.Tag.ToString() == "Box-Bomb")
                {
                    if (kit.Visible && kit.Tag.ToString() != "Health")
                    {
                        sapperActionHistory.Add("KIT");
                        boxxx.Image = Properties.Resources.bomb_sapper_defused;
                        kit.Image = Properties.Resources.heart_valentine;
                        kit.Tag = "Health";
                        points = 5;
                    }
                    else
                    {
                        sapperActionHistory.Add("DEATH");
                        kit.Tag = "";
                        kit.Visible = false;
                        boxxx.Image = Properties.Resources.bomb_sapper;

                        //if((GetControlsByTag(basicGameTabAlignmentPanel, "Sapper").Count + GetControlsByTag(basicGameTabAlignmentPanel, "Health").Count) == 2 && playerCount > 1)
                        //{
                        //    gameOverPanel.Visible = true;
                        //    winnerLabel.Text = 
                        //}

                        animationTimer.Start();
                        animationBanClick = true;
                        animationControl = boxxx;
                    }
                    boxxx.Tag = "Box-Bomb-Revealed";



                }
                else if (boxxx.Tag.ToString() == "Box-Normal")
                {
                    if (bombsMapList[indexOfBox] == 0)
                        boxxx.Image = Properties.Resources.box_FullGood;
                    if (bombsMapList[indexOfBox] == 1)
                        boxxx.Image = Properties.Resources.box_sapper_1;
                    if (bombsMapList[indexOfBox] == 2)
                        boxxx.Image = Properties.Resources.box_sapper_2;
                    if (bombsMapList[indexOfBox] == 3)
                        boxxx.Image = Properties.Resources.box_sapper_3;
                    if (bombsMapList[indexOfBox] == 4)
                        boxxx.Image = Properties.Resources.box_sapper_4;
                    if (bombsMapList[indexOfBox] == 5)
                        boxxx.Image = Properties.Resources.box_sapper_5;
                    if (bombsMapList[indexOfBox] == 6)
                        boxxx.Image = Properties.Resources.box_sapper_6;
                    if (bombsMapList[indexOfBox] == 7)
                        boxxx.Image = Properties.Resources.box_sapper_7;
                    if (bombsMapList[indexOfBox] == 8)
                        boxxx.Image = Properties.Resources.box_sapper_8;

                    points = bombsMapList[indexOfBox];
                    sapperActionHistory.Add("SAFE");
                    boxxx.Tag = "";
                }
                else
                    return;

                

                playerPoints[currentPlayer] += points;
                movesHistory.Add(indexOfBox);

                lastMovePlayer = currentPlayer;

                if(GetControlsByTag(boxesPanel, "Box-Normal").Count == 0)
                {
                    gameOverPanel.Visible = true;
                    winOutcomes();
                }

                do
                {

                    if (currentPlayer + 1 < playerCount)
                        currentPlayer++;
                    else
                    {
                        currentPlayer = 0;
                    }

                    kit = (PictureBox)GetControlByName(basicGameTabAlignmentPanel, "sapperPilers" + currentPlayer);
                } while (!kit.Visible);

                if ((GetControlsByTag(basicGameTabAlignmentPanel, "Sapper").Count + GetControlsByTag(basicGameTabAlignmentPanel, "Health").Count) == 2 && playerCount > 1)
                {
                    gameOverPanel.Visible = true;
                    winnerLabel.Text = playerName[currentPlayer];
                }

                updateScoreBoard();
            }
        }

        private void startGameMinesweeper()
        {
            int originalSize, newSize;

            if (playerCount >= 1)
            {
                makeControlsInvisible();
                boxesPanel.Visible = true;
                revertMoveButton.Visible = true;
                memoryRememberSchemeLabel.Visible = false;
                showControlsByTagName(basicGameTabAlignmentPanel, "Sapper", true);
                sapperActionHistory.Clear(); 
                movesHistory.Clear();

                resetGame();
                originalSize = boxesPanel.Width;
                updateBoardLook(boxesPanel);
                newSize = boxesPanel.Width;
                scaleBoxesPanel(newSize, originalSize);
                updateScoreBoard();
                randomizeBombs();
                sapperDetectBoxes();
                updateSapperKit();
                hideScoreBoard();
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }


    }
}