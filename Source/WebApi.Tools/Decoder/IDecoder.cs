namespace WebApi.Tools.Decoder
{
    internal interface IDecoder
    {
        InnLogin GetInnLoginFromString(string encodeString);
    }
}