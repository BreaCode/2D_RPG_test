// Group heal that heals all entities of same type in cast range
// => player heals players in cast range
// => monster heals monsters in cast range
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(menuName = "uMMORPG Skill/Area Damage", order = 999)]
public class AreaDamageSkill : DamageSkill
{
    // OverlapCircleNonAlloc array to avoid allocations.
    // -> static so we don't create one per skill
    // -> this is worth it because skills are casted a lot!
    // -> should be big enough to work in just about all cases
    static Collider2D[] hitsBuffer = new Collider2D[10000];
    private Coroutine _coroutine;

    public override bool CheckTarget(Entity caster)
    {
        // no target necessary, but still set to self so that LookAt(target)
        // doesn't cause the player to look at a target that doesn't even matter
        caster.target = caster;
        _coroutine = caster.gameObject.AddComponent<Coroutine>();
        return true;
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        // can cast anywhere
        destination = caster.transform.position;
        return true;
    }

    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        // candidates hashset to be 100% sure that we don't apply an area skill
        // to a candidate twice. this could happen if the candidate has more
        // than one collider (which it often has).
        HashSet<Entity> candidates = new HashSet<Entity>();

        // find all entities of same type in castRange around the caster
        int hits = Physics2D.OverlapCircleNonAlloc(caster.transform.position, castRange.Get(skillLevel), hitsBuffer);
        for (int i = 0; i < hits; ++i)
        {
            Collider2D co = hitsBuffer[i];
            Entity candidate = co.GetComponentInParent<Entity>();
            if (candidate != null &&
                candidate.health.current > 0 && // can't attack dead people
                caster.CanAttack(candidate)) // check attack possibility
            {
                candidates.Add(candidate);
            }
        }

        _coroutine.DamageOverTime(candidates, caster, damage.Get(skillLevel), stunChance.Get(skillLevel), stunTime.Get(skillLevel), 4f);
    }
}
