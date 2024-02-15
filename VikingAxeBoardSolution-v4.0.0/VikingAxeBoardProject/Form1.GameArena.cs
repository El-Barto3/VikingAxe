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
        private bool arenaSetUpMode = false;
        private int alivePlayers = 5;

        private void arenaWin()
        {
            for (int player = 0; player < playerCount; player++)
            {
                if (arenaHealth[player] > 0)
                {
                    winnerLabel.Text = playerName[player];
                    break;
                }
            }
        }

        private void updateArenaHealthPanel()
        {
            alivePlayers = 0;
            if (arenaMode)
            {
                arenaLabelUnderSB.Visible = true;
                for (int player = 0; player < 5; player++)
                {
                    var health1 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaHealth" + (player + 1) + "1");
                    var health2 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaHealth" + (player + 1) + "2");
                    var ban1 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaBan" + (player + 1) + "1");
                    var ban2 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaBan" + (player + 1) + "2");
                    var killerMode = this.GetControlByName(basicGameTabAlignmentPanel, "arenaKillerModeIcon" + (player + 1));

                    if (arenaBan[player] == 2)
                    {
                        ban1.Visible = true;
                        ban2.Visible = true;
                        killerMode.Visible = false;
                    }
                    else if (arenaBan[player] == 1)
                    {
                        ban1.Visible = true;
                        ban2.Visible = false;
                        killerMode.Visible = false;
                    }
                    else
                    {
                        ban1.Visible = false;
                        ban2.Visible = false;
                        killerMode.Visible = true;
                    }

                    if (arenaHealth[player] == 2)
                    {
                        health1.Visible = true;
                        health2.Visible = true;
                        alivePlayers++;
                    }
                    else if (arenaHealth[player] == 1)
                    {
                        health1.Visible = true;
                        health2.Visible = false;
                        alivePlayers++;
                    }
                    else
                    {
                        health1.Visible = false;
                        health2.Visible = false;
                        ban1.Visible = false;
                        ban2.Visible = false;
                        killerMode.Visible = false;
                    }


                }
            }
            else
            {
                arenaLabelUnderSB.Visible = false;
                arenaChooseCirclePanel.Visible = false;

                for (int player = 0; player < 5; player++)
                {
                    var health1 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaHealth" + (player + 1) + "1");
                    var health2 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaHealth" + (player + 1) + "2");
                    var ban1 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaBan" + (player + 1) + "1");
                    var ban2 = this.GetControlByName(basicGameTabAlignmentPanel, "arenaBan" + (player + 1) + "2");
                    var killerMode = this.GetControlByName(basicGameTabAlignmentPanel, "arenaKillerModeIcon" + (player + 1));

                    health1.Visible = false;
                    health2.Visible = false;
                    ban1.Visible = false;
                    ban2.Visible = false;
                    killerMode.Visible = false;

                }
            }
        }

        private void arenaSetUp()
        {
            arenaSetUpMode = true;

            for (int player = 0; player < 5; player++)
            {
                if (player < playerCount)
                {
                    arenaHealth[player] = 2;
                    arenaBan[player] = 2;
                }
                else
                {
                    arenaHealth[player] = 0;
                    arenaBan[player] = 0;
                }
            }
            updateArenaHealthPanel();
            arenaChooseCirclePanel.Width = boardSize + 2;
            arenaChooseCirclePanelLabel.Width = boardSize + 2;
            arenaChooseCirclePanel.Visible = true;
        }

        int[] arenaBan = new int[5];
        int[] arenaHealth = new int[5];
    }
}