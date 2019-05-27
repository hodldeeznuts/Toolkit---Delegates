using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit;
using TwitchToolkit.IRC;
using TwitchToolkit.Store;
using Verse;

namespace Toolkit_Delegates
{
    [StaticConstructorOnStartup]
    static class Patches
    {
        private static readonly Type patchType = typeof(Patches);

        static Patches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.twitchtoolkit.delegates");

            harmony.Patch(
                    original: AccessTools.Method(type: typeof(Viewers), name: nameof(Viewers.ParseViewersFromJsonAndFindActiveViewers)),
                    postfix: new HarmonyMethod(type: patchType, name: nameof(ParseViewersPostfix))
                );

            harmony.Patch(
                    original: AccessTools.Method(type: typeof(Purchase_Handler), name: nameof(Purchase_Handler.ResolvePurchase)),
                    prefix: new HarmonyMethod(type: patchType, name: nameof(ResolvePurchasePrefix))
                );
        }

        static List<string> ParseViewersPostfix(List<string> __result)
        {
            Delegate_Rewarder.AwardDelegates();

            if (Delegate_Settings.OnlyDelegatesReceiveCoins)
            {
                return null;
            }

            return __result;
        }

        static void ResolvePurchasePrefix(Viewer viewer, ref IRCMessage message, bool separateChannel = false)
        {
            if (Delegate_Settings.OnlyDelegatesCanSpendCoins)
            {
                bool isDelegate = Delegate_Settings.DelegateUsernames.Contains(viewer.username);

                if (!isDelegate)
                {
                    message.Message = "invalid";
                }
            }
        }
    }
}
