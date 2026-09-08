using BaseLib.Abstracts;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class HarvestTissue : PlagueBringerCard
{
    public HarvestTissue() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithVars(new DynamicVar("Specimen", 2));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Harvest Tissue",
            "Deal {Damage:diff()} damage. If this kills the enemy, gain {Specimen} Specimens."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        if (!target.IsAlive)
        {
            await SpecimenActions.Gain(
                choiceContext,
                Owner.Creature,
                DynamicVars["Specimen"].IntValue,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
