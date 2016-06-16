using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LDDModder
{
    static class NativeHelper
    {
        const uint SIID_SHIELD = 77;
        const uint SHGSI_ICON = 0x000000100;
        const uint SHGSI_SMALLICON = 0x000000001;

        [DllImport("Shell32.dll", SetLastError = true)]
        private static extern Int32 SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32")]
        public static extern UInt32 SendMessage
            (IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        internal const int BCM_FIRST = 0x1600; //Normal button
        internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public UInt32 cbSize;
            public IntPtr hIcon;
            public Int32 iSysIconIndex;
            public Int32 iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }

        public static Bitmap GetUacShieldIcon()
        {
            SHSTOCKICONINFO sii = new SHSTOCKICONINFO();
            sii.cbSize = (UInt32)Marshal.SizeOf(typeof(SHSTOCKICONINFO));
            SHGetStockIconInfo(SIID_SHIELD, SHGSI_ICON | SHGSI_SMALLICON, ref sii);
            if (sii.hIcon != IntPtr.Zero)
            {
                var icon = Icon.FromHandle(sii.hIcon);
                var bmp = icon.ToBitmap();
                bmp.MakeTransparent();
                DestroyIcon(sii.hIcon);
                return bmp;
            }
            return null;
        }

        public static void AddUacIcon(System.Windows.Forms.Control control)
        {
            SendMessage(control.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
        }
    }
}
