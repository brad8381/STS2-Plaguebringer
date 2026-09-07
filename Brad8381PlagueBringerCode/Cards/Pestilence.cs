using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Pestilence : PlagueBringerCard, IPlagueCard
{
    public Pestilence() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;

        foreach (var enemy in CombatState.HittableEnemies.Where(enemy => enemy.IsAlive).ToArray())
        {
            var plague = enemy.GetPower<PlaguePower>();
            if (plague == null || plague.Amount <= 0) continue;

            await PowerCmd.ModifyAmount(
                choiceContext,
                plague,
                plague.Amount,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
