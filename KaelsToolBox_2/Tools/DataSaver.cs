using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Tools;

public class DataSaver<T>
{
    private List<T> data = new List<T>();

    /// <summary>
    /// Gets the data list.
    /// </summary>
    public List<T> Data => new List<T>(data);

    /// <summary>
    /// Event raised when an item is deleted.
    /// </summary>
    public event EventHandler<T>? Deleted;

    /// <summary>
    /// Event raised when an item is created.
    /// </summary>
    public event EventHandler<T>? Created;

    /// <summary>
    /// Add an item to the data list.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The updated DataSaver instance.</returns>
    public DataSaver<T> Add(T item)
    {
        if (data.Contains(item)) return this;

        data.Add(item);
        Created?.Invoke(this, item);

        return this;
    }

    /// <summary>
    /// Delete an item from the data list.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    /// <returns>The updated DataSaver instance.</returns>
    public DataSaver<T> Delete(T item)
    {
        if (!data.Contains(item)) return this;

        data.Remove(item);
        Deleted?.Invoke(this, item);

        return this;
    }

    /// <summary>
    /// Adds the specified item to the data saver.
    /// </summary>
    /// <param name="left">The data saver.</param>
    /// <param name="right">The item to add.</param>
    /// <returns>The updated data saver.</returns>
    public static DataSaver<T> operator +(DataSaver<T> left, T right) => left.Add(right);

    /// <summary>
    /// Deletes the specified item from the data saver.
    /// </summary>
    /// <param name="left">The data saver.</param>
    /// <param name="right">The item to delete.</param>
    /// <returns>The updated data saver.</returns>
    public static DataSaver<T> operator -(DataSaver<T> left, T right) => left.Delete(right);
}
