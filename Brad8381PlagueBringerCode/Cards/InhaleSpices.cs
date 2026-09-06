using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Extensions;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class InhaleSpices : PlagueBringerCard, IPlagueCard
{
    public override string CustomPortraitPath => "card.png".BigCardImagePath();
    public override string PortraitPath => "card.png".CardImagePath();
    public override string BetaPortraitPath => "card.png".CardImagePath();

    public InhaleSpices() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;

        var totalPlague = CombatState.HittableEnemies
            .Where(enemy => enemy.IsAlive)
            .Sum(enemy => enemy.GetPower<PlaguePower>()?.Amount ?? 0m);

        var blockChoice = CombatState.CreateCard<InhaleSpicesBlockChoice>(Owner);
        var healChoice = CombatState.CreateCard<InhaleSpicesHealChoice>(Owner);
        var selected = await CardSelectCmd.FromChooseACardScreen(
            choiceContext,
            new CardModel[] { blockChoice, healChoice },
            Owner,
            canSkip: false);

        if (selected is InhaleSpicesBlockChoice)
        {
            if (totalPlague > 0)
                await CreatureCmd.GainBlock(Owner.Creature, totalPlague, ValueProp.Move, play);
            return;
        }

        if (selected is InhaleSpicesHealChoice)
        {
            var healAmount = Math.Ceiling(totalPlague * 0.10m);
            if (healAmount > 0)
                await CreatureCmd.Heal(Owner.Creature, healAmount);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
