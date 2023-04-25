using ExcelDataReader;
using System.Text;

var filePath = "D:\\PDF2Mail\\PDF2Mail3\\Реестр членов СНТ (вместе с лс) (для расслки квитанций).xlsx";

using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
{
    // Auto-detect format, supports:
    //  - Binary Excel files (2.0-2003 format; *.xls)
    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        // Choose one of either 1 or 2:

        // 1. Use the reader methods
        do
        {
            while (reader.Read())
            {
                // reader.GetDouble(0);
            }
        } while (reader.NextResult());

        // 2. Use the AsDataSet extension method
        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            // Gets or sets a value indicating whether to set the DataColumn.DataType 
            // property in a second pass.
            UseColumnDataType = true,

            // Gets or sets a callback to determine whether to include the current sheet
            // in the DataSet. Called once per sheet before ConfigureDataTable.
            FilterSheet = (tableReader, sheetIndex) => true,

            // Gets or sets a callback to obtain configuration options for a DataTable. 
            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
            {
                // Gets or sets a value indicating the prefix of generated column names.
                EmptyColumnNamePrefix = "Column",

                // Gets or sets a value indicating whether to use a row from the 
                // data as column names.
                UseHeaderRow = false,

                // Gets or sets a callback to determine which row is the header row. 
                // Only called when UseHeaderRow = true.
                ReadHeaderRow = (rowReader) => {
                    // F.ex skip the first row and use the 2nd row as column headers:
                    rowReader.Read();
                },

                // Gets or sets a callback to determine whether to include the 
                // current row in the DataTable.
                FilterRow = (rowReader) => {
                    return true;
                },

                // Gets or sets a callback to determine whether to include the specific
                // column in the DataTable. Called once per column after reading the 
                // headers.
                FilterColumn = (rowReader, columnIndex) => {
                    return true;
                }
            }
        });
        var fio = result.Tables[0].Rows[1][0].ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var address = result.Tables[0].Rows[47][5].ToString().Split(new char[]{',','-'});

        // The result of each spreadsheet is in result.Tables
    }
}