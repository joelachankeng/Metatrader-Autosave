using System;
using System.Runtime.InteropServices;

namespace Metatrader_Autosaver
{
    //get virtual key codes @ https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    class KeyListener
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public static bool CheckKey(int VK_Code)
        {
            short keyState = GetAsyncKeyState(VK_Code);
            return ((keyState >> 0) & 0x0001) == 0x0001;
        }

        public static bool CheckKeys(int VK_Code, int VK_Code_Secondary)
        {
            int areKeysPressed = (GetAsyncKeyState(VK_Code) & GetAsyncKeyState(VK_Code_Secondary) >> 0) & 0x8000;
            return areKeysPressed == 0x8000;
        }
    }
}
