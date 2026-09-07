using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class AirborneCultures : PlagueBringerCard, IPlagueCard
{
    public AirborneCultures() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new PowerVar<AirborneCulturesPower>("Plague", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<AirborneCulturesPower>(choiceContext, Owner.Creature, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
