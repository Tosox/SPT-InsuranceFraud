using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Models.Eft.Profile;
using System.Reflection;
using Tosox.InsuranceFraud.Helpers;

namespace Tosox.InsuranceFraud.Patches
{
    public class SendMailPatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InsuranceController), "SendMail");
        }

        [PatchPrefix]
        public static bool PatchPrefix(Insurance insurance)
        {
            // Strip the flag before the items are handed back to the player
            if (insurance.Items != null)
            {
                foreach (var item in insurance.Items)
                {
                    DroppedItemHelper.ClearDropped(item);
                }
            }

            return true;
        }
    }
}
