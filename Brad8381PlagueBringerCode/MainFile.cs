using System.Reflection;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Potions;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Relics;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Brad8381PlagueBringer";
    public const string ResPath = $"res://{ModId}";
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    private static Harmony? _harmony;
    private static bool _contentRegistered;

    public static void Initialize()
    {
        Logger.Info($"Loading Plaguebringer {typeof(MainFile).Assembly.GetName().Version}; BaseLib {typeof(BaseLib.Abstracts.CustomCardModel).Assembly.GetName().Version}");

        RegisterContentModels();

        _harmony ??= new Harmony(ModId);
        _harmony.PatchAll(typeof(MainFile).Assembly);
    }

    /// <summary>
    /// Explicitly construct each Plaguebringer card, relic and potion model once so
    /// BaseLib can register its type with the correct custom pool before ModelDb is
    /// finalised. BaseLib de-duplicates registrations, so this is safe even if another
    /// loader path has already constructed a model.
    /// </summary>
    private static void RegisterContentModels()
    {
        if (_contentRegistered)
            return;

        _contentRegistered = true;

        var assembly = typeof(MainFile).Assembly;
        var modelTypes = assembly.GetTypes()
            .Where(type =>
                !type.IsAbstract &&
                !type.IsGenericTypeDefinition &&
                type.GetConstructor(Type.EmptyTypes) != null &&
                (typeof(PlagueBringerCard).IsAssignableFrom(type) ||
                 typeof(PlagueBringerRelic).IsAssignableFrom(type) ||
                 typeof(PlagueBringerPotion).IsAssignableFrom(type)))
            .OrderBy(type => type.FullName)
            .ToArray();

        var registered = 0;

        foreach (var type in modelTypes)
        {
            try
            {
                Activator.CreateInstance(type);
                registered++;
                Logger.Info($"Registered content model: {type.Name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to register content model {type.FullName}: {ex}");
            }
        }

        Logger.Info($"Plaguebringer content bootstrap constructed {registered}/{modelTypes.Length} card/relic/potion models.");
    }
}
