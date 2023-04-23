using System;
using System.Windows.Input;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _UserDialog;
        private readonly IDataService _DataService;
        private readonly ReceiptsSplitter _Splitter;

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _Title = "Биоробот Константин";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _Status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _Status; set => Set(ref _Status, value); }

        #endregion

        #region PDFFilePath : string - Путь к файлу с квитанциями

        /// <summary>Путь к файлу с квитанциями</summary>
        private string? _PDFFilePath = string.Empty;

        /// <summary>Путь к файлу с квитанциями</summary>
        public string? PDFFilePath
        {
            get => _PDFFilePath;
            set => Set(ref _PDFFilePath, value);
        }

        #endregion

        #region XLSXFilePath : string - Путь к файлу с садоводами

        /// <summary>Путь к файлу с садоводами</summary>
        private string? _XLSXFilePath = string.Empty;

        /// <summary>Путь к файлу с садоводами</summary>
        public string? XLSXFilePath
        {
            get => _XLSXFilePath;
            set => Set(ref _XLSXFilePath, value);
        }

        #endregion

        #region Command OpenPDFCommand - команда для открытия файла с квитанциями

        /// <summary> команда для открытия файла с квитанциями </summary>
        private ICommand _OpenPDFCommand;

        /// <summary> команда для открытия файла с квитанциями </summary>
        public ICommand OpenPDFCommand => _OpenPDFCommand
            ??= new LambdaCommand(OnOpenPDFCommandExecuted, CanOpenPDFCommandExecute);

        /// <summary> Проверка возможности выполнения - команда для открытия файла с квитанциями </summary>
        private bool CanOpenPDFCommandExecute() => true;

        /// <summary> Логика выполнения - команда для открытия файла с квитанциями </summary>
        private void OnOpenPDFCommandExecuted()
        {
            var temp = _UserDialog.OpenFile("Выбор исходного файла с квитанциями");
            if (temp != null);
                PDFFilePath = temp?.DirectoryName + "\\" + temp?.Name;
        }

        #endregion

        #region Command OpenXLSXCommand - команда для открытия файла с садоводами

        /// <summary> команда для открытия файла с садоводами </summary>
        private ICommand _OpenXLSXCommand;

        /// <summary> команда для открытия файла с садоводами </summary>
        public ICommand OpenXLSXCommand => _OpenXLSXCommand
            ??= new LambdaCommand(OnOpenXLSXCommandExecuted, CanOpenXLSXCommandExecute);

        /// <summary> Проверка возможности выполнения - команда для открытия файла с садоводами </summary>
        private bool CanOpenXLSXCommandExecute() => true;

        /// <summary> Логика выполнения - команда для открытия файла с садоводами </summary>
        private void OnOpenXLSXCommandExecuted()
        {
            var temp = _UserDialog.OpenFile("Выбор исходного файла с квитанциями");
            if (temp != null);
                PDFFilePath = temp?.DirectoryName + "\\" + temp?.Name;
        }

        #endregion

        #region Command SplitPDFCommand - Команда разделения файла квитанций

        /// <summary> Команда разделения файла квитанций </summary>
        private ICommand _SplitPDFCommand;

        /// <summary> Команда разделения файла квитанций </summary>
        public ICommand SplitPDFCommand => _SplitPDFCommand
            ??= new LambdaCommand(OnSplitPDFCommandExecuted, CanSplitPDFCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
        private bool CanSplitPDFCommandExecute() => PDFFilePath!=string.Empty;

        /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
        private void OnSplitPDFCommandExecuted()
        {
            _Splitter.Path = PDFFilePath;
            _UserDialog.Information(_Splitter.PDFSplit(),"Обрезка квитанций");
        }

        #endregion


        public MainWindowViewModel(IUserDialog UserDialog, IDataService DataService, ReceiptsSplitter splitter)
        {
            _UserDialog = UserDialog;
            _DataService = DataService;
            _Splitter = splitter;
        }
    }
}
