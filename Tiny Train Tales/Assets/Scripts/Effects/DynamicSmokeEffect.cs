using UnityEngine;

public class DynamicSmokeEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem smokePS;
    [SerializeField] bool isMoving;
    [SerializeField] float transitionTime;
    [SerializeField] float moveStartX, nonMoveStartX;
    [SerializeField] float moveEndX, nonMoveEndX;

    float elapsedTime;

    ParticleSystem.ForceOverLifetimeModule forceModule;
    Train train;

    void Start()
    {
        forceModule = smokePS.forceOverLifetime;    
        train = FindObjectOfType<Train>();
    }

    public void StopMode()
    {
        isMoving = false;
        
    }

    void Update()
    {
        if (!isMoving)
        {
            moveStartX = nonMoveStartX;
            moveEndX = nonMoveEndX;

            elapsedTime = 0;
        }

        if (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);
            forceModule.x = Mathf.Lerp(moveStartX, moveEndX, t);
        }
    }
}
