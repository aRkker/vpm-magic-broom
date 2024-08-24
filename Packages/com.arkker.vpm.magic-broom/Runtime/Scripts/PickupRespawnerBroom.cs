
using MMMaellon;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PickupRespawnerBroom : UdonSharpBehaviour
{

    VRCPickup pickup;

    void Start()
    {
        pickup = GetComponent<VRCPickup>();
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision!");
        if (other.gameObject.GetComponent<SmartObjectSync>() != null && pickup.IsHeld && Networking.IsOwner(pickup.gameObject))
        {
            // Debug.Log("Respawning!");
            Networking.SetOwner(Networking.LocalPlayer, other.gameObject);
            other.gameObject.GetComponent<SmartObjectSync>().Respawn();
        }
    }
}
