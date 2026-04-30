using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
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

        #region SelectedParcel : Parcel - Выбранный участок

        /// <summary>Выбранный участок</summary>
        private Parcel _SelectedParcel;

        /// <summary>Выбранный участок</summary>
        public Parcel SelectedParcel
        {
            get => _SelectedParcel;
            set => Set(ref _SelectedParcel, value);
        }

        #endregion

        #region SelectedIndex : int - Выбранный участок

        /// <summary>Выбранный участок</summary>
        private int _SelectedIndex;

        /// <summary>Выбранный участок</summary>
        public int SelectedIndex
        {
            get => _SelectedIndex;
            set => Set(ref _SelectedIndex, value);
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

        #region Command GetCollectionsCommand - Команда получения коллекции участков

        /// <summary> Команда получения коллекции участков </summary>
        private ICommand _GetCollectionsCommand;

        /// <summary> Команда получения коллекции участков </summary>
        public ICommand GetCollectionsCommand => _GetCollectionsCommand
            ??= new LambdaCommandAsync(OnGetCollectionsCommandExecuted, CanGetCollectionsCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда получения коллекции участков </summary>
        private bool CanGetCollectionsCommandExecute() => true;

        /// <summary> Логика выполнения - Команда получения коллекции участков </summary>
        private async Task OnGetCollectionsCommandExecuted()
        {
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
            GardenerCollection = new ObservableCollection<Gardener>(await _Gardener.GetAll());
        }

        #endregion

        #region Command EditGardenerCommand - Команда редактирования данных садовода

        /// <summary> Команда редактирования данных садовода </summary>
        private ICommand _EditGardenerCommand;

        /// <summary> Команда редактирования данных садовода </summary>
        public ICommand EditGardenerCommand => _EditGardenerCommand
            ??= new LambdaCommandAsync(OnEditGardenerCommandExecuted, CanEditGardenerCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда редактирования данных садовода </summary>
        private bool CanEditGardenerCommandExecute() => SelectedParcel!=null;

        /// <summary> Логика выполнения - Команда редактирования данных садовода </summary>
        private async Task OnEditGardenerCommandExecuted()
        {
            var tempGardener = new Gardener();
            var selectedGardener = SelectedParcel.Gardener;
            CopyInfoGardener(selectedGardener, tempGardener);
            if (!_userDialog.CreateOrEditGardener(tempGardener)) return;
            CopyInfoGardener(tempGardener, selectedGardener);
            await _Gardener.Update(selectedGardener);
            GardenerCollection = new ObservableCollection<Gardener>(await _Gardener.GetAll());
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        private void CopyInfoGardener(Gardener gardenerFrom, Gardener gardenerTo)
        {
           
            gardenerTo.Address = gardenerFrom.Address;
            gardenerTo.PostAddress = gardenerFrom.PostAddress;
            gardenerTo.Account = gardenerFrom.Account;
            gardenerTo.Document = gardenerFrom.Document;
            gardenerTo.FirstEmailAddress = gardenerFrom.FirstEmailAddress;
            gardenerTo.Passport = gardenerFrom.Passport;
            gardenerTo.PhoneNumber = gardenerFrom.PhoneNumber;
            gardenerTo.SecondEmailAddress = gardenerFrom.SecondEmailAddress;
            gardenerTo.Name = gardenerFrom.Name;
            gardenerTo.SurName = gardenerFrom.SurName;
            gardenerTo.Patronymic = gardenerFrom.Patronymic;
            gardenerTo.Parcels = gardenerFrom.Parcels;
        }

        #region Command AddGardenerCommand - Команда добавления садовода

        /// <summary> Команда добавления садовода </summary>
        private ICommand _AddGardenerCommand;

        /// <summary> Команда добавления садовода </summary>
        public ICommand AddGardenerCommand => _AddGardenerCommand
            ??= new LambdaCommandAsync(OnAddGardenerCommandExecuted, CanAddGardenerCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда добавления садовода </summary>
        private bool CanAddGardenerCommandExecute() => true;

        /// <summary> Логика выполнения - Команда добавления садовода </summary>
        private async Task OnAddGardenerCommandExecuted()
        {
            var tempGardener = new Gardener();
            if (!_userDialog.CreateOrEditGardener(tempGardener)) return;
            await _Gardener.Add(tempGardener);
            GardenerCollection = new ObservableCollection<Gardener>(await _Gardener.GetAll());
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        #region Command DeleteGardenerCommand - Команда удаления садовода

        /// <summary> Команда удаления садовода </summary>
        private ICommand _DeleteGardenerCommand;

        /// <summary> Команда удаления садовода </summary>
        public ICommand DeleteGardenerCommand => _DeleteGardenerCommand
            ??= new LambdaCommandAsync(OnDeleteGardenerCommandExecuted, CanDeleteGardenerCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда удаления садовода </summary>
        private bool CanDeleteGardenerCommandExecute() => SelectedParcel != null;

        /// <summary> Логика выполнения - Команда удаления садовода </summary>
        private async Task OnDeleteGardenerCommandExecuted()
        {
            var question = $"Вы действительно хотите удалить садовода {SelectedParcel.Gardener.SurName}" +
                           $" {SelectedParcel.Gardener.Name} {SelectedParcel.Gardener.Patronymic}?";
            if (!_userDialog.OkCancelQuestion(question, "Запрос на удаление садовода")) return;
            await _Gardener.Delete(SelectedParcel.Gardener);
            GardenerCollection = new ObservableCollection<Gardener>(await _Gardener.GetAll());
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        #region Command EditParcelCommand - Команда редактирования данных садовода

        /// <summary> Команда редактирования данных садовода </summary>
        private ICommand _EditParcelCommand;

        /// <summary> Команда редактирования данных садовода </summary>
        public ICommand EditParcelCommand => _EditParcelCommand
            ??= new LambdaCommandAsync(OnEditParcelCommandExecuted, CanEditParcelCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда редактирования данных садовода </summary>
        private bool CanEditParcelCommandExecute() => SelectedParcel != null;

        /// <summary> Логика выполнения - Команда редактирования участка </summary>
        private async Task OnEditParcelCommandExecuted()
        {
            var tempParcel = new Parcel();
            var selectedParcel = SelectedParcel;
            CopyInfoParcel(selectedParcel, tempParcel);
            if (!_userDialog.CreateOrEditParcel(tempParcel, _Gardener)) return;
            CopyInfoParcel(tempParcel, selectedParcel);
            await _Parcel.Update(selectedParcel);
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        #region Command AddParcelCommand - Команда добавления участка

        /// <summary> Команда добавления участка </summary>
        private ICommand _AddParcelCommand;

        /// <summary> Команда добавления участка </summary>
        public ICommand AddParcelCommand => _AddParcelCommand
            ??= new LambdaCommandAsync(OnAddParcelCommandExecuted, CanAddParcelCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда добавления участка </summary>
        private bool CanAddParcelCommandExecute() => true;

        /// <summary> Логика выполнения - Команда добавления участка </summary>
        private async Task OnAddParcelCommandExecuted()
        {
            var newParcel = new Parcel();
            if (!_userDialog.CreateOrEditParcel(newParcel, _Gardener)) return;
            await _Parcel.Add(newParcel);
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        #region Command DeleteParcelCommand - Команда удаления участка

        /// <summary> Команда удаления участка </summary>
        private ICommand _DeleteParcelCommand;

        /// <summary> Команда удаления участка </summary>
        public ICommand DeleteParcelCommand => _DeleteParcelCommand
            ??= new LambdaCommandAsync(OnDeleteParcelCommandExecuted, CanDeleteParcelCommandExecute);

        /// <summary> Проверка возможности выполнения - Команда удаления участка </summary>
        private bool CanDeleteParcelCommandExecute() => SelectedParcel != null;

        /// <summary> Логика выполнения - Команда удаления участка </summary>
        private async Task OnDeleteParcelCommandExecuted()
        {
            var question = $"Вы действительно хотите удалить участок №{SelectedParcel.Number}" +
                           $" ({SelectedParcel.Street})?";
            if (!_userDialog.OkCancelQuestion(question, "Запрос на удаление участка")) return;
            await _Parcel.Delete(SelectedParcel);
            ParcelCollection = new ObservableCollection<Parcel>(await _Parcel.GetAll());
        }

        #endregion

        private void CopyInfoParcel(Parcel sourceParcel, Parcel destinationParcel)
        {
            destinationParcel.Gardener = sourceParcel.Gardener;
            destinationParcel.PlotArea = sourceParcel.PlotArea;
            destinationParcel.CadastralNumber = sourceParcel.CadastralNumber;
            destinationParcel.Category = sourceParcel.Category;
            destinationParcel.Description = sourceParcel.Description;
            destinationParcel.Details = sourceParcel.Details;
            destinationParcel.Electrification = sourceParcel.Electrification;
            destinationParcel.HavingHouse = sourceParcel.HavingHouse;
            destinationParcel.HouseNumber = sourceParcel.HouseNumber;
            destinationParcel.Status = sourceParcel.Status;
            destinationParcel.Street = sourceParcel.Street;
            destinationParcel.Id = sourceParcel.Id;
            destinationParcel.Number = sourceParcel.Number;
        }


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
