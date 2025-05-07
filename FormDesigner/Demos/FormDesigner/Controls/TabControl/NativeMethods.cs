#region Copyright (c) 2016-2025 Alternet Software
/*
    AlterNET Form Designer Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/
#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VSControls
{
    internal static class NativeMethods
    {
        #region Constants
        public const int TCM_HITTEST = 0x130d;
        public const int WM_SETFONT = 0x0030;
        public const int WM_THEMECHANGED = 0x031a;
        public const int WM_DESTROY = 0x0002;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_CREATE = 0x0001;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        #endregion

        // Some raster operations
        public const uint SRCCOPY = 0x00CC0020;

        #region GDI functions
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "StretchBlt", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, uint dwRop);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll", EntryPoint = "GetPixel", CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll", EntryPoint = "SetPixel", CallingConvention = CallingConvention.StdCall)]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);
        #endregion

        [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "RealGetWindowClassW", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern uint RealGetWindowClass(IntPtr hWnd, StringBuilder ClassName, uint ClassNameMax);

        #region API Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TCHITTESTINFO
        {
            public POINT pt;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }
        #endregion

    }
}