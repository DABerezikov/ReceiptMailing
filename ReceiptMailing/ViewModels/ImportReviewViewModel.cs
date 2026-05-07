using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Models;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class ImportReviewViewModel(
    IList<ParcelImportEntry> entries,
    IParcelRepository<Parcel> repository,
    IUserDialog userDialog,
    int identicalCount)
    : ViewModel
{
    #region Title / Status / IsApplying

    public string Title => "Проверка импорта";

    public string Status
    {
        get;
        set => Set(ref field, value);
    } = "Готов!";

    public bool IsApplying
    {
        get;
        set => Set(ref field, value);
    } = false;

    #endregion

    #region Entries

    public ObservableCollection<ParcelImportEntry> Entries { get; } = new(entries);

    #endregion

    #region Counts

    public int NewCount       => entries.Count(e => e.Status == ImportStatus.New);
    public int ChangedCount   => entries.Count(e => e.Status == ImportStatus.Changed);
    public int IdenticalCount => identicalCount;

    #endregion

    #region Command SelectAllCommand

    [field: AllowNull, MaybeNull]
    public ICommand SelectAllCommand => field
        ??= new LambdaCommand(() => { foreach (var e in Entries) e.IsSelected = true; });

    #endregion

    #region Command DeselectAllCommand

    [field: AllowNull, MaybeNull]
    public ICommand DeselectAllCommand => field
        ??= new LambdaCommand(() => { foreach (var e in Entries) e.IsSelected = false; });

    #endregion

    #region Command ApplyCommand

    [field: AllowNull, MaybeNull]
    public ICommand ApplyCommand => field
        ??= new LambdaCommandAsync(OnApplyCommandExecuted, CanApplyCommandExecute);

    private bool CanApplyCommandExecute(object? p) => !IsApplying;

    private async Task OnApplyCommandExecuted(object? p)
    {
        var selected = Entries.Where(e => e.IsSelected).ToList();
        if (selected.Count == 0)
        {
            userDialog.Warning("Не выбрано ни одной записи.", "Импорт БД");
            return;
        }

        IsApplying = true;
        int done = 0;

        try
        {
            foreach (var entry in selected)
            {
                Status = $"Применение: {++done} из {selected.Count}...";

                if (entry.Status == ImportStatus.New)
                {
                    await repository.Add(entry.Imported);
                }
                else
                {
                    var existing = await repository.GetByNumber(entry.Imported.Number);
                    if (existing is not null)
                    {
                        ApplyTo(existing, entry.Imported);
                        await repository.Update(existing);
                    }
                }
            }

            int addedCount   = selected.Count(e => e.Status == ImportStatus.New);
            int updatedCount = selected.Count(e => e.Status == ImportStatus.Changed);

            userDialog.Information(
                $"Применено изменений: {done}" +
                $"\n  Добавлено: {addedCount}" +
                $"\n  Обновлено: {updatedCount}",
                "Импорт БД");

            DataEvents.RaiseParcelsChanged();
            ((Window)p!).DialogResult = true;
        }
        catch (Exception ex)
        {
            Status = "Ошибка!";
            userDialog.Error($"Не удалось применить изменения:\n{ex.Message}", "Импорт БД");
        }
        finally
        {
            IsApplying = false;
            Status     = "Готов!";
        }
    }

    #endregion

    #region Command CancelCommand

    [field: AllowNull, MaybeNull]
    public ICommand CancelCommand => field
        ??= new LambdaCommandAsync(OnCancelCommandExecuted, _ => true);

    private Task OnCancelCommandExecuted(object? p)
    {
        ((Window)p!).DialogResult = false;
        return Task.CompletedTask;
    }

    #endregion

    // ── Копирование импортированных значений поверх существующей записи ───────

    private static void ApplyTo(Parcel target, Parcel source)
    {
        target.Street          = source.Street;
        target.HouseNumber     = source.HouseNumber;
        target.PlotArea        = source.PlotArea;
        target.CadastralNumber = source.CadastralNumber;
        target.Details         = source.Details;
        target.Electrification = source.Electrification;
        target.HavingHouse     = source.HavingHouse;
        target.Category        = source.Category;
        target.Status          = source.Status;
        target.Description     = source.Description;

        if (source.Gardener is null) return;

        target.Gardener ??= new Gardener();
        var tg = target.Gardener;
        var sg = source.Gardener;

        tg.SurName            = sg.SurName;
        tg.Name               = sg.Name;
        tg.Patronymic         = sg.Patronymic;
        tg.Account            = sg.Account;
        tg.PhoneNumber        = sg.PhoneNumber;
        tg.FirstEmailAddress  = sg.FirstEmailAddress;
        tg.SecondEmailAddress = sg.SecondEmailAddress;
        tg.Passport.Series    = sg.Passport.RevealedSeries;
        tg.Passport.Number    = sg.Passport.RevealedNumber;
        tg.Document           = sg.Document;

        CopyAddress(tg.Address,     sg.Address);
        CopyAddress(tg.PostAddress, sg.PostAddress);
    }

    private static void CopyAddress(Address to, Address from)
    {
        to.PostalCode = from.PostalCode;
        to.Province   = from.Province;
        to.Region     = from.Region;
        to.City       = from.City;
        to.Street     = from.Street;
        to.House      = from.House;
        to.Building   = from.Building;
        to.Room       = from.Room;
    }
}
