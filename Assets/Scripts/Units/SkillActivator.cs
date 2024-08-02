using Assets.Scripts.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActivator : MonoBehaviour
{
    // Start is called before the first frame update
    public Skill crnSkill;
    public AimRenderer aimRenderer;
    private void Awake()
    {
        aimRenderer = GetComponent<AimRenderer>();
    }
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
                    if (Input.GetKey(crnSkill.keyCode))
                    {
                            crnSkill.Cast(gameObject);
                            crnSkill.SetCasting();
                    }
                    break;
                case Skill.SkillState.casting:
                    crnSkill.Cast(gameObject);
                    if (Input.GetKeyUp(crnSkill.keyCode))
                    {
                        if (!crnSkill.isCasted)
                        {
                            crnSkill.SetReady();
                            aimRenderer.DisableAll();
                            return;
                        }    
                                
                        else crnSkill.SetActive();
                    }
                    crnSkill.UpdateAimSprite(aimRenderer);
                    break;
                case Skill.SkillState.active:
                    crnSkill.Activate(gameObject);
                    crnSkill.UpdateAimSprite(aimRenderer);
                    break;
                case Skill.SkillState.disabled:
                    aimRenderer.DisableAll();
                    return;
            }
            

        }


    }

}
