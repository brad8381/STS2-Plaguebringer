using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class NoCure : PlagueBringerCard, IPlagueCard
{
    public NoCure() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new DynamicVar("BonusGrowth", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<NoCurePower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["BonusGrowth"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BonusGrowth"].UpgradeValueBy(1m);
    }
}
