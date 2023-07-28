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
           // Properties.Settings.Default["widthBoardInit"] = 'd';
            boardSize = Properties.Settings.Default.boardSize;
            boardPositionX = Properties.Settings.Default.boardPositionX;
            boardPositionY = Properties.Settings.Default.boardPositionY;
            boardColor = Properties.Settings.Default.boardColor;
            updateSavedProperties();
            //updateBoardLook();
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

        private int boardSize;
        private int boardPositionX;
        private int boardPositionY;

        Color boardColor = Color.Black;
        bool firstLoad = true;
        private void updateBoardLook(Control boardControl)
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }
            boardSize = int.Parse(widthBoardTextBox.Text);
            boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            boardPositionY = int.Parse(PositionYBoardTextBox.Text);
            switch (colorBoardListBox.SelectedItem)
            {
                case "Black":
                    boardColor = Color.Black;
                    break;
                case "Violet":
                    boardColor = Color.Violet;
                    break;
                case "Blue":
                    boardColor = Color.Blue;
                    break;
                case "Yellow":
                    boardColor = Color.Yellow;
                    break;
                case "Green":
                    boardColor = Color.Green;
                    break;
                case "Gray":
                    boardColor = Color.Gray;
                    break;
            }

            boardControl.Width = boardSize;
            boardControl.Height = boardSize;

            Point location = new Point(boardPositionX, boardPositionY);
            boardControl.Location = location;

            boardControl.BackColor = boardColor;
        }

        private bool textboxIsPosInteger(Control textboxName)
        {
            int testInt = 0;
            if (int.TryParse(textboxName.Text, out testInt))
            {
                if (testInt >= 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private void updateSavedProperties()
        {
            widthBoardTextBox.Text = boardSize.ToString();
            PositionXBoardTextBox.Text = boardPositionX.ToString();
            PositionYBoardTextBox.Text = boardPositionY.ToString();
            colorBoardListBox.SelectedItem = boardColor;
            maxRoundsTextBox.Text = rounds.ToString();
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
            tabsControl.SelectedTab = settingsTab;
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

        private int detectPointOnBoard(Control BoardControl)
        {
            Color colorOfClicked = GetPixelColor(MousePosition.X * 3 / 2, MousePosition.Y * 3 / 2);

            int centerX = BoardControl.Width / 2 + BoardControl.Location.X,
                centerY = BoardControl.Width / 2 + BoardControl.Location.Y;

            double vectorX = centerX - this.PointToClient(Cursor.Position).X,
                vectorY = centerY - this.PointToClient(Cursor.Position).Y;

            double lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            double sixPointRadius = (double)(BoardControl.Width) / 2.0 * 18 / 122;
            double fourPointRadius = (double)(BoardControl.Width) / 2.0 * 44 / 122;
            double threePointRadius = (double)(BoardControl.Width) / 2.0 * 70 / 122;
            double twoPointRadius = (double)(BoardControl.Width) / 2.0 * 96 / 122;
            double onePointRadius = (double)(BoardControl.Width) / 2.0 * 1;

            if (lengthFromCenter <= sixPointRadius)
                return 6;
            else if (lengthFromCenter <= fourPointRadius)
                return 4;
            else if (lengthFromCenter <= threePointRadius)
                return 3;
            else if (lengthFromCenter <= twoPointRadius)
                return 2;
            else if (lengthFromCenter <= onePointRadius)
                return 1;
            else
                return 0;

        }

        private void previewBoardImage_Click(object sender, EventArgs e)
        {
            dataLabel.Text = detectPointOnBoard(previewBoardImage).ToString();
        }

        private void previewBoardImage_MouseMove(object sender, MouseEventArgs e)
        {
            dataLabel.Text = detectPointOnBoard(previewBoardImage).ToString();
        }

        //Game 1 section

        private int rounds = 10;
        private int currentRound = 1;
        private int playerTurn = 0;
        private int[] playerPoints = new int[4];
        private string[] playerName = new string[4];

        private void updateScoreBoard()
        {
            playerNameLabel1.Text = playerName[0];
            playerNameLabel2.Text = playerName[1];

            playerPointsLabel1.Text = playerPoints[0].ToString();
            playerPointsLabel2.Text = playerPoints[1].ToString();

            if (playerCount>2)
            {
                playerNameLabel3.Text = playerName[2];
                playerPointsLabel3.Text = playerPoints[2].ToString();
            }
            if(playerCount>3)
            {
                playerNameLabel4.Text = playerName[3];
                playerPointsLabel4.Text = playerPoints[3].ToString();
            }

            if(currentRound <= rounds)
                currentRoundLabel.Text = currentRound.ToString();

            for(int i=0;i<playerCount;i++)
            {
                if (i == playerTurn)
                    playerTurnLabel.Text = playerName[i];
            }

            if (textboxIsPosInteger(maxRoundsTextBox))
                rounds = int.Parse(maxRoundsTextBox.Text);
            
            allRoundsLabel.Text = rounds.ToString();
        }

        private void resetGame()
        {
            for (int i = 0; i < playerPoints.Length; i++)
                playerPoints[i] = 0;
            currentRound = 1;
            updateScoreBoard();
        }

        private void startGameOne()
        {
            if (playerCount >= 2)
            {
                tabsControl.SelectedTab = playOneTab;
                updateBoardLook(playBoardImage);
                resetGame();
            }
            else
                MessageBox.Show("Za mało graczy");

        }

        // Game 2 section

        private int[] boxesValue = new int[9];

        private void updateTTTBoardLook(Control panel)
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }
            boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            boardPositionY = int.Parse(PositionYBoardTextBox.Text);
            

            Point location = new Point(boardPositionX, boardPositionY);
            //panel.Location = location;

            panel.BackColor = boardColor;
        }

        private void uploadTTTPlayer()
        {
            if (TTTplayerTurnCross)
                TTTplayerTurnLabel.Text = "Krzyżyk";
            else
                TTTplayerTurnLabel.Text = "Kółko";
        }
        public Control GetControlByName(Control ParentCntl, string NameToSearch)
        {
            if (ParentCntl.Name == NameToSearch)
                return ParentCntl;

            foreach (Control ChildCntl in ParentCntl.Controls)
            {
                Control ResultCntl = GetControlByName(ChildCntl, NameToSearch);
                if (ResultCntl != null)
                    return ResultCntl;
            }
            return null;
        }
        private void uploadTTTBoard()
        {
            for (int i = 0; i < boxesValue.Length; i++)
            {
                string name = "TTTBox" + (i+1);
                //PictureBox box = (PictureBox)this.Controls[name];
                PictureBox box = (PictureBox)GetControlByName(this, name);
                

                if (boxesValue[i] == 0)
                    box.Image = Properties.Resources.empty;
                else if (boxesValue[i] == 1)
                    box.Image = Properties.Resources.ttt_cross;
                else if (boxesValue[i] == -1)
                    box.Image = Properties.Resources.ttt_circle;
            }
        }

        private void resetTTTGame()
        {
            TTTgameOverPanel.Visible = false;
            TTTplayerTurnCross = true;
            for (int i = 0; i < boxesValue.Length; i++)
            {
                boxesValue[i] = 0;
            }

            uploadTTTBoard();
            uploadTTTPlayer();
        }
        private void startGameTwo()
        {
            tabsControl.SelectedTab = playTwoTab;
            resetTTTGame();
            updateTTTBoardLook(TTTBoardPanel);
        }

        private void gameOneButton_Click(object sender, EventArgs e)
        {
            startGameOne();
        }

        private void gameTwoButton_Click(object sender, EventArgs e)
        {
            startGameTwo();
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

        private void saveBoardSettingsButton_Click(object sender, EventArgs e)
        {
            boardSize = int.Parse(widthBoardTextBox.Text);
            boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            boardPositionY = int.Parse(PositionYBoardTextBox.Text);
            switch(colorBoardListBox.SelectedItem)
            {
                case "Black":
                    boardColor = Color.Black;
                    break;
                case "Violet":
                    boardColor = Color.Violet;
                    break;
                case "Blue":
                    boardColor = Color.Blue;
                    break;
                case "Yellow":
                    boardColor = Color.Yellow;
                    break;
                case "Green":
                    boardColor = Color.Green;
                    break;
                case "Gray":
                    boardColor = Color.Gray;
                    break;

            }

            Properties.Settings.Default.boardSize = boardSize;
            Properties.Settings.Default.boardPositionX = boardPositionX;
            Properties.Settings.Default.boardPositionY = boardPositionY;
            Properties.Settings.Default.boardColor = boardColor;
            Properties.Settings.Default.Save();
            updateBoardLook(previewBoardImage);
        }

        private void widthBoardTextBox_TextChanged(object sender, EventArgs e)
        {
            heightBoardTextBox.Text = widthBoardTextBox.Text;
            if (textboxIsPosInteger(widthBoardTextBox))
                updateBoardLook(previewBoardImage);
        }
        private void heightBoardTextBox_TextChanged(object sender, EventArgs e)
        {
            widthBoardTextBox.Text = heightBoardTextBox.Text;
            if (textboxIsPosInteger(heightBoardTextBox))
                updateBoardLook(previewBoardImage);
        }

        private void addPositionXButton_Click(object sender, EventArgs e)
        {
            PositionXBoardTextBox.Text = (int.Parse(PositionXBoardTextBox.Text) + 5).ToString();
        }

        private void substractPositionXButton_Click(object sender, EventArgs e)
        {
            PositionXBoardTextBox.Text = (int.Parse(PositionXBoardTextBox.Text) - 5).ToString();
        }

        private void addPositionYButton_Click(object sender, EventArgs e)
        {
            PositionYBoardTextBox.Text = (int.Parse(PositionYBoardTextBox.Text) + 5).ToString();
        }

        private void substractPositionYButton_Click(object sender, EventArgs e)
        {
            PositionYBoardTextBox.Text = (int.Parse(PositionYBoardTextBox.Text) - 5).ToString();
        }

        private void PositionXBoardTextBox_TextChanged(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(PositionXBoardTextBox))
                updateBoardLook(previewBoardImage);
        }

        private void PositionYBoardTextBox_TextChanged(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(PositionYBoardTextBox))
                updateBoardLook(previewBoardImage);
        }

        int playerCount = 0;
        private void savePlayersButton_Click(object sender, EventArgs e)
        {
            playerName[0] = playerNameTextBox1.Text;
            playerName[1] = playerNameTextBox2.Text;
            playerName[2] = playerNameTextBox3.Text;
            playerName[3] = playerNameTextBox4.Text;

            playerCount = 0;
            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerName[i] != "")
                    playerCount++;
            }

        }
        int currentPlayer = 0;
        private void playBoardImage_Click(object sender, EventArgs e)
        {
            if (currentRound <= rounds)
            {
                playerPoints[currentPlayer] += detectPointOnBoard(playBoardImage);
                if (currentPlayer + 1 < playerCount)
                    currentPlayer++;
                else
                {
                    currentPlayer = 0;
                    if (currentRound <= rounds)
                        currentRound++;
                    if(currentRound>rounds)
                    {
                        bool biggest = true;
                        gameOverPanel.Visible = true;
                        for (int i = 0; i < playerPoints.Length; i++)
                        {
                            biggest = true;
                            for (int j = 0; j < playerPoints.Length; j++)
                            {
                                if (playerPoints[i] < playerPoints[j])
                                    biggest = false;

                            }
                            if (biggest)
                                winnerLabel.Text = playerName[i];
                        }

                    }
                }

                updateScoreBoard();
                playerTurnLabel.Text = playerName[currentPlayer];
            }
            
        }

        private void gameOverExitButton_Click(object sender, EventArgs e)
        {
            gameOverPanel.Visible = false;
            tabsControl.SelectedTab = mainTab;
        }
        int typeBoard = 0;

        private void checkTypeBoard()
        {
            if (typeBoard == -1)
            {
                littlePreviewImage.Image = Properties.Resources.board5;
                typeBoard = 4;
            }
            else if (typeBoard == 0)
                littlePreviewImage.Image = Properties.Resources.board;
            else if (typeBoard == 1)
                littlePreviewImage.Image = Properties.Resources.board2;
            else if (typeBoard == 2)
                littlePreviewImage.Image = Properties.Resources.board3;
            else if (typeBoard == 3)
                littlePreviewImage.Image = Properties.Resources.board4;
            else if (typeBoard == 4)
                littlePreviewImage.Image = Properties.Resources.board5;
            else if (typeBoard == 5)
            {
                littlePreviewImage.Image = Properties.Resources.board;
                typeBoard = 0;
            }
        }

        private void changeBoardTemplateRightButton_Click(object sender, EventArgs e)
        {
            typeBoard++;
            checkTypeBoard();
        }

        private void changeBoardTemplateLeftButton_Click(object sender, EventArgs e)
        {
            typeBoard--;
            checkTypeBoard();
        }

        //Tic tac toe game below

        bool TTTplayerTurnCross = true;

        private bool checkIfWin()
        {
            for (int i = 0; i < boxesValue.Length; i++)
            {
                if(i % 3 == 0)
                    if (boxesValue[i] == boxesValue[i + 1] && boxesValue[i] == boxesValue[i + 2] && boxesValue[i] != 0)
                        return true;
                if(i<3)
                    if (boxesValue[i] == boxesValue[i + 3] && boxesValue[i] == boxesValue[i + 6] && boxesValue[i] != 0)
                        return true;
                if (boxesValue[0] == boxesValue[4] && boxesValue[0] == boxesValue[8] && boxesValue[0] != 0)
                    return true;
                if (boxesValue[2] == boxesValue[4] && boxesValue[2] == boxesValue[6] && boxesValue[2] != 0)
                    return true;
            }
            return false;

        }

        private void changeTTTBox(Control controlBox, int index)
        {
            if (TTTplayerTurnCross)
            {
                if (boxesValue[index] < 1)
                    boxesValue[index]++;
                TTTplayerTurnCross = false;
            }
            else
            {
                if (boxesValue[index] > -1)
                    boxesValue[index]--;
                TTTplayerTurnCross = true;
            }
            uploadTTTBoard();
            uploadTTTPlayer();
            if(checkIfWin())
            {
                if (TTTplayerTurnCross)
                    TTTWinnerLabel.Text = "Kółko";
                else
                    TTTWinnerLabel.Text = "Krzyżyk";

                TTTgameOverPanel.Visible = true;
            }

        }

        private void TTTBox1_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox1, 0);
        }

        private void TTTBox2_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox2, 1);
        }

        private void TTTBox3_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox3, 2);
        }

        private void TTTBox4_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox4, 3);
        }

        private void TTTBox5_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox5, 4);
        }

        private void TTTBox6_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox6, 5);
        }

        private void TTTBox7_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox7, 6);
        }

        private void TTTBox8_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox8, 7);
        }

        private void TTTBox9_Click(object sender, EventArgs e)
        {
            changeTTTBox(TTTBox9, 8);
        }

        private void TTTgameOverExitButton_Click(object sender, EventArgs e)
        {
            TTTgameOverPanel.Visible = false;
            tabsControl.SelectedTab = mainTab;
        }

        private void TTTNewGameButton_Click(object sender, EventArgs e)
        {
            resetTTTGame();
        }
    }
}