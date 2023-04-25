using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.ViewModels.Base;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Data.Repositories;
using ReceiptMailing.Services;
using System;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.ViewModels
{
    internal class ImportDBViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;
        private readonly IParcelRepository<Parcel> _Parcels;
        private readonly ExcelReader _Reader;


        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _title = "Импорт базы участков";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _status; set => Set(ref _status, value); }

        #endregion
        
        #region XLSXFilePath : string - Путь к файлу с садоводами

        /// <summary>Путь к файлу с садоводами</summary>
        private string? _xlsxFilePath = string.Empty;

        /// <summary>Путь к файлу с садоводами</summary>
        public string? XlsxFilePath
        {
            get => _xlsxFilePath;
            set => Set(ref _xlsxFilePath, value);
        }

        #endregion
        
        #region Command OpenXLSXCommand - команда для открытия файла с садоводами

        /// <summary> команда для открытия файла с садоводами </summary>
        private ICommand _openXlsxCommand;

        /// <summary> команда для открытия файла с садоводами </summary>
        public ICommand OpenXlsxCommand => _openXlsxCommand
            ??= new LambdaCommand(OnOpenXLSXCommandExecuted, CanOpenXlsxCommandExecute);

        /// <summary> Проверка возможности выполнения - команда для открытия файла с садоводами </summary>
        private bool CanOpenXlsxCommandExecute() => true;

        /// <summary> Логика выполнения - команда для открытия файла с садоводами </summary>
        private void OnOpenXLSXCommandExecuted()
        {
            var temp = _userDialog.OpenFile("Выбор исходного файла с садоводами");
            if (temp != null) ;
            XlsxFilePath = temp?.DirectoryName + "\\" + temp?.Name;
        }

        #endregion

        #region Command ImportParcelsCommand - Команда импорта БД участков

        /// <summary> Команда импорта БД участков </summary>
        private ICommand _ImportParcelsCommand;

        /// <summary> Команда импорта БД участков </summary>
        public ICommand ImportParcelsCommand => _ImportParcelsCommand
            ??= new LambdaCommand(OnImportParcelsCommandExecuted, CanImportParcelsCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда импорта БД участков </summary>
        private bool CanImportParcelsCommandExecute() => XlsxFilePath!=null;

        /// <summary> Логика выполнения - Команда импорта БД участков </summary>
        private void OnImportParcelsCommandExecuted()
        {
            if (XlsxFilePath != null) ImportParcels(XlsxFilePath);
            XlsxFilePath=null;
        }

        #endregion

        private void ImportParcels(string path)
        {
            var data = _Reader.GetDataSet(path);
            var data_tables = data.Tables[0];
            for (int i = 1; i < data_tables.Rows.Count-1; i++)
            {
                //Создание нового участка
                var new_parcel = new Parcel();

                //Получение ФИО садовода и присвоение его владельцу участка
                string?[]? fio = data_tables.Rows[i][0].ToString()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                new_parcel.Gardener.Name = fio?[1];
                new_parcel.Gardener.SurName = fio?[0];
                new_parcel.Gardener.Patronymic = fio?[2];

                //Получение и присвоение лицевого счета участка садоводу 
                var account = data_tables.Rows[i][1].ToString();
                new_parcel.Gardener.Account = account;

                //Получение и присвоение документа о приеме в члены СНТ садовода
                var document = data_tables.Rows[i][3].ToString();
                new_parcel.Gardener.Document = document;

                //Получение и присвоение серии и номера паспорта садоводу
                string?[]? passport = data_tables.Rows[i][4].ToString()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (passport.Length > 0)
                {
                   new_parcel.Gardener.Passport.Series = passport?[0];
                   new_parcel.Gardener.Passport.Number = passport?[1]; 
                }
                

                //Получение и присвоение адреса садоводу
                var address = data_tables.Rows[i][5].ToString()?.Split(',');
                new_parcel.Gardener.Address.PostalCode = address?[0];
                new_parcel.Gardener.Address.Province = address?[1];
                new_parcel.Gardener.Address.Region = address?[2];
                new_parcel.Gardener.Address.City = address?[3];
                new_parcel.Gardener.Address.Street = address?[4];
                new_parcel.Gardener.Address.House = address?[5];
                new_parcel.Gardener.Address.Building = address?[6];
                new_parcel.Gardener.Address.Room = address?[7];

                //Получение и присвоение почтового адреса садоводу
                var post_address = data_tables.Rows[i][6].ToString()?.Split(',');
                new_parcel.Gardener.PostAddress.PostalCode = post_address?[0];
                new_parcel.Gardener.PostAddress.Province = post_address?[1];
                new_parcel.Gardener.PostAddress.Region = post_address?[2];
                new_parcel.Gardener.PostAddress.City = post_address?[3];
                new_parcel.Gardener.PostAddress.Street = post_address?[4];
                new_parcel.Gardener.PostAddress.House = post_address?[5];
                new_parcel.Gardener.PostAddress.Building = post_address?[6];
                new_parcel.Gardener.PostAddress.Room = post_address?[7];

                //Получение и присвоение адреса электронной почты садоводу
                var email = data_tables.Rows[i][7].ToString()?.Split(new char[]{' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries );
                switch (email.Length)
                {
                    case 2:
                        new_parcel.Gardener.FirstEmailAddress = email?[0];
                        new_parcel.Gardener.SecondEmailAddress = email?[1];
                        break;
                    case 1:
                        new_parcel.Gardener.FirstEmailAddress = email?[0];
                        break;
                }

                //Получение и присвоение телефона садоводу
                var phone = data_tables.Rows[i][9].ToString();
                new_parcel.Gardener.PhoneNumber = phone;

                //Получение названия улицы участка
                var street = data_tables.Rows[i][10].ToString();
                new_parcel.Street = street;

                //Получение номера участка
                var number = data_tables.Rows[i][11].ToString();
                new_parcel.Number = number!;

                //Получение площади участка
                var area = data_tables.Rows[i][12].ToString();
                var plotArea = new_parcel.PlotArea;
                if (!double.TryParse(area, out plotArea))
                {
                    new_parcel.PlotArea = 0.0;
                }
                //Получение кадастрового номера участка
                var cadasdral = data_tables.Rows[i][13].ToString();
                new_parcel.CadastralNumber = cadasdral;

                //Получение реквизитов провоустанавливающего документа
                var details = data_tables.Rows[i][14].ToString();
                new_parcel.Details = details;

                //Получения наличия подключения
                var electric = data_tables.Rows[i][15].ToString();
                new_parcel.Electrification = electric=="да";

                //Получение наличия дома
                var house = data_tables.Rows[i][16].ToString();
                new_parcel.HavingHouse = house == "да";

                //Получение адреса СНТ
                var address_SNT = data_tables.Rows[i][17].ToString();
                new_parcel.HouseNumber = address_SNT;

                //Получение категории участка
                var category = data_tables.Rows[i][18].ToString();
                new_parcel.Category = category;

                //Получение статуса участка
                var status = data_tables.Rows[i][19].ToString();
                new_parcel.Status = status;

                //Добавление участка в репозиторий
                _Parcels.Add(new_parcel);
            }
            
        }

        


        public ImportDBViewModel(
            IUserDialog userDialog,
            IParcelRepository<Parcel> parcels,
            ExcelReader reader
        )
        {
            _userDialog = userDialog;
            _Parcels = parcels;
            _Reader = reader;
        }
    }
}
