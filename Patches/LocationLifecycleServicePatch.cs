using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Extensions;
using SPTarkov.Server.Core.Models.Eft.Match;
using SPTarkov.Server.Core.Services;
using System.Reflection;
using Tosox.InsuranceFraud.Helpers;

namespace Tosox.InsuranceFraud.Patches
{
    public class LocationLifecycleServicePatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocationLifecycleService), "HandleInsuredItemLostEvent");
        }

        [PatchPrefix]
        public static bool PatchPrefix(EndLocalRaidRequestData request)
        {
            // Without the post-raid inventory we can't tell what was dropped, so leave insurance untouched
            var inventory = request.Results?.Profile?.Inventory;
            if (request.LostInsuredItems == null || inventory?.Items == null || inventory.Equipment == null)
            {
                return true;
            }

            // Everything still equipped when the raid ended
            var equippedItemIds = inventory.Items
                .GetItemWithChildren(inventory.Equipment.Value)
                .Select(item => item.Id)
                .ToHashSet();

            // Lost insured items that weren't on the player were dropped or stashed somewhere in the raid
            foreach (var lostItem in request.LostInsuredItems)
            {
                if (!equippedItemIds.Contains(lostItem.Id))
                {
                    DroppedItemHelper.MarkDropped(lostItem);
                }
            }

            return true;
        }
    }
}
