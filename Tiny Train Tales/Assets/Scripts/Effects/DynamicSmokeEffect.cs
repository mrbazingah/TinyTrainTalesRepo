using UnityEngine;

public class DynamicSmokeEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem smokePS;
    [SerializeField] bool isMoving;
    [SerializeField] float transitionTime;
    [SerializeField] float moveStartX, nonMoveStartX;
    [SerializeField] float moveEndX, nonMoveEndX;

    float elapsedTime;
    bool hasStopped;

    ParticleSystem.ForceOverLifetimeModule forceModule;

    void Start()
    {
        forceModule = smokePS.forceOverLifetime;
    }

    public void StopMode()
    {
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving && !hasStopped)
        {
            moveStartX = nonMoveStartX;
            moveEndX = nonMoveEndX;

            elapsedTime = 0;
            hasStopped = true;
        }

        if (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);
            forceModule.x = Mathf.Lerp(moveStartX, moveEndX, t);
        }
    }
}
