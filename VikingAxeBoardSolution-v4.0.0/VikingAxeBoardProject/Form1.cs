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
    public partial class VikingAxeProjectForm : Form
    {
        private void initialize()
        {
            InitializeComponent();
            WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds; //making problems on second monitor?
            // Properties.Settings.Default["widthBoardInit"] = 'd';
            boardSize = Properties.Settings.Default.boardSize;
            boardPositionX = Properties.Settings.Default.boardPositionX;
            boardPositionY = Properties.Settings.Default.boardPositionY;
            boardColor = Properties.Settings.Default.boardColor;
            boardType = Properties.Settings.Default.boardType;
            updateSavedProperties();
            InitCustomLabelFont();
            updateLogos();
            alignPanelsToBoard();
        }

        public VikingAxeProjectForm()
        {
            initialize();
        }

        public int boardSize;
        public int boardPositionX;
        public int boardPositionY;
        Color boardColor = Color.Black;
        public int boardType;

        public bool olimpicGame = false, survivalMode = false, arenaMode = false,  attackMode = false,  memoryMode = false,  sweeperMode = false,  colorsMode = false;
        public bool firstLoad = true;


        private void updateLogos()
        {
            updateControlLook(logoPictureBox1);
            updateControlLook(logoPictureBox2);
            updateControlLook(logoPictureBox3);
            updateControlLook(logoPictureBox4);
            updateControlLook(gameOverPanel);
            updateControlLook(TTTGameOverPanel);
        }

        public void makeControlsInvisible()
        {
            animationBanClick = false;

            survivalGamePanel.Visible = false;
            playBoardImagePanel.Visible = false;
            revertMoveButton.Visible = false;
            gameOverPanel.Visible = false;
            attackModePanel.Visible = false;
            boxesPanel.Visible = false;
            memoryTextBannerLabel.Visible = false;
            showControlsByTagName(this, "Survival", false);
            showControlsByTagName(basicGameTabAlignmentPanel, "Sapper", false);
            updateArenaHealthPanel();
            updateSapperKit();
            updateColorsIcons();
            hideScoreBoard();
        }

        private void updateControlLook(Control boardControl)
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }
            boardSize = int.Parse(widthBoardTextBox.Text);
            boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            boardPositionY = int.Parse(PositionYBoardTextBox.Text);

            boardControl.Width = boardSize;
            boardControl.Height = boardSize;

            Point location = new Point(boardPositionX, boardPositionY);
            boardControl.Location = location;

            boardControl.BackColor = boardColor;
        }

        private void alignPanelsToBoard()
        {
            //change position of all panels that has
            Point location;
            foreach (Control control in GetControlsByTag(this,"Alignment"))
            {
                    location = new Point(boardPositionX - (control.Width - boardSize) / 2, control.Location.Y);
                    control.Location = location;
            }
        }

        private void checkBoardConditions()
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }
            if (textboxIsPosInteger(widthBoardTextBox))
                boardSize = int.Parse(widthBoardTextBox.Text);
            if (textboxIsPosInteger(PositionXBoardTextBox))
                boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            if (textboxIsPosInteger(PositionYBoardTextBox))
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
            setRedDotImage();
            alignPanelsToBoard();
        }
        private void updateBoardLook(PictureBox boardControl, Panel panelBoard)
        {
            checkBoardConditions();

            boardControl.Width = boardSize;
            boardControl.Height = boardSize;

            panelBoard.Width = boardSize;
            panelBoard.Height = boardSize;


            Point location = new Point(boardPositionX, boardPositionY);
            panelBoard.Location = location;

            location = new Point(0, 0);
            boardControl.Location = location;

            boardControl.BackColor = Color.Transparent;
            panelBoard.BackColor = boardColor;

            if (olimpicGame)
                boardControl.Image = Properties.Resources.board_olimpic;
            else
            {
                if (boardType == 0)
                    boardControl.Image = Properties.Resources.board;
                if (boardType == 1)
                    boardControl.Image = Properties.Resources.board2;
                if (boardType == 2)
                    boardControl.Image = Properties.Resources.board3;
                if (boardType == 3)
                    boardControl.Image = Properties.Resources.board4;
                if (boardType == 4)
                    boardControl.Image = Properties.Resources.board5;
            }
        }
        private void updateBoardLook(PictureBox boardControl)
        {
            checkBoardConditions();

            boardControl.Width = boardSize;
            boardControl.Height = boardSize;

            Point location = new Point(boardPositionX, boardPositionY);
            boardControl.Location = location;

            boardControl.BackColor = boardColor;

            if (olimpicGame)
                boardControl.Image = Properties.Resources.board_olimpic;
            else
            {
                if (boardType == 0)
                    boardControl.Image = Properties.Resources.board;
                if (boardType == 1)
                    boardControl.Image = Properties.Resources.board2;
                if (boardType == 2)
                    boardControl.Image = Properties.Resources.board3;
                if (boardType == 3)
                    boardControl.Image = Properties.Resources.board4;
                if (boardType == 4)
                    boardControl.Image = Properties.Resources.board5;
            }
        }
        private void updateBoardLook(Control boardControl)
        {
            checkBoardConditions();

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
            TTTMaxRoundsTextBox.Text = TTTRounds.ToString();
        }

        private void VikingAxeProjectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                if (tabsControl.SelectedTab != mainTab)
                    tabsControl.SelectedTab = mainTab;
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
            Control textBox;
            for (int i = 0; i < playerName.Length; i++)
            {
                textBox = GetControlByName(this, "playerNameTextBox" + (i + 1));
                textBox.Text = playerName[i];
            }
            tabsControl.SelectedTab = playersTab;
        }

        private void previewBoardButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = previewBoardTab;
        }

        private bool checkEightOlimpic(PictureBox BoardControl)
        {
            if (olimpicGame)
            {
                double eightPointRadius = (double)(BoardControl.Width) * 50 / 700 * 0.54;
                double vector = (BoardControl.Location.X - 0.304 * BoardControl.Width);
               // MessageBox.Show(vector + " " + BoardControl.Location.X);
                if (calculateDistance(0.394 * BoardControl.Width, 0, eightPointRadius, 0, 0.394 * BoardControl.Height) <= eightPointRadius)
                    return true;
                if (calculateDistance(1.606 * BoardControl.Width, 0, eightPointRadius, 0, 0.394 * BoardControl.Height) <= eightPointRadius)
                    return true;
            }
            return false;
        }

        private double calculateDistance(double boardWidthX, double boardLocationX, double radius, double boardLocationY, double boardHeightY)
        {
            double centerX = boardWidthX / 2;
            double centerY = boardHeightY / 2 ; //this -3 idk why is neccesary to be more accurate?

            if(olimpicGame)
            {
                centerX = boardWidthX / 2;
                centerY = boardHeightY / 2;
            }
            else if (typeBoard == 1)
            {
                centerX = boardWidthX / 2 + boardWidthX / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 - (boardHeightY / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationY;
            }
            else if(typeBoard == 2)
            {
                centerX = boardWidthX / 2 + boardWidthX / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 + boardHeightY / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationY;
            }
            else if (typeBoard == 3)
            {
                centerX = boardWidthX / 2 - (boardWidthX / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 + boardHeightY / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationY;
            }
            else if (typeBoard == 4)
            {
                centerX = boardWidthX / 2 - (boardWidthX / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 - (boardHeightY / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationY;
            }

            double vectorX = centerX - playBoardImagePanel.PointToClient(Cursor.Position).X;
            double vectorY = centerY - playBoardImagePanel.PointToClient(Cursor.Position).Y;

            double lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            return lengthFromCenter;
        }

        double sixPointRadius, fourPointRadius, threePointRadius, twoPointRadius, onePointRadius;

        private int detectPointOnBoard(PictureBox BoardControl)
        {
            sixPointRadius = (double)(BoardControl.Width) * 50 / 700;
            fourPointRadius = (double)(BoardControl.Width) * 125 / 700;
            threePointRadius = (double)(BoardControl.Width) * 200 / 700;
            twoPointRadius = (double)(BoardControl.Width) * 275 / 700;
            onePointRadius = (double)(BoardControl.Width) * 350 / 700;

            if(olimpicGame)
            {
                fourPointRadius = (double)(BoardControl.Width) * 121 / 700;
                threePointRadius = (double)(BoardControl.Width) * 193 / 700;
                twoPointRadius = (double)(BoardControl.Width) * 266 / 700;
                onePointRadius = (double)(BoardControl.Width) * 350 / 700;
            }

            if (checkEightOlimpic(BoardControl))
                return 8;

            if (calculateDistance(BoardControl.Width, BoardControl.Location.X, sixPointRadius, BoardControl.Location.Y, BoardControl.Height) <= sixPointRadius)
                return 6;
            else if (calculateDistance(BoardControl.Width, BoardControl.Location.X, fourPointRadius, BoardControl.Location.Y, BoardControl.Height) <= fourPointRadius)
                return 4;
            else if (calculateDistance(BoardControl.Width, BoardControl.Location.X, threePointRadius, BoardControl.Location.Y, BoardControl.Height) <= threePointRadius)
                return 3;
            else if (calculateDistance(BoardControl.Width, BoardControl.Location.X, twoPointRadius, BoardControl.Location.Y, BoardControl.Height) <= twoPointRadius)
                return 2;
            else if (calculateDistance(BoardControl.Width, BoardControl.Location.X, onePointRadius, BoardControl.Location.Y, BoardControl.Height) <= onePointRadius)
                return 1;
            else
                return 0;
        }


        //Game section

        private int rounds = 10;
        private int TTTRounds = 5;
        private int currentRound = 1;
        private int[] playerPoints = new int[5];
        private string[] playerName = new string[5];


        private void highlightPlayerWindow(int playerIndex, bool off)
        {
            for (int i = 0; i < 5; i++)
            {
                string labelName = "playerNameLabel" + (i + 1);
                Label label = (Label)GetControlByName(this, labelName);
                if (i != playerIndex || off)
                    label.BackColor = Color.Transparent;
                else
                    //label.BackColor = Color.FromArgb(41, 40, 36); //first original color
                    label.BackColor = Color.FromArgb(31, 30, 26);


                labelName = "playerPointsLabel" + (i + 1);
                label = (Label)GetControlByName(this, labelName);
                if (i != playerIndex || off)
                    label.BackColor = Color.FromArgb(41, 40, 36);
                else
                    //label.BackColor = Color.FromArgb(94, 92, 82); //first original color
                    label.BackColor = Color.FromArgb(64, 62, 52);

            }
        }

        private void updateScoreBoard()
        {
            for (int i = 0; i < playerName.Length; i++)
            {
                string labelName = "playerNameLabel" + (i + 1);
                Label label = (Label)GetControlByName(this, labelName);
                label.Visible = true;

                labelName = "playerPointsLabel" + (i + 1);
                Label label2 = (Label)GetControlByName(this, labelName);
                label2.Visible = true;

                if (playerName[i] != "")
                {
                    label.Text = playerName[i];
                    label2.Text = playerPoints[i].ToString();
                }
                else
                {
                    label.Text = "";
                    label2.Text = "";
                }

                if (i >= playerCount)
                {
                    labelName = "playerPointsLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;

                    labelName = "playerNameLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;
                }

                roundsPanel.Location = new Point(14, 417 - 71 * (playerName.Length - playerCount));
            }

            if (currentRound <= rounds)
                currentRoundLabel.Text = currentRound.ToString();

            if (!gameOverPanel.Visible)
                highlightPlayerWindow(currentPlayer, false);

            if (textboxIsPosInteger(maxRoundsTextBox))
                rounds = int.Parse(maxRoundsTextBox.Text);

            allRoundsLabel.Text = rounds.ToString();
        }

        private void resetGame()
        {
            movesHistory.Clear();

            for (int i = 0; i < playerPoints.Length; i++)
                playerPoints[i] = 0;
            currentRound = 1;
            currentPlayer = 0;
            boardType = 0;
            typeBoard = 0;
        }

        private void startGameOne()
        {
            makeControlsInvisible();
            playBoardImagePanel.Visible = true;
            revertMoveButton.Visible = true;
            if (playerCount >= 1)
            {
                resetGame();
                updateBoardLook(playBoardImage, playBoardImagePanel);
                updateScoreBoard();
                hideScoreBoard();
                tabsControl.SelectedTab = playOneTab;
                if (arenaMode)
                    arenaSetUp();
            }
            else
                tabsControl.SelectedTab = playersTab;

        }

        private List<Control> GetControlsByTag(Control container, string tag)
        {
            List<Control> result = new List<Control>();

            foreach (Control control in container.Controls)
            {
                if (control.Tag != null && control.Tag.ToString() == tag)
                {
                    result.Add(control);
                }

                // Recursively check child controls
                if (control.HasChildren)
                {
                    result.AddRange(GetControlsByTag(control, tag));
                }
            }

            return result;
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

        private void showControlsByTagName(Control parentControl, string targetTagName, bool show)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control.Tag != null && control.Tag.ToString() == targetTagName)
                {
                    control.Visible = show;
                }

                if (control.HasChildren)
                {
                    showControlsByTagName(control, targetTagName, show);
                }

            }
        }

        private void shipSurvivalPictureBox_Click(object sender, EventArgs e)
        {
            PictureBox shipControl = null;
            var points = 1;

            if (gameOverPanel.Visible)
                return;

            try
            {
                shipControl = (PictureBox)sender;
            }
            catch
            {
                points = 0;
                survivalHealth -= survivalShipsCount;
            }

            if (shipControl != null)
            {
                if (shipControl.Tag.ToString() == "SilverShip")
                {
                    points = 2;
                    shipControl.Image = Properties.Resources.ship;
                    shipControl.Tag = "NormalShip";
                }
                else
                {
                    shipControl.Visible = false;
                    survivalShipsCount--;
                    survivalNextStage();
                }
            }

            playerPoints[currentPlayer] += points;
            survivalPoints += points;
            movesHistory.Add(points);
            lastMovePlayer = currentPlayer;

            if (currentPlayer + 1 < playerCount)
                currentPlayer++;
            else
            {
                currentPlayer = 0;
                if (currentRound <= rounds)
                    currentRound++;
            }

            //game over condidtion
            if (survivalHealth < 1)
            {
                gameOverPanel.Visible = true;
                highlightPlayerWindow(0, true);
            }

            updateScoreBoard();
            updateSurvivalStagePanel();
        }

        //Game Survival end
        //Game Arena Mode



        private void arenaModePictureBox_Click(object sender, EventArgs e)
        {
            if (arenaSetUpMode)
            {
                arenaSetUpMode = false;

                for (int player = 0; player < playerCount; player++)
                {
                    if (playerPoints[player] == 0)
                    {
                        playerPoints[player] = detectPointOnBoard(playBoardImage);

                        for (int p = 0; p < player; p++)
                        {
                            if (playerPoints[player] == playerPoints[p])
                            {
                                playerPoints[player] = 0;
                                break;
                            }
                        }
                        arenaSetUpMode = true;
                        if(player+1 == playerCount && playerPoints[player] != 0)
                        {
                            arenaChooseCirclePanel.Visible = false;
                            arenaSetUpMode = false;
                        }
                        break;
                    }
                }

                updateScoreBoard();
            }
            else
            {
                var throwPoint = detectPointOnBoard(playBoardImage);
                movesHistory.Add(throwPoint);
                if (throwPoint == playerPoints[currentPlayer])
                    arenaBan[currentPlayer]--;
                else
                {
                    if (arenaBan[currentPlayer] < 1)
                        for (int player = 0; player < playerCount; player++)
                        {
                            if (playerPoints[player] == throwPoint)
                                arenaHealth[player]--;
                        }
                }

                do
                {
                    if (currentPlayer + 1 < playerCount)
                        currentPlayer++;
                    else
                    {
                        currentPlayer = 0;
                        updateBoardLook(playBoardImage, playBoardImagePanel);
                    }
                } while (arenaHealth[currentPlayer] < 1);
                

                updateScoreBoard();

                updateArenaHealthPanel(); 
                
                if (alivePlayers < 2)
                {
                    gameOverPanel.Visible = true;
                    arenaWin();
                }
            }
        }

        //Game Arena Mode end

        private void gameChanger(int selectedGame)
        {
            olimpicGame = false;
            survivalMode = false;
            arenaMode = false;
            attackMode = false;
            memoryMode = false;
            sweeperMode = false;
            colorsMode = false;
            switch (selectedGame)
            {
                case 1:
                    startGameOne();
                    break;
                case 2:
                    startGameTTT();
                    break;
                case 3:
                    olimpicGame = true;
                    startGameOne();
                    break;
                case 4:
                    survivalMode = true;
                    startGameSurvival();
                    break;
                case 5:
                    arenaMode = true;
                    startGameOne();
                    break;
                case 6:
                    attackMode = true;
                    startGameAttack();
                    break;
                case 7:
                    memoryMode = true;
                    startGameMemory();
                    break;
                case 8:
                    sweeperMode = true;
                    startGameMinesweeper();
                    break;
                case 9:
                    colorsMode = true;
                    startGameColors();
                    break;

            }
        }

        private void gameOneButton_Click(object sender, EventArgs e)
        {
            gameChanger(1);
        }

        private void gameTTTButton_Click(object sender, EventArgs e)
        {
            gameChanger(2);
        }
        private void gameOlimpicButton_Click(object sender, EventArgs e)
        {
            gameChanger(3);
        }
        private void gameSurvivalButton_Click(object sender, EventArgs e)
        {
            gameChanger(4);
        }

        private void gameArenaButton_Click(object sender, EventArgs e)
        {
            if(playerCount>=2)
                gameChanger(5);
            else if (playerCount < 1)
                tabsControl.SelectedTab = playersTab;
        }
        private void gameAttackButton_Click(object sender, EventArgs e)
        {
            gameChanger(6);
        }
        private void gameMemoryButton_Click(object sender, EventArgs e)
        {
            gameChanger(7);
        }

        private void gameMinesweeperButton_Click(object sender, EventArgs e)
        {
            gameChanger(8);
        }
        private void gameColorsButton_Click(object sender, EventArgs e)
        {
            gameChanger(9);
        }

        private void minimalizeSettingButton_Click(object sender, EventArgs e)
        {
            maximalizeSettingsButton.Visible = true;
            widthBoardTextBox.Visible = false;
            boardSettingsGroupBox.Width = 392;
            boardSettingsGroupBox.Height = 133;
        }

        private void maximalizeSettingsButton_Click(object sender, EventArgs e)
        {
            maximalizeSettingsButton.Visible = false;
            widthBoardTextBox.Visible = true;
            boardSettingsGroupBox.Width = 925;
            boardSettingsGroupBox.Height = 625;
        }

        private void boardCalibrationButton_Click(object sender, EventArgs e)
        {
            updateBoardLook(previewBoardImage);
            tabsControl.SelectedTab = previewBoardTab;
        }

        private void saveBoardSettingsButton_Click(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(widthBoardTextBox))
                boardSize = int.Parse(widthBoardTextBox.Text);
            if (textboxIsPosInteger(PositionXBoardTextBox))
                boardPositionX = int.Parse(PositionXBoardTextBox.Text);
            if (textboxIsPosInteger(PositionYBoardTextBox))
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

            Properties.Settings.Default.boardSize = boardSize;
            Properties.Settings.Default.boardPositionX = boardPositionX;
            Properties.Settings.Default.boardPositionY = boardPositionY;
            Properties.Settings.Default.boardColor = boardColor;
            Properties.Settings.Default.boardType = typeBoard;
            Properties.Settings.Default.Save();
            updateBoardLook(previewBoardImage);
            updateLogos();
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
            if (textboxIsPosInteger(PositionXBoardTextBox))
                PositionXBoardTextBox.Text = (int.Parse(PositionXBoardTextBox.Text) + 5).ToString();
        }

        private void substractPositionXButton_Click(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(PositionXBoardTextBox))
                PositionXBoardTextBox.Text = (int.Parse(PositionXBoardTextBox.Text) - 5).ToString();
        }

        private void addPositionYButton_Click(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(PositionYBoardTextBox))
                PositionYBoardTextBox.Text = (int.Parse(PositionYBoardTextBox.Text) + 5).ToString();
        }

        private void substractPositionYButton_Click(object sender, EventArgs e)
        {
            if (textboxIsPosInteger(PositionYBoardTextBox))
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
            playerCount = 0;
            for (int i = 0; i < playerName.Length; i++)
            {
                string textBoxName = "playerNameTextBox" + (i + 1);
                TextBox control = (TextBox)GetControlByName(this, textBoxName);

                if (control.Text != "")
                {
                    playerName[playerCount] = control.Text;
                    playerCount++;
                }
            }
            for (int i = playerCount; i < 5; i++)
                playerName[i] = null; 
                

                tabsControl.SelectedTab = playTab;
        }

        private void winOutcomes()
        {
            int biggest = 0;
            int biggestCount = 0;
            int biggestIndex = 0;

            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerPoints[i] > biggest)
                    biggest = playerPoints[i];
            }
            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerPoints[i] == biggest)
                {
                    biggestCount++;
                    biggestIndex = i;
                }
            }
            if (biggestCount > 1)
                winnerLabel.Text = drawText;
            else
                winnerLabel.Text = playerName[biggestIndex];

            //dont show "SCORE" at gameover panel
            scoreTextAfterGameLabel.Visible = false;
        }

        private void showScoreBoard()
        {
            string labelName;
            Label label;

            roundsPanel.Visible = false;
            activeGamePanel.Visible = false;
            int roundsCount = rounds;
            if (roundsCount > 10)
                roundsCount = 10;

            for (int player = 0; player < playerCount; player++)
            {
                for (int round = 0; round < roundsCount; round++)
                {
                    labelName = "PlayerPoints" + player + round;
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = true;
                    label.Text = movesHistory[player + (round * playerCount)].ToString();
                    if (label.Text == "6")
                        label.ForeColor = Color.LightGreen;
                    else if (label.Text == "8")
                        label.ForeColor = Color.FromArgb(101, 129, 250);
                    else
                        label.ForeColor = Color.White;
                }
            }
            highlightPlayerWindow(0, true);
        }
        private void hideScoreBoard()
        {
            string labelName;
            Control label;

            roundsPanel.Visible = true;
            activeGamePanel.Visible = true;

            for (int player = 0; player < 5; player++)
            {
                for (int round = 0; round < 10; round++)
                {
                    labelName = "PlayerPoints" + player + round;
                    label = GetControlByName(this, labelName);
                    label.Visible = false;
                }
            }
            highlightPlayerWindow(0, false);
        }

        int currentPlayer = 0; 
        
        int typeBoard = 0;
        private void checkTypeBoard()
        {
            boardType = typeBoard;
            if (typeBoard == -1)
            {
                typeBoard = 4;
            }
            else if (typeBoard == 5)
            {
                typeBoard = 0;
            }
            boardType = typeBoard;
        }

        //when shot in shield is accurate
        private void shieldAttackPB_Click(object sender, EventArgs e)
        {
            if (currentRound <= rounds)
            {
                PictureBox box = (PictureBox)sender;

                var points = 0;

                if (box.Tag.ToString() == "AttackPoint-0")
                    return;

                if (box.Tag.ToString() == "AttackPoint-1")
                {
                    points = 1;
                    box.Tag = "AttackPoint-2";
                    box.Image = Properties.Resources.shield_point2;
                }
                else if (box.Tag.ToString() == "AttackPoint-2")
                {
                    points = 2;
                    box.Tag = "AttackPoint-3";
                    box.Image = Properties.Resources.shield_point3;
                }
                else if (box.Tag.ToString() == "AttackPoint-3")
                {
                    points = 3;
                    box.Tag = "AttackPoint-0";
                    box.Image = null;
                }

                var indexOfBox = int.Parse(box.Name.Substring(14, (box.Name.Length - 14)));

                if (points == 0)
                    indexOfBox = -1;
                
                playerPoints[currentPlayer] += points;

                attackMovesHistory.Add(indexOfBox);
                movesHistory.Add(points);

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
                        showScoreBoard();
                    }
                }

                updateScoreBoard();
            }
        }

        //memory shoting

        private void box_Click(object sender, EventArgs e)
        {
            if (memoryMode)
                memoryBoxClick(sender);
            else if (sweeperMode)
                sweeperBoxClick(sender);
            else if (colorsMode)
                colorsBoxClick(sender);
        }


        //when shot is accurate or missed
        private void playBoardImage_Click(object sender, EventArgs e)
        {
            if (gameOverPanel.Visible)
                return;

            //Miss button functionality
            if (survivalMode)
            {
                shipSurvivalPictureBox_Click(sender, e);
                return;
            }
            else if (arenaMode)
            {
                arenaModePictureBox_Click(sender, e);
                return;
            }
            else if (attackMode)
                attackMovesHistory.Add(-1);
            else if(memoryMode)
            {
                memoryMissButton();
                return;
            }
            else if (sweeperMode)
            {
                sweeperMissButton();
                return;
            }
            else if (colorsMode)
            {
                colorsMissButton();
                return;
            }

            if (currentRound <= rounds)
            {
                var points = detectPointOnBoard(playBoardImage);
                playerPoints[currentPlayer] += points;

                movesHistory.Add(points);
                lastMovePlayer = currentPlayer;

                if (currentPlayer + 1 < playerCount)
                    currentPlayer++;
                else
                {
                    currentPlayer = 0;
                    if (currentRound <= rounds)
                    {
                        currentRound++;

                        if (!olimpicGame) //olimpic vs normal throwing difference
                            typeBoard++;

                        checkTypeBoard();
                        updateBoardLook(playBoardImage, playBoardImagePanel);
                    }
                    if (currentRound > rounds)
                    {
                        gameOverPanel.Visible = true;
                        winOutcomes();
                        showScoreBoard();
                    }
                }

                updateScoreBoard();
            }

        }

        private void gameOverExitButton_Click(object sender, EventArgs e)
        {
            gameOverPanel.Visible = false;
            tabsControl.SelectedTab = playTab;
        }

        private void resetGameButton_Click(object sender, EventArgs e)
        {
            if (survivalMode)
                startGameSurvival();
            else if (attackMode)
                startGameAttack();
            else if (memoryMode)
                startGameMemory();
            else if (sweeperMode)
                startGameMinesweeper();
            else if (colorsMode)
                startGameColors();
            else
                startGameOne();
        }


        //Tic tac toe game below
               
        private void TTTBox_Click(object sender, EventArgs e)
        {
            Control x = (Control)sender;
            int number = int.Parse(x.Name.Substring(x.Name.Length - 1)) - 1;
            changeTTTBox(number);
        }

        private void TTTgameOverExitButton_Click(object sender, EventArgs e)
        {
            TTTGameOverPanel.Visible = false;
            tabsControl.SelectedTab = playTab;
        }

        int TTTCrossPoints = 0;
        int TTTCirclePoints = 0;
        private void TTTNewGameButton_Click(object sender, EventArgs e)
        {
            currentRound = 1;
            TTTCrossPoints = 0;
            TTTCirclePoints = 0;
            resetTTTGame();
        }

        List<int> TTTMovesHistory = new List<int>();
        bool reverting = false;
        private void TTTRevertMoveButton_Click(object sender, EventArgs e)
        {
            if (TTTGameOverPanel.Visible)
                return;

            if (endOfTTTRound)
            {
                endOfTTTRound = false;

                if (TTTplayerTurnCross)
                    TTTCirclePoints -= 1;
                else
                    TTTCrossPoints -= 1;

                currentRound -= 1;
            }

            if (TTTMovesHistory.Count > 0)
            {
                reverting = true;
                changeTTTBox(TTTMovesHistory[TTTMovesHistory.Count - 1]);
                TTTMovesHistory.RemoveAt(TTTMovesHistory.Count - 1);
                reverting = false;
            }
        }

        private void TTTMissButton_Click(object sender, EventArgs e)
        {
            if (TTTGameOverPanel.Visible || endOfTTTRound)
                return;

            changeTTTBoxWhenMiss();
        }


        //TTT - end

        //reverting system updated from one move to whole history
        int lastMovePlayer = 0;

        List<int> movesHistory = new List<int>();
        List<int> attackMovesHistory = new List<int>();

        private void revertMoveButton_Click(object sender, EventArgs e)
        {
            if (gameOverPanel.Visible || animationBanClick)
                return;

            if (movesHistory.Count > 0)
            {
                if (arenaMode)
                {
                    do
                    {
                        if (currentPlayer - 1 >= 0)
                            currentPlayer--;
                        else
                            currentPlayer = playerCount - 1;

                    } while (arenaHealth[currentPlayer] < 1);

                    var throwPoint = movesHistory[movesHistory.Count - 1];
                    if (throwPoint == playerPoints[currentPlayer])
                        arenaBan[currentPlayer]++;
                    else
                    {
                        if (arenaBan[currentPlayer] < 1)
                            for (int player = 0; player < playerCount; player++)
                            {
                                if (playerPoints[player] == throwPoint)
                                    arenaHealth[player]++;
                            }
                    }

                    updateArenaHealthPanel();
                }
                else if (attackMode)
                {
                    if (currentPlayer - 1 >= 0)
                        currentPlayer--;
                    else
                    {
                        if (currentRound > 0)
                        {
                            currentPlayer = playerCount - 1;
                            currentRound -= 1;
                        }
                    }

                    var lastMove = attackMovesHistory[attackMovesHistory.Count - 1];

                    if (lastMove != -1) // -1 means missed shot and no shield was added
                    {
                        playerPoints[currentPlayer] -= movesHistory[movesHistory.Count - 1];
                        PictureBox boxToRevert = (PictureBox)GetControlByName(attackModePanel, "shieldAttackPB" + lastMove);
                        if (movesHistory[movesHistory.Count - 1] == 3)
                            boxToRevert.Image = Properties.Resources.shield_point3;
                        else if (movesHistory[movesHistory.Count - 1] == 2)
                            boxToRevert.Image = Properties.Resources.shield_point2;
                        else if (movesHistory[movesHistory.Count - 1] == 1)
                            boxToRevert.Image = Properties.Resources.shield_point1;

                        boxToRevert.Tag = "AttackPoint-" + movesHistory[movesHistory.Count - 1];
                    }

                    attackMovesHistory.RemoveAt(attackMovesHistory.Count - 1);

                }
                else if (memoryMode)
                    memoryRevertMove();    
                else if (sweeperMode)                   
                    sweeperRevertMove();
                else if (colorsMode)
                    colorsRevertMove();
                else
                {
                    playerPoints[lastMovePlayer] -= movesHistory[movesHistory.Count - 1];

                    //assess if reverting move will change round for current player or not
                    if (currentPlayer - 1 >= 0)
                        currentPlayer--;
                    else
                    {
                        currentPlayer = playerCount - 1;

                        if (currentRound > 0)
                        {
                            currentRound -= 1;
                            typeBoard--;
                            checkTypeBoard();
                            updateBoardLook(playBoardImage, playBoardImagePanel);
                        }

                    }

                    //assess if reverting move will change round for player before current or not
                    if (currentPlayer - 1 >= 0)
                        lastMovePlayer--;
                    else
                        lastMovePlayer = playerCount - 1;
                }

                movesHistory.RemoveAt(movesHistory.Count - 1);

            }

            updateScoreBoard();
        }

        string drawText = "Remis", pageText = "STRONA";
        private void changeLanguageEnglish_Click(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.EnglishName != "English")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                this.Controls.Clear();
                initialize();
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.VikingAxeProjectForm_KeyDown);

                drawText = "Draw";
                pageText = "PAGE";
            }
        }

        private void changeLanguagePolish_Click(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.EnglishName != "Polish")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl");
                this.Controls.Clear();
                initialize();
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.VikingAxeProjectForm_KeyDown);

                drawText = "Remis";
                pageText = "STRONA";
            }
        }

        //make hover animation
        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label.Tag.ToString() == "Gold")
                label.Image = Properties.Resources.Gold_button_hover;
            else if (label.Tag.ToString() == "Grey")
                label.Image = Properties.Resources.grey_small_button_hover;
            else if(label.Tag.ToString() == "GreyLong")
                label.Image = Properties.Resources.grey_wide_button_hover;
            else if(label.Tag.ToString() == "GreySec")
                label.Image = Properties.Resources.secondary_button_hover;
            else if(label.Tag.ToString() == "GreySecLong")
                label.Image = Properties.Resources.secondary_wide_button_hover;
            else if(label.Tag.ToString() == "Red")
                label.Image = Properties.Resources.exitButton_hover;
        }

        private void exitMenuButton_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureb = sender as PictureBox;
            pictureb.Image = Properties.Resources.exitButton_hover;
        }

        private void exitMenuButton_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureb = sender as PictureBox;
            pictureb.Image = Properties.Resources.exitButton_normal;
        }

        //setting red dot after changing mode
        private void setRedDotImage()
        {
            if (typeBoard == 0)
                playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal;
            if (typeBoard == 1)
                playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal2;
            if (typeBoard == 2)
                playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal3;
            if (typeBoard == 3)
                playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal4;
            if (typeBoard == 4)
                playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal5;
            if (olimpicGame)
                playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circle_normal;
        }

        //board animation
        private void playBoardImage_MouseMove(object sender, MouseEventArgs e)
        {
            var point = detectPointOnBoard(playBoardImage);
            if(survivalMode != true && olimpicGame != true)
            {
                if (typeBoard == 0)
                {
                    if (point == 1)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleOne_hover;
                    else if (point == 2)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleTwo_hover;
                    else if (point == 3)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleThree_hover;
                    else if (point == 4)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleFour_hover;
                    else if (point == 6)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleSix_hover;
                    else
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal;
                }
                if (typeBoard == 1)
                {
                    if (point == 1)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleOne_hover2;
                    else if (point == 2)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleTwo_hover2;
                    else if (point == 3)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleThree_hover2;
                    else if (point == 4)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleFour_hover2;
                    else if (point == 6)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleSix_hover2;
                    else
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal2;
                }
                if (typeBoard == 2)
                {

                    if (point == 1)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleOne_hover3;
                    else if (point == 2)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleTwo_hover3;
                    else if (point == 3)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleThree_hover3;
                    else if (point == 4)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleFour_hover3;
                    else if (point == 6)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleSix_hover3;
                    else
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal3;
                }
                if (typeBoard == 3)
                {
                    if (point == 1)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleOne_hover4;
                    else if (point == 2)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleTwo_hover4;
                    else if (point == 3)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleThree_hover4;
                    else if (point == 4)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleFour_hover4;
                    else if (point == 6)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleSix_hover4;
                    else
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal4;
                }
                if (typeBoard == 4)
                {

                    if (point == 1)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleOne_hover5;
                    else if (point == 2)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleTwo_hover5;
                    else if (point == 3)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleThree_hover5;
                    else if (point == 4)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleFour_hover5;
                    else if (point == 6)
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circleSix_hover5;
                    else
                        playBoardImagePanel.BackgroundImage = Properties.Resources.circle_normal5;
                }
            }
            else if(olimpicGame)
            {
                if (point == 1)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleOne_hover;
                else if (point == 2)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleTwo_hover;
                else if (point == 3)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleThree_hover;
                else if (point == 4)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleFour_hover;
                else if (point == 6)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleSix_hover;
                else if (point == 8)
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circleEight_hover;
                else
                    playBoardImagePanel.BackgroundImage = Properties.Resources.olimpic_circle_normal;
            }
        }

        private void playBoardImage_MouseLeave(object sender, EventArgs e)
        {
            setRedDotImage();
        }

        //ttt animation
        private void TTTBox_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox x = (PictureBox)sender;
            if(!endOfTTTRound)
            {
                if (TTTplayerTurnCross)
                    x.Image = Properties.Resources.ttt_cross_hover;
                else
                    x.Image = Properties.Resources.ttt_circle_hover;
            }
            else
                x.Image = null;
        }

        private void TTTBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox x = (PictureBox)sender;
            x.Image = null;
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            if(animationControl != null)
            {
                animationControl.Image = null;
                animationControl.Visible = false;
            }

            animationTimer.Stop();
            animationBanClick = false;
        }



        private void changePageButton_Click(object sender, EventArgs e)
        {
            if (secondPageGamesPanel.Visible)
            {
                secondPageGamesPanel.Visible = false;
                changePage1Button.Text = pageText + "  1 / 2";
            }
            else
            {
                secondPageGamesPanel.Visible = true;
                changePage1Button.Text = pageText + "  2 / 2";

            }
        }

        //highlighting buttons events
        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label.Tag.ToString() == "Gold")
                label.Image = Properties.Resources.Gold_button_normal;
            else if (label.Tag.ToString() == "Grey")
                label.Image = Properties.Resources.grey_small_button_normal;
            else if (label.Tag.ToString() == "GreyLong")
                label.Image = Properties.Resources.grey_wide_button_normal;
            else if (label.Tag.ToString() == "GreySec")
                label.Image = Properties.Resources.secondary_button_normal;
            else if (label.Tag.ToString() == "GreySecLong")
                label.Image = Properties.Resources.secondary_wide_button_normal;
            else if (label.Tag.ToString() == "Red")
                label.Image = Properties.Resources.exitButton_normal;
        }

        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;
            Label label = (Label)GetControlByName(this, control.Name + "Label");
            TextBox textbox = (TextBox)GetControlByName(this, control.Name);
            label.Image = label.Image = Properties.Resources.secondary_wide_button_hover;
            textbox.BackColor = Color.FromArgb(40, 40, 40);
        }
        private void textBox_FocusLeave(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;
            Label label = (Label)GetControlByName(this, control.Name + "Label");
            TextBox textbox = (TextBox)GetControlByName(this, control.Name);
            label.Image = label.Image = Properties.Resources.secondary_wide_button_normal;
            textbox.BackColor = Color.FromArgb(31, 30, 26);
        }
        private void textBox_MouseLeave(object sender, EventArgs e)
        {
            TextBox control = sender as TextBox;
            Label label = (Label)GetControlByName(this, control.Name + "Label");
            TextBox textbox = (TextBox)GetControlByName(this, control.Name);
            if (!control.Focused)
            {
                label.Image = label.Image = Properties.Resources.secondary_wide_button_normal;
                textbox.BackColor = Color.FromArgb(31, 30, 26);
            }
        }
    }
}