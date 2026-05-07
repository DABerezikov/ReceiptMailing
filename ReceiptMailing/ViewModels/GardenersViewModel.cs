using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class GardenersViewModel(
    IUserDialog userDialog,
    IGardenerRepository<Gardener> gardener,
    IParcelRepository<Parcel> parcel)
    : ViewModel
{
    private readonly IParcelRepository<Parcel> _parcel = parcel;

    #region Title : string - Заголовок окна

    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Садоводы";

    #endregion

    #region GardenerCollection : ObservableCollection<Gardener>

    public ObservableCollection<Gardener>? GardenerCollection
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region SelectedGardener : Gardener - Выбранный садовод

    public Gardener? SelectedGardener
    {
        get;
        set
        {
            if (Set(ref field, value))
                IsPassportVisible = false;
        }
    }

    #endregion

    #region IsPassportVisible : bool - Видимость паспортных данных

    public bool IsPassportVisible
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region Command TogglePassportVisibilityCommand - Показать/скрыть паспорт

    [field: AllowNull, MaybeNull]
    public ICommand TogglePassportVisibilityCommand => field
        ??= new LambdaCommand(() => IsPassportVisible = !IsPassportVisible);

    #endregion

    #region Command LoadGardenersCommand - Загрузка списка садоводов

    [field: AllowNull, MaybeNull]
    public ICommand LoadGardenersCommand => field
        ??= new LambdaCommandAsync(OnLoadGardenersCommandExecuted);

    private async Task OnLoadGardenersCommandExecuted()
    {
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
    }

    #endregion

    #region Command AddGardenerCommand - Добавление садовода

    [field: AllowNull, MaybeNull]
    public ICommand AddGardenerCommand => field
        ??= new LambdaCommandAsync(OnAddGardenerCommandExecuted);

    private async Task OnAddGardenerCommandExecuted()
    {
        var tempGardener = new Gardener();
        if (!userDialog.CreateOrEditGardener(tempGardener)) return;
        await gardener.Add(tempGardener);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
    }

    #endregion

    #region Command EditGardenerCommand - Редактирование садовода

    [field: AllowNull, MaybeNull]
    public ICommand EditGardenerCommand => field
        ??= new LambdaCommandAsync(OnEditGardenerCommandExecuted, CanEditGardenerCommandExecute);

    private bool CanEditGardenerCommandExecute() => SelectedGardener is not null;

    private async Task OnEditGardenerCommandExecuted()
    {
        var tempGardener = new Gardener();
        CopyInfoGardener(SelectedGardener!, tempGardener);
        if (!userDialog.CreateOrEditGardener(tempGardener)) return;
        CopyInfoGardener(tempGardener, SelectedGardener!);
        await gardener.Update(SelectedGardener!);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
    }

    #endregion

    #region Command DeleteGardenerCommand - Удаление садовода

    [field: AllowNull, MaybeNull]
    public ICommand DeleteGardenerCommand => field
        ??= new LambdaCommandAsync(OnDeleteGardenerCommandExecuted, CanDeleteGardenerCommandExecute);

    private bool CanDeleteGardenerCommandExecute() => SelectedGardener is not null;

    private async Task OnDeleteGardenerCommandExecuted()
    {
        var g = SelectedGardener!;
        var question = $"Вы действительно хотите удалить садовода {g.SurName} {g.Name} {g.Patronymic}?";
        if (!userDialog.OkCancelQuestion(question, "Запрос на удаление садовода")) return;
        await gardener.Delete(g);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
    }

    #endregion

    private static void CopyInfoGardener(Gardener from, Gardener to)
    {
        to.Address = from.Address;
        to.PostAddress = from.PostAddress;
        to.Account = from.Account;
        to.Document = from.Document;
        to.FirstEmailAddress = from.FirstEmailAddress;
        to.Passport = from.Passport;
        to.PhoneNumber = from.PhoneNumber;
        to.SecondEmailAddress = from.SecondEmailAddress;
        to.Name = from.Name;
        to.SurName = from.SurName;
        to.Patronymic = from.Patronymic;
        to.Parcels = from.Parcels;
    }
}
