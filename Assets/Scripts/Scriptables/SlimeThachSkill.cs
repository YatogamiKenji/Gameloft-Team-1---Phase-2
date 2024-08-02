using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu]
    public class SlimeThachSkill : Skill
    {
        //Press X to cast, IJKL to move the aimer
        public float radius = 0.5f;
        public float radiusMultipler = 0.5f;
        public Vector3 aimPos;
        public float aimSpeed;
        public Sprite skillAim;
        public int remainingHealth;

        //Update the SkillAimVisualize
        public override void UpdateAimSprite(AimRenderer aimRenderer)
        {
            aimRenderer.AimSpriteRenderer.enabled = true;
            // Set the position of the sprite to the caster position
            aimRenderer.AimSpriteRenderer.sprite = skillAim;
            aimRenderer.AimSpriteRenderer.transform.position = aimPos;

            // Scale the sprite based on the desired radius
            float scale = radius * 2.0f / aimRenderer.AimSpriteRenderer.sprite.bounds.size.x;
            aimRenderer.AimSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
        }

        public override void Activate(GameObject Caster)
        {
            //Use CircleCast to know if any object is within the skill range
            RaycastHit2D [] hit;

            Vector3 p1 = aimPos;

            hit = Physics2D.CircleCastAll(p1, radius, Vector2.right, 1);
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

            //Change Character Health
            if (Caster.GetComponent<MainCharacter>() != null)
            {
                MainCharacter main = Caster.GetComponent<MainCharacter>();
                main.ChangeHealth(-(main.currentHealth - remainingHealth));
            }
            SetDisabled(); //Disable the skill on complete
        }

        public override void Cast(GameObject Caster)
        {
            //Moving the aim Circle
            isCasted = true;
            
            if (state == SkillState.ready)
            {
                aimPos = Caster.transform.position;
                if (Caster.GetComponent<MainCharacter>() != null)
                {
                    MainCharacter main = Caster.GetComponent<MainCharacter>();
                    radius = (main.currentHealth - remainingHealth) * radiusMultipler;
                }
            }

            if (Input.GetKey(KeyCode.I))
            {
                aimPos.y += aimSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                aimPos.y -= aimSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.J))
            {
                aimPos.x -= aimSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                aimPos.x += aimSpeed * Time.deltaTime;
            }

            
        }

    }
}
