using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class PlaguePower : PlagueBringerPower
{
    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/plague_power.svg";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/plague_power.svg";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side || !participants.Contains(Owner) || !Owner.IsAlive || Amount <= 0)
            return;

        var target = Owner;
        var stacks = Amount;

        MainFile.Logger.Debug($"Plague tick: target={target}, stacks={stacks}");
        Flash();

        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            target,
            stacks,
            ValueProp.Unpowered | ValueProp.Unblockable,
            null,
            null);

        if (target.IsAlive && target.GetPower<PlaguePower>() == this)
        {
            var nextAmount = Math.Ceiling(stacks * 1.15m);
            var increase = nextAmount - stacks;

            MainFile.Logger.Debug($"Plague growth: {stacks} -> {nextAmount} (+{increase})");

            if (increase > 0)
                await PowerCmd.ModifyAmount(
                    new ThrowingPlayerChoiceContext(),
                    this,
                    increase,
                    null,
                    null);
        }
        else
        {
            // Match the game's Poison handling so a lethal damage-over-time tick
            // has time to finish the creature death flow before combat continues.
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }
}
