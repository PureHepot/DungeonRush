using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButton : MonoBehaviour
{
    public Sprite Up;
    public Sprite Down;

    public string txt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            txt.Replace("\\n", "\n");
            GameApp.ViewManager.Open(ViewType.TalkingView, txt);
            GetComponent<SpriteRenderer>().sprite = Down;
            GameApp.SoundManager.PlayEffect("Talking", GameApp.PlayerManager.Player.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            GameApp.ViewManager.Close(ViewType.TalkingView);
            GetComponent<SpriteRenderer>().sprite = Up;
        }
    }
}
