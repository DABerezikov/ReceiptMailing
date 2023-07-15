using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
    private string _title = "Добавление/редактирование участка";

    /// <summary>Заголовок окна</summary>
    public string Title { get => _title; set => Set(ref _title, value); }

    #endregion

    #region Street : string - Улица участка

    /// <summary>Улица участка</summary>
    private string? _Street;

    /// <summary>Улица участка</summary>
    public string? Street
    {
        get => _Parcel.Street;
        set
        {
            Set(ref _Street, value);
            _Parcel.Street = value;
        }
    }

    #endregion

    #region Number : string - Номер участка

    /// <summary>Номер участка</summary>
    private string? _Number;

    /// <summary>Номер участка</summary>
    public string? Number
    {
        get => _Parcel.Number;
        set
        {
            Set(ref _Number, value);
            _Parcel.Number = value;
        }
    }

    #endregion

    #region PlotArea : string - Площадь участка

    /// <summary>Площадь участка</summary>
    private string? _PlotArea;

    /// <summary>Площадь участка</summary>
    public string? PlotArea
    {
        get => _Parcel.PlotArea.ToString(CultureInfo.CurrentCulture);
        set
        {
            Set(ref _PlotArea, value);
            double.TryParse(value, out var result);
            _Parcel.PlotArea = result;
        }
    }

    #endregion

    #region CadastralNumber : string - кадастровый номер участка

    /// <summary>кадастровый номер участка</summary>
    private string? _CadastralNumber;

    /// <summary>кадастровый номер участка</summary>
    public string? CadastralNumber
    {
        get => _Parcel.CadastralNumber;
        set
        {
            Set(ref _CadastralNumber, value);
            _Parcel.CadastralNumber = value;
        }
    }

    #endregion

    #region Details : string - Реквизиты правоустанавливающего документа участка

    /// <summary>Реквизиты правоустанавливающего документа участка</summary>
    private string? _Details;

    /// <summary>Реквизиты правоустанавливающего документа участка</summary>
    public string? Details
    {
        get => _Parcel.Details;
        set
        {
            Set(ref _Details, value);
            _Parcel.Details = value;
        }
    }

    #endregion

    #region HouseNumber : string - Номер дома по внутренней нумерации СНТ

    /// <summary>Номер дома по внутренней нумерации СНТ</summary>
    private string? _HouseNumber;

    /// <summary>Номер дома по внутренней нумерации СНТ</summary>
    public string? HouseNumber
    {
        get => _Parcel.HouseNumber;
        set
        {
            Set(ref _HouseNumber, value);
            _Parcel.HouseNumber = value;
        }
    }

    #endregion

    #region Category : string - Категория участка

    /// <summary>Категория участка</summary>
    private string? _Category;

    /// <summary>Категория участка</summary>
    public string? Category
    {
        get => _Parcel.Category;
        set
        {
            Set(ref _Category, value);
            _Parcel.Category = value;
        }
    }

    #endregion

    #region Status : string - Статус участка

    /// <summary>Статус участка</summary>
    private string? _Status;

    /// <summary>Статус участка</summary>
    public string? Status
    {
        get => _Parcel.Status;
        set
        {
            Set(ref _Status, value);
            _Parcel.Status = value;
        }
    }

    #endregion

    #region Description : string - Примечание

    /// <summary>Примечание</summary>
    private string? _Description;

    /// <summary>Примечание</summary>
    public string? Description
    {
        get => _Parcel.Description;
        set
        {
            Set(ref _Description, value);
            _Parcel.Description = value;
        }
    }

    #endregion

    #region HavingHouse : bool- Наличие дома

    /// <summary>Наличие дома</summary>
    private bool _HavingHouse;

    /// <summary>Наличие дома</summary>
    public bool HavingHouse
    {
        get => _Parcel.HavingHouse;
        set
        {
            Set(ref _HavingHouse, value);
            _Parcel.HavingHouse = value;
        }
    }

    #endregion

    #region Electrification : bool- Наличие электричества на участке

    /// <summary>Наличие электричества на участке</summary>
    private bool _Electrification;

   

    /// <summary>Наличие электричества на участке</summary>
    public bool Electrification
    {
        get => _Parcel.Electrification;
        set
        {
            Set(ref _Electrification, value);
            _Parcel.Electrification = value;
        }
    }

    #endregion

    #region GardenerFilter : string- Фильтр садоводов

    /// <summary>Фильтр садоводов</summary>
    private string _GardenerFilter;



    /// <summary>Фильтр садоводов</summary>
    public string GardenerFilter
    {
        get => _GardenerFilter;
        set => Set(ref _GardenerFilter, value);
    }

    #endregion

    #region GardenerIndex : int - Фильтр садоводов

    /// <summary>Фильтр садоводов</summary>
    private int _GardenerIndex;



    /// <summary>Фильтр садоводов</summary>
    public int GardenerIndex
    {
        get => _GardenerIndex;
        set
        {
            Set(ref _GardenerIndex, value);
           
        }
    }

    #endregion

    #region Gardener : Gardener- Владелец участка

    /// <summary>Владелец участка</summary>
    private Gardener _Gardener;



    /// <summary>Владелец участка</summary>
    public Gardener Gardener
    {
        get => _Parcel.Gardener;
        set
        {
            Set(ref _Gardener, value);
            _Parcel.Gardener = value;
        }
    }

    #endregion

    #region SurName : string - Фамилия садовода

    /// <summary>Фамилия садовода</summary>
    private string? _SurName;

    /// <summary>Фамилия садовода</summary>
    public string? SurName
    {
        get => _Gardener.SurName;
        set
        {
            _Gardener.SurName = value;
            Set(ref _SurName, value);
        }
    }

    #endregion

    #region Name : string - Имя садовода

    /// <summary>Имя садовода</summary>
    private string? _Name;

    /// <summary>Имя садовода</summary>
    public string? Name
    {
        get => _Gardener.Name;
        set
        {
            _Gardener.Name = value;
            Set(ref _Name, value);
        }
    }

    #endregion

    #region Patronymic : string - Отчество садовода

    /// <summary>Отчество садовода</summary>
    private string? _Patronymic;

    /// <summary>Отчество садовода</summary>
    public string? Patronymic
    {
        get => _Gardener.Patronymic;
        set
        {
            _Gardener.Patronymic = value;
            Set(ref _Patronymic, value);
        }
    }

    #endregion

   
    #region PhoneNumber : string - Номер телефона садовода

    /// <summary>Номер телефона садовода</summary>
    private string? _PhoneNumber;

    /// <summary>Номер телефона садовода</summary>
    public string? PhoneNumber
    {
        get => _Gardener.PhoneNumber;
        set
        {
            _Gardener.PhoneNumber = value;
            Set(ref _PhoneNumber, value);
        }
    }

    #endregion

    #region FirstEmailAddress : string - Адрес основной электронной почты садовода

    /// <summary>Адрес основной электронной почты садовода</summary>
    private string? _FirstEmailAddress;

    /// <summary>Адрес основной электронной почты садовода</summary>
    public string? FirstEmailAddress
    {
        get => _Gardener.FirstEmailAddress;
        set
        {
            _Gardener.FirstEmailAddress = value;
            Set(ref _FirstEmailAddress, value);
        }
    }

    #endregion

    #region SecondEmailAddress : string - Адрес дополнительной электронной почты садовода

    /// <summary>Адрес дополнительной электронной почты садовода</summary>
    private string? _SecondEmailAddress;

    /// <summary>Адрес дополнительной электронной почты садовода</summary>
    public string? SecondEmailAddress
    {
        get => _Gardener.SecondEmailAddress;
        set
        {
            _Gardener.SecondEmailAddress = value;
            Set(ref _SecondEmailAddress, value);
        }
    }

    #endregion

    #region Account : string - Лицевой счет садовода

    /// <summary>Лицевой счет садовода</summary>
    private string? _Account;

    /// <summary>Лицевой счет садовода</summary>
    public string? Account
    {
        get => _Gardener.Account;
        set
        {
            _Gardener.Account = value;
            Set(ref _Account, value);
        }
    }

    #endregion

    #region Command AcceptCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    private ICommand _AcceptCommand;

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    public ICommand AcceptCommand => _AcceptCommand
        ??= new LambdaCommandAsync(OnAcceptCommandExecuted, CanAcceptCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanAcceptCommandExecute(object p) => true;

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private async Task OnAcceptCommandExecuted(object p)
    {
        ((Window)p).DialogResult = true;
    }

    #endregion

    #region Command CancelCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    private ICommand _CancelCommand;

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    public ICommand CancelCommand => _CancelCommand
        ??= new LambdaCommandAsync(OnCancelCommandExecuted, CanCancelCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanCancelCommandExecute(object p) => true;

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private async Task OnCancelCommandExecuted(object p)
    {
        ((Window)p).DialogResult = false;
    }

    #endregion

    public EditParcelViewModel(Parcel parcel, IRepository<Gardener> gardenerRepository)
    {
        _Parcel = parcel;
        
        _GardenerRepository = gardenerRepository;
        _GardenerView = new CollectionViewSource
        {
            SortDescriptions =
            {
                new SortDescription(nameof(Gardener.SurName), ListSortDirection.Ascending)
            }
        };
        _GardenerView.Filter += _GardenerViewSourceFilter;
        
    }
    private void _GardenerViewSourceFilter(object sender, FilterEventArgs e)
    {
        if (!(e.Item is Gardener gardener) || string.IsNullOrEmpty(GardenerFilter)) return;
        if (!gardener.SurName.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase) ||
            !gardener.Name.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase) ||
            !gardener.Patronymic.Contains(GardenerFilter, StringComparison.OrdinalIgnoreCase))
            e.Accepted = false;
    }

   

    
}