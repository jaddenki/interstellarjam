using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public Sprite[] timeSprites;
    public SpriteRenderer timeRenderer;
    public AudioSource ETea;

    public float timeLeft = 300f;
    public float test = 300f;
    int change = 0;
    public bool isDay;

    public float difficulty;

    // Start is called before the first frame update
    void Start()
    {
        timeRenderer.sprite = timeSprites[0];
        ETea.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if(test >= timeLeft + (300.0f / 9))
        {
            isDay = true;
            change++;
            test = timeLeft;
            if(change >= 10)
            {
                change = 0;
            }
            timeRenderer.sprite = timeSprites[change];
        }

        difficulty = (-0.037037f)*(change-4.5f)*(change-4.5f)+1.75f; //fucken magic


        if (timeLeft <= 0.0f)
        {
            timeRenderer.sprite = timeSprites[9];
            isDay = false;
            StartCoroutine(wait());
            SceneManager.LoadScene("End2");
        }


    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(12);
    }
}
