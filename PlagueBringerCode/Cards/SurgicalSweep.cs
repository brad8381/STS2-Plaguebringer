using PB.Compatibility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Cards;

public sealed class SurgicalSweep : PlagueBringerCard, IPlagueCard
{
    public SurgicalSweep() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(4);
        WithVars(new DynamicVar("InfectedBonusDamage", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        var infected = combatState
            .GetOpponentsOf(Owner.Creature)
            .Where(enemy => enemy.IsAlive && PlagueCardUtils.GetPlague(enemy) > 0)
            .ToArray();

        var attack = GameCompat.FromCard(
            DamageCmd.Attack(DynamicVars.Damage.IntValue),
            this,
            play);

        await attack
            .TargetingAllOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var bonus = DynamicVars["InfectedBonusDamage"].IntValue;

        foreach (var enemy in infected.Where(enemy => enemy.IsAlive))
        {
            await GameCompat.Damage(
                choiceContext,
                enemy,
                bonus,
                ValueProp.Unpowered,
                Owner.Creature,
                this,
                play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["InfectedBonusDamage"].UpgradeValueBy(1m);
    }
}