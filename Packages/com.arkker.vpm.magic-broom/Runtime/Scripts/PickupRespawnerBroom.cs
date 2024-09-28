
using MMMaellon;
using TMPro;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]

public class PickupRespawnerBroom : UdonSharpBehaviour
{

    VRCPickup pickup;

    [Header("Fun stuff")]
    [Tooltip("Set this to true if you want to show the throwing statistics")]
    public bool gameMode = false;


    [Header("References needed")]
    [SerializeField] private GameObject scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    public Vector3 throwStartPosition = Vector3.zero;
    [HideInInspector, UdonSynced] public bool isThrown = false;

    [UdonSynced] public string scoreTextString = "Previous throw: Unknown\nInstance best: Unknown";
    [UdonSynced] public float bestDistance = 0;
    [UdonSynced] public string bestDistancePlayer = "Unknown";

    AutoRespawn autoRespawn;

    public void Respawned()
    {
    }

    void Start()
    {
        pickup = GetComponent<VRCPickup>();

        if (gameMode)
        {
            scoreText.SetActive(true);
        }
        else
        {
            scoreText.SetActive(false);
        }

        autoRespawn = GetComponent<AutoRespawn>();
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

    public void RecordDistance()
    {
        if (gameMode)
        {
            // Create new vectors with the same x and z components but y set to 0
            Vector3 horizontalStartPosition = new Vector3(throwStartPosition.x, 0, throwStartPosition.z);
            Vector3 horizontalCurrentPosition = new Vector3(transform.position.x, 0, transform.position.z);

            // Calculate the horizontal distance
            float distance = Vector3.Distance(horizontalStartPosition, horizontalCurrentPosition);

            if (distance > bestDistance)
            {
                bestDistance = distance;
                bestDistancePlayer = Networking.LocalPlayer.displayName;
            }
            scoreTextString = $"Previous throw: {distance:0.00}m by {Networking.LocalPlayer.displayName}\nInstance best: {bestDistance:0.00}m by {bestDistancePlayer}";
            scoreTextMesh.text = scoreTextString;

            RequestSerialization();
        }
    }

    public override void OnDrop()
    {
        // base.OnDrop();
        if (gameMode)
        {
            isThrown = true;
            throwStartPosition = transform.position;
            RequestSerialization();

            SendCustomEventDelayedSeconds("RecordDistance", autoRespawn.respawnCooldown - 0.2f);
        }
    }

    public override void OnDeserialization()
    {
        // base.OnDeserialization();
        if (gameMode)
        {
            if (isThrown)
            {
                scoreTextMesh.text = scoreTextString;
            }
        }
    }
}
