﻿using HarmonyLib;
using UnityEngine;

namespace TownOfUs.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class KillTimer
    {
        public static bool Prefix(PlayerControl __instance, ref float time)
        {
            if (__instance.Data.Role.CanUseKillButton)
            {
                if (GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown <= 0f)
                {
                    return false;
                }

                var maxvalue = time > GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown ? time + 1f : GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                __instance.killTimer = Mathf.Clamp(time, 0, maxvalue);
                HudManager.Instance.KillButton.SetCoolDown(__instance.killTimer, maxvalue);
            }

            return false;
        } 
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.IsKillTimerEnabled), MethodType.Getter)]
    public static class SpawnMinigameKillTimer
    {
        public static void Postfix(ref bool __result)
        {
            __result = __result || (Minigame.Instance && Minigame.Instance.TryCast<SpawnInMinigame>() != null);
        } 
    }
}