namespace WebApi.Tools.Decoder
{
    public class InnLogin
    {
        public InnLogin(string INN, string Login)
        {
            Inn = INN;
            this.Login = Login;
        }

        public string Inn { get; }

        public string Login { get; }
    }
}