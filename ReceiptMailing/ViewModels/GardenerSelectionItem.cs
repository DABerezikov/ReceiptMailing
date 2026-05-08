using System.ComponentModel;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.ViewModels;

public class GardenerSelectionItem : INotifyPropertyChanged
{
    private bool _isSelected;

    public GardenerSelectionItem(Gardener gardener) => Gardener = gardener;

    public Gardener Gardener { get; }

    public bool HasEmail =>
        !string.IsNullOrWhiteSpace(Gardener.FirstEmailAddress) ||
        !string.IsNullOrWhiteSpace(Gardener.SecondEmailAddress);

    public string DisplayName =>
        $"{Gardener.SurName} {Gardener.Name} {Gardener.Patronymic}".Trim();

    public string EmailDisplay =>
        Gardener.FirstEmailAddress ?? Gardener.SecondEmailAddress ?? "нет email";

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
