using System.Linq;
using System.Text;

namespace PileRef.Common;

public static class EncodingExtensions
{
    public static EncodingInfo GetInfo(this Encoding encoding)
    {
        return Encoding.GetEncodings().Single(e => e.CodePage == encoding.CodePage);
    }
}