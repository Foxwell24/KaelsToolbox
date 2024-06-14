using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.GameStuff.Modularity;

public class Attribute_Number : IChange
{
	private double value = 0;
	public double Value
    {
        get => value;
        set
        {
            var old = Value;
            this.value = value;

            AttributeChanged.Invoke(this, new AttributeChangedArgs<T>(old, Value));
        }
    }
    public EventHandler<AttributeChangedArgs>? AttributeChanged;


    private double changeOverTime = 0;
    public double ChangeOverTime
    {
        get => changeOverTime;
        set
        {
            var old = ChangeOverTime;
            changeOverTime = value;

            ChangeOverTimeChanged.Invoke(this, new AttributeChangedArgs<T>(old, ChangeOverTime));
        }
    }
    public EventHandler<AttributeChangedArgs>? ChangeOverTimeChanged { get; set; }
    public double ChangePerSecond { get; set; } = 1;
    public void Change(double delta) => Value *= delta;

    public Attribute_Number Clone()
    {
        return new()
        {
            Value = Value,
            ChangeOverTime = ChangeOverTime,
            ChangePerSecond = ChangePerSecond
        };
    }
}

public interface IChange
{
    double ChangeOverTime { get; set; }
    double ChangePerSecond { get; set; }
    EventHandler<AttributeChangedArgs>? ChangeOverTimeChanged { get; set; }

    void Change(double delta);
}

public class AttributeChangedArgs(double newValue, double oldValue) : EventArgs
{
    public double NewValue { get; set; } = newValue;
    public double OldValue { get; set; } = oldValue;
}

public class Gun : IUpdate
{
    static readonly System.Reflection.FieldInfo[] fields = typeof(Gun).GetFields();

    public Attribute_Number ReloadSpeed = new();
    public Attribute_Number MagazineSize = new();
    public Bullet BulletTemplate = new();

    public void Update(double delta)
    {
        foreach (var field in fields)
            if (field.GetValue(this) is IChange changer)
                changer.Change(delta);
    }

    public void Shoot()
    {
        BulletTemplate.Clone().Shoot();
    }
}

public class Bullet : IUpdate
{
    static readonly System.Reflection.FieldInfo[] fields = typeof(Gun).GetFields();

    public Attribute_Number Speed = new();
    public Attribute_Number Damage = new();

    public void Update(double delta)
    {
        foreach (var field in fields)
            if (field.GetValue(this) is IChange changer)
                changer.Change(delta);
    }

    public void Shoot()
    {

    }

    public Bullet Clone()
    {
        return new()
        {
            Speed = Speed.Clone(),
            Damage = Damage.Clone()
        };
    }
}

public interface IUpdate
{
    void Update(double delta);
}