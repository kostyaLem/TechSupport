using HandyControl.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TechSupport.UI.ViewModels.Base;

/// <summary>
/// Базовый класс для всех ViewModel'ей.
/// Имеется флаг IsUploading для отображения индикатора загрузки, используя метод Execute
/// </summary>
public abstract class BaseItemsViewModel<T> : BaseViewModel
{
    protected readonly ObservableCollection<T> _items;
    // Элементы коллекции, отображаемые на View
    public ICollectionView ItemsView { get; }

    // Поле для текстового поиска с автоматическим обновлением коллекции
    public virtual string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => ItemsView.Refresh(), nameof(SearchText));
    }

    public BaseItemsViewModel()
    {
        _items = new ObservableCollection<T>();
        ItemsView = CollectionViewSource.GetDefaultView(_items);
    }
}
