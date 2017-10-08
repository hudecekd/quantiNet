using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Quanti.Utils.Helpers
{
    public static class XmlHelper
    {
        public static XmlDocument Sign<T>(T data, X509Certificate2 certificate)
            where T : class
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";

            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true; // WARNING: pozor. Pri podpisu  pri overeni musis obsah sedet!

            var str = Serialization.Xml.Serializer.SerializeToString(data);
            xmlDoc.LoadXml(str);

            var signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = certificate.PrivateKey;

            var rsaprovider = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            var rkv = new RSAKeyValue(rsaprovider);

            var keyInfo = new KeyInfo();
            var keyInfoData = new KeyInfoX509Data(certificate);
            keyInfo.AddClause(keyInfoData);
            keyInfo.AddClause(rkv);
            signedXml.KeyInfo = keyInfo;

            // which part of xml to sign
            var reference = new Reference();
            reference.Uri = ""; // "" indicates that we want to sign whole xml (main element)

            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();

            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true)); // add signature to raw form of request

            return xmlDoc;
        }

        public static bool VerifySigning(string data, X509Certificate2 certificate)
        {
            // verify
            var vXmlDoc = new XmlDocument();
            vXmlDoc.PreserveWhitespace = true; // carefully used. It should be set when docuemnt was signed with this flag set.
            vXmlDoc.LoadXml(data);

            var signedXml = new SignedXml(vXmlDoc);

            XmlNodeList nodeList = vXmlDoc.GetElementsByTagName("Signature");

            signedXml.LoadXml((XmlElement)nodeList[0]);

            var verified = signedXml.CheckSignature(certificate.PublicKey.Key);
            return verified;
        }

        /// <summary>
        /// Encodes xml by using specified encoding.
        /// WARNING: If <see cref="Sign"/> method is used and this method is used to send data to another party
        /// same encoding which was used by <see cref="Sign"/> method should be used.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>

        public static byte[] GetBytes(XmlDocument xmlDocument, Encoding encoding)
        {
            using (var ms = new MemoryStream())
            using (TextWriter sw = new StreamWriter(ms, Encoding.UTF8)) //Set encoding
            {
                xmlDocument.Save(sw);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Changes xml represented by string to formatted xml.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string Format(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var sb = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(sb, new XmlWriterSettings() { Indent = true, NewLineHandling = NewLineHandling.Replace}))
            {
                xmlDocument.Save(xmlWriter);
            }
            return sb.ToString();
        }
    }
}
