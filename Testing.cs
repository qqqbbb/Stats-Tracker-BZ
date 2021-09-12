
using HarmonyLib;
using QModManager.API.ModLoading;
using System.Reflection;
using System;
using SMLHelper.V2.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Text;
using static ErrorMessage;

namespace Stats_Tracker
{
    class Testing
    {

        //[HarmonyPatch(typeof(Player), "Update")]
        class Player_Update_Patch
        {
            //static HashSet<string> biomeNames = new HashSet<string>();
            static void Postfix(Player __instance)
            {
                //string biomeName = Stats_Patch.GetBiomeName(LargeWorld.main.GetBiome(Player.main.transform.position));
                //bool inSeatruck = Player.main.currentInterior is SeaTruckSegment;
     
                //AddDebug(" " + LargeWorld.main.GetBiome(Player.main.transform.position));
                //float movementSpeed = (float)System.Math.Round(__instance.movementSpeed * 10f) / 10f;
                //biomeNames.Add(biomeName);
                if (Input.GetKey(KeyCode.B))
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
                else if (Input.GetKey(KeyCode.C))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        Player.main.GetComponent<Survival>().water++;
                    else
                        Player.main.GetComponent<Survival>().food++;
                }
                else if (Input.GetKey(KeyCode.X))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        //survival.water--;
                        __instance.liveMixin.health--;
                    else
                        //survival.food--;
                        __instance.liveMixin.health++;
                }
                else if(Input.GetKey(KeyCode.Z))
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
                            AddDebug("target TechType " + CraftData.GetTechType( pi.gameObject));
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



    }
}
