using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PdfTimestamp
{
    [ServiceContract]
    public interface IPdfTimestamp
    {
        [OperationContract]
        void AddTimestamp(string inputFilePath, string outputFilePath, string[] contentLines, int positionX, int positionY, string hexColorCode, int fontSize, int lineHeight);
    }

}
