using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class BlackTonic : PlagueBringerCard, IPlagueCard
{
    public BlackTonic() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(
            new PowerVar<PlaguePower>("SelfPlague", 1),
            new DynamicVar("Energy", 3));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<PlaguePower>(choiceContext, Owner.Creature, DynamicVars["SelfPlague"].IntValue, Owner.Creature, this);
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
        await PowerCmd.Apply<RetainEnergyNextTurnPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Energy"].UpgradeValueBy(1m);
    }
}
