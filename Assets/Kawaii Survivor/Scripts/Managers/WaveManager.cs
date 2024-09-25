using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements ")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;

    [Header(" Settings ")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header(" Waves ")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();

            string timerString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);
        }
        else
            StartWaveTransition();
    }
    private void StartWave(int waveIndex)
    {
        Debug.Log("--- Start wave " + waveIndex + " ---");
        ui.UpdateWaveText($"Wave {currentWaveIndex + 1} / {waves.Length}");

        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(1);

        timer = 0;
        isTimerOn = true;
    }


    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay > localCounters[i]) //?
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;
            }
        }

        timer += Time.deltaTime;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("--- waves completed---");
            ui.UpdateWaveText("All waves completed!");
        }
        else
            GameManager.instance.WaveCompletedCallback();
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    private void DefeatAllEnemies()
    {
        transform.Clear();
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;

        Vector2 offset = direction.normalized * Random.Range(5, 10);

        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -9, 9);

        return targetPosition;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
        }
    }
}

//
[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}


[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0f, 100f)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
}