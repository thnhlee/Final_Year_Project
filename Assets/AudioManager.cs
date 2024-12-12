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
     private void Start()
     {
        musicSource.clip = background;
        musicSource.Play();
     }

     private void Update()
        {
            // Kiểm tra nếu nhân vật đã chết thì dừng nhạc nền
            if (PlayerHealth.Instance.isDead)
            {
                musicSource.Stop();
            }
        }


     public void PlaySFX(AudioClip clip)
     {
        SFXSource.PlayOneShot(clip);
     }

}
