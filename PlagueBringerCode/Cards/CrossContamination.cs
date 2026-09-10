using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class CrossContamination : PlagueBringerCard, IPlagueCard
{
    public CrossContamination() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("Percent", 50));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null || play.Target is not { IsAlive: true } target) return;
        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0) return;

        var spread = Math.Ceiling(plague.Amount * DynamicVars["Percent"].BaseValue / 100m);
        if (spread <= 0) return;

        foreach (var enemy in CombatState.HittableEnemies.Where(enemy => enemy.IsAlive && enemy != target).ToArray())
            await PB.Mechanics.PlagueActions.Apply(choiceContext, enemy, (int)spread, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Percent"].UpgradeValueBy(25m);
    }
}

