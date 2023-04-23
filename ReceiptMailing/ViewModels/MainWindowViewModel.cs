using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _userDialog;
        private readonly IParcelRepository<Parcel> _Parcel;
        private readonly IRepository<Gardener> _Gardener;

       
        

        #region Title : string - Заголовок окна

        /// <summary>Заголовок окна</summary>
        private string _title = "СНТ Тимирязевец";

        /// <summary>Заголовок окна</summary>
        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        #region Status : string - Статус

        /// <summary>Статус</summary>
        private string _status = "Готов!";

        /// <summary>Статус</summary>
        public string Status { get => _status; set => Set(ref _status, value); }

        #endregion

        #region ParcelCollection : ObservableCollection<Parcel> - Description

        /// <summary>Коллекция участков</summary>
        private ObservableCollection<Parcel> _ParcelCollection;

        public ObservableCollection<Parcel> ParcelCollection
        {
            get => _ParcelCollection;
            set => Set(ref _ParcelCollection, value);
        }
        #endregion

        #region GardenerCollection : ObservableCollection<Gardener> - Description

        /// <summary>Коллекция садоводов</summary>
        public ObservableCollection<Gardener> _GardenerCollection;
        public ObservableCollection<Gardener> GardenerCollection
        {
            get => _GardenerCollection;
            set => Set(ref _GardenerCollection, value);
        }

        #endregion

        #region Command GetParcelCollectionsCommand - Команда получения коллекции участков

        /// <summary> Команда получения коллекции участков </summary>
        private ICommand _GetParcelCollectionsCommand;

        /// <summary> Команда получения коллекции участков </summary>
        public ICommand GetParcelCollectionsCommand => _GetParcelCollectionsCommand
            ??= new LambdaCommandAsync(OnGetParcelCollectionsCommandExecuted, CanGetParcelCollectionsCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда получения коллекции участков </summary>
        private bool CanGetParcelCollectionsCommandExecute() => true;

        /// <summary> Логика выполнения - Команда получения коллекции участков </summary>
        private async Task OnGetParcelCollectionsCommandExecuted()
        {
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        #region Command GetGardenerCollectionsCommand - Команда получения коллекции садоводов

        /// <summary> Команда получения коллекции садоводов </summary>
        private ICommand _GetGardenerCollectionsCommand;

        /// <summary> Команда получения коллекции садоводов </summary>
        public ICommand GetGardenerCollectionsCommand => _GetGardenerCollectionsCommand
            ??= new LambdaCommandAsync(OnGetGardenerCollectionsCommandExecuted, CanGetGardenerCollectionsCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда получения коллекции садоводов </summary>
        private bool CanGetGardenerCollectionsCommandExecute() => true;

        /// <summary> Логика выполнения - Команда получения коллекции садоводов </summary>
        private async Task OnGetGardenerCollectionsCommandExecuted()
        {
            GardenerCollection = new ObservableCollection<Gardener>(await _Gardener.GetAll());
        }

        #endregion


        public MainWindowViewModel(
            IUserDialog userDialog,
            IParcelRepository<Parcel> parcel,
            IRepository<Gardener> gardener)
        {
            _userDialog = userDialog;
            _Parcel = parcel;
            _Gardener = gardener;
        }
    }
}
