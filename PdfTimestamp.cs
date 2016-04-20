using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace PdfTimestamp
{
    public class PdfTimestamp : IPdfTimestamp
    {
        public void AddTimestamp(string inputFilePath, string outputFilePath, string[] contentLines, int positionX, int positionY, string hexColorCode, int fontSize, int lineHeight)
        {
            if (String.IsNullOrEmpty(inputFilePath))
                return;
            if (!File.Exists(inputFilePath))
                return;
            if (String.Compare(Path.GetExtension(inputFilePath), ".pdf", true) != 0)
                return;

            PdfReader reader = new PdfReader(File.ReadAllBytes(inputFilePath));

            using (MemoryStream ms = new MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields form = stamper.AcroFields;

                iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(1);
                PdfContentByte pdfPageContents = stamper.GetOverContent(1);
                pdfPageContents.BeginText();

                foreach (string contentLine in contentLines)
                {
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
                    pdfPageContents.SetFontAndSize(baseFont, fontSize);
                    var color = ColorTranslator.FromHtml(hexColorCode);
                    pdfPageContents.SetRGBColorFill(color.R, color.G, color.B);
                    pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_LEFT, contentLine, positionX, pageSize.Height - positionY, 0);
                    positionY = positionY + lineHeight;
                }

                pdfPageContents.EndText();

                if (form.Fields.Count != 0)
                    stamper.FormFlattening = true;

                stamper.Close();

                if (File.Exists(outputFilePath))
                    File.Delete(outputFilePath);

                File.WriteAllBytes(outputFilePath, ms.ToArray());
                ms.Flush();
            }
        }
    }
}
