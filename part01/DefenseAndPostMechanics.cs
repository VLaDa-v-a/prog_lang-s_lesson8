using System;

namespace part01
{
    [GameAttribute]
    public class DefenseAndPostMechanics
    {
        private static readonly Func<int, int> HalveDamage = IlMathFactory.CreateHalveInt();
        private static readonly Func<int, int> ReduceByTen = IlMathFactory.CreateSubtractConst(10);

        [CombatSkill("ShieldWall", TriggerType.OnDefense, 50)]
        public void ReduceIncomingDamage(BattleContext ctx)
        {
            ctx.DamageDealt = HalveDamage(ctx.DamageDealt);
            Console.WriteLine($"[System] Shield Wall: incoming damage reduced to {ctx.DamageDealt}.");
        }

        [CombatSkill("Aftermath", TriggerType.PostBattle, 1)]
        public void ApplyPostBattlePenalty(BattleContext ctx)
        {
            ctx.Attacker.Hp = ReduceByTen(ctx.Attacker.Hp);
            ctx.Defender.Hp = ReduceByTen(ctx.Defender.Hp);
            Console.WriteLine($"[System] Aftermath: both units lose 10 HP.");
        }
    }
}

