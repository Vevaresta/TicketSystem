using iTextSharp.text.pdf;

namespace Ticketsystem.Utilities
{
    public class PdfUtility
    {
        public static void SaveToPdf()
        {
            using PdfReader reader = new(@"Kundenauftrag.pdf");
            using PdfStamper stamper = new(reader, new FileStream(@"Filled Form.pdf", FileMode.Create));
            AcroFields form = stamper.AcroFields;
            form.SetField("Frau", "Yes");
            form.SetField("Name", "Test");
        }
    }
}
