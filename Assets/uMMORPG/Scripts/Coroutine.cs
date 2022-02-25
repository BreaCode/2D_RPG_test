using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour
{
    public void DamageOverTime(HashSet<Entity> candidates, Entity caster, int damage, float stunChance, float stunTime, float duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(candidates, caster, damage, stunChance, stunTime, duration));
    }
    public IEnumerator DamageOverTimeCoroutine(HashSet<Entity> candidates, Entity caster, int damage, float stunChance, float stunTime, float duration)
    {
        float damagedValue = 0;
        float loopDamage = (caster.combat.damage + damage) / duration;
        while (damagedValue < (caster.combat.damage + damage))
        {
            foreach (Entity candidate in candidates)
            {
                caster.combat.DealDamageAt(candidate, caster.combat.damage + damage, stunChance, stunTime);
            }
            damagedValue += loopDamage;
            yield return new WaitForSeconds(1f);
        }
    }
}
