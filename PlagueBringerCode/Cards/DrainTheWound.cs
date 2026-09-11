using PB.Compatibility;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class DrainTheWound : PlagueBringerCard, IPlagueCard
{
    public DrainTheWound() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("Multiplier", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var consumed = await PlagueCardUtils.RemoveAllPlague(choiceContext, target, this);
        if (consumed <= 0)
            return;

        var attack = GameCompat.FromCard(
            DamageCmd.Attack(
                consumed * DynamicVars["Multiplier"].IntValue),
            this,
            play);

        await attack
            .Targeting(target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}