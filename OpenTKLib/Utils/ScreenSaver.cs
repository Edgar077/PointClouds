// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//

using System;
using System.Runtime.InteropServices;

namespace OpenTKExtension
{
  public static class ScreenSaver
  {
    private const int SPI_GETSCREENSAVERACTIVE = 16;
    private const int SPI_SETSCREENSAVERACTIVE = 17;
    private const int SPI_GETSCREENSAVERTIMEOUT = 14;
    private const int SPI_SETSCREENSAVERTIMEOUT = 15;
    private const int SPI_GETSCREENSAVERRUNNING = 114;
    private const int SPIF_SENDWININICHANGE = 2;
    private const uint DESKTOP_WRITEOBJECTS = 128U;
    private const uint DESKTOP_READOBJECTS = 1U;
    private const int WM_CLOSE = 16;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, ref int lpvParam, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, ref bool lpvParam, int flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int PostMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr OpenDesktop(string hDesktop, int Flags, bool Inherit, uint DesiredAccess);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseDesktop(IntPtr hDesktop);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool EnumDesktopWindows(IntPtr hDesktop, ScreenSaver.EnumDesktopWindowsProc callback, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();

    public static bool GetScreenSaverActive()
    {
      bool lpvParam = false;
      ScreenSaver.SystemParametersInfo(16, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void SetScreenSaverActive(int Active)
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(17, Active, ref lpvParam, 2);
    }

    public static int GetScreenSaverTimeout()
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(14, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void SetScreenSaverTimeout(int Value)
    {
      int lpvParam = 0;
      ScreenSaver.SystemParametersInfo(15, Value, ref lpvParam, 2);
    }

    public static bool GetScreenSaverRunning()
    {
      bool lpvParam = false;
      ScreenSaver.SystemParametersInfo(114, 0, ref lpvParam, 0);
      return lpvParam;
    }

    public static void KillScreenSaver()
    {
      IntPtr hDesktop = ScreenSaver.OpenDesktop("Screen-saver", 0, false, 129U);
      if (hDesktop != IntPtr.Zero)
      {
        ScreenSaver.EnumDesktopWindows(hDesktop, new ScreenSaver.EnumDesktopWindowsProc(ScreenSaver.KillScreenSaverFunc), IntPtr.Zero);
        ScreenSaver.CloseDesktop(hDesktop);
      }
      else
        ScreenSaver.PostMessage(ScreenSaver.GetForegroundWindow(), 16, 0, 0);
    }

    private static bool KillScreenSaverFunc(IntPtr hWnd, IntPtr lParam)
    {
      if (ScreenSaver.IsWindowVisible(hWnd))
        ScreenSaver.PostMessage(hWnd, 16, 0, 0);
      return true;
    }

    private delegate bool EnumDesktopWindowsProc(IntPtr hDesktop, IntPtr lParam);
  }
}
