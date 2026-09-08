using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ClinicalReserve : PlagueBringerCard
{
    public ClinicalReserve() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(
            new DynamicVar("BaseBlock", 5),
            new DynamicVar("BonusBlock", 7),
            new DynamicVar("SpecimenCost", 1));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Clinical Reserve",
            "Gain {BaseBlock:diff()} [gold]Block[/gold]. Spend {SpecimenCost} Specimen to gain {BonusBlock:diff()} additional Block."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var block = DynamicVars["BaseBlock"].IntValue;

        var spent = await SpecimenActions.Spend(
            choiceContext,
            Owner.Creature,
            DynamicVars["SpecimenCost"].IntValue,
            this);

        if (spent > 0)
            block += DynamicVars["BonusBlock"].IntValue;

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
        DynamicVars["BonusBlock"].UpgradeValueBy(2m);
    }
}
