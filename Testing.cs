
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ErrorMessage;

namespace Stats_Tracker
{
    class Testing
    {
        static BodyTemperature bodyTemperature;


        //[HarmonyPatch(typeof(Player), "Update")]
        class Player_Update_Patch
        {
            //static HashSet<string> biomeNames = new HashSet<string>();
            static void Postfix(Player __instance)
            {
                //if (__instance.currentSub != null)
                //    AddDebug("sub Temperature: " + (int)__instance.currentSub.internalTemperature);
                //if (__instance.currentInterior != null)
                //    AddDebug("Interior Temperature: " + (int)__instance.currentInterior.GetInsideTemperature());
                //AddDebug(PlatformUtils.main.GetServices().GetRichPresence());
                //bool inLifepodDrop = Player.main.currentInterior is LifepodDrop;
                //AddDebug("inLifepodDrop " + inLifepodDrop);
                if (bodyTemperature == null)
                    bodyTemperature = __instance.GetComponent<BodyTemperature>();

                //AddDebug("isExposed " + bodyTemperature.isExposed);
                //AddDebug("GetWaterTemperature " + (int)bodyTemperature.GetWaterTemperature());
                //AddDebug("GetAmbientTemperature " + (int)bodyTemperature.GetAmbientTemperature());
                //AddDebug("CalculateEffectiveAmbientTemperature " + (int)bodyTemperature.CalculateEffectiveAmbientTemperature());
                //AddDebug("effectiveAmbientTemperature " + (int)bodyTemperature.effectiveAmbientTemperature);

                //float movementSpeed = (float)System.Math.Round(__instance.movementSpeed * 10f) / 10f;
                //biomeNames.Add(biomeName);
                if (Input.GetKeyDown(KeyCode.B))
                {
                    //AddDebug("currentSlot " + Main.config.escapePodSmokeOut[SaveLoadManager.main.currentSlot]);
                    //if (Player.main.IsInBase())
                    //    AddDebug("IsInBase");
                    //else if (Player.main.IsInSubmarine())
                    //    AddDebug("IsInSubmarine");
                    //else if (Player.main.inExosuit)
                    //    AddDebug("GetInMechMode");
                    //else if (Player.main.inSeamoth)
                    //    AddDebug("inSeamoth");
                    int x = Mathf.RoundToInt(Player.main.transform.position.x);
                    int y = Mathf.RoundToInt(Player.main.transform.position.y);
                    int z = Mathf.RoundToInt(Player.main.transform.position.z);
                    AddDebug(x + " " + y + " " + z);
                    AddDebug("" + Player.main.GetBiomeString());
                    //Inventory.main.container.Resize(8,8);   GetPlayerBiome()
                    //HandReticle.main.SetInteractText(nameof(startingFood) + " " + dict[i]);
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    TimeSpan ts = TimeSpan.FromSeconds(66);
                    AddDebug(" TimeSpan 66 " + ts.TotalMinutes);
                    ts = TimeSpan.FromSeconds(33);
                    AddDebug(" TimeSpan 33 " + ts.TotalMinutes);
                    DumpEncy();
                    //if (Input.GetKey(KeyCode.LeftShift))
                    //    Player.main.GetComponent<Survival>().water++;
                    //else
                    //    Player.main.GetComponent<Survival>().food++;
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {

                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        //survival.water--;
                        __instance.liveMixin.health--;
                    else
                        //survival.food--;
                        __instance.liveMixin.health++;
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    //AddDebug("timePassedSinceOrigin " + DayNightCycle.main.timePassedSinceOrigin);
                    //AddDebug("KnownTech " + KnownTech.Contains(TechType.Seaglide));
                    //AddDebug("sub EcoTargetType " + BehaviourData.GetEcoTargetType(Player.main.currentSub.gameObject));
                    //AddDebug("Exosuit " + BehaviourData.GetEcoTargetType(TechType.Exosuit));
                    //AddDebug("GetDepth " + Player.main.GetDepth());
                    //Vector3 vel = Player.main.currentMountedVehicle.useRigidbody.velocity;
                    //bool moving = vel.x > 1f || vel.y > 1f || vel.z > 1f;
                    //AddDebug("moving " + moving);
                    Targeting.GetTarget(Player.main.gameObject, 5f, out GameObject target, out float targetDist);
                    if (target)
                    {
                        PrefabIdentifier pi = target.GetComponentInParent<PrefabIdentifier>();
                        if (pi)
                        {
                            AddDebug("target " + pi.gameObject.name);
                            AddDebug("target TechType " + CraftData.GetTechType(pi.gameObject));
                        }
                    }
                    if (Player.main.guiHand.activeTarget)
                    {
                        //VFXSurface[] vFXSurfaces = __instance.GetAllComponentsInChildren<VFXSurface>();
                        //if (vFXSurfaces.Length == 0)
                        //    AddDebug(" " + Main.guiHand.activeTarget.name + " no VFXSurface");
                        //else
                        ChildObjectIdentifier coi = Player.main.guiHand.activeTarget.GetComponentInParent<ChildObjectIdentifier>();
                        PrefabIdentifier pi = Player.main.guiHand.activeTarget.GetComponentInParent<PrefabIdentifier>();
                        //if (coi)
                        //    AddDebug("activeTarget child " + coi.gameObject.name);
                        if (pi)
                            AddDebug("activeTarget  " + pi.gameObject.name);
                        LiveMixin lm = pi.GetComponent<LiveMixin>();
                        if (lm)
                        {
                            //AddDebug("max HP " + lm.data.maxHealth);
                            //AddDebug(" HP " + lm.health);
                        }
                    }
                }
            }
        }


        //[HarmonyPatch(typeof(Targeting), "GetTarget", new Type[] { typeof(float), typeof(GameObject), typeof(float), typeof(Targeting.FilterRaycast) }, new[] { ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out, ArgumentType.Normal })]
        class Targeting_GetTarget_PostfixPatch
        {
            public static void Postfix(ref GameObject result)
            {
                //AddDebug(" Targeting GetTarget  " + result.name);
            }
        }

        private static void DumpEncy()
        {
            Main.logger.LogMessage("Dump ency");
            AddDebug("Dump ency");
            foreach (var kv in PDAEncyclopedia.mapping)
            {
                PDAEncyclopedia.EntryData data = kv.Value;
                bool unlocked = PDAEncyclopedia.entries.ContainsKey(kv.Key);
                Main.logger.LogMessage($" key: {data.key} path: {data.path} unlocked: {unlocked}");
            }
        }


    }
}
