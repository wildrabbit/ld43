using System.Collections.Generic;
using UnityEngine;

public static class BattleUtils
{
    public static void SolveAttack(Entity attacker, Entity defender, out ResultData results)
    {
        results = new ResultData();
        results.DefenderName = defender.Name;
        results.DefenderHP = defender.HP;
        // TODO: Can the attacker actually do anything?

        // 1. Solve chance to hit
        float attackerHitChance = attacker.HitChance;
        if(Random.value > attackerHitChance)
        {
            results.AttackerFlopped = true;
            return;
        }

        // 2. Solve defender's deflection
        float defenderAvoidChance = defender.AvoidChance;
        if (Random.value <= defenderAvoidChance)
        {
            results.DefenderAvoided = true;
            return;
        }

        // 3. Crit check?
        float attackBonus = 0.0f;

        float attackerCritChance = attacker.CritChance;        
        if(Random.value <= attackerCritChance)
        {
            attackBonus = attacker.CritDamageBonus;
            results.Critical = true;
        }

        // 4. Attack
        float attack = Random.Range(attacker.MinAttack, attacker.MaxAttack);
        attack *= (1 + attackBonus);
        float defense = defender.Defense;

        float damageInflicted = (attack * attack) / (attack + defense);
        results.AttackerDmgInflicted = damageInflicted;
        results.DefenderDmgTaken = Mathf.Min(damageInflicted, defender.HP);
        results.DefenderDefeated = defender.TakeHit(damageInflicted);
        results.DefenderHP = defender.HP;
    }
}