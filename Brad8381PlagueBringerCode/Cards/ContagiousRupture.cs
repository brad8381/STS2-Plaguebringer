using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ContagiousRupture : PlagueBringerCard, IPlagueCard
{
    public ContagiousRupture() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("MultiplierTenths", 15));
        WithKeywords(CardKeyword.Exhaust);
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Contagious Rupture",
            "Remove ALL [gold]Plague[/gold] from the target. Deal {IfUpgraded:show:3|1.5} times that much damage to ALL enemies."
        );

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

        var multiplier = DynamicVars["MultiplierTenths"].IntValue / 10m;
        var damage = Math.Ceiling(consumed * multiplier);

        await DamageCmd.Attack(damage)
            .FromCard(this, play)
            .TargetingAllOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MultiplierTenths"].UpgradeValueBy(15m);
    }
}
