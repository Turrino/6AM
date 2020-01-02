using UnityEngine;

public class Player : MonoBehaviour {

    public float Speed;
    public int SortingOrder;

    private Vector2 TargetPos = Vector2.zero;
    private bool Moving;

    void FixedUpdate()
    {
        if(!Moving && Input.GetMouseButton(0) && !Master.GM.PomaButtonOver)
        {
            TargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            var collider = hit.collider;
            if (collider == null)
                return;

            if (Master.GM.ScenarioData.LocationsDict.ContainsKey(hit.collider.gameObject.name)
                    || hit.collider.gameObject.name == "FinalLocCollider")
            {
                Moving = true;
                GetComponentInChildren<SpriteRenderer>().flipX = TargetPos.x > transform.position.x;
            }
        }

        if (Moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Master.GM.ScenarioData.LocationsDict.ContainsKey(other.gameObject.name))
        {
            Master.GM.ChangeScene(Master.GM.ScenarioData.LocationsDict[other.gameObject.name]);
            Moving = false;
            TargetPos = Vector2.zero;
        }
        else if (other.gameObject.name == "FinalLocCollider")
        {
            Master.GM.FinalLoc();
            Moving = false;
            TargetPos = Vector2.zero;
        }
    }
}
