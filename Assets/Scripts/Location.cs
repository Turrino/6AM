using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Location : MonoBehaviour {

    public Sprite ph;
    public GameObject Character;
    public GameObject Background;
    // TODO replace with the actual locations
    public GameObject MapLocation;
    public GameObject Prop;
    public Vector2 Anchor;
    private LocationInfo _locInfo;

    public bool Ready;

    private void Start()
    {
        Ready = true;
    }

    public void CreateLocation(LocationInfo locInfo, Transform parentTransform)
    {
        _locInfo = locInfo;
        Anchor = _locInfo.Assets.Anchor;
        var parent = parentTransform;
        InstantiateBackground(parent);

        if (_locInfo.IsMap)
        {
            foreach (var location in Master.GM.ScenarioData.Locations)
            {                
                var instance = Instantiate(MapLocation, location.CoordsOnMainMap, Quaternion.identity);
                instance.name = location.Person.Name;
                instance.transform.SetParent(parent);
                var renderer = instance.GetComponent<SpriteRenderer>();
                renderer.sprite = location.Assets.MainMapSprite;
                renderer.sortingLayerName = NamesList.SortingLayerBackground;
                renderer.sortingOrder = 1;
            }

            
        }
        else
        {
            SpriteRenderer renderer;

            // Create the character
            var instance = Instantiate(Character, GetSpawnPoint(ColorsList.OverlayCharacter), Quaternion.identity);
            instance.transform.SetParent(parent);
            // set the animation for the character
            //var animator = instance.GetComponent<Animator>();
            //var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            //animator.runtimeAnimatorController = animatorOverrideController;
            //animatorOverrideController["idling"] = _locInfo.Person.Resource.Image.ToSprite();
            renderer = instance.GetComponent<SpriteRenderer>();
            // TODO fix - we have to use the sprite now, but need to get the animation..
            renderer.sprite = _locInfo.Person.Sprite;
            renderer.sortingLayerName = NamesList.SortingLayerWorld;
            renderer.SetSortingOrder();

            // Create the props
            InstantiateProps(parent);

            //// Create the exit
            //var exit = Instantiate(Exit, GetSpawnPoint(NamesList.OverlayExit), Quaternion.identity);
            //exit.transform.SetParent(parent);
            //exit.name = NamesList.ExitToMain;
        }
    }

    private void InstantiateProps(Transform parent)
    {      
        // overlay test (keep in mind that spawn points already taken by props won't be in this list!)
        //foreach (var item in _locInfo.Assets.PropSpawnPoints)
        //{
        //    var propInstance = Instantiate(Prop, item, Quaternion.identity);
        //    propInstance.transform.SetParent(parent);
        //    var renderer = propInstance.GetComponent<SpriteRenderer>();
        //    renderer.sprite = ph;
        //    renderer.sortingLayerName = NamesList.SortingLayerForeground;
        //}

        SpriteRenderer renderer;
        foreach (var prop in _locInfo.Assets.Props)
        {
            if (prop.NoProp)
                continue;
            var propInstance = Instantiate(Prop, prop.SpawnPoint, Quaternion.identity);
            propInstance.transform.SetParent(parent);
            renderer = propInstance.GetComponent<SpriteRenderer>();
            renderer.sprite = prop.Sprite;
            renderer.sortingLayerName = NamesList.SortingLayerWorld;
            renderer.SetSortingOrder();
        }
    }

    private void InstantiateBackground(Transform parent)
    {        
        AddBgParts(_locInfo.Assets.BackgroundSprites, parent, false);
        if (_locInfo.Assets.ForegroundSprites != null)
            AddBgParts(_locInfo.Assets.ForegroundSprites, parent, true);

        // Create the collider sprite
        if (_locInfo.Assets.ColliderSprite != null)
        {
            var cInstance = Instantiate(Background, Anchor, Quaternion.identity);
            cInstance.transform.SetParent(parent);
            var renderer = cInstance.GetComponent<SpriteRenderer>();
            renderer.sprite = _locInfo.Assets.ColliderSprite;
            renderer.sortingOrder = 0;
            // add the collider component to it
            cInstance.AddComponent<PolygonCollider2D>();
            renderer.enabled = false;
        }
    }

    private void AddBgParts(List<Sprite> sprites, Transform parent, bool foreGround)
    {
        var sortingOrder = foreGround ? 100 : -100;
        var sortingLayer = foreGround ? NamesList.SortingLayerForeground
            : NamesList.SortingLayerBackground;

        foreach (var bgPart in sprites)
        {
            var bgInstance = Instantiate(Background, Anchor, Quaternion.identity);
            bgInstance.transform.SetParent(parent);
            var renderer = bgInstance.GetComponent<SpriteRenderer>();
            renderer.sprite = bgPart;
            renderer.sortingLayerName = sortingLayer;
            renderer.sortingOrder = sortingOrder++;
        }
    }

    private Vector2 GetSpawnPoint(PixelInfo overlayType)
    {
        return _locInfo.Assets.OverlayData.Points[overlayType]
           .PickRandom().Anchor(Anchor);
    }
}
