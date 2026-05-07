using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class EditParcelViewModel : ViewModel
{
    private readonly Parcel _Parcel;
    private readonly IRepository<Gardener> _GardenerRepository;
    private readonly CollectionViewSource _GardenerView;
    public ICollectionView? ListGardener => _GardenerView?.View;

    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Добавление/редактирование участка";

    #endregion

    #region Street : string - Улица участка

    /// <summary>Улица участка</summary>
    public string? Street
    {
        get => _Parcel.Street;
        set
        {
            Set(ref field, value);
            _Parcel.Street = value;
        }
    }

    #endregion

    #region Number : string - Номер участка

    /// <summary>Номер участка</summary>
    public string? Number
    {
        get => _Parcel.Number;
        set
        {
            Set(ref field, value);
            _Parcel.Number = value ?? string.Empty;
        }
    }

    #endregion

    #region PlotArea : string - Площадь участка

    /// <summary>Площадь участка</summary>
    public string? PlotArea
    {
        get => _Parcel.PlotArea.ToString(CultureInfo.CurrentCulture);
        set
        {
            Set(ref field, value);
            double.TryParse(value, out var result);
            _Parcel.PlotArea = result;
        }
    }

    #endregion

    #region CadastralNumber : string - кадастровый номер участка

    /// <summary>кадастровый номер участка</summary>
    public string? CadastralNumber
    {
        get => _Parcel.CadastralNumber;
        set
        {
            Set(ref field, value);
            _Parcel.CadastralNumber = value;
        }
    }

    #endregion

    #region Details : string - Реквизиты правоустанавливающего документа участка

    /// <summary>Реквизиты правоустанавливающего документа участка</summary>
    public string? Details
    {
        get => _Parcel.Details;
        set
        {
            Set(ref field, value);
            _Parcel.Details = value;
        }
    }

    #endregion

    #region HouseNumber : string - Номер дома по внутренней нумерации СНТ

    /// <summary>Номер дома по внутренней нумерации СНТ</summary>
    public string? HouseNumber
    {
        get => _Parcel.HouseNumber;
        set
        {
            Set(ref field, value);
            _Parcel.HouseNumber = value;
        }
    }

    #endregion

    #region Category : string - Категория участка

    /// <summary>Категория участка</summary>
    public string? Category
    {
        get => _Parcel.Category;
        set
        {
            Set(ref field, value);
            _Parcel.Category = value;
        }
    }

    #endregion

    #region Status : string - Статус участка

    /// <summary>Статус участка</summary>
    public string? Status
    {
        get => _Parcel.Status;
        set
        {
            Set(ref field, value);
            _Parcel.Status = value;
        }
    }

    #endregion

    #region Description : string - Примечание

    /// <summary>Примечание</summary>
    public string? Description
    {
        get => _Parcel.Description;
        set
        {
            Set(ref field, value);
            _Parcel.Description = value;
        }
    }

    #endregion

    #region HavingHouse : bool- Наличие дома

    /// <summary>Наличие дома</summary>
    public bool HavingHouse
    {
        get => _Parcel.HavingHouse;
        set
        {
            Set(ref field, value);
            _Parcel.HavingHouse = value;
        }
    }

    #endregion

    #region Electrification : bool- Наличие электричества на участке

    /// <summary>Наличие электричества на участке</summary>
    public bool Electrification
    {
        get => _Parcel.Electrification;
        set
        {
            Set(ref field, value);
            _Parcel.Electrification = value;
        }
    }

    #endregion

    #region GardenerFilter : string- Фильтр садоводов

    /// <summary>Фильтр садоводов</summary>
    public string? GardenerFilter
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region GardenerIndex : int - Фильтр садоводов

    /// <summary>Фильтр садоводов</summary>
    public int GardenerIndex
    {
        get;
        set { Set(ref field, value); }
    }

    #endregion

    #region Gardener : Gardener- Владелец участка

    private static readonly Gardener NoGardener = new() { SurName = "Нет садовода" };

    /// <summary>Владелец участка</summary>
    private Gardener _Gardener = NoGardener;



    /// <summary>Владелец участка</summary>
    public Gardener Gardener
    {
        get => _Parcel.Gardener ?? NoGardener;
        set
        {
            _Gardener = value ?? NoGardener;
            Set(ref _Gardener, _Gardener);
            _Parcel.Gardener = _Gardener;
            OnPropertyChanged(nameof(SurName));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Patronymic));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(FirstEmailAddress));
            OnPropertyChanged(nameof(SecondEmailAddress));
            OnPropertyChanged(nameof(Account));
        }
    }

    #endregion

    #region SurName : string - Фамилия садовода

    /// <summary>Фамилия садовода</summary>
    public string? SurName
    {
        get => _Gardener.SurName;
        set
        {
            _Gardener.SurName = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Name : string - Имя садовода

    /// <summary>Имя садовода</summary>
    public string? Name
    {
        get => _Gardener.Name;
        set
        {
            _Gardener.Name = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Patronymic : string - Отчество садовода

    /// <summary>Отчество садовода</summary>
    public string? Patronymic
    {
        get => _Gardener.Patronymic;
        set
        {
            _Gardener.Patronymic = value;
            Set(ref field, value);
        }
    }

    #endregion

   
    #region PhoneNumber : string - Номер телефона садовода

    /// <summary>Номер телефона садовода</summary>
    public string? PhoneNumber
    {
        get => _Gardener.PhoneNumber;
        set
        {
            _Gardener.PhoneNumber = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region FirstEmailAddress : string - Адрес основной электронной почты садовода

    /// <summary>Адрес основной электронной почты садовода</summary>
    public string? FirstEmailAddress
    {
        get => _Gardener.FirstEmailAddress;
        set
        {
            _Gardener.FirstEmailAddress = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region SecondEmailAddress : string - Адрес дополнительной электронной почты садовода

    /// <summary>Адрес дополнительной электронной почты садовода</summary>
    public string? SecondEmailAddress
    {
        get => _Gardener.SecondEmailAddress;
        set
        {
            _Gardener.SecondEmailAddress = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Account : string - Лицевой счет садовода

    /// <summary>Лицевой счет садовода</summary>
    public string? Account
    {
        get => _Gardener.Account;
        set
        {
            _Gardener.Account = value ?? string.Empty;
            Set(ref field, value);
        }
    }

    #endregion

    #region Command AcceptCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    [field: AllowNull, MaybeNull]
    public ICommand AcceptCommand => field
        ??= new LambdaCommandAsync(OnAcceptCommandExecuted, CanAcceptCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanAcceptCommandExecute(object? p) => true;

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private Task OnAcceptCommandExecuted(object? p)
    {
        if (ReferenceEquals(_Gardener, NoGardener))
            _Parcel.Gardener = null;
        ((Window)p!).DialogResult = true;
        return Task.CompletedTask;
    }

    #endregion

    #region Command CancelCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    [field: AllowNull, MaybeNull]
    public ICommand CancelCommand => field
        ??= new LambdaCommandAsync(OnCancelCommandExecuted, CanCancelCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanCancelCommandExecute(object? p) => true;

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private Task OnCancelCommandExecuted(object? p)
    {
        ((Window)p!).DialogResult = false;
        return Task.CompletedTask;
    }

    #endregion

    public EditParcelViewModel(Parcel parcel, IRepository<Gardener> gardenerRepository)
    {
        _Parcel = parcel;
        _Gardener = parcel.Gardener ?? NoGardener;

        _GardenerRepository = gardenerRepository;
        _GardenerView = new CollectionViewSource();
        _GardenerView.Filter += _GardenerViewSourceFilter;

        LoadGardenersAsync();
    }

    private async void LoadGardenersAsync()
    {
        var gardeners = await _GardenerRepository.GetAll();
        var list = new ObservableCollection<Gardener>(
            gardeners.OrderBy(g => g.SurName).Prepend(NoGardener));
        _GardenerView.Source = list;
        OnPropertyChanged(nameof(ListGardener));

        if (_Parcel.Gardener is null || _Parcel.Gardener.Id == 0)
            Gardener = NoGardener;
    }

    private void _GardenerViewSourceFilter(object sender, FilterEventArgs e)
    {
        if (ReferenceEquals(e.Item, NoGardener)) return;
        if (!(e.Item is Gardener gardener) || string.IsNullOrEmpty(GardenerFilter)) return;
        if (gardener.SurName?.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase) != true &&
            gardener.Name?.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase) != true &&
            gardener.Patronymic?.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase) != true)
            e.Accepted = false;
    }

   

    
}