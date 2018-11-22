using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.EF.Models
{
   public partial class SupportAsk
    {
        public SupportAsk(Session session, string title, string text, string contactInfo) : this(text,contactInfo,session)
        {
            AskTitle.Add(new AskTitle(title, this));
        }
    }
}
