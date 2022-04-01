using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private ParticleSystemController pscontroller;

    [SerializeField]
    private float spawnRadius = 10f;

    [SerializeField]
    [Range(0f, 1f)]
    private float percentage = 0.75f;

    private float _time = 0f;
    private bool isSpawning = false;
    private Vector2 SpawnPosition;
    private Quaternion SpawningAngle;
    private Color SpawnColor;
    private bool hasInstantiated = false;
    private SpriteRenderer player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Spawn();

        float percent = 0f;
        if (isSpawning) percent = pscontroller.getPercent();
        if (isSpawning && (percent >= percentage) && !hasInstantiated)
        {
            player = Instantiate(PlayerPrefab, SpawnPosition, SpawningAngle).GetComponent<SpriteRenderer>();
            player.color = SpawnColor;
            hasInstantiated = true;
        }
        if (player != null)
        {
            player.color = new Color(player.color.r, player.color.g, player.color.b, (percent - percentage) / (1f - percentage));
        }
        if ((isSpawning && percent >= 0.99f) || !pscontroller.isAnimating())
        {
            pscontroller.enabled = false;

            isSpawning = false;
            _time = 0f;
            player = null;
        }
    }

    public void Spawn()
    {
        if (isSpawning)
        {
            Debug.Log("Already Spawning, wait till the process is over");
            return;
        }
        hasInstantiated = false; 
        SpawnPosition = Random.insideUnitCircle * spawnRadius;
        var angle = Random.Range(0f, 360f);
        SpawnColor = Random.ColorHSV();
        if (SpawnColor.maxColorComponent < 0.5f)
        {
            SpawnColor *= 1 / SpawnColor.maxColorComponent;
        }
        SpawnColor.a = 1f;
        pscontroller.transform.position = (Vector3) SpawnPosition;
        SpawningAngle = Quaternion.Euler(new Vector3(0f, 0f, angle));
        pscontroller.transform.rotation = SpawningAngle;
        pscontroller.enabled = true;
        var PS = pscontroller.getPS();
        var main = PS.main;
        var color = new ParticleSystem.MinMaxGradient();
        color.color = SpawnColor;
        main.startColor = color;
        isSpawning = true;
    }
}
