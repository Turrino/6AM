using UnityEngine;
using UnityEngine.UI;

public class ListTextScript : MonoBehaviour
{
    public Text ListText;
    public bool Initialised;

    void Start()
    {
        ListText = GetComponent<Text>();
        ListText.enabled = false;
        Master.GM.ListUi = this;
        //Master.GM.SetupList();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Trigger();
        }
    }

    public void Initialise(bool restart = false)
    {
        if (Initialised && !restart)
            return;
        ListText.enabled = false;
        ListText.text = Master.GM.ScenarioData.InformationText;
        Initialised = true;
    }

    public void Trigger()
    {
        Initialise();
        ListText.enabled = !ListText.enabled;
    }
}
