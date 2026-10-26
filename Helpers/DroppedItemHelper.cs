using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using System.Text.Json;

namespace Tosox.InsuranceFraud.Helpers
{
    internal static class DroppedItemHelper
    {
        // Stored in the item's extension data so the flag survives being saved
        // inside the profile's insurance list until the package is processed
        private const string DroppedKey = "insuranceFraudDropped";

        public static void MarkDropped(Item item)
        {
            if (item.ExtensionData != null)
            {
                item.ExtensionData[DroppedKey] = true;
            }
        }

        public static bool IsDropped(Item? item)
        {
            if (item?.ExtensionData == null || !item.ExtensionData.TryGetValue(DroppedKey, out var value))
            {
                return false;
            }

            // In-memory the flag is a bool, after a profile reload it's a JsonElement
            return value is true or JsonElement { ValueKind: JsonValueKind.True };
        }

        public static void ClearDropped(Item item)
        {
            item.ExtensionData?.Remove(DroppedKey);
        }
    }
}
