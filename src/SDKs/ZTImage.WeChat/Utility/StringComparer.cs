using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTImage.WeChat
{
    public class StringComparer: IComparer<string>
    {
        public int Compare(string left, string right)
        {
            int iLeftLength = left.Length;
            int iRightLength = right.Length;
            int index = 0;
            while (index < iLeftLength && index < iRightLength)
            {
                if (left[index] < right[index])
                    return -1;
                else if (left[index] > right[index])
                    return 1;
                else
                    index++;
            }
            return iLeftLength - iRightLength;

        }
    }
}
