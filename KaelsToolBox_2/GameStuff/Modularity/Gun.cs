using KaelsToolBox_2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KaelsToolBox_2.GameStuff.Modularity;

//public class Gun
//{
//    public float Damage;
//    public float ReloadSpeed;
//    public float ShootingSpeed;
//    public float Multishot;
//    public float Spread;


//    private List<IUpgrade<Gun>> upgrades = [];
//    private void ApplyUpgrades()
//    {
//        upgrades.ForEach(i => i.ApplyUpgrade(this));
//    }
//}

//public class Sword
//{
//    public float BaseDamage;
//    public float Speed;
//    public float Size;
//    public Dictionary<DamageType, float> DamageTypes = new(){ {DamageType.Slash, 1f} };

//    public Dictionary<DamageType, float> GetDamage() => WeaponHelpers.ApplyDamage(DamageTypes, BaseDamage);
//}

//public static class WeaponHelpers
//{
//    public static Dictionary<DamageType, float> ApplyDamage(Dictionary<DamageType, float> DamageTypes, float Damage)
//    {
//        // normalize damage types
//        var normalized = Normalizer.Floats([.. DamageTypes.Values]);

//        // multiply each by Damage
//        for (int i = 0; i < normalized.Length; i++) normalized[i] = normalized[i] * Damage;

//        // return dictionary of damage delt
//        return DamageTypes.Keys.ToArray().Zip(normalized).ToDictionary();
//    }
//}

//public enum DamageType
//{
//    Slash,
//    Pearce,
//    Fire,
//    Ice
//}

//public interface IUpgrade<T>
//{
//    public void ApplyUpgrade(T item);
//}

//public class DamageUpgrade : IUpgrade<Gun>, IUpgrade<Sword>
//{
//    public void ApplyUpgrade(Gun item)
//    {
//        item.Damage *= 1.1f;
//    }

//    public void ApplyUpgrade(Sword item)
//    {
//        throw new NotImplementedException();
//    }
//}
