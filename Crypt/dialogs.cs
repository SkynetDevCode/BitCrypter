using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypt
{

  
    class dialogs
    {

        [DllImport("comctl32.dll", CharSet = CharSet.Unicode, EntryPoint = "TaskDialog")]
        static extern int TaskDialog(IntPtr hWndParent, IntPtr hInstance, String pszWindowTitle,
            String pszMainInstruction, String pszContent, int dwCommonButtons, IntPtr pszIcon, out int pnButton);
        public void MsgOK(Form winhandle, string title,string txt, string type, string wintitle = "SDVCD Internal Error")
        {
            try
            {
                if (type == "info")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 2), out result);
                }
                else if (type == "warning")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue)), out result);
                }
                else if (type == "error")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 1), out result);
                }
                else if (type == "none")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(0), out result);
                }
                else if (type == "securityerror")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 6), out result);
                }
                else if (type == "securitywarning")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 5), out result);
                }
                else if (type == "securitysuccess")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 7), out result);
                }
                else if (type == "securityblue")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 4), out result);
                }
                else if (type == "security")
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(Convert.ToInt32(UInt16.MaxValue) - 3), out result);
                }
                else
                {
                    int result = 0;
                    TaskDialog(winhandle.Handle, IntPtr.Zero, wintitle, title, txt, 1, new IntPtr(0), out result);
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
