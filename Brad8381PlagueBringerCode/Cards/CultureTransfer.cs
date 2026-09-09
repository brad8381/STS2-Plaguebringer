using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class CultureTransfer : PlagueBringerCard, IPlagueCard
{
    public CultureTransfer() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("SpecimenCost", 1),
            new PowerVar<PlaguePower>("Plague", 7));
    }

    // Reuse the old Miasma portrait rather than adding new art.
    public override string CustomPortraitPath => "miasma.png".CardImagePath();
    public override string PortraitPath => "miasma.png".CardImagePath();
    public override string BetaPortraitPath => "beta/miasma.png".CardImagePath();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

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

        await PowerCmd.Apply<PlaguePower>(
            choiceContext,
            target,
            DynamicVars["Plague"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(3m);
    }
}
