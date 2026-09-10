using BaseLib.Abstracts;
using BaseLib.Utils;
using PB.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class BoneSaw : PlagueBringerCard
{
    public BoneSaw() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8);
        WithVars(new DynamicVar("SpecimenCost", 1));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Bone Saw",
            "Deal {Damage:diff()} damage. Spend {SpecimenCost} Specimen to deal {Damage:diff()} damage again."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive)
            return;

        var spent = await SpecimenActions.Spend(
            choiceContext,
            Owner.Creature,
            DynamicVars["SpecimenCost"].IntValue,
            this);
        if (spent > 0 && target.IsAlive)
            await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
