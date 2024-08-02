using Assets.Scripts.Units;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu]

    //Press C to cast
    public class SummonSkill : Skill
    {
        public float activeDuration;
        private List<Vector3> linePositions = new List<Vector3>();
        public override void UpdateAimSprite(AimRenderer aimRenderer)
        {
            aimRenderer.AimLineRenderer.enabled = true;
            aimRenderer.AimLineRenderer.positionCount = linePositions.Count;

            for (int i = 0; i < linePositions.Count; i++)
            {
                aimRenderer.AimLineRenderer.SetPosition(i, linePositions[i]);
            }
        }

        private void AddLinePosition(Vector3 start, Vector3 end)
        {
            linePositions.Add(start);
            linePositions.Add(end);
        }

        public override void Activate(GameObject Caster)
        {
            if (Caster.GetComponent<MainCharacter>() != null)
            {
                MainCharacter main = Caster.GetComponent<MainCharacter>();
                main.ChangeHealth(main.maxHealth - main.currentHealth);
            }

            //Find each bullet and try a Linecast
            Bullet []bullets = FindObjectsOfType<Bullet>();
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null)
                {
                    if (bullet.IsCollectable)
                    {
                        // Perform the Linecast
                        RaycastHit2D []hit = Physics2D.LinecastAll(bullet.transform.position, Caster.transform.position);

                        // Check if the line hit something
                        if (hit.Length > 0)
                        {
                            Debug.Log("Hit objects: " + hit.Length);
                            for (int i = 0; i < hit.Length; i++)
                            {
                                Debug.Log("Hit: " + hit[i].collider.name);
                                //Change enemy's health
                                if (hit[i].collider.TryGetComponent<SkillTestEnemy>(out SkillTestEnemy enemy))
                                {
                                    enemy.ChangeHealth(dmg);
                                }
                            }
                        }
                        bullet.GetComponent<Collider2D>().isTrigger = false; //Turn of the collision of the bullet, as we use raycast instead 
                        //Move the bullet back to Caster (doTween)
                        bullet.transform.DOMove(Caster.transform.position, activeDuration)
                            .SetEase(Ease.Linear) // Set movement to linear (no acceleration/deceleration)
                            .OnComplete(() => OnMovementComplete(bullet));
                    }
                    
                }    
            }
        }

        void OnMovementComplete(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            linePositions.Clear();
            SetDisabled();
        }    
        public override void Cast(GameObject Caster)
        {
            if (state == SkillState.ready)
            {
                countdownTime = castTime;
                Bullet[] bullets = FindObjectsOfType<Bullet>();
                foreach (Bullet bullet in bullets)
                {
                    if (bullet != null)
                    {
                        if (bullet.IsCollectable)
                        {
                            
                            // Visualize the line in the Scene view
                            AddLinePosition(bullet.transform.position, Caster.transform.position);
                        }
                    }
                }
                isCasted = false;
            }
            if (countdownTime <= 0)
            {
                isCasted = true;
                this.SetActive();
                countdownTime = castTime;
            }
            else
            {
                countdownTime -= Time.deltaTime;
            }    
        }

        public float countdownTime; // The time in seconds for the countdown
        //public float maxCountdownTime = 3f;


    }
}
