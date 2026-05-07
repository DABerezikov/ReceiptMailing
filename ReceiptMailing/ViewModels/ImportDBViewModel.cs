using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Models;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.ViewModels;

// Порядок столбцов соответствует структуре ExcelExporter
// Col  0  Номер участка          Col 19  Номер паспорта
// Col  1  Улица СНТ              Col 20  Документ
// Col  2  Номер дома СНТ         Col 21  Индекс (проживание)
// Col  3  Площадь (соток)        Col 22  Область (проживание)
// Col  4  Кадастровый номер      Col 23  Округ (проживание)
// Col  5  Подробности            Col 24  Населённый пункт (проживание)
// Col  6  Электрификация         Col 25  Улица (проживание)
// Col  7  Есть дом               Col 26  Дом (проживание)
// Col  8  Категория              Col 27  Корпус (проживание)
// Col  9  Статус                 Col 28  Квартира (проживание)
// Col 10  Примечание             Col 29  Индекс (прописка)
// Col 11  Фамилия                Col 30  Область (прописка)
// Col 12  Имя                    Col 31  Округ (прописка)
// Col 13  Отчество               Col 32  Нас. пункт (прописка)
// Col 14  Лицевой счёт           Col 33  Улица (прописка)
// Col 15  Телефон                Col 34  Дом (прописка)
// Col 16  Email 1                Col 35  Корпус (прописка)
// Col 17  Email 2                Col 36  Квартира (прописка)
// Col 18  Серия паспорта

internal class ImportDBViewModel(
    IUserDialog userDialog,
    IParcelRepository<Parcel> parcels)
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

    #region XLSXFilePath : string - Путь к файлу

    /// <summary>Путь к выбранному файлу Excel</summary>
    public string? XlsxFilePath
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Command OpenXLSXCommand

    /// <summary>Открыть файл Excel</summary>
    public ICommand OpenXlsxCommand => field
        ??= new LambdaCommand(OnOpenXLSXCommandExecuted);

    private void OnOpenXLSXCommandExecuted()
    {
        var temp = userDialog.OpenFile(
            "Выбор файла Excel с базой участков",
            "Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*");
        if (temp is null) return;
        XlsxFilePath = temp.FullName;
    }

    #endregion

    #region Command ImportParcelsCommand

    /// <summary>Запустить анализ и импорт</summary>
    public ICommand ImportParcelsCommand => field
        ??= new LambdaCommandAsync(OnImportParcelsCommandExecuted, CanImportParcelsCommandExecute);

    private bool CanImportParcelsCommandExecute() => !string.IsNullOrEmpty(XlsxFilePath);

    private async Task OnImportParcelsCommandExecuted()
    {
        if (string.IsNullOrEmpty(XlsxFilePath)) return;

        var entries  = new List<ParcelImportEntry>();
        int identical = 0;

        try
        {
            Status = "Чтение файла...";

            var data      = ExcelReader.GetDataSet(XlsxFilePath);
            var table     = data.Tables[0];
            int totalRows = table.Rows.Count;

            for (int i = 1; i < totalRows; i++) // i=0 — строка заголовков, пропускаем
            {
                var row    = table.Rows[i];
                var number = Cell(row, 0);

                // Строки без номера участка (заголовки, пустые строки) — пропускаем
                if (string.IsNullOrWhiteSpace(number)) continue;

                Status = $"Проверка: {i + 1} из {totalRows}...";

                var imported = BuildParcel(row);
                var existing = await parcels.GetByNumber(number);

                if (existing is null)
                {
                    entries.Add(new ParcelImportEntry
                    {
                        Status   = ImportStatus.New,
                        Imported = imported,
                    });
                }
                else
                {
                    var diffs = ParcelComparer.Compare(existing, imported);
                    if (diffs.Count == 0)
                    {
                        identical++;
                        continue;
                    }

                    entries.Add(new ParcelImportEntry
                    {
                        Status      = ImportStatus.Changed,
                        Imported    = imported,
                        Existing    = existing,
                        Differences = diffs,
                    });
                }
            }

            Status = "Готов!";

            if (entries.Count == 0)
            {
                userDialog.Information(
                    $"Нет новых или изменённых записей.\nИдентичных пропущено: {identical}",
                    "Импорт БД");
                return;
            }

            // Открываем окно проверки
            var vm     = new ImportReviewViewModel(entries, parcels, userDialog, identical);
            var window = new ImportReviewWindow { DataContext = vm };
            window.ShowDialog();

            XlsxFilePath = string.Empty;
        }
        catch (Exception ex)
        {
            Status = "Ошибка!";
            userDialog.Error($"Не удалось прочитать файл:\n{ex.Message}", "Импорт БД");
        }
        finally
        {
            Status = "Готов!";
        }
    }

    #endregion

    // ── Построение объекта Parcel из строки DataTable ─────────────────────────

    private static Parcel BuildParcel(DataRow row)
    {
        var p = new Parcel
        {
            Number          = Cell(row, 0)  ?? string.Empty,
            Street          = Cell(row, 1),
            HouseNumber     = Cell(row, 2),
            PlotArea        = ParseDouble(Cell(row, 3)),
            CadastralNumber = Cell(row, 4),
            Details         = Cell(row, 5),
            Electrification = IsYes(Cell(row, 6)),
            HavingHouse     = IsYes(Cell(row, 7)),
            Category        = Cell(row, 8),
            Status          = Cell(row, 9),
            Description     = Cell(row, 10),
        };

        p.Gardener ??= new Gardener();
        p.Gardener.SurName            = Cell(row, 11);
        p.Gardener.Name               = Cell(row, 12);
        p.Gardener.Patronymic         = Cell(row, 13);
        p.Gardener.Account            = Cell(row, 14) ?? string.Empty;
        p.Gardener.PhoneNumber        = Cell(row, 15);
        p.Gardener.FirstEmailAddress  = Cell(row, 16);
        p.Gardener.SecondEmailAddress = Cell(row, 17);
        p.Gardener.Passport.Series    = Cell(row, 18);
        p.Gardener.Passport.Number    = Cell(row, 19);
        p.Gardener.Document           = Cell(row, 20);

        p.Gardener.Address.PostalCode = Cell(row, 21);
        p.Gardener.Address.Province   = Cell(row, 22);
        p.Gardener.Address.Region     = Cell(row, 23);
        p.Gardener.Address.City       = Cell(row, 24);
        p.Gardener.Address.Street     = Cell(row, 25);
        p.Gardener.Address.House      = Cell(row, 26);
        p.Gardener.Address.Building   = Cell(row, 27);
        p.Gardener.Address.Room       = Cell(row, 28);

        p.Gardener.PostAddress.PostalCode = Cell(row, 29);
        p.Gardener.PostAddress.Province   = Cell(row, 30);
        p.Gardener.PostAddress.Region     = Cell(row, 31);
        p.Gardener.PostAddress.City       = Cell(row, 32);
        p.Gardener.PostAddress.Street     = Cell(row, 33);
        p.Gardener.PostAddress.House      = Cell(row, 34);
        p.Gardener.PostAddress.Building   = Cell(row, 35);
        p.Gardener.PostAddress.Room       = Cell(row, 36);

        return p;
    }

    // ── Вспомогательные ──────────────────────────────────────────────────────

    /// <summary>Безопасно читает ячейку; возвращает null для пустых значений.</summary>
    private static string? Cell(DataRow row, int col)
    {
        if (col >= row.Table.Columns.Count) return null;
        var val = row[col]?.ToString()?.Trim();
        return string.IsNullOrEmpty(val) ? null : val;
    }

    /// <summary>Разбирает double; принимает '.' и ',' как разделитель.</summary>
    private static double ParseDouble(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0d;
        s = s.Replace(',', '.');
        return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0d;
    }

    /// <summary>true, если значение "Да" (без учёта регистра).</summary>
    private static bool IsYes(string? s) =>
        string.Equals(s, "Да", StringComparison.OrdinalIgnoreCase);
}
