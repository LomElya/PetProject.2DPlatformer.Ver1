using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface;

namespace Items.Weapons
{
    public class Sword : Weapon
    {
        public override string GetDescription()
        {
            return "Тип: " + ItemData.TypeItem + "\n" + ItemData.Description + "\n" + "Название: " + ItemData.NameItem + "\nУрон: " + Damage;
        }
    }
}

