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

        Stream fontStream = new MemoryStream(Properties.Resources.ArtifexCF_Book);
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        //Create your private font collection object.
        PrivateFontCollection pfc = new PrivateFontCollection();

        private void InitCustomLabelFont()
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

        }

        private void SetAllControlsFontSize(
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
    }
}