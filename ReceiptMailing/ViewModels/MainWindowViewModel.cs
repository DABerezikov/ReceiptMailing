using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class MainWindowViewModel(
    IUserDialog userDialog,
    IParcelRepository<Parcel> parcel,
    IRepository<Gardener> gardener)
    : ViewModel
{
    static MainWindowViewModel()
    {
        // Подписка на событие импорта — перезагружаем список участков
        DataEvents.ParcelsChanged += async (_, _) =>
        {
            if (Application.Current.MainWindow?.DataContext is MainWindowViewModel vm)
                await vm.OnGetCollectionsCommandExecuted();
        };
    }

    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "СНТ Тимирязевец";

    #endregion

    #region Status : string - Статус

    /// <summary>Статус</summary>
    public string Status
    {
        get;
        set => Set(ref field, value);
    } = "Готов!";

    #endregion

    #region ParcelCollection : ObservableCollection<Parcel> - Description

    public ObservableCollection<Parcel>? ParcelCollection
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region SelectedParcel : Parcel - Выбранный участок

    /// <summary>Выбранный участок</summary>
    public Parcel? SelectedParcel
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region SelectedIndex : int - Выбранный участок

    /// <summary>Выбранный участок</summary>
    public int SelectedIndex
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region GardenerCollection : ObservableCollection<Gardener>

    public ObservableCollection<Gardener>? _GardenerCollection;

    public ObservableCollection<Gardener>? GardenerCollection
    {
        get => _GardenerCollection;
        set => Set(ref _GardenerCollection, value);
    }

    #endregion

    #region SelectedGardener : Gardener? - Выбранный садовод

    public Gardener? SelectedGardener
    {
        get;
        set
        {
            if (Set(ref field, value))
                IsGardenerPassportVisible = false;
        }
    }

    #endregion

    #region IsGardenersView : bool - Текущее представление

    private bool _IsGardenersView;

    public bool IsGardenersView
    {
        get => _IsGardenersView;
        set
        {
            if (!Set(ref _IsGardenersView, value)) return;
            OnPropertyChanged(nameof(IsParcelView));
            OnPropertyChanged(nameof(ViewToggleHeader));
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public bool IsParcelView => !_IsGardenersView;
    public string ViewToggleHeader => _IsGardenersView ? "Список участков" : "Список садоводов";

    #endregion

    #region IsGardenerPassportVisible : bool - Показать паспорт в списке садоводов

    public bool IsGardenerPassportVisible
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region Command ToggleViewCommand - Переключить представление

    [field: AllowNull, MaybeNull]
    public ICommand ToggleViewCommand => field
        ??= new LambdaCommand(() => IsGardenersView = !IsGardenersView);

    #endregion

    #region Command ToggleGardenerPassportVisibilityCommand

    [field: AllowNull, MaybeNull]
    public ICommand ToggleGardenerPassportVisibilityCommand => field
        ??= new LambdaCommand(() => IsGardenerPassportVisible = !IsGardenerPassportVisible);

    #endregion

    #region Command GetCollectionsCommand - Команда получения коллекции участков

    /// <summary> Команда получения коллекции участков </summary>
    [field: AllowNull, MaybeNull]
    public ICommand GetCollectionsCommand => field
        ??= new LambdaCommandAsync(OnGetCollectionsCommandExecuted, CanGetCollectionsCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда получения коллекции участков </summary>
    private bool CanGetCollectionsCommandExecute() => true;

    /// <summary> Логика выполнения - Команда получения коллекции участков </summary>
    private async Task OnGetCollectionsCommandExecuted()
    {
        ParcelCollection = new ObservableCollection<Parcel>(await parcel.GetAll());
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
    }

    #endregion

    #region Command EditGardenerCommand - Команда редактирования данных садовода

    /// <summary> Команда редактирования данных садовода </summary>
    [field: AllowNull, MaybeNull]
    public ICommand EditGardenerCommand => field
        ??= new LambdaCommandAsync(OnEditGardenerCommandExecuted, CanEditGardenerCommandExecute);

    private bool CanEditGardenerCommandExecute() =>
        _IsGardenersView ? SelectedGardener != null : SelectedParcel != null;

    private async Task OnEditGardenerCommandExecuted()
    {
        var target = _IsGardenersView ? SelectedGardener! : SelectedParcel!.Gardener;
        var tempGardener = new Gardener();
        CopyInfoGardener(target, tempGardener);
        if (!userDialog.CreateOrEditGardener(tempGardener)) return;
        CopyInfoGardener(tempGardener, target);
        await gardener.Update(target);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
        ParcelCollection   = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    private void CopyInfoGardener(Gardener gardenerFrom, Gardener gardenerTo)
    {
        gardenerTo.Address = gardenerFrom.Address;
        gardenerTo.PostAddress = gardenerFrom.PostAddress;
        gardenerTo.Account = gardenerFrom.Account;
        gardenerTo.Document = gardenerFrom.Document;
        gardenerTo.FirstEmailAddress = gardenerFrom.FirstEmailAddress;
        gardenerTo.Passport = gardenerFrom.Passport;
        gardenerTo.PhoneNumber = gardenerFrom.PhoneNumber;
        gardenerTo.SecondEmailAddress = gardenerFrom.SecondEmailAddress;
        gardenerTo.Name = gardenerFrom.Name;
        gardenerTo.SurName = gardenerFrom.SurName;
        gardenerTo.Patronymic = gardenerFrom.Patronymic;
        gardenerTo.Parcels = gardenerFrom.Parcels;
    }

    #region Command AddGardenerCommand - Команда добавления садовода

    /// <summary> Команда добавления садовода </summary>
    [field: AllowNull, MaybeNull]
    public ICommand AddGardenerCommand => field
        ??= new LambdaCommandAsync(OnAddGardenerCommandExecuted, CanAddGardenerCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда добавления садовода </summary>
    private bool CanAddGardenerCommandExecute() => true;

    /// <summary> Логика выполнения - Команда добавления садовода </summary>
    private async Task OnAddGardenerCommandExecuted()
    {
        var tempGardener = new Gardener();
        if (!userDialog.CreateOrEditGardener(tempGardener)) return;
        await gardener.Add(tempGardener);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
        ParcelCollection = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    #region Command DeleteGardenerCommand - Команда удаления садовода

    /// <summary> Команда удаления садовода </summary>
    [field: AllowNull, MaybeNull]
    public ICommand DeleteGardenerCommand => field
        ??= new LambdaCommandAsync(OnDeleteGardenerCommandExecuted, CanDeleteGardenerCommandExecute);

    private bool CanDeleteGardenerCommandExecute() =>
        _IsGardenersView ? SelectedGardener != null : SelectedParcel != null;

    private async Task OnDeleteGardenerCommandExecuted()
    {
        var g = _IsGardenersView ? SelectedGardener! : SelectedParcel!.Gardener;
        var question = $"Вы действительно хотите удалить садовода {g?.SurName} {g?.Name} {g?.Patronymic}?";
        if (!userDialog.OkCancelQuestion(question, "Запрос на удаление садовода")) return;
        await gardener.Delete(g!);
        GardenerCollection = new ObservableCollection<Gardener>(await gardener.GetAll());
        ParcelCollection   = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    #region Command EditParcelCommand - Команда редактирования данных садовода

    /// <summary> Команда редактирования данных садовода </summary>
    [field: AllowNull, MaybeNull]
    public ICommand EditParcelCommand => field
        ??= new LambdaCommandAsync(OnEditParcelCommandExecuted, CanEditParcelCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда редактирования данных садовода </summary>
    private bool CanEditParcelCommandExecute() => SelectedParcel != null;

    /// <summary> Логика выполнения - Команда редактирования участка </summary>
    private async Task OnEditParcelCommandExecuted()
    {
        var tempParcel = new Parcel();
        var selectedParcel = SelectedParcel;
        CopyInfoParcel(selectedParcel, tempParcel);
        if (!userDialog.CreateOrEditParcel(tempParcel, gardener)) return;
        CopyInfoParcel(tempParcel, selectedParcel);
        await parcel.Update(selectedParcel);
        ParcelCollection = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    #region Command AddParcelCommand - Команда добавления участка

    /// <summary> Команда добавления участка </summary>
    [field: AllowNull, MaybeNull]
    public ICommand AddParcelCommand => field
        ??= new LambdaCommandAsync(OnAddParcelCommandExecuted, CanAddParcelCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда добавления участка </summary>
    private bool CanAddParcelCommandExecute() => true;

    /// <summary> Логика выполнения - Команда добавления участка </summary>
    private async Task OnAddParcelCommandExecuted()
    {
        var newParcel = new Parcel();
        if (!userDialog.CreateOrEditParcel(newParcel, gardener)) return;
        await parcel.Add(newParcel);
        ParcelCollection = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    #region Command DeleteParcelCommand - Команда удаления участка

    /// <summary> Команда удаления участка </summary>
    [field: AllowNull, MaybeNull]
    public ICommand DeleteParcelCommand => field
        ??= new LambdaCommandAsync(OnDeleteParcelCommandExecuted, CanDeleteParcelCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда удаления участка </summary>
    private bool CanDeleteParcelCommandExecute() => SelectedParcel != null;

    /// <summary> Логика выполнения - Команда удаления участка </summary>
    private async Task OnDeleteParcelCommandExecuted()
    {
        var question = $"Вы действительно хотите удалить участок №{SelectedParcel.Number}" +
                       $" ({SelectedParcel.Street})?";
        if (!userDialog.OkCancelQuestion(question, "Запрос на удаление участка")) return;
        await parcel.Delete(SelectedParcel);
        ParcelCollection = new ObservableCollection<Parcel>(await parcel.GetAll());
    }

    #endregion

    private void CopyInfoParcel(Parcel sourceParcel, Parcel destinationParcel)
    {
        destinationParcel.Gardener = sourceParcel.Gardener;
        destinationParcel.PlotArea = sourceParcel.PlotArea;
        destinationParcel.CadastralNumber = sourceParcel.CadastralNumber;
        destinationParcel.Category = sourceParcel.Category;
        destinationParcel.Description = sourceParcel.Description;
        destinationParcel.Details = sourceParcel.Details;
        destinationParcel.Electrification = sourceParcel.Electrification;
        destinationParcel.HavingHouse = sourceParcel.HavingHouse;
        destinationParcel.HouseNumber = sourceParcel.HouseNumber;
        destinationParcel.Status = sourceParcel.Status;
        destinationParcel.Street = sourceParcel.Street;
        destinationParcel.Id = sourceParcel.Id;
        destinationParcel.Number = sourceParcel.Number;
    }
}
