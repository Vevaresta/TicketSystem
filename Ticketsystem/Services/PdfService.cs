using IronPdf;

namespace Ticketsystem.Services
{
    public class PdfService
    {

        public void SaveToPdf()
        {



            PdfDocument doc = PdfDocument.FromFile(@"C:\Users\BFW\Desktop\Freitagsprojekt\Kundenauftrag.pdf");

            var form = doc.Form;

            form.Fields[0].Value = "On";
            form.Fields[3].Value = "Nikola";
            form.Fields[4].Value = "5555";
            form.Fields[5].Value = "55555555";
            form.Fields[6].Value = "nikola@gmail.com";
            form.Fields[7].Value = "02.05.2023";
            form.Fields[8].Value = "On";
            form.Fields[9].Value = "03.06.2023";
            form.Fields[10].Value = "PC";


            form.Fields[16].Value = "On";



            form.Fields[19].Value = "Es gibt große Fehler ";

            form.Fields[21].Value = "On";
            form.Fields[22].Value = "On";
            form.Fields[24].Value = "Nikola Krpan";
            form.Fields[25].Value = "07.09.2023";
            form.Fields[26].Value = "Nikola";
            form.Fields[27].Value = "War gebrochen";
            form.Fields[28].Value = "Wir sollten es reparieren";

            doc.SaveAs(@"C:\Users\BFW\Desktop\Freitagsprojekt\Filled Form.pdf");
        }
    }
}
