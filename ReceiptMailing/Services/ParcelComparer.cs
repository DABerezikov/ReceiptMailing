using System;
using System.Collections.Generic;
using System.Globalization;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Models;

namespace ReceiptMailing.Services;

/// <summary>Сравнивает два объекта Parcel и возвращает список отличий по полям.</summary>
public static class ParcelComparer
{
    private static readonly IFormatProvider Fmt = CultureInfo.InvariantCulture;

    public static List<FieldDiff> Compare(Parcel existing, Parcel imported)
    {
        var d = new List<FieldDiff>();

        // ── Поля участка ─────────────────────────────────────────────────────
        Str(d, "Улица СНТ",         existing.Street,          imported.Street);
        Str(d, "Номер дома СНТ",    existing.HouseNumber,     imported.HouseNumber);
        Dbl(d, "Площадь (соток)",   existing.PlotArea,        imported.PlotArea);
        Str(d, "Кадастровый номер", existing.CadastralNumber, imported.CadastralNumber);
        Str(d, "Подробности",       existing.Details,         imported.Details);
        Boo(d, "Электрификация",    existing.Electrification, imported.Electrification);
        Boo(d, "Есть дом",          existing.HavingHouse,     imported.HavingHouse);
        Str(d, "Категория",         existing.Category,        imported.Category);
        Str(d, "Статус",            existing.Status,          imported.Status);
        Str(d, "Примечание",        existing.Description,     imported.Description);

        // ── Садовод ──────────────────────────────────────────────────────────
        var eg = existing.Gardener;
        var ig = imported.Gardener;

        if (eg is null && ig is not null)
        {
            d.Add(new FieldDiff("Садовод", "Не назначен", ig.ToString()));
            return d;
        }

        if (eg is null) return d;

        Str(d, "Фамилия",          eg.SurName,                 ig?.SurName);
        Str(d, "Имя",              eg.Name,                    ig?.Name);
        Str(d, "Отчество",         eg.Patronymic,              ig?.Patronymic);
        Str(d, "Лицевой счёт",     eg.Account,                 ig?.Account);
        Str(d, "Телефон",          eg.PhoneNumber,             ig?.PhoneNumber);
        Str(d, "Email 1",          eg.FirstEmailAddress,       ig?.FirstEmailAddress);
        Str(d, "Email 2",          eg.SecondEmailAddress,      ig?.SecondEmailAddress);
        Str(d, "Серия паспорта",   eg.Passport.RevealedSeries, ig?.Passport.RevealedSeries);
        Str(d, "Номер паспорта",   eg.Passport.RevealedNumber, ig?.Passport.RevealedNumber);
        Str(d, "Документ",         eg.Document,                ig?.Document);

        // ── Адрес проживания ─────────────────────────────────────────────────
        Str(d, "Индекс",                eg.Address.PostalCode, ig?.Address.PostalCode);
        Str(d, "Область",               eg.Address.Province,   ig?.Address.Province);
        Str(d, "Округ",                 eg.Address.Region,     ig?.Address.Region);
        Str(d, "Населённый пункт",      eg.Address.City,       ig?.Address.City);
        Str(d, "Улица (проживание)",    eg.Address.Street,     ig?.Address.Street);
        Str(d, "Дом (проживание)",      eg.Address.House,      ig?.Address.House);
        Str(d, "Корпус (проживание)",   eg.Address.Building,   ig?.Address.Building);
        Str(d, "Квартира (проживание)", eg.Address.Room,       ig?.Address.Room);

        // ── Адрес прописки ───────────────────────────────────────────────────
        Str(d, "Индекс (прописка)",       eg.PostAddress.PostalCode, ig?.PostAddress.PostalCode);
        Str(d, "Область (прописка)",      eg.PostAddress.Province,   ig?.PostAddress.Province);
        Str(d, "Округ (прописка)",        eg.PostAddress.Region,     ig?.PostAddress.Region);
        Str(d, "Нас. пункт (прописка)",   eg.PostAddress.City,       ig?.PostAddress.City);
        Str(d, "Улица (прописка)",        eg.PostAddress.Street,     ig?.PostAddress.Street);
        Str(d, "Дом (прописка)",          eg.PostAddress.House,      ig?.PostAddress.House);
        Str(d, "Корпус (прописка)",       eg.PostAddress.Building,   ig?.PostAddress.Building);
        Str(d, "Квартира (прописка)",     eg.PostAddress.Room,       ig?.PostAddress.Room);

        return d;
    }

    // ── Вспомогательные ──────────────────────────────────────────────────────

    private static void Str(List<FieldDiff> d, string name, string? a, string? b)
    {
        a = string.IsNullOrWhiteSpace(a) ? null : a.Trim();
        b = string.IsNullOrWhiteSpace(b) ? null : b.Trim();
        if (!string.Equals(a, b, StringComparison.Ordinal))
            d.Add(new FieldDiff(name, a ?? "—", b ?? "—"));
    }

    private static void Boo(List<FieldDiff> d, string name, bool a, bool b)
    {
        if (a != b) d.Add(new FieldDiff(name, a ? "Да" : "Нет", b ? "Да" : "Нет"));
    }

    private static void Dbl(List<FieldDiff> d, string name, double a, double b)
    {
        if (Math.Abs(a - b) > 1e-9)
            d.Add(new FieldDiff(name, a.ToString(Fmt), b.ToString(Fmt)));
    }
}
