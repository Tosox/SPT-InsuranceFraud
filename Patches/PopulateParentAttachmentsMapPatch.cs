using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using System.Reflection;
using Tosox.InsuranceFraud.Helpers;

namespace Tosox.InsuranceFraud.Patches
{
    public class PopulateParentAttachmentsMapPatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InsuranceController), "PopulateParentAttachmentsMap");
        }

        [PatchPostfix]
        public static void PatchPostfix(Dictionary<MongoId, List<Item>> __result)
        {
            // Exclude dropped attachments so they aren't rolled for and don't count
            // towards the number of attachments taken from their parent
            foreach (var (parentId, attachments) in __result.ToList())
            {
                attachments.RemoveAll(DroppedItemHelper.IsDropped);
                if (attachments.Count == 0)
                {
                    __result.Remove(parentId);
                }
            }
        }
    }
}
