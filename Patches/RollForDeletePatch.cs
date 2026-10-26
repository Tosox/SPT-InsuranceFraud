using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using System.Reflection;
using Tosox.InsuranceFraud.Helpers;

namespace Tosox.InsuranceFraud.Patches
{
    public class RollForDeletePatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InsuranceController), "RollForDelete");
        }

        [PatchPrefix]
        public static bool PatchPrefix(Item? insuredItem, ref bool? __result)
        {
            // Dropped items are always kept
            if (DroppedItemHelper.IsDropped(insuredItem))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
