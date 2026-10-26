using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;

using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace Tosox.InsuranceFraud
{
    [Injectable(TypePriority = OnLoadOrder.PreSptModLoader)]
    public class InsuranceFraud(
        ISptLogger<InsuranceFraud> logger
    ) : IOnLoad
    {
        public Task OnLoad()
        {
            new Patches.LocationLifecycleServicePatch().Enable();
            new Patches.RollForDeletePatch().Enable();
            new Patches.PopulateParentAttachmentsMapPatch().Enable();
            new Patches.SendMailPatch().Enable();

            logger.Info($"[{ModMetadata.ModName}] Insured items dropped during a raid will now always be returned");
            return Task.CompletedTask;
        }
    }

    public record ModMetadata : AbstractModMetadata
    {
        internal const string ModName = "Insurance Fraud";
        internal const string ModVersion = "1.0.0";
        internal const string ModAuthor = "Tosox";
        internal const string ModSource = "https://github.com/Tosox/SPT-InsuranceFraud";

        public override string ModGuid { get; init; } = "de.tosox.insurancefraud";
        public override string Name { get; init; } = ModName;
        public override string Author { get; init; } = ModAuthor;
        public override List<string>? Contributors { get; init; } = ["PenguinGreenTea", "DragonX86"];
        public override Version Version { get; init; } = new(ModVersion);
        public override Range SptVersion { get; init; } = new("~4.0.2");
        public override List<string>? Incompatibilities { get; init; }
        public override Dictionary<string, Range>? ModDependencies { get; init; }
        public override string? Url { get; init; } = ModSource;
        public override bool? IsBundleMod { get; init; }
        public override string License { get; init; } = "MIT";
    }
}
