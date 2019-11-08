using BayeuxBundle;
using UnityEngine;

public class SfxScript : MonoBehaviour
{
    public AudioClip[] ToolTipSfx;
    public AudioClip[] DrawerSfx;
    private AudioSource _src;

    private void Start()
    {
        _src = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip)
    {        
        _src.clip = clip;
        _src.time = 0;
        _src.enabled = true;
        _src.Play();
    }

    public void Tooltip() => Play(ToolTipSfx.PickRandom());
    public void Drawer() => Play(DrawerSfx.PickRandom());
}
