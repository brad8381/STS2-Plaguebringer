using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class ContagiousCut : PlagueBringerCard, IPlagueCard
{
    public ContagiousCut() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(7);
        WithVars(new PowerVar<PlaguePower>("Plague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, play)
            .TargetingAllOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        foreach (var enemy in combatState
                     .GetOpponentsOf(Owner.Creature)
                     .Where(enemy => enemy.IsAlive)
                     .ToArray())
        {
            await PB.Mechanics.PlagueActions.Apply(
                choiceContext,
                enemy,
                DynamicVars["Plague"].IntValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
