using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;
using System.Text;

namespace ReceiptMailing.ViewModels
{
    internal class PdfSplitterViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;

        private readonly ReceiptsSplitter _splitter;
        private readonly IMailService _email;
        private readonly IParcelRepository<Parcel> _parcel;


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

        #region ListNotSendReceipts : List<string> - Список неотправленных файлов

        /// <summary>Список неотправленных файлов</summary>
        private List<string> _ListNotSendReceipts = new ();

        /// <summary>Список неотправленных файлов</summary>
        public List<string> ListNotSendReceipts
        {
            get => _ListNotSendReceipts;
            set => Set(ref _ListNotSendReceipts, value);
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

        #region Command SendReceiptCommand - Команда разделения файла квитанций

        /// <summary> Команда разделения файла квитанций </summary>
        private ICommand _sendReceiptCommand;

        /// <summary> Команда разделения файла квитанций </summary>
        public ICommand SendReceiptCommand => _sendReceiptCommand
            ??= new LambdaCommandAsync(OnSendReceiptCommandExecuted, CanSendReceiptCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
        private bool CanSendReceiptCommandExecute() => SplitFilePath != string.Empty;

        /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
        private async Task OnSendReceiptCommandExecuted()
        {
            var listFiles = GetFileList(SplitFilePath).ToList();
            int countSendFile = 0;

            foreach (var filePath in listFiles)
            {
                if (!await SendReceipt(filePath))
                {
                    ListNotSendReceipts.Add(filePath);
                    continue;
                }

                countSendFile++;
            }

            SaveListFileNotSend();

            _userDialog.Information($"Отправлено {countSendFile} из {listFiles.Count}", "Почтальон");
            
        }

        #endregion

        private IEnumerable<string> GetFileList(string filePath) => Directory.EnumerateFiles(filePath);

        private async Task<(string?, string?)> GetEmailCurrentParcel(string filePath)
        {
            var inputString = filePath;
            var indexStart = inputString.LastIndexOf(" ") + 1;
            var length = inputString.LastIndexOf(".") - indexStart;
            var parcelNumber = filePath.Substring(indexStart, length);
            var currentParcel = await _parcel.GetByNumber(parcelNumber);
            if (currentParcel==null) return (null, null);
            return (currentParcel.Gardener.FirstEmailAddress, currentParcel.Gardener.FirstEmailAddress) ;

        }

        private async Task<bool> SendReceipt(string filePath)
        {
            var listTo = new List<string>();
            var receipt = filePath;
            var email = await GetEmailCurrentParcel(receipt).ConfigureAwait(false);
            if (string.IsNullOrEmpty(email.Item1)) return false;
            listTo.Add(email.Item1);
            if (email.Item2 != null)
                listTo.Add(email.Item2);
            var attachment = new List<string> { receipt };
            var ct = CancellationToken.None;
            var msg = new MailData(listTo, "Квитанция СНТ \"Тимирязевец\"",
                "C уважением, \nПравление СНТ \"Тимирязевец\"", attachment);
            return await _email.SendAsync(msg, ct);
        }

        private void SaveListFileNotSend()
        {
            string file = @"NotSend.csv";
            string separator = ",";
            StringBuilder output = new StringBuilder();
            foreach (var notSend in ListNotSendReceipts)
            {
               output.AppendLine(string.Join(separator, notSend));
            }

            try
            {
                File.AppendAllText(file, output.ToString(), Encoding.Unicode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data could not be written to the CSV file.");
                return;
            }
        }

        public PdfSplitterViewModel(
            IUserDialog userDialog,
            ReceiptsSplitter splitter,
            IMailService email,
            IParcelRepository<Parcel> parcel)
        {
            _userDialog = userDialog;
            _splitter = splitter;
            _email = email;
            _parcel = parcel;
            
        }
    }
}
