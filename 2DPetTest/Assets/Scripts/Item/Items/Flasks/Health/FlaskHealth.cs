using System.Collections;
using System.Collections.Generic;
using Items.Weapons;
using UnityEngine;

namespace Items.Flasks
{
    public class FlaskHealth : Flask
    {
        public override string GetDescription()
        {
            return "Название: " + ItemData.NameItem + ". Восстановление здоровья: " + Value;
        }
    }
}
