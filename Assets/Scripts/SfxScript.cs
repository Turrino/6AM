using BayeuxBundle;
using UnityEngine;

public class SfxScript : MonoBehaviour
{
    public AudioClip[] ToolTipSfx;
    public AudioClip[] DrawerSfx;

    public AudioClip TimeUpSfx;
    public AudioClip ArrestSuccessSfx;
    public AudioClip ArrestFailSfx;
    public AudioClip TotalTimeWidget;

    private AudioSource _src;
    private AudioClip _playNext;

    public void Stop() => _src.Stop();
    public void Tooltip() => Play(ToolTipSfx.PickRandom());
    public void Drawer() => Play(DrawerSfx.PickRandom());

    public void Fail() => Play(ArrestFailSfx);
    public void Success()  {
        Play(TotalTimeWidget);
        _playNext = ArrestSuccessSfx;
    } 
    public void TimeUp() => Play(TimeUpSfx);

    private void Update()
    {
        if (!_src.isPlaying && _playNext != null)
        {
            Play(_playNext, false);
            _playNext = null;
        }
    }

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
