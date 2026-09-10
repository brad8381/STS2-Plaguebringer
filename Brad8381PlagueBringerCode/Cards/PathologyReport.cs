using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PathologyReport : PlagueBringerCard, IPlagueCard
{
    public PathologyReport() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithVars(
            new DynamicVar("SpecimenCap", 3),
            new DynamicVar("PlaguePerSpecimen", 2));
        WithKeywords(CardKeyword.Exhaust);
    }

    // Use the card's dedicated generated portrait.
    public override string CustomPortraitPath => "pathology_report.png".CardImagePath();
    public override string PortraitPath => "pathology_report.png".CardImagePath();
    public override string BetaPortraitPath => "beta/pathology_report.png".CardImagePath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var available = Math.Max(0, SpecimenActions.Count(Owner.Creature));
        var toSpend = Math.Min(DynamicVars["SpecimenCap"].IntValue, available);
        if (toSpend <= 0)
            return;

        var spent = await SpecimenActions.Spend(
            choiceContext,
            Owner.Creature,
            toSpend,
            this);

        if (spent <= 0)
            return;

        await CardPileCmd.Draw(choiceContext, spent, Owner);

        var combatState = CombatState;
        if (combatState == null)
            return;

        var plague = spent * DynamicVars["PlaguePerSpecimen"].IntValue;
        foreach (var enemy in combatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray())
        {
            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                enemy,
                plague,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
