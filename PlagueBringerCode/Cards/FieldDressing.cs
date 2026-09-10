using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class FieldDressing : PlagueBringerCard
{
    public FieldDressing() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(8);
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Field Dressing",
            "Gain {Block:diff()} [gold]Block[/gold]."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
