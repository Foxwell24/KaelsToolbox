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

            AttributeChanged?.Invoke(this, new AttributeChangedArgs(old, Value));
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

            ChangeOverTimeChanged?.Invoke(this, new AttributeChangedArgs(old, ChangeOverTime));
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

public class Attribute_Damage : Attribute_Number
{
    public Type DamageType = Type.None;
    public enum Type
    {
        None,
        Slash,
        Peirce,
        Fire,
        Frost
    }

    public new Attribute_Damage Clone()
    {
        return new()
        {
            Value = Value,
            ChangeOverTime = ChangeOverTime,
            ChangePerSecond = ChangePerSecond,
            DamageType = DamageType,
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

public class Bullet : WorldSpaceObject, IUpdate
{
    static readonly System.Reflection.FieldInfo[] fields = typeof(Gun).GetFields();

    public Attribute_Number Speed = new();
    public Attribute_Damage Damage = new();

    public void Update(double delta)
    {
        foreach (var field in fields)
            if (field.GetValue(this) is IChange changer)
                changer.Change(delta);
    }

    
    public void Shoot()
    {

    }

    public void Collide<T>(T other) where T : IDamageable
    {
        other.ApplyDamage(Damage.Value, Damage.DamageType);
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

public interface IDamageable
{
    double Health { get; }

    void ApplyDamage(double amount, Attribute_Damage.Type type);
}

public interface IUpdate
{
    void Update(double delta);
}

// ----------------------------- //

public abstract class WorldSpaceObject
{
    protected Vector2 position = Vector2.Zero;
    public Vector2 Position => position;

    protected float size = 1f;
    public float Size => size;
}

public class Entity : WorldSpaceObject, IDamageable
{
    protected double health = 1;
    public double Health => health;

    protected Vector2 direction = Vector2.Zero;
    public Vector2 Direction => direction;


    protected List<Attribute_Damage.Type> vulnerabilities = [];
    protected List<Attribute_Damage.Type> resistances = [];

    public void ApplyDamage(double amount, Attribute_Damage.Type type)
    {
        if (vulnerabilities.Contains(type)) amount *= 2;
        if (resistances.Contains(type)) amount /= 2;

        health -= amount;
    }
}

public class Slime : Entity
{
    public Slime()
    {
        vulnerabilities.Add(Attribute_Damage.Type.Slash);
        resistances.Add(Attribute_Damage.Type.Peirce);

        health = 10;
        position.Y = 10;
    }
}