using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace MSC.CustomTTF
{
    public class CustomFont
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr PB_FONT, uint CBFONT, IntPtr PDV, [In] ref uint PCFONTS);

        public int InputFontLength; // 
        public int dataLength;      //
        public IntPtr ptrData;      //
        public uint customFonts;    //
        public FontFamily Standard; //
        public Font InputFont;      //

        public string CF_Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; 

        // Command takes five arguements, a byte (the font file), its length, a font style, a size, and lastly a target control.

        public void UseCustom_TTF(byte[] InputTTF, int InputTTF_Length, FontStyle InputTTF_Style, float InputTTF_EMSize, Control InputControl_Target)
        { 
                // set the array
                byte[] fontArray = InputTTF;
                int dataLength = InputTTF_Length;
                // set the COM Task.
                IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);
                Marshal.Copy(fontArray, 0, ptrData, dataLength);
                //Copy font to memory.
                uint customFonts = 0;
                AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref customFonts);
                // add font to memory...
                PrivateFontCollection PFC = new PrivateFontCollection();
                PFC.AddMemoryFont(ptrData, dataLength);
                // free data.
                Marshal.FreeCoTaskMem(ptrData);
                // increment. and set families.
                Standard = PFC.Families[0];
                // set the font.
                InputFont = new Font(Standard, InputTTF_EMSize, InputTTF_Style);
                // allocate the font to the control.
                AllocateFont(InputFont, InputControl_Target, InputTTF_EMSize);
        }

        internal void AllocateFont(Font F, Control CC, float size)
        {
                FontStyle Normal = FontStyle.Regular;
                CC.Font = new Font(Standard, size, Normal);
        }

    }

}

