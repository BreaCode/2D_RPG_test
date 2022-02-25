using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Dash", order = 999)]
public class DashSkill : ScriptableSkill
{
    public override bool CheckTarget(Entity caster)
    {
        caster.target = caster;
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = (Vector2)caster.transform.position;
        return true;
    }

    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        for (int i = 0; i < castRange.Get(skillLevel); i++)
        {
            var destination = caster.transform.position + (Vector3)direction;
            caster.movement.Warp(destination);
        }
    }
}
