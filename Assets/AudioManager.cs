using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------Audio Source-----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------Audio Clip-----------")]


    public AudioClip background;
    public AudioClip portalIn;
    public AudioClip portalOut;
    public AudioClip Dash;

    public static AudioManager instance;

    //private void Awake() 
    //{ 
    //    if (instance == null) 
    //    { 
    //        instance = this; DontDestroyOnLoad(gameObject); 
    //    } 
    //    else 
    //    { 
    //        Destroy(gameObject); 
    //    } 
    //}

    private void Start()
     {
        musicSource.clip = background;
        musicSource.Play();
     }

     private void Update()
     {
        if (PlayerHealth.Instance.isDead)
        {
            musicSource.Stop();
        }
     }


     public void PlaySFX(AudioClip clip)
     {
        SFXSource.PlayOneShot(clip);
     }


    public void PlayDashSound()
    {
        SFXSource.PlayOneShot(Dash);
    }

}
