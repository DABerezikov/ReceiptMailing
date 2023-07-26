using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReceiptMailing.ViewModels;

internal class EditGardenerViewModel:ViewModel
{
    private readonly Gardener _Gardener;

    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    private string _title = "Добавление/редактирование садовода";

    /// <summary>Заголовок окна</summary>
    public string Title { get => _title; set => Set(ref _title, value); }

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
    
    #region PassportSeries : string - Серия паспорта садовода

    /// <summary>Серия паспорта садовода</summary>
    private string? _PassportSeries;

    /// <summary>Серия паспорта садовода</summary>
    public string? PassportSeries
    {
        get => _Gardener.Passport.Series;
        set
        {
            _Gardener.Passport.Series = value;
            Set(ref _PassportSeries, value);
        }
    }

    #endregion
    
    #region PassportNumber : string - Номер паспорта садовода

    /// <summary>Номер паспорта садовода</summary>
    private string? _PassportNumber;

    /// <summary>Номер паспорта садовода</summary>
    public string? PassportNumber
    {
        get => _Gardener.Passport.Number;
        set
        {
            _Gardener.Passport.Number = value;
            Set(ref _PassportNumber, value);
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

    #region Document : string - Документ о приеме в члены СНТ

    /// <summary>Документ о приеме в члены СНТ</summary>
    private string? _Document;

    /// <summary>Документ о приеме в члены СНТ</summary>
    public string? Document
    {
        get => _Gardener.Document;
        set
        {
            _Gardener.Document = value;
            Set(ref _Document, value);
        }
    }

    #endregion

    #region PostPostPostalCode : string - Индекс прописки

    /// <summary>Индекс прописки</summary>
    private string? _PostPostalCode;

    /// <summary>Индекс прописки</summary>
    public string? PostPostalCode
    {
        get => _Gardener.PostAddress.PostalCode;
        set
        {
            _Gardener.PostAddress.PostalCode = value;
            Set(ref _PostPostalCode, value);
        }
    }

    #endregion

    #region PostProvince : string - Область прописки

    /// <summary>Область прописки</summary>
    private string? _PostProvince;

    /// <summary>Область прописки</summary>
    public string? PostProvince
    {
        get => _Gardener.PostAddress.Province;
        set
        {
            _Gardener.PostAddress.Province = value;
            Set(ref _PostProvince, value);
        }
    }

    #endregion

    #region PostRegion : string - Округ прописки

    /// <summary>Округ прописки</summary>
    private string? _PostRegion;

    /// <summary>Округ прописки</summary>
    public string? PostRegion
    {
        get => _Gardener.PostAddress.Region;
        set
        {
            _Gardener.PostAddress.Region = value;
            Set(ref _PostRegion, value);
        }
    }

    #endregion

    #region PostCity : string - Населенный пункт прописки

    /// <summary>Населенный пункт прописки</summary>
    private string? _PostCity;

    /// <summary>Населенный пункт прописки</summary>
    public string? PostCity
    {
        get => _Gardener.PostAddress.City;
        set
        {
            _Gardener.PostAddress.City = value;
            Set(ref _PostCity, value);
        }
    }

    #endregion

    #region PostStreet : string - Улица прописки 

    /// <summary>Улица прописки</summary>
    private string? _PostStreet;

    /// <summary>Улица прописки</summary>
    public string? PostStreet
    {
        get => _Gardener.PostAddress.Street;
        set
        {
            _Gardener.PostAddress.Street = value;
            Set(ref _PostStreet, value);
        }
    }

    #endregion

    #region PostHouse : string - Дом прописки

    /// <summary>Дом прописки</summary>
    private string? _PostHouse;

    /// <summary>Дом прописки</summary>
    public string? PostHouse
    {
        get => _Gardener.PostAddress.House;
        set
        {
            _Gardener.PostAddress.House = value;
            Set(ref _PostHouse, value);
        }
    }

    #endregion

    #region PostBuilding : string - Корпус прописки

    /// <summary>Корпус прописки</summary>
    private string? _PostBuilding;

    /// <summary>Корпус прописки</summary>
    public string? PostBuilding
    {
        get => _Gardener.PostAddress.Building;
        set
        {
            _Gardener.PostAddress.Building = value;
            Set(ref _PostBuilding, value);
        }
    }

    #endregion

    #region PostRoom : string - Квартира прописки

    /// <summary>Квартира прописки</summary>
    private string? _PostRoom;

    /// <summary>Квартира прописки</summary>
    public string? PostRoom
    {
        get => _Gardener.PostAddress.Room;
        set
        {
            _Gardener.PostAddress.Room = value;
            Set(ref _PostRoom, value);
        }
    }

    #endregion
     #region PostalCode : string - Индекс

    /// <summary>Индекс</summary>
    private string? _PostalCode;

    /// <summary>Индекс</summary>
    public string? PostalCode
    {
        get => _Gardener.Address.PostalCode;
        set
        {
            _Gardener.Address.PostalCode = value;
            Set(ref _PostalCode, value);
        }
    }

    #endregion

    #region Province : string - Область

    /// <summary>Область</summary>
    private string? _Province;

    /// <summary>Область</summary>
    public string? Province
    {
        get => _Gardener.Address.Province;
        set
        {
            _Gardener.Address.Province = value;
            Set(ref _Province, value);
        }
    }

    #endregion

    #region Region : string - Округ

    /// <summary>Округ</summary>
    private string? _Region;

    /// <summary>Округ</summary>
    public string? Region
    {
        get => _Gardener.Address.Region;
        set
        {
            _Gardener.Address.Region = value;
            Set(ref _Region, value);
        }
    }

    #endregion

    #region City : string - Населенный пункт

    /// <summary>Населенный пункт</summary>
    private string? _City;

    /// <summary>Населенный пункт</summary>
    public string? City
    {
        get => _Gardener.Address.City;
        set
        {
            _Gardener.Address.City = value;
            Set(ref _City, value);
        }
    }

    #endregion

    #region Street : string - Улица

    /// <summary>Улица</summary>
    private string? _Street;

    /// <summary>Улица</summary>
    public string? Street
    {
        get => _Gardener.Address.Street;
        set
        {
            _Gardener.Address.Street = value;
            Set(ref _Street, value);
        }
    }

    #endregion

    #region House : string - Дом

    /// <summary>Дом</summary>
    private string? _House;

    /// <summary>Дом</summary>
    public string? House
    {
        get => _Gardener.Address.House;
        set
        {
            _Gardener.Address.House = value;
            Set(ref _House, value);
        }
    }

    #endregion

    #region Building : string - Корпус

    /// <summary>Корпус</summary>
    private string? _Building;

    /// <summary>Корпус</summary>
    public string? Building
    {
        get => _Gardener.Address.Building;
        set
        {
            _Gardener.Address.Building = value;
            Set(ref _Building, value);
        }
    }

    #endregion

    #region Room : string - Квартира

    /// <summary>Квартира</summary>
    private string? _Room;

    /// <summary>Квартира</summary>
    public string? Room
    {
        get => _Gardener.Address.Room;
        set
        {
            _Gardener.Address.Room = value;
            Set(ref _Room, value);
        }
    }

    #endregion

    #region Command MatchAddressCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    private ICommand _MatchAddressCommand;

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    public ICommand MatchAddressCommand => _MatchAddressCommand
        ??= new LambdaCommandAsync(OnMatchAddressCommandExecuted, CanMatchAddressCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanMatchAddressCommandExecute() => _Gardener.PostAddress!=_Gardener.Address;

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private async Task OnMatchAddressCommandExecuted()
    {
        _Gardener.PostAddress = _Gardener.Address;
    }

    #endregion

    #region Command AcceptCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    private ICommand _AcceptCommand;

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    public ICommand AcceptCommand => _AcceptCommand
        ??= new LambdaCommandAsync(OnAcceptCommandExecuted, CanAcceptCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanAcceptCommandExecute(object p) => !string.IsNullOrWhiteSpace(Account);

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

    public EditGardenerViewModel(Gardener gardener)
    {
        _Gardener = gardener;
    }
}