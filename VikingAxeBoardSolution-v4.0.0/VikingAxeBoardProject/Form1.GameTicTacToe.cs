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
    public partial class VikingAxeProjectForm 
    {

        private int[] boxesValue = new int[9];

        private void scaleTTTBoard(int newSize, int originalSize)
        {
            double scale = newSize * 1.0 / originalSize * 1.0;
            Point location;
            for (int i = 1; i <= 9; i++)
            {
                Control tttBox = (Control)GetControlByName(this, ("TTTBox" + i));
                tttBox.Width = (int)Math.Round(tttBox.Width * scale);
                tttBox.Height = (int)Math.Round(tttBox.Height * scale);
                location = new Point((int)Math.Round(tttBox.Location.X * scale), (int)Math.Round(tttBox.Location.Y * scale));
                tttBox.Location = location;
            }
            TTTBoardImage.Width = (int)Math.Round(TTTBoardImage.Width * scale);
            TTTBoardImage.Height = (int)Math.Round(TTTBoardImage.Height * scale);
            location = new Point((int)Math.Round(TTTBoardImage.Location.X * scale), (int)Math.Round(TTTBoardImage.Location.Y * scale));
            TTTBoardImage.Location = location;

            TTTBoardPanel.Width = (int)Math.Round(TTTBoardPanel.Width * scale);
            TTTBoardPanel.Height = (int)Math.Round(TTTBoardPanel.Height * scale);
            location = new Point((int)Math.Round(TTTBoardPanel.Location.X * scale), (int)Math.Round(TTTBoardPanel.Location.Y * scale));
            TTTBoardPanel.Location = location;
        }

        private void updateTTTBoardLook(Control panel)
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }

            boardSize = int.Parse(widthBoardTextBox.Text);

            boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            boardPositionY = int.Parse(PositionYBoardTextBox.Text);
            boardSize = int.Parse(heightBoardTextBox.Text);

            if (textboxIsPosInteger(TTTMaxRoundsTextBox))
                TTTRounds = int.Parse(TTTMaxRoundsTextBox.Text);

            TTTAllRoundsLabel.Text = TTTRounds.ToString();

            if (currentRound <= TTTRounds)
                currentRoundLabel.Text = currentRound.ToString();

            Point location = new Point(boardPositionX, boardPositionY);
            panel.Location = location;

            scaleTTTBoard(boardSize, panel.Width);

            panel.BackColor = boardColor;
        }

        private void highlightTTTPlayer(bool off)
        {
            if (TTTplayerTurnCross)
            {
                TTTScoreCircleLabel.BackColor = Color.Transparent;
                TTTCircleTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
                TTTScoreCrossLabel.BackColor = Color.FromArgb(41, 40, 36);
                TTTCrossTurnLabel.BackColor = Color.FromArgb(94, 92, 82);
            }
            else
            {
                TTTScoreCircleLabel.BackColor = Color.FromArgb(41, 40, 36);
                TTTCircleTurnLabel.BackColor = Color.FromArgb(94, 92, 82);
                TTTScoreCrossLabel.BackColor = Color.Transparent;
                TTTCrossTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
            }
            if (off)
            {
                TTTScoreCircleLabel.BackColor = Color.Transparent;
                TTTCircleTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
                TTTScoreCrossLabel.BackColor = Color.Transparent;
                TTTCrossTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
            }
        }

        private void uploadTTTBoard()
        {
            TTTScoreCircleLabel.Text = TTTCirclePoints.ToString();
            TTTScoreCrossLabel.Text = TTTCrossPoints.ToString();
            TTTCurrentRoundLabel.Text = currentRound.ToString();

            for (int i = 0; i < boxesValue.Length; i++)
            {
                string name = "TTTBox" + (i + 1);
                PictureBox box = (PictureBox)GetControlByName(this, name);

                if (boxesValue[i] == 0)
                    box.BackgroundImage = null;
                else if (boxesValue[i] == 1)
                    box.BackgroundImage = Properties.Resources.ttt_cross;
                else if (boxesValue[i] == -1)
                    box.BackgroundImage = Properties.Resources.ttt_circle;
            }
            highlightTTTPlayer(false);
        }

        private void resetTTTGame()
        {
            TTTMovesHistory.Clear();

            TTTGameOverPanel.Visible = false;
            if (currentRound % 2 == 1)
                TTTplayerTurnCross = true;
            else
                TTTplayerTurnCross = false;

            //MessageBox.Show(currentRound + " not devidable by two: " + TTTplayerTurnCross.ToString());
            //checking if shifting priority works

            for (int i = 0; i < boxesValue.Length; i++)
            {
                boxesValue[i] = 0;
            }

            uploadTTTBoard();
            updateTTTBoardLook(TTTBoardPanel);
        }
        private void startGameTTT()
        {
            resetTTTGame();
            currentRound = 1;
            TTTCrossPoints = 0;
            TTTCirclePoints = 0;

            uploadTTTBoard();
            updateTTTBoardLook(TTTBoardPanel);

            tabsControl.SelectedTab = playTwoTab;
        }

        bool TTTplayerTurnCross = true;

        private void makeWinLineTTT(int index, int crossWon)
        {
            string name = "TTTBox" + index;
            PictureBox box = (PictureBox)GetControlByName(this, name);
            if (crossWon == 1)
                box.BackgroundImage = Properties.Resources.ttt_cross_win;
            else
                box.BackgroundImage = Properties.Resources.ttt_circle_win;
        }

        private bool TTTCheckIfWinRound()
        {
            bool win = false;
            for (int i = 0; i < boxesValue.Length; i++)
            {
                if (i % 3 == 0)
                    if (boxesValue[i] == boxesValue[i + 1] && boxesValue[i] == boxesValue[i + 2] && boxesValue[i] != 0)
                    {
                        makeWinLineTTT(i + 1, boxesValue[i]);
                        makeWinLineTTT(i + 2, boxesValue[i]);
                        makeWinLineTTT(i + 3, boxesValue[i]);
                        win = true;
                    }
                if (i < 3)
                    if (boxesValue[i] == boxesValue[i + 3] && boxesValue[i] == boxesValue[i + 6] && boxesValue[i] != 0)
                    {

                        makeWinLineTTT(i + 1, boxesValue[i]);
                        makeWinLineTTT(i + 4, boxesValue[i]);
                        makeWinLineTTT(i + 7, boxesValue[i]);
                        win = true;
                    }
            }

            if (boxesValue[0] == boxesValue[4] && boxesValue[0] == boxesValue[8] && boxesValue[0] != 0)
            {
                makeWinLineTTT(1, boxesValue[0]);
                makeWinLineTTT(5, boxesValue[0]);
                makeWinLineTTT(9, boxesValue[0]);
                win = true;
            }
            if (boxesValue[2] == boxesValue[4] && boxesValue[2] == boxesValue[6] && boxesValue[2] != 0)
            {
                makeWinLineTTT(3, boxesValue[2]);
                makeWinLineTTT(5, boxesValue[2]);
                makeWinLineTTT(7, boxesValue[2]);
                win = true;
            }

            if (win)
                return true;

            return false;

        }

        private bool TTTCheckIfWinGame()
        {
            bool win = false;
            if (TTTCrossPoints > TTTRounds / 2 || TTTCirclePoints > TTTRounds / 2 || currentRound == TTTRounds)
            {
                win = true;
                if (TTTCrossPoints > TTTCirclePoints)
                    TTTWinnerLabel.Text = TTTCrossTurnLabel.Text;
                else if (TTTCrossPoints < TTTCirclePoints)
                    TTTWinnerLabel.Text = TTTCircleTurnLabel.Text;
                else
                    TTTWinnerLabel.Text = drawText;

            }

            if (win)
            {
                uploadTTTBoard();

                TTTGameOverPanel.Visible = true;
                return true;
            }
            else
            {
                TTTGameOverPanel.Visible = false;
                return false;
            }

        }

        bool endOfTTTRound = false;

        private void changeTTTBox(int index)
        {
            if (TTTGameOverPanel.Visible)
                return;

            if (index == -1)
            {
                TTTplayerTurnCross = !TTTplayerTurnCross;
                highlightTTTPlayer(false);
                return;
            }

            if (!endOfTTTRound)
            {
                if (!reverting)
                    TTTMovesHistory.Add(index);

                if (TTTplayerTurnCross)
                {
                    if (boxesValue[index] < 1)
                        boxesValue[index]++;
                    else
                        TTTMovesHistory[TTTMovesHistory.Count - 1] = -1;

                    TTTplayerTurnCross = false;
                }
                else
                {
                    if (boxesValue[index] > -1)
                        boxesValue[index]--;
                    else
                        TTTMovesHistory[TTTMovesHistory.Count - 1] = -1;

                    TTTplayerTurnCross = true;
                }

                uploadTTTBoard();

                if (TTTCheckIfWinRound())
                {

                    if (TTTplayerTurnCross)
                        TTTCirclePoints += 1;
                    else
                        TTTCrossPoints += 1;

                    if (TTTCheckIfWinGame())
                    {
                        TTTGameOverPanel.Visible = true;
                        highlightTTTPlayer(true);
                    }

                    if (currentRound + 1 <= TTTRounds)
                        currentRound++;

                    endOfTTTRound = true;
                    //click anywhere on form
                }
            }
            else
            {
                endOfTTTRound = false;

                resetTTTGame();
                uploadTTTBoard();
                updateTTTBoardLook(TTTBoardPanel);
            }

        }
        private void changeTTTBoxWhenMiss()
        {
            TTTMovesHistory.Add(-1);

            if (TTTplayerTurnCross)
            {
                TTTplayerTurnCross = false;
            }
            else
            {
                TTTplayerTurnCross = true;
            }
            uploadTTTBoard();
        }
    }
}