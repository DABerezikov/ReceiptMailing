using Aspose.Pdf;
using Aspose.Pdf.Text;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace ReceiptMailing.Services;

public class ReceiptsSplitter
{
    public string Path { get; set; }
    private const string FolderPath = "documents";
}