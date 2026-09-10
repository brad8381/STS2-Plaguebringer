using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class TerminalDiagnosis : PlagueBringerCard, IPlagueCard
{
    public TerminalDiagnosis() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVars(
            new PowerVar<PlaguePower>("Plague", 10),
            new DynamicVar("Percent", 50));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        await PowerCmd.Apply<PlaguePower>(choiceContext, target, DynamicVars["Plague"].IntValue, Owner.Creature, this);

        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0) return;

        var increase = Math.Ceiling(plague.Amount * DynamicVars["Percent"].BaseValue / 100m);
        if (increase > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, increase, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
