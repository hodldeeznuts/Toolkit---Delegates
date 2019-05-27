using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit;
using TwitchToolkit.Utilities;

namespace Toolkit_Delegates
{
    public static class Delegate_Rewarder
    {
        public static void AwardDelegates(bool ignoreLastReward = false)
        {
            if (!ignoreLastReward && Delegate_Settings.LastDelegateCoinReward != 0)
            {
                DateTime lastDelegateReward = DateTime.FromFileTime(Delegate_Settings.LastDelegateCoinReward);

                if (TimeHelper.MinutesElapsed(lastDelegateReward) < Delegate_Settings.DelegateCoinRewardInterval)
                {
                    return;
                }
            }

            foreach (string username in Delegate_Settings.DelegateUsernames)
            {
                Viewer viewer = Viewers.GetViewer(username);

                viewer.GiveViewerCoins(Delegate_Settings.DelegateCoinReward);
            }

            Delegate_Settings.LastDelegateCoinReward = DateTime.Now.ToFileTime();

            Toolkit.client.SendMessage("Delegates are receiving " + Delegate_Settings.DelegateCoinReward + " coins.");
        }
    }
}
