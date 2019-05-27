using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Toolkit_Delegates
{
    public class Delegate_Settings : ModSettings
    {
        public static List<string> DelegateUsernames = new List<string>();

        public static bool OnlyDelegatesReceiveCoins = true;

        public static int DelegateCoinReward = 2500;

        public static int DelegateCoinRewardInterval = 60;

        public static bool OnlyDelegatesCanSpendCoins = true;

        public static long LastDelegateCoinReward = 0;

        public void DoWindowContents(Rect rect)
        {
            Listing_Standard settings = new Listing_Standard();
            settings.Begin(rect);

            settings.ColumnWidth = rect.width / 1.1f;

            Text.Font = GameFont.Medium;
            settings.Label("Delegate Settings");
            Text.Font = GameFont.Small;
            settings.Gap();
            settings.GapLine();

            settings.Label("Delegates are specific viewers that you have nominated as a delegate.");
            if (settings.ButtonTextLabeled("Set viewers as Delegates", "Delegates"))
            {
                Window_NominateDelegates window = new Window_NominateDelegates();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            settings.GapLine();
            settings.Gap(24);

            settings.Label("Delegates receive coins separately from the toolkits main coin rewarding system. Disabling the main coin system makes it so <b>ONLY DELEGATES</b> will receive coins.");

            settings.CheckboxLabeled("Disable main coin system?", ref OnlyDelegatesReceiveCoins);

            settings.GapLine();
            settings.Gap(24);

            string coinRewardBuffer = DelegateCoinReward.ToString();
            settings.TextFieldNumericLabeled("How many coins should delegates receive per interval?", ref DelegateCoinReward, ref coinRewardBuffer, 0, 100000);

            settings.Gap();

            string coinIntervalBuffer = DelegateCoinRewardInterval.ToString();
            settings.TextFieldNumericLabeled("How many minutes between payment intervals?", ref DelegateCoinRewardInterval, ref coinIntervalBuffer, 0, 360);

            settings.Gap();

            if (settings.ButtonTextLabeled("Reward Delegates Coins Now", "Reward"))
            {
                Delegate_Rewarder.AwardDelegates(true);
            }

            settings.GapLine();
            settings.Gap(24);

            settings.Label("If you re-enable the main coin system, you may wish to force viewers to gift their coins to a delegate instead of spending them.");
            settings.CheckboxLabeled("Prevent viewers from spending coins?", ref OnlyDelegatesCanSpendCoins);

            settings.End();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref DelegateUsernames, "DelegateUsernames", LookMode.Value);

            Scribe_Values.Look(ref OnlyDelegatesReceiveCoins, "OnlyDelegatesReceiveCoins", true);
            Scribe_Values.Look(ref DelegateCoinReward, "DelegateCoinReward", 2500);
            Scribe_Values.Look(ref DelegateCoinRewardInterval, "DelegateCoinRewardInterval", 60);
            Scribe_Values.Look(ref OnlyDelegatesCanSpendCoins, "OnlyDelegatesCanSpendCoins", true);
            Scribe_Values.Look(ref LastDelegateCoinReward, "LastDelegateCoinReward", 0);
        }
    }
}
