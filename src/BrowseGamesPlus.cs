using DuckGame;
using System;
using HarmonyLib;
using System.Reflection;

[assembly: AssemblyTitle("|0,94,223|Browse Games|242,76,5|+")]
[assembly: AssemblyCompany("|GREEN|TheKingOfCringe|RED|11")]
[assembly: AssemblyDescription("Provides you with detailed information about lobbies!")]
[assembly: AssemblyVersion("1.0.0.2")]

namespace BrowseGamesPlus
{
    public class BrowseGamesPlus : ClientMod
    {
        protected override void OnPreInitialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Utilities.OnAssemblyResolve;
        }

        protected override void OnPostInitialize()
		{
            Options.Load();

            if (Steam.IsInitialized())
                new Harmony("0-0").PatchAll();
        }
    }
}
