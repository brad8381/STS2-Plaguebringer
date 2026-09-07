using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Autopsy : PlagueBringerCard, IPlagueCard
{
    public Autopsy() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("PlaguePerCard", 3),
            new DynamicVar("MaxDraw", 3),
            new DynamicVar("SpecimenPercent", 10));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var removed = await PlagueCardUtils.RemoveAllPlague(choiceContext, target, this);
        if (removed <= 0)
            return;

        var draw = Math.Min(
            DynamicVars["MaxDraw"].IntValue,
            removed / DynamicVars["PlaguePerCard"].IntValue);

        if (draw > 0)
            await CardPileCmd.Draw(choiceContext, draw, Owner);

        var specimens = (int)Math.Floor(
            removed * DynamicVars["SpecimenPercent"].IntValue / 100m);

        if (specimens > 0)
            await SpecimenActions.Gain(choiceContext, Owner.Creature, specimens, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MaxDraw"].UpgradeValueBy(1m);
    }
}
