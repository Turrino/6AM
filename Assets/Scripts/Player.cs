using Assets.Scripts.Resources;
using UnityEngine;

public class Player : MonoBehaviour {

    public float Speed;
    //public bool CaptureMessageOn;
    public int SortingOrder;

    //private Stopwatch _dialogueCooldown;
    /// <summary>
    /// The entrance colliders are placed right by the player spawn point
    /// this flag tracks whether the player is still colliding with the entrance
    /// </summary>
    private int _hasEnteredLocation;


    //void Start()
    //{        
    //    _dialogueCooldown = new Stopwatch();
    //}

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && !Master.GM.PomaButtonOver)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
        }

        //if (!CaptureMessageOn && Input.GetMouseButton(0) && !Master.GM.ListButton.MouseOver)
        //{
        //    if (Master.GM.DialogueManager.DialogueEnabled && _dialogueCooldown.ElapsedMilliseconds > 400)
        //    {
        //        _dialogueCooldown.Reset();
        //        Master.GM.DialogueManager.CloseDialogue();
        //    }
        //    else
        //    {               
        //        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        targetPos.z = transform.position.z;

        //        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
        //    }
        //}

        //if (Input.GetKeyUp("space"))
        //{
        //    if (Master.GM.CurrentLocation.IsMap)
        //        return;

        //    if (!CaptureMessageOn)
        //    {
        //        Master.GM.CaptureAttempt();
        //        CaptureMessageOn = true;
        //    }
        //    else
        //    {
        //        CaptureMessageOn = false;
        //        Master.GM.DialogueManager.CloseDialogue();
        //    }            
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (Master.GM.ScenarioData.LocationsDict.ContainsKey(other.gameObject.name))
        {
            _hasEnteredLocation++;
            if (_hasEnteredLocation == 1)
                Master.GM.ChangeScene(Master.GM.ScenarioData.LocationsDict[other.gameObject.name]);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Master.GM.ScenarioData.LocationsDict.ContainsKey(other.gameObject.name))
        {
            _hasEnteredLocation = _hasEnteredLocation == 1 ? 2 : 0;
        }
    }

    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.name == NamesList.ExitToMain)
    //    {
            
    //    }
    //    if (other.gameObject.tag == NamesList.Character)
    //    {
    //        Master.GM.DialogueManager.ShowDialogue();
    //        _dialogueCooldown.Start();
    //    }
    //}

    public enum AnimationType
    {
        map,
        location
    }
}
