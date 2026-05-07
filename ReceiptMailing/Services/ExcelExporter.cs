using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Services;

public class ExcelExporter
{
    // Порядок столбцов фиксированный — по нему же будет строиться новый импорт
    private static readonly (string Header, Func<Parcel, object?> Value)[] Columns =
    [
        // ── Участок ──────────────────────────────────────────────────────────
        ("Номер участка",           p => p.Number),
        ("Улица СНТ",               p => p.Street),
        ("Номер дома СНТ",          p => p.HouseNumber),
        ("Площадь (соток)",         p => p.PlotArea),
        ("Кадастровый номер",       p => p.CadastralNumber),
        ("Подробности",             p => p.Details),
        ("Электрификация",          p => p.Electrification ? "Да" : "Нет"),
        ("Есть дом",                p => p.HavingHouse    ? "Да" : "Нет"),
        ("Категория",               p => p.Category),
        ("Статус",                  p => p.Status),
        ("Примечание",              p => p.Description),
        // ── Садовод ──────────────────────────────────────────────────────────
        ("Фамилия",                 p => p.Gardener?.SurName),
        ("Имя",                     p => p.Gardener?.Name),
        ("Отчество",                p => p.Gardener?.Patronymic),
        ("Лицевой счёт",            p => p.Gardener?.Account),
        ("Телефон",                 p => p.Gardener?.PhoneNumber),
        ("Email 1",                 p => p.Gardener?.FirstEmailAddress),
        ("Email 2",                 p => p.Gardener?.SecondEmailAddress),
        ("Серия паспорта",          p => p.Gardener?.Passport.RevealedSeries),
        ("Номер паспорта",          p => p.Gardener?.Passport.RevealedNumber),
        ("Документ",                p => p.Gardener?.Document),
        // ── Адрес проживания ─────────────────────────────────────────────────
        ("Индекс",                  p => p.Gardener?.Address.PostalCode),
        ("Область",                 p => p.Gardener?.Address.Province),
        ("Округ",                   p => p.Gardener?.Address.Region),
        ("Населённый пункт",        p => p.Gardener?.Address.City),
        ("Улица (проживание)",      p => p.Gardener?.Address.Street),
        ("Дом (проживание)",        p => p.Gardener?.Address.House),
        ("Корпус (проживание)",     p => p.Gardener?.Address.Building),
        ("Квартира (проживание)",   p => p.Gardener?.Address.Room),
        // ── Адрес прописки ───────────────────────────────────────────────────
        ("Индекс (прописка)",       p => p.Gardener?.PostAddress.PostalCode),
        ("Область (прописка)",      p => p.Gardener?.PostAddress.Province),
        ("Округ (прописка)",        p => p.Gardener?.PostAddress.Region),
        ("Нас. пункт (прописка)",   p => p.Gardener?.PostAddress.City),
        ("Улица (прописка)",        p => p.Gardener?.PostAddress.Street),
        ("Дом (прописка)",          p => p.Gardener?.PostAddress.House),
        ("Корпус (прописка)",       p => p.Gardener?.PostAddress.Building),
        ("Квартира (прописка)",     p => p.Gardener?.PostAddress.Room),
    ];

    /// <summary>Экспортирует список участков в xlsx-файл.</summary>
    public void Export(IEnumerable<Parcel> parcels, string filePath)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Данные БД");

        // ── Заголовок ────────────────────────────────────────────────────────
        for (int col = 0; col < Columns.Length; col++)
        {
            var cell = ws.Cell(1, col + 1);
            cell.Value = Columns[col].Header;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D9E1F2");
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
        }

        // ── Данные ───────────────────────────────────────────────────────────
        int row = 2;
        foreach (var parcel in parcels)
        {
            for (int col = 0; col < Columns.Length; col++)
            {
                var value = Columns[col].Value(parcel);
                var cell  = ws.Cell(row, col + 1);

                switch (value)
                {
                    case double d:   cell.Value = d;            break;
                    case bool b:     cell.Value = b;            break;
                    case null:       cell.Value = string.Empty; break;
                    default:         cell.Value = value.ToString(); break;
                }
            }
            row++;
        }

        // ── Оформление ───────────────────────────────────────────────────────
        ws.SheetView.FreezeRows(1);
        ws.Columns().AdjustToContents(8d, 50d);

        wb.SaveAs(filePath);
    }
}
