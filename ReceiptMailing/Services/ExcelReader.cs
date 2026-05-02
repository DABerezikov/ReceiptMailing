using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace ReceiptMailing.Services;

public class ExcelReader
{
    public static DataSet GetDataSet(string path)
    {
        using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        do
        {
            while (reader.Read()) { }
        } while (reader.NextResult());

        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            UseColumnDataType = true,
            FilterSheet = (_, _) => true,
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
                EmptyColumnNamePrefix = "Column",
                UseHeaderRow = false,
                ReadHeaderRow = (rowReader) =>
                {
                    rowReader.Read();
                },
                FilterRow = (rowReader) => true,
                FilterColumn = (rowReader, columnIndex) => true
            }
        });
        return result;
    }
}
