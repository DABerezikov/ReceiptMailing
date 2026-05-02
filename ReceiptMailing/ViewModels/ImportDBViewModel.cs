using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class ImportDBViewModel(
    IUserDialog userDialog,
    IParcelRepository<Parcel> parcels,
    ExcelReader reader)
    : ViewModel
{
    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Импорт базы участков";

    #endregion

    #region Status : string - Статус

    /// <summary>Статус</summary>
    public string Status
    {
        get;
        set => Set(ref field, value);
    } = "Готов!";

    #endregion

    #region XLSXFilePath : string - Путь к файлу с садоводами

    /// <summary>Путь к файлу с садоводами</summary>
    public string? XlsxFilePath
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Command OpenXLSXCommand - команда для открытия файла с садоводами

    /// <summary> команда для открытия файла с садоводами </summary>
    public ICommand OpenXlsxCommand => field
        ??= new LambdaCommand(OnOpenXLSXCommandExecuted, CanOpenXlsxCommandExecute);

    /// <summary> Проверка возможности выполнения - команда для открытия файла с садоводами </summary>
    private bool CanOpenXlsxCommandExecute() => true;

    /// <summary> Логика выполнения - команда для открытия файла с садоводами </summary>
    private void OnOpenXLSXCommandExecuted()
    {
        var temp = userDialog.OpenFile("Выбор исходного файла с садоводами");
        if (temp is null) return;
        XlsxFilePath = temp.DirectoryName + "\\" + temp.Name;
    }

    #endregion

    #region Command ImportParcelsCommand - Команда импорта БД участков

    /// <summary> Команда импорта БД участков </summary>
    public ICommand ImportParcelsCommand => field
        ??= new LambdaCommandAsync(OnImportParcelsCommandExecuted, CanImportParcelsCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда импорта БД участков </summary>
    private bool CanImportParcelsCommandExecute() => XlsxFilePath != null;

    /// <summary> Логика выполнения - Команда импорта БД участков </summary>
    private async Task OnImportParcelsCommandExecuted()
    {
        if (XlsxFilePath != null) await ImportParcels(XlsxFilePath);
        XlsxFilePath = null;
    }

    #endregion

    private async Task ImportParcels(string path)
    {
        var data = ExcelReader.GetDataSet(path);
        var data_tables = data.Tables[0];
        for (int i = 1; i < data_tables.Rows.Count - 1; i++)
        {
            var new_parcel = new Parcel();

            string?[]? fio = data_tables.Rows[i][0].ToString()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            new_parcel.Gardener.Name = fio?[1];
            new_parcel.Gardener.SurName = fio?[0];
            new_parcel.Gardener.Patronymic = fio?[2];

            var account = data_tables.Rows[i][1].ToString();
            new_parcel.Gardener.Account = account ?? string.Empty;

            var electric = data_tables.Rows[i][2].ToString();
            new_parcel.Electrification = electric == "С ЭЭ";

            var document = data_tables.Rows[i][3].ToString();
            new_parcel.Gardener.Document = document;

            string[]? passport = data_tables.Rows[i][4].ToString()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (passport?.Length > 0)
            {
                new_parcel.Gardener.Passport.Series = passport[0];
                new_parcel.Gardener.Passport.Number = passport.Length > 1 ? passport[1] : null;
            }

            var email = data_tables.Rows[i][7].ToString()?.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            switch (email?.Length)
            {
                case 2:
                    new_parcel.Gardener.FirstEmailAddress = email?[0].Trim();
                    new_parcel.Gardener.SecondEmailAddress = email?[1].Trim();
                    break;
                case 1:
                    new_parcel.Gardener.FirstEmailAddress = email?[0].Trim();
                    break;
            }

            var phone = data_tables.Rows[i][9].ToString();
            new_parcel.Gardener.PhoneNumber = phone;

            var street = data_tables.Rows[i][10].ToString();
            new_parcel.Street = street;

            var number = data_tables.Rows[i][11].ToString();
            new_parcel.Number = number!;

            var area = data_tables.Rows[i][12].ToString();
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "," };
            if (!double.TryParse(area, NumberStyles.AllowDecimalPoint, formatter, out var plotArea))
            {
                new_parcel.PlotArea = 0.0;
            }
            new_parcel.PlotArea = plotArea;

            var cadasdral = data_tables.Rows[i][13].ToString();
            new_parcel.CadastralNumber = cadasdral;

            var details = data_tables.Rows[i][14].ToString();
            new_parcel.Details = details;

            var house = data_tables.Rows[i][16].ToString();
            new_parcel.HavingHouse = house != "";

            var address_SNT = data_tables.Rows[i][17].ToString();
            new_parcel.HouseNumber = address_SNT;

            var category = data_tables.Rows[i][18].ToString();
            new_parcel.Category = category;

            var status = data_tables.Rows[i][19].ToString();
            new_parcel.Status = status;

            await parcels.Add(new_parcel);
        }
    }
}
