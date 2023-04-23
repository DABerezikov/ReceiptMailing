using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services;
using System.Windows.Input;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels
{
    internal class PdfSplitterViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;

        private readonly ReceiptsSplitter _splitter;
        private readonly IMailService _email;


        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _title = "Биоробот Константин";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _status; set => Set(ref _status, value); }

        #endregion

        #region PDFFilePath : string - Путь к файлу с квитанциями

        /// <summary>Путь к файлу с квитанциями</summary>
        private string? _pdfFilePath = string.Empty;

        /// <summary>Путь к файлу с квитанциями</summary>
        public string? PdfFilePath
        {
            get => _pdfFilePath;
            set => Set(ref _pdfFilePath, value);
        }

        #endregion

        #region SplitFilePath : string - Путь к папке с квитанциями

        /// <summary>Путь к файлу с квитанциями</summary>
        private string? _splitFilePath = string.Empty;

        /// <summary>Путь к файлу с квитанциями</summary>
        public string? SplitFilePath
        {
            get => _splitFilePath;
            set => Set(ref _splitFilePath, value);
        }

        #endregion
        
        #region Command OpenPDFCommand - команда для открытия файла с квитанциями

        /// <summary> команда для открытия файла с квитанциями </summary>
        private ICommand _openPdfCommand;

        /// <summary> команда для открытия файла с квитанциями </summary>
        public ICommand OpenPdfCommand => _openPdfCommand
            ??= new LambdaCommand(OnOpenPDFCommandExecuted, CanOpenPdfCommandExecute);

        /// <summary> Проверка возможности выполнения - команда для открытия файла с квитанциями </summary>
        private bool CanOpenPdfCommandExecute() => true;

        /// <summary> Логика выполнения - команда для открытия файла с квитанциями </summary>
        private void OnOpenPDFCommandExecuted()
        {
            var temp = _userDialog.OpenFile("Выбор исходного файла с квитанциями");
            if (temp != null) ;
            PdfFilePath = temp?.DirectoryName + "\\" + temp?.Name;
        }

        #endregion
        
        #region Command SplitPDFCommand - Команда разделения файла квитанций

        /// <summary> Команда разделения файла квитанций </summary>
        private ICommand _splitPdfCommand;

        /// <summary> Команда разделения файла квитанций </summary>
        public ICommand SplitPdfCommand => _splitPdfCommand
            ??= new LambdaCommand(OnSplitPDFCommandExecuted, CanSplitPdfCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
        private bool CanSplitPdfCommandExecute() => PdfFilePath != string.Empty;

        /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
        private void OnSplitPDFCommandExecuted()
        {
            _splitter.Path = PdfFilePath;
            _userDialog.Information(_splitter.PdfSplit(), "Обрезка квитанций");
            SplitFilePath = _splitter.FileFolderPath;
        }

        #endregion


        public PdfSplitterViewModel(
            IUserDialog userDialog,
            ReceiptsSplitter splitter,
            IMailService email)
        {
            _userDialog = userDialog;
            _splitter = splitter;
            _email = email;
        }
    }
}
