using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Settings;
using UnityEngine;
using Verse;

namespace Toolkit_Delegates
{
    public class Toolkit_Delegates : Mod
    {
        public Toolkit_Delegates(ModContentPack content) : base(content)
        {
            GetSettings<Delegate_Settings>();
            Settings_ToolkitExtensions.RegisterExtension(new ToolkitExtension(this, typeof(DelegatePatchSettings)));
        }

        public override void DoSettingsWindowContents(Rect inRect) => GetSettings<Delegate_Settings>().DoWindowContents(inRect);

        public override string SettingsCategory() => "Toolkit - Delegates";
    }

    public class DelegatePatchSettings : ToolkitWindow
    {
        public DelegatePatchSettings(Mod mod) : base(mod) => this.Mod = mod;

        public override void DoWindowContents(Rect inRect) => Mod.DoSettingsWindowContents(inRect);
    }
}
