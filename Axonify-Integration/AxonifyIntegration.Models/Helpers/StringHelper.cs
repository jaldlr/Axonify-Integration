using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AxonifyIntegration.Models.Helpers
{
    public class StringHelper
    {
        public static string ObjectToXML(Object obj)
        {
            if (obj != null)
            {
                string result = null;
                using (System.IO.StringWriter stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(stringwriter, obj);
                    result = stringwriter.ToString();
                }

                return result;//.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");
            }
            else
            {
                return null;
            }
        }
    }
}