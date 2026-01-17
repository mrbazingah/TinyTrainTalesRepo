using UnityEngine;

public class DynamicSmokeEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem smokePS;
    [SerializeField] bool isMoving;
    [SerializeField] float transitionTime;
    [SerializeField] float startX;
    [SerializeField] float endX;

    float elapsedTime;
    bool lastMode;

    ParticleSystem.ForceOverLifetimeModule forceModule;
    Train train;

    void Start()
    {
        forceModule = smokePS.forceOverLifetime;    
        train = FindObjectOfType<Train>();
    }

    void Update()
    {
        

        if (isMoving != train.GetHasStopped())
        {
            lastMode = isMoving;
            float sX = startX;
            float eX = endX;

            startX = eX;
            endX = sX;

            elapsedTime = 0;

            isMoving = !train.GetHasStopped();
        }

        if (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);
            forceModule.x = Mathf.Lerp(startX, endX, t);
        }
    }
}
