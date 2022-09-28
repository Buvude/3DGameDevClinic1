using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameManager instance;
    private static int curAmmo,maxAmmo;
    protected static int curHealth, MaxHealthl;

    private static float Score;

    //undamaged and damaged heart icons
    private static GameObject un1, un2, un3, d1, d2, d3;
    private static GameObject AmmoText, ScoreCounter, redScreen, deathText;

    private static bool gameOver = false;

    private void Awake()
    {
        AmmoText = GameObject.Find("Ammo Text");
        ScoreCounter = GameObject.Find("Score Counter");
        redScreen = GameObject.Find("Red Screen");


        // grab references to all the ui hearts
        un1 = GameObject.Find("Health 1");
        d1 = GameObject.Find("Health 1 Damaged");
        un2 = GameObject.Find("Health 2");
        d2 = GameObject.Find("Health 2 Damaged");
        un3 = GameObject.Find("Health 3");
        d3 = GameObject.Find("Health 3 Damaged");


        curAmmo = 6;
        maxAmmo = 6;
        curHealth = 3;
        Score = 0;
        //set initial state of ui
        updateUi();

    }

    public static void playerTakeDamage()
    {
        //subtract one hp
        curHealth--;

        //end game is no more hp left
        if (curHealth <= 0)
            GameEnd();

       
       
        updateUi();

        return;
    }

    public static void reduceAmmo()
    {
        curAmmo--;
        updateUi();
        return;
    }

    public static void Reloaded()
    {
        curAmmo = 6;
        updateUi();
    }

    public static void RaptorKilled(float style)
    {
        //add one score point times a style modifier 
        Score+= 1*style;
        updateUi();
        return;
    }

    public static void updateUi()
    {
        //update Health
        if (curHealth == 3)
        {
            //all health undamaged enabled

            un1.gameObject.SetActive(true);
           
            d1.gameObject.SetActive(false);

           
            un2.gameObject.SetActive(true);
           
            d2.gameObject.SetActive(false);

           
            un3.gameObject.SetActive(true);
           
            d3.gameObject.SetActive(false);

        }
        else if(curHealth== 2)
        {
           un3.gameObject.SetActive(false);
           d3.gameObject.SetActive(true);

        }
        else if(curHealth == 1)
        {
           un2.gameObject.SetActive(false);
           d2.gameObject.SetActive(true);
        }
        else
        {
            un1.gameObject.SetActive(false);
            d1.gameObject.SetActive(true);
            GameEnd();
        }

        //UpdateAmmo
        AmmoText.GetComponent<TextMeshProUGUI>().text = curAmmo + "/" + maxAmmo;

        //update Score
        ScoreCounter.GetComponent<TextMeshProUGUI>().text = "Complaints Lodged: " + Score;
        return;
    }


    //handles game ending scenario
    private static void GameEnd()
    {
        //disable player controls
        gameOver = true;
        //fade screen in 

        //fade text in Score/ message

        //restart button?




        return;
    }


    private void Update()
    {

        if (gameOver)
        {

            GameObject.FindGameObjectWithTag("Player").SetActive(false);
            StartCoroutine(alphaFade());
        }

    }


   

    private IEnumerator alphaFade() {
        Image curColor = redScreen.GetComponent<Image>();
        TextMeshProUGUI text = GameObject.Find("Death Text").GetComponent<TextMeshProUGUI>();
        while (curColor.color.a < 255)
        {
            yield return new WaitForSeconds(.05f);
            
            curColor.color = new Color(curColor.color.r, curColor.color.g, curColor.color.b, curColor.color.a+.01f);
            text.color = new Color(curColor.color.r, curColor.color.g, curColor.color.b, curColor.color.a + .01f);
        }
        //enable you died text
    }

}
