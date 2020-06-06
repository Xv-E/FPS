using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;



public class WinningLogic : MonoBehaviour
{
    CharacterAttr[] players;
    int teamCode;
    int teamR = 0;
    int teamB = 0;
    Timer winCountDown = new Timer();
    int second = 10;
    GameObject text;
    GameObject playerCountText;
    bool t = false;

    // Start is called before the first frame update
    void Start()
    {
        //先获得己方队伍
        teamCode = PlayerPrefs.GetInt("team");
        //胜利倒计时
        winCountDown.Interval = 1000;
        winCountDown.Enabled = false;
        winCountDown.Elapsed += new System.Timers.ElapsedEventHandler(WaitForWin);
        text = Instantiate(Resources.Load("WinCountDownText") as GameObject);
        playerCountText = Instantiate(Resources.Load("WinCountDownText") as GameObject);
    }
    // Update is called once per frame
    void Update()
    {   //这里是被气死的localPlayer
        if (PlayerManager.localPlayer != null && t == false)
        {
            Transform localUI = null;
            foreach (Transform child in PlayerManager.localPlayer.transform)
            {
                if (child.gameObject.name.Equals("UI"))
                    localUI = child;
            }
            text.GetComponent<RectTransform>().parent = localUI;
            text.GetComponent<Text>().text = " ";
            text.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 100, 0);
            text.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
            text.SetActive(true);
            playerCountText.GetComponent<RectTransform>().parent = localUI;
            playerCountText.GetComponent<Text>().text = " ";
            playerCountText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(80, -80, 0);
            playerCountText.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            playerCountText.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            playerCountText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 50);
            playerCountText.GetComponent<Text>().color=UnityEngine.Color.red;
            playerCountText.SetActive(true);
            t = true;
        }
        //不断搜索两方队伍的所有player
        players = FindObjectsOfType<CharacterAttr>();
        int i = players.Length - 1;
        int tempB = 0, tempR = 0;
        while (i >= 0)
        {
            teamR = 0; teamB = 0;
            if (players[i].teamCode == 0)
            { tempB++; }
            else if (players[i].teamCode == 1)
            { tempR++; }
            i--;
        }
        teamB = tempB; teamR = tempR;
        if (teamCode == 0)
            playerCountText.GetComponent<Text>().text = "敌方数量：" + teamR + "\n我方数量：" + teamB;
        else if (teamCode == 1)
            playerCountText.GetComponent<Text>().text = "敌方数量：" + teamB + "\n我方数量：" + teamR;
        //当一方归零后启动倒计时，10秒内没有新增就退出
        if (teamB == 0 || teamR == 0)
        {
            winCountDown.Enabled = true;
            text.GetComponent<Text>().text = "胜利倒计时\n" + second.ToString();
        }
        else
        {
            winCountDown.Enabled = false;
            second = 10;
            text.GetComponent<Text>().text = " ";
        }
        if (second == 0)//原本想放在timer的事件函数里的归零判断
        {
            winCountDown.Enabled = false;
            if (PlayerManager.localPlayer.Equals(null))
            {

            }
            else
            {
                text.GetComponent<Text>().text = "在奋战下，你的队伍获得了胜利\n可以通过商店页面退出";
            }
        }

    }

    //计时器
    void WaitForWin(object sender, System.Timers.ElapsedEventArgs e)
    {
        second--;
        text.GetComponent<Text>().text = "胜利倒计时\n" + second.ToString();

    }

}
