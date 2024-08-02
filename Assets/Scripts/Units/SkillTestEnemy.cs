using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class SkillTestEnemy : BaseCharacter
    {
        public override void Attack()
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            currentHealth = maxHealth;
            //changeHealthSO.RaiseEvent(currentHealth);
        }

        public override void ChangeHealth(float health)
        {
            currentHealth -= health;
            Debug.Log(this.name + ": " + health);
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }
    }
}
