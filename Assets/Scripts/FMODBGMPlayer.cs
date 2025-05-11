using UnityEngine;
using FMODUnity;

public class FMODBGMPlayer : MonoBehaviour
{
    public static FMODBGMPlayer Instance;

    [Header("FMOD")]
    [SerializeField]
    private EventReference bgmEvent;

    private FMOD.Studio.EventInstance bgmInstance;
    private bool isPlaying = false;

    private void Awake()
    {
        if (bgmEvent.IsNull)
        {
            Debug.LogWarning("❌ BGM Event is NULL.");
            return;
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmInstance = RuntimeManager.CreateInstance(bgmEvent);
        
        FMOD.RESULT result = bgmInstance.start();

        if (result == FMOD.RESULT.OK)
        {
            Debug.Log("🎵 FMOD BGM started successfully.");
            bgmInstance.release();
            isPlaying = true;
        }
        else
        {
            Debug.LogError($"❌ Failed to start BGM: {result}");
        }
    }


    public void StopBGM()
    {
        if (isPlaying)
        {
            bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }
}
