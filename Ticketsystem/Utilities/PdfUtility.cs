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

                // Client Info

                form.GetField("txtTicketNumber").SetValue(formData.TicketId ?? "");
                form.GetField("txtClientFirstName").SetValue(formData.ClientFirstName ?? "");
                form.GetField("txtClientLastName").SetValue(formData.ClientLastName ?? "");
                form.GetField("txtClientEmail").SetValue(formData.ClientEmail ?? "");
                form.GetField("txtClientPhone").SetValue(formData.ClientPhone ?? "");
                form.GetField("txtParticipantNumber").SetValue(formData.ParticipantNumber ?? "");
                form.GetField("txtCourse").SetValue(formData.Course ?? "");
                form.GetField("txtStreetAndHouseNumber").SetValue(formData.StreetAndHouseNumber ?? "");
                form.GetField("txtPostalCode").SetValue(formData.PostalCode ?? "");
                form.GetField("txtCity").SetValue(formData.City ?? "");
                
                // Device Info

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

                if(formData.VirusQuarantine == true)
                {
                    form.GetField("chbVirusQuarantine").SetValue("Yes");
                }
                form.GetField("txtDeviceType").SetValue(formData.DeviceType ?? "");
                form.GetField("txtSerialNumber").SetValue(formData.SerialNumber ?? "");
                form.GetField("txtAccessories").SetValue(formData.Accessories ?? "");
                form.GetField("txtDeviceProducer").SetValue(formData.DeviceProducer ?? "");
                form.GetField("txtDeviceDescription").SetValue(formData.DeviceDescription ?? "");

                // Tasks

                form.GetField("txtTicketName").SetValue(formData.TicketName ?? "");
                form.GetField("txtWorkDescription").SetValue(formData.WorkDescription ?? "");
                
                if (formData.BackupByClient)
                {
                    form.GetField("chbBackupByClient").SetValue("Yes");
                }
                if (formData.BackupByStaff)
                {
                    form.GetField("chbBackupByStaff").SetValue("Yes");
                }
                form.GetField("txtClientSignature").SetValue(formData.ClientSignature ?? "");
                form.GetField("txtOrderEnd").SetValue(formData.OrderEnd ?? "");
                form.GetField("txtSignatureOfAcceptance").SetValue(formData.SignatureOfAcceptance ?? "");
            }

            return outputPdfStream.ToArray();
        }

        private static async Task<byte[]> GetPdfNewTicket()
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
