using UnityEngine;

public class SoundManager : MonoBehaviour {
    public float musicDivider = 1.25f;
    [SerializeField] private AudioSource music, bfx;
    private float volume = 0.5f;

    private void Start() {
        UpdateVolume(PlayerPrefs.GetFloat("volume", volume));
    }

    public void UpdateVolume(float vol) {
        volume = vol;
        music.volume = volume / musicDivider;
        bfx.volume = volume;
    }

    public void PlaySound(AudioClip clip, float volume = 1) {
        bfx.PlayOneShot(clip, volume);
    }
}
