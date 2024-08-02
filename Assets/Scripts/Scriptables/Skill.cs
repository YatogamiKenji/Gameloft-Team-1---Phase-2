using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    // Start is called before the first frame update
    public new string name;
    public int id;
    public KeyCode keyCode;
    public bool isCasted = false;
    public int dmg;
    public float castTime;
    public enum SkillState
    {
        disabled, ready, casting, active
    }

    public SkillState state = SkillState.ready;
    public void SetReady()
    {
        state = SkillState.ready;
    }
    public void SetDisabled()
    {
        state = SkillState.disabled;
    }
    public void SetCasting()
    {
        state = SkillState.casting;
    }
    public void SetActive()
    {
        state = SkillState.active;
    }
    public SkillState GetState()
    {
        return state;
    }
    public KeyCode GetKey()
    {
        return keyCode;
    }

    public virtual void UpdateAimSprite(SpriteRenderer aimSpriteRenderer) { }
    public abstract void Cast(GameObject player = null);
    public abstract void Activate(GameObject player = null);
}
