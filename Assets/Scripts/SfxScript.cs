using BayeuxBundle;
using UnityEngine;

public class SfxScript : MonoBehaviour
{
    public AudioClip[] ToolTipSfx;
    public AudioClip[] DrawerSfx;

    public AudioClip TimeUpSfx;
    public AudioClip ArrestSuccessSfx;
    public AudioClip ArrestFailSfx;

    private AudioSource _src;

    public void Stop() =>_src.Stop();
    public void Tooltip() => Play(ToolTipSfx.PickRandom());
    public void Drawer() => Play(DrawerSfx.PickRandom());

    public void Fail() => Play(ArrestFailSfx);
    public void Success() => Play(ArrestSuccessSfx);
    public void TimeUp() => Play(TimeUpSfx);

    private void Start()
    {
        _src = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip, bool loop = false)
    {        
        _src.clip = clip;
        _src.time = 0;
        _src.enabled = true;
        _src.loop = loop;
        _src.Play();
    }
}
