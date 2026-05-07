using System;

namespace ReceiptMailing.Services;

/// <summary>
/// Простая шина событий для уведомления об изменениях данных в БД.
/// </summary>
public static class DataEvents
{
    /// <summary>Срабатывает после успешного импорта участков.</summary>
    public static event EventHandler? ParcelsChanged;

    public static void RaiseParcelsChanged() =>
        ParcelsChanged?.Invoke(null, EventArgs.Empty);
}
