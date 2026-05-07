using System.Diagnostics.CodeAnalysis;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ReceiptMailing.ViewModels;

internal class EditGardenerViewModel(Gardener gardener) : ViewModel
{
    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Добавление/редактирование садовода";

    #endregion

    #region SurName : string - Фамилия садовода

    /// <summary>Фамилия садовода</summary>
    public string? SurName
    {
        get => gardener.SurName;
        set
        {
            gardener.SurName = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Name : string - Имя садовода

    /// <summary>Имя садовода</summary>
    public string? Name
    {
        get => gardener.Name;
        set
        {
            gardener.Name = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Patronymic : string - Отчество садовода

    /// <summary>Отчество садовода</summary>
    public string? Patronymic
    {
        get => gardener.Patronymic;
        set
        {
            gardener.Patronymic = value;
            Set(ref field, value);
        }
    }

    #endregion
    
    #region IsPassportVisible : bool - Видимость паспортных данных

    private bool _IsPassportVisible;

    public bool IsPassportVisible
    {
        get => _IsPassportVisible;
        set
        {
            if (!Set(ref _IsPassportVisible, value)) return;
            OnPropertyChanged(nameof(PassportSeries));
            OnPropertyChanged(nameof(PassportNumber));
            OnPropertyChanged(nameof(IsPassportReadOnly));
        }
    }

    /// <summary>Поля паспорта доступны только для чтения пока скрыты</summary>
    public bool IsPassportReadOnly => !_IsPassportVisible;

    #endregion

    #region Command TogglePassportVisibilityCommand

    [field: AllowNull, MaybeNull]
    public ICommand TogglePassportVisibilityCommand => field
        ??= new LambdaCommand(() => IsPassportVisible = !IsPassportVisible);

    #endregion

    #region PassportSeries : string - Серия паспорта садовода

    /// <summary>Серия паспорта садовода</summary>
    public string? PassportSeries
    {
        get => _IsPassportVisible
            ? gardener.Passport.RevealedSeries
            : gardener.Passport.Series;
        set
        {
            gardener.Passport.Series = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PassportNumber : string - Номер паспорта садовода

    /// <summary>Номер паспорта садовода</summary>
    public string? PassportNumber
    {
        get => _IsPassportVisible
            ? gardener.Passport.RevealedNumber
            : gardener.Passport.Number;
        set
        {
            gardener.Passport.Number = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PhoneNumber : string - Номер телефона садовода

    /// <summary>Номер телефона садовода</summary>
    public string? PhoneNumber
    {
        get => gardener.PhoneNumber;
        set
        {
            gardener.PhoneNumber = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region FirstEmailAddress : string - Адрес основной электронной почты садовода

    /// <summary>Адрес основной электронной почты садовода</summary>
    public string? FirstEmailAddress
    {
        get => gardener.FirstEmailAddress;
        set
        {
            gardener.FirstEmailAddress = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region SecondEmailAddress : string - Адрес дополнительной электронной почты садовода

    /// <summary>Адрес дополнительной электронной почты садовода</summary>
    public string? SecondEmailAddress
    {
        get => gardener.SecondEmailAddress;
        set
        {
            gardener.SecondEmailAddress = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Account : string - Лицевой счет садовода

    /// <summary>Лицевой счет садовода</summary>
    public string? Account
    {
        get => gardener.Account;
        set
        {
            gardener.Account = value ?? string.Empty;
            Set(ref field, value);
        }
    }

    #endregion

    #region Document : string - Документ о приеме в члены СНТ

    /// <summary>Документ о приеме в члены СНТ</summary>
    public string? Document
    {
        get => gardener.Document;
        set
        {
            gardener.Document = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostPostPostalCode : string - Индекс прописки

    /// <summary>Индекс прописки</summary>
    public string? PostPostalCode
    {
        get => gardener.PostAddress.PostalCode;
        set
        {
            gardener.PostAddress.PostalCode = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostProvince : string - Область прописки

    /// <summary>Область прописки</summary>
    public string? PostProvince
    {
        get => gardener.PostAddress.Province;
        set
        {
            gardener.PostAddress.Province = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostRegion : string - Округ прописки

    /// <summary>Округ прописки</summary>
    public string? PostRegion
    {
        get => gardener.PostAddress.Region;
        set
        {
            gardener.PostAddress.Region = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostCity : string - Населенный пункт прописки

    /// <summary>Населенный пункт прописки</summary>
    public string? PostCity
    {
        get => gardener.PostAddress.City;
        set
        {
            gardener.PostAddress.City = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostStreet : string - Улица прописки

    /// <summary>Улица прописки</summary>
    public string? PostStreet
    {
        get => gardener.PostAddress.Street;
        set
        {
            gardener.PostAddress.Street = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostHouse : string - Дом прописки

    /// <summary>Дом прописки</summary>
    public string? PostHouse
    {
        get => gardener.PostAddress.House;
        set
        {
            gardener.PostAddress.House = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostBuilding : string - Корпус прописки

    /// <summary>Корпус прописки</summary>
    public string? PostBuilding
    {
        get => gardener.PostAddress.Building;
        set
        {
            gardener.PostAddress.Building = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region PostRoom : string - Квартира прописки

    /// <summary>Квартира прописки</summary>
    public string? PostRoom
    {
        get => gardener.PostAddress.Room;
        set
        {
            gardener.PostAddress.Room = value;
            Set(ref field, value);
        }
    }

    #endregion
     #region PostalCode : string - Индекс

     /// <summary>Индекс</summary>
    public string? PostalCode
    {
        get => gardener.Address.PostalCode;
        set
        {
            gardener.Address.PostalCode = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Province : string - Область

    /// <summary>Область</summary>
    public string? Province
    {
        get => gardener.Address.Province;
        set
        {
            gardener.Address.Province = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Region : string - Округ

    /// <summary>Округ</summary>
    public string? Region
    {
        get => gardener.Address.Region;
        set
        {
            gardener.Address.Region = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region City : string - Населенный пункт

    /// <summary>Населенный пункт</summary>
    public string? City
    {
        get => gardener.Address.City;
        set
        {
            gardener.Address.City = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Street : string - Улица

    /// <summary>Улица</summary>
    public string? Street
    {
        get => gardener.Address.Street;
        set
        {
            gardener.Address.Street = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region House : string - Дом

    /// <summary>Дом</summary>
    public string? House
    {
        get => gardener.Address.House;
        set
        {
            gardener.Address.House = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Building : string - Корпус

    /// <summary>Корпус</summary>
    public string? Building
    {
        get => gardener.Address.Building;
        set
        {
            gardener.Address.Building = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region Room : string - Квартира

    /// <summary>Квартира</summary>
    public string? Room
    {
        get => gardener.Address.Room;
        set
        {
            gardener.Address.Room = value;
            Set(ref field, value);
        }
    }

    #endregion

    #region IsAddressMatched : bool - Адрес прописки совпадает с адресом проживания

    private bool _IsAddressMatched = AddressesEqual(gardener.Address, gardener.PostAddress);

    public bool IsAddressMatched
    {
        get => _IsAddressMatched;
        set
        {
            if (!Set(ref _IsAddressMatched, value)) return;

            if (value)
            {
                gardener.PostAddress.PostalCode = gardener.Address.PostalCode;
                gardener.PostAddress.Province   = gardener.Address.Province;
                gardener.PostAddress.Region     = gardener.Address.Region;
                gardener.PostAddress.City       = gardener.Address.City;
                gardener.PostAddress.Street     = gardener.Address.Street;
                gardener.PostAddress.House      = gardener.Address.House;
                gardener.PostAddress.Building   = gardener.Address.Building;
                gardener.PostAddress.Room       = gardener.Address.Room;
            }
            else
            {
                gardener.PostAddress.PostalCode = null;
                gardener.PostAddress.Province   = null;
                gardener.PostAddress.Region     = null;
                gardener.PostAddress.City       = null;
                gardener.PostAddress.Street     = null;
                gardener.PostAddress.House      = null;
                gardener.PostAddress.Building   = null;
                gardener.PostAddress.Room       = null;
            }

            OnPropertyChanged(nameof(PostPostalCode));
            OnPropertyChanged(nameof(PostProvince));
            OnPropertyChanged(nameof(PostRegion));
            OnPropertyChanged(nameof(PostCity));
            OnPropertyChanged(nameof(PostStreet));
            OnPropertyChanged(nameof(PostHouse));
            OnPropertyChanged(nameof(PostBuilding));
            OnPropertyChanged(nameof(PostRoom));
            OnPropertyChanged(nameof(IsPostAddressEditable));
        }
    }

    public bool IsPostAddressEditable => !_IsAddressMatched;

    #endregion

    #region Command AcceptCommand - Команда приравнивания адресов проживания и прописки

    /// <summary> Команда приравнивания адресов проживания и прописки </summary>
    [field: AllowNull, MaybeNull]
    public ICommand AcceptCommand => field
        ??= new LambdaCommandAsync(OnAcceptCommandExecuted, CanAcceptCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private bool CanAcceptCommandExecute(object? p) => !string.IsNullOrWhiteSpace(Account);

    /// <summary> Логика выполнения - Команда приравнивания адресов проживания и прописки </summary>
    private Task OnAcceptCommandExecuted(object? p)
    {
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

    private static bool AddressesEqual(
        Data.Entities.Base.Address a,
        Data.Entities.Base.Address b) =>
        a.PostalCode  == b.PostalCode  &&
        a.Province    == b.Province    &&
        a.Region      == b.Region      &&
        a.City        == b.City        &&
        a.Street      == b.Street      &&
        a.House       == b.House       &&
        a.Building    == b.Building    &&
        a.Room        == b.Room;
}