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

namespace VikingAxeBoardProject
{
    public partial class VikingAxeProject : Form
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

        public VikingAxeProject()
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


            //updateBoardLook();
        }
        Stream fontStream = new MemoryStream(Properties.Resources.ArtifexCF_Book);
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        //Create your private font collection object.
        PrivateFontCollection pfc = new PrivateFontCollection();

        void InitCustomLabelFont()
        {    //create an unsafe memory block for the data
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
        public int boardType;

        bool firstLoad = true;

        private void updateBoardLook(PictureBox boardControl)
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
        }

        bool playMode = false;

        private void VikingAxeProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (playMode)
            {
                if (e.KeyCode == Keys.Escape)
                    tabsControl.SelectedTab = playTab;
            }
            else
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (tabsControl.SelectedTab == mainTab)
                        this.Close();
                    else
                        tabsControl.SelectedTab = mainTab;
                }
            }

            if (e.Control && e.Alt && e.Shift && e.KeyCode == Keys.P) // press ctrl + alt + shift + P to exit play mode
                playMode = false;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            tabsControl.SelectedTab = playTab;
            playMode = true;
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

        private int detectPointOnBoard(Control BoardControl, int x)
        {
            Color colorOfClicked = GetPixelColor(MousePosition.X * 3 / 2, MousePosition.Y * 3 / 2);

            int centerX = BoardControl.Width / 2 + BoardControl.Location.X-4,
                centerY = BoardControl.Width / 2 + BoardControl.Location.Y-6;

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
        private double calculateDistance(double boardWidthX, double boardLocationX, double radius, double boardLocationY, double boardHeightY)
        {
            double centerX = boardWidthX / 2 + boardLocationX;
            double centerY = boardHeightY / 2 + boardLocationY -3; //this -3 idk why is neccesary to be more accurate?
            if (typeBoard == 1)
                centerY = radius + boardLocationY;
            if (typeBoard == 2)
                centerX = boardWidthX - radius + boardLocationX;
            if (typeBoard == 3)
                centerY = boardHeightY - radius + boardLocationY;
            if (typeBoard == 4)
                centerX = radius + boardLocationX;

            double vectorX = centerX - this.PointToClient(Cursor.Position).X;
            double vectorY = centerY - this.PointToClient(Cursor.Position).Y;
            //MessageBox.Show(vectorX.ToString(), vectorY.ToString());

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

        private int detectPointOnBoardg(Control BoardControl)
        {

            double sixPointRadius = (double)(BoardControl.Width) * 50 / 700;

            double centerX = BoardControl.Width / 2 + BoardControl.Location.X;
            double centerY = BoardControl.Width / 14 + BoardControl.Location.Y;

            double vectorX = centerX - this.PointToClient(Cursor.Position).X;
            double vectorY = centerY - this.PointToClient(Cursor.Position).Y;
           // MessageBox.Show(vectorX.ToString(), vectorY.ToString());

            double lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            if (lengthFromCenter <= sixPointRadius)
               return 6;


             sixPointRadius = (double)(BoardControl.Width) * 125 / 700;

             centerX = BoardControl.Width / 2 + BoardControl.Location.X;
             centerY = sixPointRadius + BoardControl.Location.Y;

             vectorX = centerX - this.PointToClient(Cursor.Position).X;
             vectorY = centerY - this.PointToClient(Cursor.Position).Y;

             lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            if (lengthFromCenter <= sixPointRadius)
                return 4;

            sixPointRadius = (double)(BoardControl.Width) * 200 / 700;

            centerX = BoardControl.Width / 2 + BoardControl.Location.X;
            centerY = sixPointRadius + BoardControl.Location.Y;

            vectorX = centerX - this.PointToClient(Cursor.Position).X;
            vectorY = centerY - this.PointToClient(Cursor.Position).Y;

            lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            if (lengthFromCenter <= sixPointRadius)
                return 3;

            sixPointRadius = (double)(BoardControl.Width) * 275 / 700;

            centerX = BoardControl.Width / 2 + BoardControl.Location.X;
            centerY = sixPointRadius + BoardControl.Location.Y;

            vectorX = centerX - this.PointToClient(Cursor.Position).X;
            vectorY = centerY - this.PointToClient(Cursor.Position).Y;

            lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            if (lengthFromCenter <= sixPointRadius)
                return 2;

            sixPointRadius = (double)(BoardControl.Width) * 350 / 700;

            centerX = BoardControl.Width / 2 + BoardControl.Location.X;
            centerY = sixPointRadius + BoardControl.Location.Y;

            vectorX = centerX - this.PointToClient(Cursor.Position).X;
            vectorY = centerY - this.PointToClient(Cursor.Position).Y;

            lengthFromCenter = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);

            if (lengthFromCenter <= sixPointRadius)
                return 1;


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
        private int[] playerPoints = new int[5];
        private string[] playerName = new string[5];


        private void highlightPlayerWindow(int playerIndex)
        {
            for (int i = 0; i < 5; i++)
            {
                string labelName = "playerNameLabel" + (i + 1);
                Label label = (Label)GetControlByName(this, labelName);
                if (i != playerIndex)
                    label.BackColor = Color.Transparent;
                else
                    label.BackColor = Color.FromArgb(41, 40, 36);


                labelName = "playerPointsLabel" + (i + 1);
                label = (Label)GetControlByName(this, labelName);
                label.BackColor = Color.FromArgb(41, 40, 36);
                if (i != playerIndex)
                    label.BackColor = Color.FromArgb(41, 40, 36);
                else
                    label.BackColor = Color.FromArgb(94, 92, 82);

            }
        }

        private void updateScoreBoard()
        {
            playerNameLabel1.Text = playerName[0];
            playerPointsLabel1.Text = playerPoints[0].ToString();

            for (int i = 0;i< playerName.Length; i++)
            {
                string labelName = "playerNameLabel" + (i + 1);
                Label label = (Label)GetControlByName(this, labelName);
                label.Visible = true;

                string labelName2 = "playerPointsLabel" + (i + 1);
                Label label2 = (Label)GetControlByName(this, labelName2);
                label.Visible = true;

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
                if(i > playerCount)
                {
                    labelName = "playerPointsLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;

                    labelName = "playerNameLabel" + (i + 1);
                    label = (Label)GetControlByName(this, labelName);
                    label.Visible = false;
                }

                roundsPanel.Location = new Point(607, 367 - 71 * (playerName.Length - playerCount));
            }

            if (currentRound <= rounds)
                currentRoundLabel.Text = currentRound.ToString();

            highlightPlayerWindow(currentPlayer);

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
            updateScoreBoard();
        }

        private void startGameOne()
        {
            if (playerCount >= 1)
            {
                tabsControl.SelectedTab = playOneTab;
                updateBoardLook(playBoardImage);
                updateScoreBoard();
                resetGame();
            }
            else
                tabsControl.SelectedTab = playersTab;

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


            if (textboxIsPosInteger(maxRoundsTextBox))
                rounds = int.Parse(maxRoundsTextBox.Text);

            TTTAllRoundsLabel.Text = rounds.ToString();

            if (currentRound <= rounds)
                currentRoundLabel.Text = currentRound.ToString();

            Point location = new Point(boardPositionX, boardPositionY);
            //panel.Location = location;

            panel.BackColor = boardColor;
        }

        private void uploadTTTPlayer()
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
            uploadTTTPlayer();
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
            tabsControl.SelectedTab = playTwoTab;

            resetTTTGame();
            currentRound = 1;
            TTTCrossPoints = 0;
            TTTCirclePoints = 0;

            uploadTTTBoard();
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
            widthBoardTextBox.Visible = false;
            boardSettingsGroupBox.Width = 373;
            boardSettingsGroupBox.Height = 131;
        }

        private void maximalizeSettingsButton_Click(object sender, EventArgs e)
        {
            maximalizeSettingsButton.Visible = false;
            widthBoardTextBox.Visible = true;
            boardSettingsGroupBox.Width = 925;
            boardSettingsGroupBox.Height = 540;
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
           // MessageBox.Show(Properties.Settings.Default.boardType.ToString());
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

            playMode = true;
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
                //biggest = true;
                //for (int j = 0; j < playerPoints.Length; j++)
                //{
                //    if (playerPoints[i] < playerPoints[j])
                //        biggest = false;

                //}
                //if (biggest)
                //    winnerLabel.Text = playerName[i];
            }
            for (int i = 0; i < playerPoints.Length; i++)
            {
                if (playerPoints[i] == biggest)
                {
                    biggestCount++;
                    biggestIndex = i;
                }
                //biggest = true;
                //for (int j = 0; j < playerPoints.Length; j++)
                //{
                //    if (playerPoints[i] < playerPoints[j])
                //        biggest = false;

                //}
                //if (biggest)
                //    winnerLabel.Text = playerName[i];
            }
            if(biggestCount > 1)
                winnerLabel.Text = "Remis";
            else
                winnerLabel.Text = playerName[biggestIndex];



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
            gameOverPanel.Visible = false;
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
            if (TTTCrossPoints > rounds / 2 || TTTCirclePoints > rounds / 2 || currentRound == rounds)
            {
                win = true;
                if (TTTCrossPoints > TTTCirclePoints)
                    TTTWinnerLabel.Text = "Krzyżyk";
                else if (TTTCrossPoints < TTTCirclePoints)
                    TTTWinnerLabel.Text = "Kółko";
                else
                    TTTWinnerLabel.Text = "Remis";

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
                uploadTTTPlayer();
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
                    }

                    if (currentRound + 1 <= rounds)
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
                        currentRound -= 1;
                    
                }

                //assess if reverting move will change round for player before current or not
                if (currentPlayer - 1 >= 0)
                    lastMovePlayer--;
                else
                    lastMovePlayer = playerCount - 1;

            }
            

            updateScoreBoard();
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
            label.Image = label.Image = Properties.Resources.secondary_wide_button_hover;
        }
        private void textBox_MouseLeave(object sender, EventArgs e) 
        {
            TextBox control = sender as TextBox;
            Label label = (Label)GetControlByName(this, control.Name + "Label");
            if(!control.Focused)
                label.Image = label.Image = Properties.Resources.secondary_wide_button_normal;
        }
    }
}