using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ReclaimSupplies : PlagueBringerCard
{
    public ReclaimSupplies() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(
            new DynamicVar("BaseBlock", 4),
            new DynamicVar("BlockPerSpecimen", 4));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Reclaim Supplies",
            "Gain {BaseBlock:diff()} [gold]Block[/gold]. Spend ALL Specimens. Gain {BlockPerSpecimen:diff()} additional Block for each Specimen spent."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var available = SpecimenActions.Count(Owner.Creature);
        var spent = await SpecimenActions.Spend(
            choiceContext,
            Owner.Creature,
            available,
            this);

        var block = DynamicVars["BaseBlock"].IntValue +
                    spent * DynamicVars["BlockPerSpecimen"].IntValue;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Move,
            play,
            false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BaseBlock"].UpgradeValueBy(2m);
        DynamicVars["BlockPerSpecimen"].UpgradeValueBy(1m);
    }
}
