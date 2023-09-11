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
        public static class SpecialMethods
        {
            public static IEnumerable<Control> GetAllControls(Control aControl)
            {
                Stack<Control> stack = new Stack<Control>();

                stack.Push(aControl);

                while (stack.Any())
                {
                    var nextControl = stack.Pop();

                    foreach (Control childControl in nextControl.Controls)
                    {
                        stack.Push(childControl);
                    }

                    yield return nextControl;
                }
            }
        }
        private void updateLogos()
        {
            updateControlLook(logoPictureBox1);
            updateControlLook(logoPictureBox2);
            updateControlLook(logoPictureBox3);
            updateControlLook(logoPictureBox4);
            updateControlLook(gameOverPanel);
            updateControlLook(TTTGameOverPanel);
        }
        private void initialize()
        {
            InitializeComponent();
            WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //Bounds = Screen.PrimaryScreen.Bounds; //making problems on second monitor?
            // Properties.Settings.Default["widthBoardInit"] = 'd';
            boardSize = Properties.Settings.Default.boardSize;
            boardPositionX = Properties.Settings.Default.boardPositionX;
            boardPositionY = Properties.Settings.Default.boardPositionY;
            boardColor = Properties.Settings.Default.boardColor;
            boardType = Properties.Settings.Default.boardType;
            updateSavedProperties();
            InitCustomLabelFont();
            updateLogos();
        }
        public VikingAxeProjectForm()
        {
            initialize();

            //updateBoardLook();
        }
        Stream fontStream = new MemoryStream(Properties.Resources.ArtifexCF_Book);
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        //Create your private font collection object.
        PrivateFontCollection pfc = new PrivateFontCollection();

        void InitCustomLabelFont()
        {    //create an unsafe memory block for the data
            fontStream = new MemoryStream(Properties.Resources.ArtifexCF_Book);

            System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
            //create a buffer to read in to
            Byte[] fontData = new Byte[fontStream.Length];
            //fetch the font program from the resource
            fontStream.Read(fontData, 0, (int)fontStream.Length);
            //copy the bytes to the unsafe memory block
            Marshal.Copy(fontData, 0, data, (int)fontStream.Length);

            // We HAVE to do this to register the font to the system (Weird .NET bug !)
            uint cFonts = 0;
            AddFontMemResourceEx(data, (uint)fontData.Length, IntPtr.Zero, ref cFonts);

            //pass the font to the font collection
            pfc.AddMemoryFont(data, (int)fontStream.Length);
            //close the resource stream
            fontStream.Close();
            //free the unsafe memory
            Marshal.FreeCoTaskMem(data);


            foreach (Control theControl in (SpecialMethods.GetAllControls(this)))
            {
                theControl.Font = new Font(pfc.Families[0], theControl.Font.Size);
            }
            ////

            ////Select your font from the resources.
            ////My font here is "Digireu.ttf"
            //int fontLength = Properties.Resources.ArtifexCF_BoldItalic.Length;

            //// create a buffer to read in to
            //byte[] fontdata = Properties.Resources.ArtifexCF_BoldItalic;

            //// create an unsafe memory block for the font data
            //System.IntPtr data1 = Marshal.AllocCoTaskMem(fontLength);

            //// copy the bytes to the unsafe memory block
            //Marshal.Copy(fontdata, 0, data, fontLength);

            //// pass the font to the font collection
            //pfc.AddMemoryFont(data, fontLength);
            ////SetAllControlsFontSize(this.Controls,100);
        }

        public void SetAllControlsFontSize(
                   System.Windows.Forms.Control.ControlCollection ctrls,
                   int amount = 0, bool amountInPercent = true)
        {
            if (amount == 0) return;
            foreach (Control ctrl in ctrls)
            {
                // recursive
                if (ctrl.Controls != null) SetAllControlsFontSize(ctrl.Controls,
                                                                  amount, amountInPercent);
                if (ctrl != null)
                {
                    var oldSize = ctrl.Font.Size;
                    float newSize =
                       (amountInPercent) ? oldSize + oldSize * (amount / 100) : oldSize + amount;
                    if (newSize < 4) newSize = 4; // don't allow less than 4
                    var fontFamilyName = ctrl.Font.FontFamily.Name;
                    
                    ctrl.Font = new Font(pfc.Families[0], newSize);
                };
            };
        }

        private int boardSize;
        private int boardPositionX;
        private int boardPositionY;
        Color boardColor = Color.Black;
        public int boardType;

        bool firstLoad = true;
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

        private void updateBoardLook(PictureBox boardControl)
        {
            if (firstLoad)
            {
                updateSavedProperties();
                firstLoad = false;
            }
            if(textboxIsPosInteger(widthBoardTextBox))
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

            boardControl.Width = boardSize;
            boardControl.Height = boardSize;

            Point location = new Point(boardPositionX, boardPositionY);
            boardControl.Location = location;

            boardControl.BackColor = boardColor;


            if(boardType == 0)
                boardControl.Image = Properties.Resources.board;
            if (boardType == 1)
                boardControl.Image = Properties.Resources.board2;
            if (boardType == 2)
                boardControl.Image = Properties.Resources.board3;
            if (boardType == 3)
                boardControl.Image = Properties.Resources.board4;
            if (boardType == 4)
                boardControl.Image = Properties.Resources.board5;
            if (boardType == 0)
                boardControl.Image = Properties.Resources.board;
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
            for(int i=0;i<playerName.Length;i++)
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

        private double calculateDistance(double boardWidthX, double boardLocationX, double radius, double boardLocationY, double boardHeightY)
        {
            double centerX = boardWidthX / 2 + boardLocationX;
            double centerY = boardHeightY / 2 + boardLocationY -3; //this -3 idk why is neccesary to be more accurate?
            if (typeBoard == 1)
            {
                centerX = boardWidthX / 2 + boardWidthX / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 - (boardHeightY / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationY;
            }
            if (typeBoard == 2)
            {
                centerX = boardWidthX / 2 + boardWidthX / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 + boardHeightY / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationY;
            }
            if (typeBoard == 3)
            {
                centerX = boardWidthX / 2 - (boardWidthX / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 + boardHeightY / 2 / Math.Sqrt(2) - radius / Math.Sqrt(2) + boardLocationY;
            }
            if (typeBoard == 4)
            {
                centerX = boardWidthX / 2  - (boardWidthX / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationX;
                centerY = boardHeightY / 2 - (boardHeightY / 2 / Math.Sqrt(2)) + radius / Math.Sqrt(2) + boardLocationY;
            }

            double vectorX = centerX - this.PointToClient(Cursor.Position).X;
            double vectorY = centerY - this.PointToClient(Cursor.Position).Y;

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
            for (int i = 0;i< playerName.Length; i++)
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
                if(i >= playerCount)
                {
                    labelName = "playerPointsLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;

                    labelName = "playerNameLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;
                }

                roundsPanel.Location = new Point(503, 417 - 71 * (playerName.Length - playerCount));
            }

            if (currentRound <= rounds)
                currentRoundLabel.Text = currentRound.ToString();

            if(!gameOverPanel.Visible)
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
            gameOverPanel.Visible = false;
            if (playerCount >= 1)
            {
                resetGame();
                updateBoardLook(playBoardImage);
                updateScoreBoard();
                hideScoreBoard();
                tabsControl.SelectedTab = playOneTab;
            }
            else
                tabsControl.SelectedTab = playersTab;

        }

        // Game 2 section

        private int[] boxesValue = new int[9];

        private void scaleTTTBoard(int newSize, int originalSize)
        {
            double scale = newSize * 1.0 / originalSize * 1.0;
            Point location;
            for (int i = 1;i<=9;i++)
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
            if(off)
            {
                TTTScoreCircleLabel.BackColor = Color.Transparent;
                TTTCircleTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
                TTTScoreCrossLabel.BackColor = Color.Transparent;
                TTTCrossTurnLabel.BackColor = Color.FromArgb(41, 40, 36);
            }
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
            TTTScoreCircleLabel.Text = TTTCirclePoints.ToString();
            TTTScoreCrossLabel.Text = TTTCrossPoints.ToString();
            TTTCurrentRoundLabel.Text = currentRound.ToString();

            for (int i = 0; i < boxesValue.Length; i++)
            {
                string name = "TTTBox" + (i + 1);
                PictureBox box = (PictureBox)GetControlByName(this, name);

                if (boxesValue[i] == 0)
                    box.Image = Properties.Resources.empty;
                else if (boxesValue[i] == 1)
                    box.Image = Properties.Resources.ttt_cross;
                else if (boxesValue[i] == -1)
                    box.Image = Properties.Resources.ttt_circle;
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
        private void startGameTwo()
        {
            resetTTTGame();
            currentRound = 1;
            TTTCrossPoints = 0;
            TTTCirclePoints = 0;

            uploadTTTBoard();
            updateTTTBoardLook(TTTBoardPanel);

            tabsControl.SelectedTab = playTwoTab;
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
            if(textboxIsPosInteger(widthBoardTextBox))
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
            if(textboxIsPosInteger(PositionXBoardTextBox))
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
            for(int i = 0;i<playerName.Length;i++)
            {
                string textBoxName = "playerNameTextBox" + (i + 1);
                TextBox control = (TextBox)GetControlByName(this, textBoxName);

                if (control.Text != "")
                {
                    playerName[playerCount] = control.Text;
                    playerCount++;
                }

            }

            tabsControl.SelectedTab = playTab;
        }

        string drawText = "Remis";
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
            if(biggestCount > 1)
                winnerLabel.Text = drawText;
            else
                winnerLabel.Text = playerName[biggestIndex];



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

            for (int player=0;player<playerCount;player++)
            {
                for(int round=0;round<roundsCount;round++)
                {
                    labelName = "PlayerPoints" + player + round;
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = true;
                    label.Text = movesHistory[player + (round * playerCount)].ToString();
                    if (label.Text == "6")
                        label.ForeColor = Color.Green;
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

        //when shot is accurate
        private void playBoardImage_Click(object sender, EventArgs e)
        {
            if (gameOverPanel.Visible)
                return;

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
                        typeBoard++;
                        checkTypeBoard();
                        updateBoardLook(playBoardImage);
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

        //when shot is missed 
        private void missButton_Click(object sender, EventArgs e)
        {
            if (gameOverPanel.Visible)
                return;

            if (currentRound <= rounds)
            {
                //miss
                var points = 0;
                playerPoints[currentPlayer] += points;

                movesHistory.Add(points);
                lastMovePlayer = currentPlayer;

                if (currentPlayer + 1 < playerCount)
                    currentPlayer++;
                else
                {
                    currentPlayer = 0;
                    if (currentRound <= rounds)
                        currentRound++;
                    if (currentRound > rounds)
                    {
                        gameOverPanel.Visible = true;
                        winOutcomes();
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
            startGameOne();
        }

        int typeBoard = 0;
        private void checkTypeBoard()
        {
            boardType = typeBoard;
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
            boardType = typeBoard;
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
        
        private void makeWinLineTTT(int index, int crossWon)
        {
            string name = "TTTBox" + index;
            PictureBox box = (PictureBox)GetControlByName(this, name);
            if (crossWon == 1)
                box.Image = Properties.Resources.ttt_cross_win;
            else
                box.Image = Properties.Resources.ttt_circle_win;
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
                if(!reverting)
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

        private void TTTBox1_Click(object sender, EventArgs e)
        {
            changeTTTBox(0);
        }

        private void TTTBox2_Click(object sender, EventArgs e)
        {
            changeTTTBox(1);
        }

        private void TTTBox3_Click(object sender, EventArgs e)
        {
            changeTTTBox(2);
        }

        private void TTTBox4_Click(object sender, EventArgs e)
        {
            changeTTTBox(3);
        }

        private void TTTBox5_Click(object sender, EventArgs e)
        {
            changeTTTBox(4);
        }

        private void TTTBox6_Click(object sender, EventArgs e)
        {
            changeTTTBox(5);
        }

        private void TTTBox7_Click(object sender, EventArgs e)
        {
            changeTTTBox(6);
        }

        private void TTTBox8_Click(object sender, EventArgs e)
        {
            changeTTTBox(7);
        }

        private void TTTBox9_Click(object sender, EventArgs e)
        {
            changeTTTBox(8);
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
        private void TTTNextRoundButton_Click(object sender, EventArgs e)
        {
            if (TTTGameOverPanel.Visible)
                return;

            if(endOfTTTRound)
            {
                endOfTTTRound = false;

                resetTTTGame();
                uploadTTTBoard();
                updateTTTBoardLook(TTTBoardPanel);
            }
            else if (!TTTCheckIfWinGame())
            {
                currentRound += 1;
                resetTTTGame();
            }
        }


        //system cofania w TTT ewentualnie do poprawy

        List<int> TTTMovesHistory = new List<int>();
        bool reverting = false;
        private void TTTRevertMoveButton_Click(object sender, EventArgs e)
        {
            if (TTTGameOverPanel.Visible)
                return;

            if(endOfTTTRound)
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

        private void revertMoveButton_Click(object sender, EventArgs e)
        {
            if (gameOverPanel.Visible)
                return;

            if (movesHistory.Count>0)
            {
                playerPoints[lastMovePlayer] -= movesHistory[movesHistory.Count - 1];
                movesHistory.RemoveAt(movesHistory.Count - 1);

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
                        updateBoardLook(playBoardImage);
                    }
                    
                }

                //assess if reverting move will change round for player before current or not
                if (currentPlayer - 1 >= 0)
                    lastMovePlayer--;
                else
                    lastMovePlayer = playerCount - 1;

            }

            updateScoreBoard();
        }

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
            }
        }

        private void changeLanguagePolish_Click(object sender, EventArgs e)
        {
            if(CultureInfo.CurrentCulture.EnglishName != "Polish")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl");
                this.Controls.Clear();
                initialize();
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.VikingAxeProjectForm_KeyDown);

                drawText = "Remis";
            }
        }

        //make hover animation
        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if(label.Tag.ToString() == "Gold")
                label.Image = Properties.Resources.Gold_button_hover;
            if (label.Tag.ToString() == "Grey")
                label.Image = Properties.Resources.grey_small_button_hover;
            if (label.Tag.ToString() == "GreyLong")
                label.Image = Properties.Resources.grey_wide_button_hover;
            if (label.Tag.ToString() == "GreySec")
                label.Image = Properties.Resources.secondary_button_hover;
            if (label.Tag.ToString() == "GreySecLong")
                label.Image = Properties.Resources.secondary_wide_button_hover;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label.Tag.ToString() == "Gold")
                label.Image = Properties.Resources.Gold_button_normal;
            if (label.Tag.ToString() == "Grey")
                label.Image = Properties.Resources.grey_small_button_normal;
            if (label.Tag.ToString() == "GreyLong")
                label.Image = Properties.Resources.grey_wide_button_normal;
            if (label.Tag.ToString() == "GreySec")
                label.Image = Properties.Resources.secondary_button_normal;
            if (label.Tag.ToString() == "GreySecLong")
                label.Image = Properties.Resources.secondary_wide_button_normal;
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