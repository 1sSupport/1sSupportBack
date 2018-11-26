// --------------------------------------------------------------------------------------------------------------------
// <copyright file="1SServerAnswer.cs" company="">
//   
// </copyright>
// <summary>
//   The return.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Tools.ValidationChecker
{
    using System.Xml.Serialization;

    /// <summary>
    ///     The return.
    /// </summary>
    [XmlRoot(ElementName = "return", Namespace = "http://api.repository.onec.ru/v2")]
    public class Return
    {
        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        [XmlElement(ElementName = "status", Namespace = "http://api.repository.onec.ru/v2")]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        [XmlElement(ElementName = "code", Namespace = "http://api.repository.onec.ru/v2")]
        public string Code { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [XmlElement(ElementName = "description", Namespace = "http://api.repository.onec.ru/v2")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the element.
        /// </summary>
        [XmlElement(ElementName = "element", Namespace = "http://api.repository.onec.ru/v2")]
        public string Element { get; set; }
    }

    /// <summary>
    ///     The check its by login response.
    /// </summary>
    [XmlRoot(ElementName = "checkItsByLoginResponse", Namespace = "http://api.repository.onec.ru/v2")]
    public class CheckItsByLoginResponse
    {
        /// <summary>
        ///     Gets or sets the return.
        /// </summary>
        [XmlElement(ElementName = "return", Namespace = "http://api.repository.onec.ru/v2")]
        public Return Return { get; set; }

        /// <summary>
        ///     Gets or sets the xmlns.
        /// </summary>
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    /// <summary>
    ///     The body.
    /// </summary>
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        /// <summary>
        ///     Gets or sets the check its by login response.
        /// </summary>
        [XmlElement(ElementName = "checkItsByLoginResponse", Namespace = "http://api.repository.onec.ru/v2")]
        public CheckItsByLoginResponse CheckItsByLoginResponse { get; set; }
    }

    /// <summary>
    ///     The envelope.
    /// </summary>
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        /// <summary>
        ///     Gets or sets the body.
        /// </summary>
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }

        /// <summary>
        ///     Gets or sets the soap.
        /// </summary>
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }
}