using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class outroscript : MonoBehaviour
{
    public GameObject Sprites;
    public SpriteRenderer RolloSprite;
    float speed = 10f; 
    float amount = 0.05f;
    int frame;
    public SpriteRenderer RolloText;
    bool keepchant;
    public GameObject Tally;
    public Text TallyText;
    public Text ModeText;

    private void Start()
    {
        ModeText.text = Master.Difficulty == 0 ? "Easy-Peasy Mode >" 
            : Master.Difficulty == 1 ? "El Normal Mode >" : "Hurd Mode >";
        var chars = Master.GM.GetFinalChars();

        foreach (var item in Sprites.GetComponentsInChildren<SpriteRenderer>())
        {
            if (item.name.Contains("token"))
            {
                item.sprite = chars.Dequeue();
            }
        }
        TallyText.text = Master.GM.TotalTimeSpent.ToString(@"mm\:ss");



        InvokeRepeating("Chant", 0, 1f);
    }

    void Chant()
    {
        if (RolloText.enabled && keepchant)
        {
            
            keepchant = false;
        }
        else
        {
            RolloText.enabled = !RolloText.enabled;
            keepchant = true;
        }        
    }

    bool flip;

    private void Update()
    {
        if (RolloSprite.transform.position.x > 9)
            flip = false;

        if (RolloSprite.transform.position.x < -10 || flip)
        {
            flip = true;
            foreach (var item in Sprites.GetComponentsInChildren<SpriteRenderer>())
            {
                var y = item.transform.position.y;
                item.transform.position = Vector2.MoveTowards(item.transform.position, new Vector2(12, y), 2 * Time.deltaTime);
                item.flipX = !item.name.Contains("text");
                if (item.name.Contains("rollosprite"))
                {
                    item.transform.position = item.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
                }
            }
        }
        else
        {
            foreach (var item in Sprites.GetComponentsInChildren<SpriteRenderer>())
            {
                var y = item.transform.position.y;
                item.flipX = false;
                item.transform.position = Vector2.MoveTowards(item.transform.position, new Vector2(-12, y), 2 * Time.deltaTime);
                if (item.name.Contains("rollosprite"))
                {
                    item.transform.position = item.transform.position + new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
                }
            }
        }


        Tally.transform.position = Vector2.MoveTowards(Tally.transform.position,
            new Vector2(-5.5f, Tally.transform.position.y), 2 * Time.deltaTime);
    }
}
