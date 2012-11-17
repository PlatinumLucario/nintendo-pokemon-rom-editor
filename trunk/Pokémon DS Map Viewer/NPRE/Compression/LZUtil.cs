using System;
using System.Runtime.InteropServices;

internal static class LZUtil
{
    internal static unsafe int GetOccurrenceLength(byte* newPtr, int newLength, byte* oldPtr, int oldLength, out int disp)
    {
        disp = 0;
        if (newLength == 0)
        {
            return 0;
        }
        int num = 0;
        for (int i = 0; i < (oldLength - 1); i++)
        {
            byte* numPtr = oldPtr + i;
            int num3 = 0;
            for (int j = 0; j < newLength; j++)
            {
                if (numPtr[j] != newPtr[j])
                {
                    break;
                }
                num3++;
            }
            if (num3 > num)
            {
                num = num3;
                disp = oldLength - i;
            }
        }
        return num;
    }
}

