using UnityEngine;

namespace ItemSystem
{
    public class Sword : Weapon
    {
        public float ActualDamage => BaseDamage + itemData.ItemStats[EItemStat.DamageFlat].StatValue + (BaseDamage + itemData.ItemStats[EItemStat.DamageFlat].StatValue) * (itemData.ItemStats[EItemStat.DamagePercent].StatValue / 100);
        public float ActualAttackSpeed => BaseAttackSpeed + BaseAttackSpeed * (itemData.ItemStats[EItemStat.AttackSpeed].StatValue / 100);

        protected override void Start()
        {
            base.Start();
            Debug.Log(ActualDamage + " | " + ActualAttackSpeed);
            Debug.Log(gameObject);
        }
    }
}