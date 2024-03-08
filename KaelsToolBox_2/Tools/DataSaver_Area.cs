using KaelsToolBox_2.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Tools;

public class DataSaver_Area<T>
{
    public DataSaver<T> Data = new();

    public float Size { get; private set; } = 100f;
    public int DensityLimit { get; private set; } = 10;

    
}

internal class Container<T>
{
    const int size = 4;

    public T? Value { get; set; }
    Container<T>?[][] others = new Container<T>[size][];
    ParentReference parent;

    public Container(ParentReference parent, T? value = default)
    {
        Value = value;
        this.parent = parent;
        for (int i = 0; i < size; i++) others[i] = new Container<T>[size];
    }

    public void Set(int X, int Y, Container<T> item)
    {
        others[X][Y] = item;
    }
    public Container<T>? Get(int X, int Y)
    {
        return others[X][Y];
    }

    public async Task ForEach(Func<Container<T>, Task> func, int depth)
    {
        depth--;
        List<Task> tasks = [];

        foreach (var x in others)
        {
            foreach (var y in x)
            {
                if (y is null) continue;

                await func.Invoke(y);

                if (depth > 0) tasks.Add(y.ForEach(func, depth));
            }
        }

        tasks.ForEach(t => t.Start());
        Task.WaitAll([.. tasks]);
    }

    internal class ParentReference
    {
        public Container<T>? Value { get; set; }
        public (int X, int Y) Position { get; init; }
        public int Depth { get; init; }
    }
}
