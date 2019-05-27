using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit;
using UnityEngine;
using Verse;

namespace Toolkit_Delegates
{
    public class Window_NominateDelegates : Window
    {
        public Window_NominateDelegates()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (!viewerSelected && searchBox != lastSearch)
            {
                ViewerSearch();
            }

            Rect searchBar = new Rect(100f, 0f, 250f, 28f);
            Rect searchX = new Rect(350f, 0f, 60f, 28f);

            searchBox = Widgets.TextEntryLabeled(searchBar, "Search:", searchBox);

            if (Widgets.ButtonText(searchX, "X"))
            {
                ClearViewer();
            }

            if (!viewerSelected)
            {
                Rect viewerButton = new Rect(100f, 32f, 250f, 28f);

                foreach (Viewer viewer in cachedViewers)
                {
                    if (Widgets.ButtonText(viewerButton, viewer.username))
                    {
                        SetViewer(viewer);
                    }

                    viewerButton.y += 32f;
                }
            }
            else
            {
                Rect viewerInfo = new Rect(100f, 32f, 400f, 48f);
                Text.Font = GameFont.Medium;
                Widgets.Label(viewerInfo, "<b>Viewer:</b> " + selectedViewer.username);
                Text.Font = GameFont.Small;

                Rect viewerButton = new Rect(100f, 80f, 250f, 32f);

                if (!viewerIsDelegate)
                {
                    if (Widgets.ButtonText(viewerButton, "Set as Delegate"))
                    {
                        SetViewerAsDelegate();
                    }
                }
                else
                {
                    if (Widgets.ButtonText(viewerButton, "Remove as Delegate"))
                    {
                        UnSetViewerAsDelegate();
                    }
                }

            }
        }

        static void ViewerSearch()
        {
            searchBox = searchBox.ToLower();
            lastSearch = searchBox;

            if (searchBox == "")
            {
                cachedViewers = new List<Viewer>();
                return;
            }

            cachedViewers = Viewers.All.Where(s =>
                s.username.Contains(searchBox) ||
                s.username == searchBox
            ).Take(6).ToList();
        }

        static void SetViewer(Viewer viewer)
        {
            viewerSelected = true;
            selectedViewer = viewer;
            viewerIsDelegate = Delegate_Settings.DelegateUsernames.Contains(viewer.username);
            searchBox = viewer.username;
            lastSearch = viewer.username;
        }

        static void SetViewerAsDelegate()
        {
            if (!Delegate_Settings.DelegateUsernames.Contains(selectedViewer.username))
            {
                Delegate_Settings.DelegateUsernames.Add(selectedViewer.username);
                viewerIsDelegate = true;
            }
        }

        static void UnSetViewerAsDelegate()
        {
            if (Delegate_Settings.DelegateUsernames.Contains(selectedViewer.username))
            {
                Delegate_Settings.DelegateUsernames.Remove(selectedViewer.username);
                viewerIsDelegate = false;
            }
        }

        static void ClearViewer()
        {
            cachedViewers = new List<Viewer>();

            viewerSelected = false;
            selectedViewer = null;
            viewerIsDelegate = false;
            searchBox = "";
            lastSearch = "";
        }

        static List<Viewer> cachedViewers = new List<Viewer>();

        static bool viewerSelected = false;
        static Viewer selectedViewer = null;
        static bool viewerIsDelegate = false;
        static string searchBox = "";
        static string lastSearch = "";
    }
}
