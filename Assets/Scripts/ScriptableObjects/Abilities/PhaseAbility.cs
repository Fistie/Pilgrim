using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Abilities/Phase Ability", fileName = "Phase Ability")]
public class PhaseAbility : GenericAbility
{
    public float phaseDistance = 1000f;
    public bool hasPhased = false;

    public override void Ability(Vector2 playerPosition, Vector2 playerFacingDirection,
    Animator playerAnimator = null, Rigidbody2D playerRigidbody = null)
    {
        if (hasPhased != true)
        {
            Vector3 tempVector = playerRigidbody.transform.position;
            tempVector.x += 100;
            playerRigidbody.transform.position = tempVector;
            hasPhased = true;
        }
        else
        {
            Vector3 tempVector = playerRigidbody.transform.position;
            tempVector.x -= 100;
            playerRigidbody.transform.position = tempVector;
            hasPhased = false;
        }
    }
}
