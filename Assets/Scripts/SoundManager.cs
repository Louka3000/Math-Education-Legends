using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource music, bfx;
    public float musicDivider = 1.2f;
    private float volume;
    private void Start()
    {
        UpdateVolume(PlayerPrefs.GetFloat("volume", 0.2f));
        //StartCoroutine("MusicFadeIn");
    }
    public void UpdateVolume(float vol)
    {
        volume = vol;
        music.volume = volume / musicDivider;
        bfx.volume = volume;
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        bfx.PlayOneShot(clip, volume);
    }
    /*IEnumerator MusicFadeIn(float v)
    {
        while (music.volume < v)
        {
            music.volume = Mathf.Clamp01(music.volume + 30);
            yield return new WaitForFixedUpdate();
        }
        music.volume = v;
    }
    IEnumerator MusicFadeOut()
    {
        while (music.volume > 30)
        {
            music.volume = Mathf.Clamp01(music.volume - 30);
            yield return new WaitForFixedUpdate();
        }
        music.volume = 0;
    }*/
}
