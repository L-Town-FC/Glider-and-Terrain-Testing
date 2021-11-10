using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostExplosion : MonoBehaviour
{
    ParticleSystem myParticleSystem;
    ParticleSystem.ColorOverLifetimeModule colorModule;
    Gradient ourGradient;
    float lifetime = 10f;
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.gameObject, lifetime);

        //Need to apply these to trail color as well
        // Get the system and the color module.
        myParticleSystem = GetComponent<ParticleSystem>();
        colorModule = myParticleSystem.colorOverLifetime;

        startColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        ParticleGradient();
    }

    void ParticleGradient()
    {
        float alpha = 1.0f;
        ourGradient = new Gradient();
        ourGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(Color.white, 3.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha - 0.5f, 3.0f) }
        );

        // Apply the gradient.
        colorModule.color = ourGradient;
    }

}
