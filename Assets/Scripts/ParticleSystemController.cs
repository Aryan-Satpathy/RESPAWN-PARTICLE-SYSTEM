using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button animate;

    [SerializeField]
    private bool playOnEnable;

    private ParticleSystem ps;

    private bool animating = false;
    private float animTime = 0f;

    private uint seed;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        if (playOnEnable)
        {
            // animate.gameObject.SetActive(false);
            Animate();
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            // ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            // ps.useAutoRandomSeed = false;
            // ps.Stop();
            // ps.Play(false);
            ps.Simulate(ps.main.duration - animTime, true, true);
            // ps.useAutoRandomSeed = true;
            // ps.Play();
            ps.randomSeed = seed;
        }

        if (animating) animTime += Time.deltaTime;
        if (animTime > ps.main.duration) Disable();
    }

    private void OnDisable()
    {
        Disable();
    }

    void Disable()
    {
        animating = false;
        animTime = 0f;
        // ps.Play(false);
        ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Animate()
    {
        if (animating)
        {
            Debug.Log("Already animating, please wait for animation to be over.");
        }
        else
        {
            animating = true;
            animTime = 0f;
            ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play(false);
            seed = ps.randomSeed;
        }
    }

    public ParticleSystem getPS()
    {
        return ps;
    }

    public bool isAnimating()
    {
        return animating;
    }

    public float getPercent()
    {
        return (animTime / ps.main.duration); 
    }    
}
