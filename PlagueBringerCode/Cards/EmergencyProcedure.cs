using BaseLib.Utils;
using PB.Extensions;
using PB.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class EmergencyProcedure : PlagueBringerCard
{
    public EmergencyProcedure() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(
            new DynamicVar("SpecimenCost", 2),
            new DynamicVar("Energy", 2),
            new CardsVar(2));
        WithKeywords(CardKeyword.Exhaust);
    }

    // Use the card's dedicated generated portrait.
    public override string CustomPortraitPath => "emergency_procedure.png".CardImagePath();
    public override string PortraitPath => "emergency_procedure.png".CardImagePath();
    public override string BetaPortraitPath => "beta/emergency_procedure.png".CardImagePath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var cost = DynamicVars["SpecimenCost"].IntValue;
        if (SpecimenActions.Count(Owner.Creature) < cost)
            return;

        var spent = await SpecimenActions.Spend(
            choiceContext,
            Owner.Creature,
            cost,
            this);

        if (spent < cost)
            return;

        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
