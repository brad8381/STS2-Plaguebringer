using BaseLib.Abstracts;
using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PestilentBlow : PlagueBringerCard, IPlagueCard
{
    private const decimal MultiplierFraction = 0.5m;

    public PestilentBlow() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithVars(new DynamicVar("MultiplierWhole", 1m));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Pestilent Blow",
            "Deal {Damage:diff()} damage. Multiply this enemy's current [gold]Plague[/gold] by {MultiplierWhole:diff()}.5x."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive)
            return;

        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0)
            return;

        var multiplier = DynamicVars["MultiplierWhole"].BaseValue + MultiplierFraction;
        var newAmount = Math.Ceiling(plague.Amount * multiplier);
        var increase = newAmount - plague.Amount;

        if (increase > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, increase, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["MultiplierWhole"].UpgradeValueBy(1m);
    }
}
