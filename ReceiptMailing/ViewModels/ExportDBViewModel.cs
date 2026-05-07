using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class ExportDBViewModel(
    IUserDialog userDialog,
    IParcelRepository<Parcel> parcels,
    ExcelExporter exporter)
    : ViewModel
{
    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Экспорт базы данных";

    #endregion

    #region Status : string - Статус

    /// <summary>Статус</summary>
    public string Status
    {
        get;
        set => Set(ref field, value);
    } = "Готов!";

    #endregion

    #region SavePath : string? - Путь к сохранённому файлу

    /// <summary>Путь к последнему сохранённому файлу</summary>
    public string? SavePath
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region IsExporting : bool - Идёт экспорт

    /// <summary>Признак выполнения экспорта</summary>
    public bool IsExporting
    {
        get;
        set => Set(ref field, value);
    } = false;

    #endregion

    #region Command ExportDbCommand - Команда экспорта БД в Excel

    /// <summary>Команда экспорта БД в Excel</summary>
    [field: AllowNull, MaybeNull]
    public ICommand ExportDbCommand => field
        ??= new LambdaCommandAsync(OnExportDbCommandExecuted, CanExportDbCommandExecute);

    private bool CanExportDbCommandExecute() => !IsExporting;

    private async Task OnExportDbCommandExecuted()
    {
        var file = userDialog.SaveFile(
            "Сохранить базу данных как Excel",
            "Excel (*.xlsx)|*.xlsx",
            "ДанныеБД.xlsx");

        if (file is null) return;

        IsExporting = true;
        Status = "Загрузка данных...";

        try
        {
            var allParcels = (await parcels.GetAll()).ToList();

            Status = $"Экспорт {allParcels.Count} записей...";

            await Task.Run(() => exporter.Export(allParcels, file.FullName));

            SavePath = file.FullName;
            Status   = "Готов!";

            userDialog.Information(
                $"Экспорт завершён.\nСохранено записей: {allParcels.Count}\n\n{file.FullName}",
                "Экспорт БД");
        }
        catch (Exception ex)
        {
            Status = "Ошибка!";
            userDialog.Error($"Не удалось выполнить экспорт:\n{ex.Message}", "Экспорт БД");
        }
        finally
        {
            IsExporting = false;
            Status      = "Готов!";
        }
    }

    #endregion
}
