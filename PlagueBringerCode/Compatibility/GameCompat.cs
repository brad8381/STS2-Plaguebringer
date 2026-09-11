using System.Reflection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Compatibility;

public static class GameCompat
{
    private static readonly MethodInfo? BetaFromCard = typeof(AttackCommand)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .FirstOrDefault(method =>
        {
            if (method.Name != "FromCard")
                return false;

            var p = method.GetParameters();

            return p.Length == 2 &&
                   p[0].ParameterType == typeof(CardModel) &&
                   p[1].ParameterType == typeof(CardPlay);
        });

    private static readonly MethodInfo? MainFromCard = typeof(AttackCommand)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .FirstOrDefault(method =>
        {
            if (method.Name != "FromCard")
                return false;

            var p = method.GetParameters();

            return p.Length == 1 &&
                   p[0].ParameterType == typeof(CardModel);
        });

    public static bool IsBetaApi => BetaFromCard != null;

    public static AttackCommand FromCard(
        AttackCommand command,
        CardModel card,
        CardPlay play)
    {
        if (BetaFromCard != null)
        {
            var result = BetaFromCard.Invoke(
                command,
                new object?[] { card, play });

            return (AttackCommand)(result ?? command);
        }

        if (MainFromCard != null)
        {
            var result = MainFromCard.Invoke(
                command,
                new object?[] { card });

            return (AttackCommand)(result ?? command);
        }

        throw new MissingMethodException(
            typeof(AttackCommand).FullName,
            "FromCard");
    }

    public static async Task Damage(
        PlayerChoiceContext choiceContext,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? card,
        CardPlay? play)
    {
        var methods = typeof(CreatureCmd)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.Name == nameof(CreatureCmd.Damage))
            .ToArray();

        var betaMethod = methods.FirstOrDefault(method =>
        {
            var p = method.GetParameters();

            return p.Length == 6 &&
                   p[1].ParameterType == typeof(Creature) &&
                   p[2].ParameterType == typeof(decimal) &&
                   p[3].ParameterType == typeof(ValueProp) &&
                   p[4].ParameterType == typeof(CardModel) &&
                   p[5].ParameterType == typeof(CardPlay);
        });

        object? result;

        if (betaMethod != null)
        {
            result = betaMethod.Invoke(
                null,
                new object?[]
                {
                    choiceContext,
                    target,
                    amount,
                    props,
                    card,
                    play
                });
        }
        else
        {
            var mainMethod = methods.FirstOrDefault(method =>
            {
                var p = method.GetParameters();

                return p.Length == 6 &&
                       p[1].ParameterType == typeof(Creature) &&
                       p[2].ParameterType == typeof(decimal) &&
                       p[3].ParameterType == typeof(ValueProp) &&
                       p[4].ParameterType == typeof(Creature) &&
                       p[5].ParameterType == typeof(CardModel);
            });

            if (mainMethod == null)
            {
                throw new MissingMethodException(
                    typeof(CreatureCmd).FullName,
                    nameof(CreatureCmd.Damage));
            }

            result = mainMethod.Invoke(
                null,
                new object?[]
                {
                    choiceContext,
                    target,
                    amount,
                    props,
                    dealer,
                    card
                });
        }

        if (result is Task task)
        {
            await task;
            return;
        }

        throw new InvalidOperationException(
            "CreatureCmd.Damage did not return a Task.");
    }
}