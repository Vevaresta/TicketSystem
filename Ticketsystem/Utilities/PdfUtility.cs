using iText.Kernel.Pdf;
using iText.Forms;
using Ticketsystem.Models.Data;
using Ticketsystem.Enums;

namespace Ticketsystem.Utilities
{
    public class PdfUtility
    {
        public async Task<byte[]> FillPdfNewTicket(PdfFormData formData)
        {
            var pdfFileAsByteArray = await GetPdfNewTicket();

            using MemoryStream inputPdfStream = new(pdfFileAsByteArray);
            using MemoryStream outputPdfStream = new();
            using (PdfDocument pdfDoc = new(new PdfReader(inputPdfStream), new PdfWriter(outputPdfStream)))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                form.GetField("txtTicketNumber").SetValue(formData.TicketId ?? "");
                switch (formData.TicketType)
                {
                    case "Repair":
                        form.GetField("chbTicketTypeRepair").SetValue("Yes");
                        break;
                    case "DataRecovery":
                        form.GetField("chbTicketTypeRecovery").SetValue("Yes");
                        break;
                    case "Consultation":
                        form.GetField("chbTicketTypeConsultation").SetValue("Yes");
                        break;
                    case "Special":
                        form.GetField("chbTicketTypeSpecial").SetValue("Yes");
                        break;
                }
                form.GetField("txtOrderDescription").SetValue(formData.WorkOrder ?? "");
                form.GetField("txtClientName").SetValue(formData.ClientName ?? "");
                form.GetField("txtClientEmail").SetValue(formData.ClientEmail ?? "");
                form.GetField("txtClientPhone").SetValue(formData.ClientPhone ?? "");
                form.GetField("txtDeviceType").SetValue(formData.DeviceType ?? "");
                form.GetField("txtSerialNumber").SetValue(formData.DeviceSerialNumber ?? "");
                form.GetField("txtAccessories").SetValue(formData.DeviceAccessories ?? "");
                
                if (formData.BackupByClient)
                {
                    form.GetField("chbBackupByClient").SetValue("Yes");
                }
                if (formData.BackupByStaff)
                {
                    form.GetField("chbBackupByStaff").SetValue("Yes");
                }
            }

            return outputPdfStream.ToArray();
        }

        private async Task<byte[]> GetPdfNewTicket()
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
