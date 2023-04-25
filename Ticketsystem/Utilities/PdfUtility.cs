using iText.Kernel.Pdf;
using iText.Forms;

namespace Ticketsystem.Utilities
{
    public class PdfUtility
    {
        public byte[] FillPdfNewTicket(byte[] pdfFileAsByteArray)
        {
            using MemoryStream inputPdfStream = new(pdfFileAsByteArray);
            using MemoryStream outputPdfStream = new();
            using PdfDocument pdfDoc = new(new PdfReader(inputPdfStream), new PdfWriter(outputPdfStream));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.GetField("txtClientName").SetValue("value2");

            return outputPdfStream.ToArray();
        }

        public async Task<byte[]> GetPdfNewTicket()
        {
            byte[] pdfFile = null;
            try
            {
                pdfFile = await File.ReadAllBytesAsync("Files/Kundenauftrag.pdf");
            }
            catch
            {

            }

            return pdfFile;
        }

    }
}
