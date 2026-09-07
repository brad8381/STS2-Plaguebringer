using BaseLib.Abstracts;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ContaminatedNeedle : PlagueBringerCard, IPlagueCard
{
    public ContaminatedNeedle() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithVars(new PowerVar<PlaguePower>("Plague", 4));
        WithKeywords(CardKeyword.Exhaust);
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Contaminated Needle",
            "Deal {Damage:diff()} damage. Apply {Plague:diff()} [gold]Plague[/gold]."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (target.IsAlive)
            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                target,
                DynamicVars["Plague"].IntValue,
                Owner.Creature,
                this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
