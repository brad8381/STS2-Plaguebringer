using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class BoneSaw : PlagueBringerCard
{
    public BoneSaw() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive || SpecimenActions.Count(Owner.Creature) <= 0) return;

        await SpecimenActions.Spend(choiceContext, Owner.Creature, 1, this);
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
