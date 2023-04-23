using Aspose.Pdf;
using Aspose.Pdf.Text;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReceiptMailing.Services;

public class ReceiptsSplitter
{
    public string Path { get; set; } = string.Empty;
    private string Folder { get; } = Directory.GetCurrentDirectory();
    public string FileFolderPath { get; set; } = string.Empty;
    private const string FolderPath = "documents";

    public string PDFSplit()
    {
        if (Path == string.Empty) return "Не выбран файл с квитанциями";

        if (FileFolderPath==string.Empty)
            FileFolderPath = GetFolderPath();
        
        var list = GetListPDF(FileFolderPath);

        return RanamePDF(list, FileFolderPath);

    }

    public string GetFolderPath()
    {
        var mount = DateTime.Now.ToString("y");
        var dir = $"pdf {mount}";
        var filesPath = $"{FolderPath}\\{dir}";

        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);
        if (!Directory.Exists(filesPath))
            Directory.CreateDirectory(filesPath);

        return Folder + "\\"+ filesPath;
    }

    private List<string> GetListPDF(string filesPath)
    {
       
        var listFile = new List<string>();
        var pdf = PdfReader.Open(Path, PdfDocumentOpenMode.Import);

        for (var i = 0; i < pdf.PageCount; i++)
        {
            var page = pdf.Pages[i];

            var doc1 = new PdfDocument();
            doc1.PageLayout = PdfPageLayout.SinglePage;

            var doc2 = new PdfDocument();
            doc2.PageLayout = PdfPageLayout.SinglePage;

            var point1 = new XPoint(0, 0);
            var point2 = new XPoint(page.BleedBox.X2, page.BleedBox.Y2 / 2);
            var point3 = new XPoint(0, page.BleedBox.Y2 / 2);
            var point4 = new XPoint(page.BleedBox.X2, page.BleedBox.Y2);
            var cutRectangle1 = new PdfRectangle(point1, point2);
            var cutRectangle2 = new PdfRectangle(point3, point4);

            doc1.AddPage(page);
            doc1.Pages[0].TrimBox = cutRectangle1;
            doc1.Pages[0].BleedBox = cutRectangle1;
            doc1.Pages[0].CropBox = cutRectangle1;
            doc1.Pages[0].ArtBox = cutRectangle1;
            doc1.Pages[0].MediaBox = cutRectangle1;

            doc2.Pages.Add(page);
            doc2.Pages[0].TrimBox = cutRectangle2;
            doc2.Pages[0].BleedBox = cutRectangle2;
            doc2.Pages[0].CropBox = cutRectangle2;
            doc2.Pages[0].ArtBox = cutRectangle2;
            doc2.Pages[0].MediaBox = cutRectangle2;

            var file1 = $"{filesPath}\\{i + 1}.1.pdf";
            var file2 = $"{filesPath}\\{i + 1}.2.pdf";

            listFile.Add(file1);
            listFile.Add(file2);

            doc1.Save(file1);
            doc2.Save(file2);
        }

        return listFile;
    }

    private string RanamePDF(List<string> listFile, string filesPath)
    {
        var newFileName = string.Empty;
        for (var i = 0; i < listFile.Count; i++)
        {
            if (newFileName != string.Empty && !File.Exists(newFileName))
                File.Move(listFile[i - 1], newFileName);

            using var pdfDocument = new Document(listFile[i]);

            var option1 = i % 2 == 0
                ? new TextSearchOptions(new Rectangle(0, 250, 595, 140))
                : new TextSearchOptions(new Rectangle(0, 530, 595, 620));

            var absorber1 = new TextFragmentAbsorber("", option1);


            pdfDocument.Pages.Accept(absorber1);
            var textArray1 = absorber1.TextFragments;
            if (textArray1.Count == 0)
            {
                File.Delete(listFile[i]);
                continue;
            }
            var fioAccountArray = textArray1[1].Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sntAddressArray = textArray1[3].Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
           

            if (sntAddressArray.Length < 5)
            {
                File.Delete(listFile[i]);
                continue;
            }
            foreach (var unused in sntAddressArray)
            {
                sntAddressArray[4] = sntAddressArray[4].Replace('/', '_');
            }

            newFileName = sntAddressArray.Length == 5
                ? filesPath + "\\"
                  + fioAccountArray[1] + " "
                  + fioAccountArray[2][0] + "."
                  + fioAccountArray[3][0] + "._"
                  + sntAddressArray[1] + " "
                  + sntAddressArray[4] + ".pdf"
                : filesPath + "\\"
                  + fioAccountArray[1] + " "
                  + fioAccountArray[2][0] + "."
                  + fioAccountArray[3][0] + "._"
                  + sntAddressArray[2] + " "
                  + sntAddressArray[5] + ".pdf";

        }

        if (newFileName != string.Empty && !File.Exists(newFileName))
            File.Move(listFile[^1], newFileName);
        return "Разделение прошло успешно!";
    }
}