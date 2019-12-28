using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScript : MonoBehaviour
{
    public CircleCollider2D LocCollider;
    public SpriteRenderer PolisSpawnPoint;
    public SpriteRenderer Map;
    public SpriteRenderer Location;
    public Sprite MayoWithMask;
    public Sprite MayoNoMask;
    public Sprite LocationNoMask;

    public void LocationSpriteNoMask()
    {
        Location.sprite = LocationNoMask;
    }
}
