using ReceiptMailing.Data.Entities;
using ReceiptMailing.ViewModels.Base;

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

    public EditGardenerViewModel(Gardener gardener)
    {
        _Gardener = gardener;
    }
}