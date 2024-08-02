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
        public float radius = 0.5f;
        public float radiusMultipler = 0.5f;
        public Vector3 aimPos;
        public float aimSpeed;
        public Sprite skillAim;
        public int remainingHealth;

        public override void UpdateAimSprite(SpriteRenderer aimSpriteRenderer)
        {
            aimSpriteRenderer.sprite = skillAim;
            // Set the position of the sprite to the caster position
            aimSpriteRenderer.transform.position = aimPos;

            // Scale the sprite based on the desired radius
            float scale = radius * 2.0f / aimSpriteRenderer.sprite.bounds.size.x;
            aimSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
        }

        public override void Activate(GameObject Caster)
        {
            RaycastHit2D [] hit;

            Vector3 p1 = aimPos;

            hit = Physics2D.CircleCastAll(p1, radius, Vector2.right, 1);
            if (hit.Length > 0)
            {
                Debug.Log("Hit objects: " + hit.Length);
                for (int i = 0; i < hit.Length; i++)
                {
                    Debug.Log("Hit: " + hit[i].collider.name);
                    if (hit[i].collider.TryGetComponent<SkillTestEnemy>(out SkillTestEnemy enemy))
                    {
                        enemy.ChangeHealth(dmg);
                        // collect event
                    }
                }
                
            }
            if (Caster.GetComponent<MainCharacter>() != null)
            {
                MainCharacter main = Caster.GetComponent<MainCharacter>();
                main.ChangeHealth(-(main.currentHealth - remainingHealth));
            }
        }

        public override void Cast(GameObject Caster)
        {
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
