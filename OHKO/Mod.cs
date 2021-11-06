using PulsarModLoader;
using HarmonyLib;
namespace OHKO
{
    public class Mod : PulsarMod
    {
        public override string Version => "1.0";

        public override string Author => "pokegustavo";

        public override string ShortDescription => "Enables OHKO for the game";

        public override string Name => "One Hit KO";

        public override string HarmonyIdentifier()
        {
            return "Pokegustavo.OHKO";
        }

        public override bool CanBeDisabled()
        {
            return true;
        }
    }

    [HarmonyPatch(typeof(PLShipStats), "CalculateStats")]
    class Patch 
    {
        static void Postfix(PLShipStats __instance) 
        {
            if(__instance.Ship != null && __instance.Ship.GetIsPlayerShip() && PhotonNetwork.isMasterClient && ModManager.Instance.GetMod("One Hit KO").IsEnabled()) 
            {
                __instance.ShieldsMax = 1;
                if (__instance.ShieldsCurrent > 1)
                {
                    __instance.ShieldsCurrent = 1;
                }
                if (__instance.Ship.MyShieldGenerator != null)
                {
                    __instance.Ship.MyShieldGenerator.Current = 1;
                }
                __instance.QuantumShieldDefensesActive = true;
                __instance.HullMax = 1;
                if (__instance.HullCurrent > 1) __instance.HullCurrent = 1;
            }
        }
    }

    [HarmonyPatch(typeof(PLPawn), "TakeDamage")]
    class PlayerPatch 
    {
        static void Postfix(PLPawn __instance) 
        {
            if(__instance.TeamID == 0 && PhotonNetwork.isMasterClient && ModManager.Instance.GetMod("One Hit KO").IsEnabled()) 
            {
                __instance.IsDead = true;
            }
        }
    }
}
