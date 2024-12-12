using UnityEngine;

public class SfxManager : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] AudioClip clip;
    private void OnEnable()
    {
        BallController.OnTileHit += PlayAudio;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();        
    }

    private void PlayAudio() 
    {
        source.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        BallController.OnTileHit -= PlayAudio;

    }
}
