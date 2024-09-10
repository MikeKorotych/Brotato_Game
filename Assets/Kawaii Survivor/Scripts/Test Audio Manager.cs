using UnityEngine;

public class TestAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActionTester.myAction += PlayTakeDamageSound;
    }

    private void OnDestroy()
    {
        ActionTester.myAction -= PlayTakeDamageSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTakeDamageSound(int health)
    {

        Debug.Log("--- Playing taking damage sound ---");
    }
}
