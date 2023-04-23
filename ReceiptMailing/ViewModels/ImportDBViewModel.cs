using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services;
using ReceiptMailing.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReceiptMailing.ViewModels
{
    internal class ImportDBViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;
        private readonly MainWindowViewModel _MainWindow;


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

        

        


        public ImportDBViewModel(
            IUserDialog userDialog,
            MainWindowViewModel mainWindow
            )
        {
            _userDialog = userDialog;
            _MainWindow = mainWindow;
        }
    }
}
