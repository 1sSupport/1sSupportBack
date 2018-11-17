namespace WebApi.Tools.ValidationChecker
{
    using System.Xml.Serialization;

    /// <summary>
    /// The return.
    /// </summary>
    [XmlRoot(ElementName = "return", Namespace = "http://api.repository.onec.ru/v2")]
    public class Return
    {
        [XmlElement(ElementName = "status", Namespace = "http://api.repository.onec.ru/v2")]
        public string Status { get; set; }

        [XmlElement(ElementName = "code", Namespace = "http://api.repository.onec.ru/v2")]
        public string Code { get; set; }

        [XmlElement(ElementName = "description", Namespace = "http://api.repository.onec.ru/v2")]
        public string Description { get; set; }

        [XmlElement(ElementName = "element", Namespace = "http://api.repository.onec.ru/v2")]
        public string Element { get; set; }
    }

    /// <summary>
    /// The check its by login response.
    /// </summary>
    [XmlRoot(ElementName = "checkItsByLoginResponse", Namespace = "http://api.repository.onec.ru/v2")]
    public class CheckItsByLoginResponse
    {
        [XmlElement(ElementName = "return", Namespace = "http://api.repository.onec.ru/v2")]
        public Return Return { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    /// <summary>
    /// The body.
    /// </summary>
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "checkItsByLoginResponse", Namespace = "http://api.repository.onec.ru/v2")]
        public CheckItsByLoginResponse CheckItsByLoginResponse { get; set; }
    }

    /// <summary>
    /// The envelope.
    /// </summary>
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }

        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }
}