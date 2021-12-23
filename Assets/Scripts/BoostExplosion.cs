using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostExplosion : MonoBehaviour
{
    ParticleSystem myParticleSystem;
    ParticleSystem.ColorOverLifetimeModule colorModule;
    ParticleSystem.TrailModule trailModule;
    Gradient particleGradient;
    Gradient trailGradient;
    float lifetime = 10f;
    Color startColor;
    float alpha1 = 0.3f;
    float alpha2 = 0.0f;

    private void OnEnable()
    {
        //Need to apply these to trail color as well
        // Get the system and the color module.
        myParticleSystem = GetComponent<ParticleSystem>();
        colorModule = myParticleSystem.colorOverLifetime;
        trailModule = myParticleSystem.trails;

        RandomColor();
        ParticleGradient();
        TrailGradient();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.gameObject, lifetime);
    }

    void ParticleGradient()
    {
        particleGradient = new Gradient();
        particleGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(Color.white, 3.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha1, 0.0f), new GradientAlphaKey(alpha2, 3.0f) }
        );

        // Apply the gradient.
        colorModule.color = particleGradient;
    }

    void TrailGradient()
    {
        trailGradient = new Gradient();
        trailGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(Color.black, 100f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha1, 0.0f), new GradientAlphaKey(alpha2, 3.0f) }
        );

        trailModule.colorOverLifetime = trailGradient;
        trailModule.colorOverTrail = trailGradient;
    }

    void RandomColor()
    {
        startColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        myParticleSystem.startColor = startColor;
    }

}
