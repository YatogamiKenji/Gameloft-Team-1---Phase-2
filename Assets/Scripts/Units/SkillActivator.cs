using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActivator : MonoBehaviour
{
    // Start is called before the first frame update
    public Skill crnSkill;
    public SpriteRenderer aimSpriteRenderer;

    public void SetSkill (Skill skill)
    {
        if (crnSkill != null)
        {
            if (crnSkill.GetState() == Skill.SkillState.casting || crnSkill.GetState() == Skill.SkillState.active)
            {
                return;
            }
        }
        crnSkill = skill;
    }    
    private void Update()
    {
         
        if (crnSkill != null)
        {
            switch (crnSkill.GetState())
            {
                case Skill.SkillState.ready:
                    aimSpriteRenderer.gameObject.SetActive(false);
                    if (Input.GetKey(crnSkill.keyCode))
                    {
                            crnSkill.Cast(gameObject);
                            crnSkill.SetCasting();
                        aimSpriteRenderer.gameObject.SetActive(true);
                    }
                    break;
                case Skill.SkillState.casting:
                        crnSkill.Cast(gameObject);
                    if (Input.GetKeyUp(crnSkill.keyCode))
                    {
                        if (!crnSkill.isCasted)
                                crnSkill.SetReady();
                        else crnSkill.SetActive();
                    }
                    break;
                case Skill.SkillState.active:
                        crnSkill.Activate(gameObject);
                    break;
                case Skill.SkillState.disabled:
                    aimSpriteRenderer.gameObject.SetActive(false);
                    break;
            }
                crnSkill.UpdateAimSprite(aimSpriteRenderer);

        }


    }

}
