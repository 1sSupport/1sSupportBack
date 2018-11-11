using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tools.Decoder
{
    interface IDecoder
    {
        InnLogin GetInnLoginFromString(string encodeString);
    }
}
