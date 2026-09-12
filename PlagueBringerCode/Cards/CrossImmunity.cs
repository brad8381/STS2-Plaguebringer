using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using PB.Powers;

namespace PB.Cards;

public sealed class CrossImmunity : PlagueBringerCard
{
    public CrossImmunity()
        : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new DynamicVar("Triggers", 1));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<CrossImmunityPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["Triggers"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Triggers"].UpgradeValueBy(1m);
    }
}
