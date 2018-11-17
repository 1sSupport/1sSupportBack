using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Tools.Decoder
{
    public class InnLogin
    {
        public string Inn { get; }
        public string Login { get; }

        public InnLogin(string INN, string Login)
        {
            Inn = INN;
            this.Login = Login;
        }
    }
}
