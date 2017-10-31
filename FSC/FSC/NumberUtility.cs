using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSPCB.FSC
{
    static class NumberUtility
    {
        public static int FromBcd(byte[] bcds)
        {
            int result = 0;
            for (int i = 0; i < bcds.Length; i++)
            {
                result *= 100;
                result += (10 * (bcds[i] >> 4));
                result += bcds[i] & 0xf;
            }
            return result;
        }
    }
}
