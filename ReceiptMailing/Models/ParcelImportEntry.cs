using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Models;

public enum ImportStatus { New, Changed }

/// <summary>Разница в одном поле между импортированным и существующим значением.</summary>
public record FieldDiff(string Field, string? OldValue, string? NewValue);

/// <summary>Одна запись для окна проверки импорта.</summary>
public class ParcelImportEntry : INotifyPropertyChanged
{
    private bool _isSelected = true;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public required ImportStatus Status   { get; init; }
    public required Parcel       Imported { get; init; }
    public          Parcel?      Existing { get; init; }

    public IReadOnlyList<FieldDiff> Differences { get; init; } = [];

    // ── Вычисляемые свойства для привязки ────────────────────────────────────

    public string StatusText  => Status == ImportStatus.New ? "Новый" : "Изменён";
    public string DiffSummary => Status == ImportStatus.New
        ? string.Empty
        : string.Join(", ", Differences.Select(d => d.Field));
    public bool HasDiffs => Differences.Count > 0;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
