using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ContagiousRupture : PlagueBringerCard, IPlagueCard
{
    public ContagiousRupture() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("Multiplier", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var combatState = Owner.Creature.CombatState;
        if (combatState == null)
            return;

        var consumed = await PlagueCardUtils.RemoveAllPlague(choiceContext, target, this);
        if (consumed <= 0)
            return;

        await DamageCmd.Attack(consumed * DynamicVars["Multiplier"].IntValue)
            .FromCard(this, play)
            .TargetingAllOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}
