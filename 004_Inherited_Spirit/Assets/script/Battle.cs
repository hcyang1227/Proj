using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle : MonoBehaviour
{
    int battle_action1 = -1; //逐步執行角色動畫
    int battle_char1 = 0; //要動作的角色
    int battle_foe1 = 0; //目標打擊角色
    float[,] battle_animate1; //角色的招式動畫
    int[][] everyone_speed; //同個回合所有人的速度排序，得到先後攻擊順序
    int roundnum = 1; //目前回合數
    int battle_order = -1; //目前輪到哪個順位的人攻擊

    System.Random rand = new System.Random(Guid.NewGuid().GetHashCode()); //定義亂數種子rand

    //定義生物，包含3位我方和1位敵方
    public class creature
    {
        public bool move_end; //人物移動結束的flag
        public int hpmax;  //人物最大HP值
        public int hp;  //人物HP值
        public int atk;  //人物攻擊值
        public int def;  //人物防禦值
        public int spd; //人物速度值
        public bool status_psn; //人物中毒狀態
        public int status_psn_round; //人物中毒剩下回合數
        public int status_psn_tm; //人物中毒時間點
        public float ori_x; //人物原本的位置x
        public float ori_y; //人物原本的位置y
        public float now_x; //人物動畫進行中的位置x
        public float now_y; //人物動畫進行中的位置y
    }

    //連結需要的物件
    public GameObject Canvas;
    public GameObject ObjEffect;

    public GameObject ObjPlayer1;
    public Animator AniPlayer1;
    public TextMeshProUGUI TxtPlayer1;
    public GameObject ObjPlayer2;
    public Animator AniPlayer2;
    public TextMeshProUGUI TxtPlayer2;
    public GameObject ObjPlayer3;
    public Animator AniPlayer3;
    public TextMeshProUGUI TxtPlayer3;
    public GameObject ObjEnemy;
    public Animator AniEnemy;
    public TextMeshProUGUI TxtEnemy;

    //創建角色
    creature op1 = new creature();
    creature op2 = new creature();
    creature op3 = new creature();
    creature oe = new creature();

    void Awake()
    {
        //角色位置數值初始化
        op1.ori_x = ObjPlayer1.transform.position.x;
        op1.ori_y = ObjPlayer1.transform.position.y;
        op1.now_x = ObjPlayer1.transform.position.x;
        op1.now_y = ObjPlayer1.transform.position.y;
        op2.ori_x = ObjPlayer2.transform.position.x;
        op2.ori_y = ObjPlayer2.transform.position.y;
        op2.now_x = ObjPlayer2.transform.position.x;
        op2.now_y = ObjPlayer2.transform.position.y;
        op3.ori_x = ObjPlayer3.transform.position.x;
        op3.ori_y = ObjPlayer3.transform.position.y;
        op3.now_x = ObjPlayer3.transform.position.x;
        op3.now_y = ObjPlayer3.transform.position.y;
        oe.ori_x = ObjEnemy.transform.position.x;
        oe.ori_y = ObjEnemy.transform.position.y;
        oe.now_x = ObjEnemy.transform.position.x;
        oe.now_y = ObjEnemy.transform.position.y;
        //temp!!!角色能力數值初始化
        op1.hpmax = 50+rand.Next(0,150);
        op1.hp = op1.hpmax;
        op1.atk = rand.Next(10,20);
        op1.def = rand.Next(10,20);
        op1.spd = rand.Next(30,50);
        op2.hpmax = 50+rand.Next(0,150);
        op2.hp = op2.hpmax;
        op2.atk = rand.Next(10,20);
        op2.def = rand.Next(10,20);
        op2.spd = rand.Next(30,50)+50;
        op3.hpmax = 50+rand.Next(0,150);
        op3.hp = op3.hpmax;
        op3.atk = rand.Next(10,20);
        op3.def = rand.Next(10,20);
        op3.spd = rand.Next(30,50)+100;
        oe.hpmax = 50+rand.Next(0,150);
        oe.hp = oe.hpmax;
        oe.atk = rand.Next(10,20);
        oe.def = rand.Next(10,20);
        oe.spd = rand.Next(30,50)+150;
    }

    void Start()
    {
        //temp!!!
        AniPlayer1.Play("KateWalk");
    }

    void Update()
    {

        //總是更新數值
        TxtPlayer1.text = op1.hp.ToString() + "/" + op1.hpmax.ToString();
        TxtPlayer2.text = op2.hp.ToString() + "/" + op2.hpmax.ToString();
        TxtPlayer3.text = op3.hp.ToString() + "/" + op3.hpmax.ToString();
        TxtEnemy.text = oe.hp.ToString() + "/" + oe.hpmax.ToString();

        //如果在副本模式中發現所有動作已經結束
        if (Field.battle_flag && Field.battle_mode && (battle_action1 == -1) && (battle_order == -1))
        {
            battle_order = 0;
            //先比較彼此的速度，用二維陣列排序
            everyone_speed = new int[][] {
                new int[] {1, 2, 3, -1},
                new int[] {op1.spd, op2.spd, op3.spd, oe.spd}
            };
            Sort<int>(everyone_speed, 2);
        }
        //按照速度高低依序行動
        if (Field.battle_flag && Field.battle_mode && (battle_action1 == -1) && (battle_order >= 0) && (battle_order <= everyone_speed[0].GetLength(0)-1))
        {
            switch(everyone_speed[0][everyone_speed[0].GetLength(0)-battle_order-1])
            {
                case 1:
                    battle_char1 = 1;
                    battle_foe1 = -1;
                    battle_animate1 = a03_normal_attack;
                    //中毒扣HP機制
                    if (op1.status_psn_round > 1)
                    {
                        op1.hp -= rand.Next(4,8);
                        op1.status_psn_round -= 1;
                    }
                    else if (op1.status_psn_round == 1)
                    {
                        op1.hp -= rand.Next(4,8);
                        op1.status_psn_round = 0;
                        op1.status_psn_tm = 0;
                        op1.status_psn = false;
                    }
                    break;
                case 2:
                    battle_char1 = 2;
                    battle_foe1 = -1;
                    battle_animate1 = a03_normal_attack;
                    //中毒扣HP機制
                    if (op2.status_psn_round > 1)
                    {
                        op2.hp -= rand.Next(4,8);
                        op2.status_psn_round -= 1;
                    }
                    else if (op2.status_psn_round == 1)
                    {
                        op2.hp -= rand.Next(4,8);
                        op2.status_psn_round = 0;
                        op2.status_psn_tm = 0;
                        op2.status_psn = false;
                    }
                    break;
                case 3:
                    battle_char1 = 3;
                    battle_foe1 = -1;
                    battle_animate1 = a03_normal_attack;
                    //中毒扣HP機制
                    if (op3.status_psn_round > 1)
                    {
                        op3.hp -= rand.Next(4,8);
                        op3.status_psn_round -= 1;
                    }
                    else if (op3.status_psn_round == 1)
                    {
                        op3.hp -= rand.Next(4,8);
                        op3.status_psn_round = 0;
                        op3.status_psn_tm = 0;
                        op3.status_psn = false;
                    }
                    break;
                case -1:
                    battle_char1 = -1;
                    battle_foe1 = rand.Next(1,everyone_speed[0].GetLength(0)-1);
                    battle_animate1 = a04_normal_attack;
                    //中毒扣HP機制
                    if (oe.status_psn_round > 1)
                    {
                        oe.hp -= rand.Next(4,8);
                        oe.status_psn_round -= 1;
                    }
                    else if (oe.status_psn_round == 1)
                    {
                        oe.hp -= rand.Next(4,8);
                        oe.status_psn_round = 0;
                        oe.status_psn_tm = 0;
                        oe.status_psn = false;
                    }
                    break;
                default:
                    break;
            }
            battle_action1 = 0;
            battle_order += 1;
        }
        //如果當回合所有人都行動過，則換下一回合
        if (Field.battle_flag && Field.battle_mode && (battle_action1 == -1) && (battle_order > everyone_speed[0].GetLength(0)-1))
        {
            battle_order = -1;
            roundnum += 1;
        }

        //角色狀態顏色變化
        if (!Field.frame_unchange)
        {
            if (op1.status_psn)
                ChangeColor("psn", ObjPlayer1, op1);
            if (op2.status_psn)
                ChangeColor("psn", ObjPlayer2, op2);
            if (op3.status_psn)
                ChangeColor("psn", ObjPlayer3, op3);
            if (oe.status_psn)
                ChangeColor("psn", ObjEnemy, oe);
            if (!op1.status_psn)
                ObjPlayer1.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0f, 0f, 1f);
            if (!op2.status_psn)
                ObjPlayer2.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0f, 0f, 1f);
            if (!op3.status_psn)
                ObjPlayer3.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0f, 0f, 1f);
            if (!oe.status_psn)
                ObjEnemy.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0f, 0f, 1f);
        }


        if (battle_action1 != -1 && !Field.frame_unchange)
        {
            if (battle_action1 < battle_animate1.GetLength(0))
            {
                //執行角色的移動
                //(ObjPlayer2, op2.now_x, op2.now_y, op2.move_end) = Movement(ObjPlayer2, op2.ori_x, op2.ori_y, op2.now_x, op2.now_y, op2.pos_x, op2.pos_y, op2.speed, op2.speedz, op2.move_mode, op2.move_end);
                switch (battle_char1)
                {
                    case 1:
                        (ObjPlayer1, op1.now_x, op1.now_y, op1.move_end) =
                        Movement(ObjPlayer1, op1.ori_x, op1.ori_y, op1.now_x, op1.now_y,
                            get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]),
                            get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]),
                            battle_animate1[battle_action1,4], battle_animate1[battle_action1,5], (int)battle_animate1[battle_action1,0], op1.move_end);
                        if ((int)battle_animate1[battle_action1,6] == 1)
                            ObjPlayer1.GetComponent<SpriteRenderer>().flipX = false;
                        else if ((int)battle_animate1[battle_action1,6] == -1)
                            ObjPlayer1.GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 2:
                        (ObjPlayer2, op2.now_x, op2.now_y, op2.move_end) =
                        Movement(ObjPlayer2, op2.ori_x, op2.ori_y, op2.now_x, op2.now_y,
                            get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]),
                            get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]),
                            battle_animate1[battle_action1,4], battle_animate1[battle_action1,5], (int)battle_animate1[battle_action1,0], op2.move_end);
                        if ((int)battle_animate1[battle_action1,6] == 1)
                            ObjPlayer2.GetComponent<SpriteRenderer>().flipX = false;
                        else if ((int)battle_animate1[battle_action1,6] == -1)
                            ObjPlayer2.GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 3:
                        (ObjPlayer3, op3.now_x, op3.now_y, op3.move_end) =
                        Movement(ObjPlayer3, op3.ori_x, op3.ori_y, op3.now_x, op3.now_y,
                            get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]),
                            get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]),
                            battle_animate1[battle_action1,4], battle_animate1[battle_action1,5], (int)battle_animate1[battle_action1,0], op3.move_end);
                        if ((int)battle_animate1[battle_action1,6] == 1)
                            ObjPlayer3.GetComponent<SpriteRenderer>().flipX = false;
                        else if ((int)battle_animate1[battle_action1,6] == -1)
                            ObjPlayer3.GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case -1:
                        (ObjEnemy, oe.now_x, oe.now_y, oe.move_end) =
                        Movement(ObjEnemy, oe.ori_x, oe.ori_y, oe.now_x, oe.now_y,
                            get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]),
                            get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]),
                            battle_animate1[battle_action1,4], battle_animate1[battle_action1,5], (int)battle_animate1[battle_action1,0], oe.move_end);
                        if ((int)battle_animate1[battle_action1,6] == 1)
                            ObjEnemy.GetComponent<SpriteRenderer>().flipX = true;
                        else if ((int)battle_animate1[battle_action1,6] == -1)
                            ObjEnemy.GetComponent<SpriteRenderer>().flipX = false;
                        break;
                }
                if (op1.move_end)
                {
                    if ((int)battle_animate1[battle_action1,0] >= 0)
                    {
                        op1.ori_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op1.ori_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                        op1.now_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op1.now_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                    }
                    battle_action1 += 1;
                    op1.move_end = false;
                }
                if (op2.move_end)
                {
                    if ((int)battle_animate1[battle_action1,0] >= 0)
                    {
                        op2.ori_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op2.ori_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                        op2.now_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op2.now_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                    }
                    battle_action1 += 1;
                    op2.move_end = false;
                }
                if (op3.move_end)
                {
                    if ((int)battle_animate1[battle_action1,0] >= 0)
                    {
                        op3.ori_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op3.ori_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                        op3.now_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        op3.now_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                    }
                    battle_action1 += 1;
                    op3.move_end = false;
                }
                if (oe.move_end)
                {
                    if ((int)battle_animate1[battle_action1,0] >= 0)
                    {
                        oe.ori_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        oe.ori_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                        oe.now_x = get_pos_x(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,2]);
                        oe.now_y = get_pos_y(battle_char1, (int)battle_animate1[battle_action1,1]*battle_foe1, battle_animate1[battle_action1,3]);
                    }
                    battle_action1 += 1;
                    oe.move_end = false;
                }
            }
            else
            {
                switch (battle_char1)
                {
                    case 1:
                        ObjPlayer1.GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case 2:
                        ObjPlayer2.GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case 3:
                        ObjPlayer3.GetComponent<SpriteRenderer>().flipX = false;
                        break;
                    case -1:
                        ObjEnemy.GetComponent<SpriteRenderer>().flipX = true;
                        break;
                }
                battle_action1 = -1;
            }
        }
    }

    void ChangeColor(string str, GameObject Obj, creature person)
    {
        if (str == "psn")
            Obj.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0.778f, (Mathf.Cos((Field.SysFrameCount - person.status_psn_tm + 45f)/45f*Mathf.PI)+1f)*0.4f, 1f);
    }

    //移動指令解析：{move_mode, character, pos_x, pos_y, speed, speedz, direction, -hp}
    //其中character=0代表只看指定的pos_x, pos_y，=1代表Player，=-1代表Enemy，10代表角色原來位置
    //其中direction=1代表往前，-1代表往後，0代表方向不變
    //普通攻擊(指數移動)
    float[,] a01_normal_attack = new float[4,8]
    {{0f, 1f, 20f, 0f, 20f, 0f, 1f, 0f},
    {-1f, 1f, 0f, 50f, 0f, 0f, 0f, 1f},
    {4f, 1f, 0f, 0f, 5f, 5f, 1f, 0f},
    {0f, 10f, 0f, 0f, 5f, 0f, -1f, 0f}};
    //普通攻擊(跳砍)
    float[,] a02_normal_attack = new float[4,8]
    {{2f, 1f, 20f, 0f, 10f, 7f, 1f, 0f},
    {-1f, 1f, 0f, 50f, 0f, 0f, 0f, 1f},
    {4f, 1f, 0f, 0f, 5f, 5f, 1f, 0f},
    {0f, 10f, 0f, 0f, 5f, 0f, -1f, 0f}};
    //普通攻擊(跳跳跳砍)
    float[,] a03_normal_attack = new float[6,8]
    {{2f, 1f, 200f, 0f, 5f, 3f, 1f, 0f},
    {2f, 1f, 130f, 0f, 5f, 6f, 1f, 0f},
    {2f, 1f, 0f, 20f, 5f, 9f, 1f, 0f},
    {-1f, 1f, 0f, 50f, 0f, 0f, 0f, 1f},
    {4f, 1f, 0f, 0f, 5f, 5f, 1f, 0f},
    {0f, 10f, 0f, 0f, 5f, 0f, -1f, 0f}};
    //普通毒攻擊(線性移動)
    float[,] a04_normal_attack = new float[4,8]
    {{1f, 1f, 20f, 0f, 20f, 0f, 1f, 0f},
    {-2f, 1f, 0f, 50f, 0f, 0f, 0f, 1f},
    {4f, 1f, 0f, 0f, 5f, 5f, 1f, 0f},
    {0f, 10f, 0f, 0f, 5f, 0f, -1f, 0f}};

    float get_pos_x(int battle_char, int char_pos, float pos_x)
    {
        switch(char_pos)
        {
            case 1:
                return ObjPlayer1.transform.position.x + pos_x + 50f;
            case 2:
                return ObjPlayer2.transform.position.x + pos_x + 50f;
            case 3:
                return ObjPlayer3.transform.position.x + pos_x + 50f;
            case -1:
                return ObjEnemy.transform.position.x - pos_x - 50f;
            case -10:
            case 10:
            case 20:
            case 30:
                if (battle_char > 0)
                    return -150f;
                else
                    return 150f;
            default:
                return pos_x;
        }
    }

    float get_pos_y(int battle_char, int char_pos, float pos_y)
    {
        switch(char_pos)
        {
            case 1:
                return ObjPlayer1.transform.position.y + pos_y;
            case 2:
                return ObjPlayer2.transform.position.y + pos_y;
            case 3:
                return ObjPlayer3.transform.position.y + pos_y;
            case -1:
                return ObjEnemy.transform.position.y + pos_y;
            case -10:
            case 10:
            case 20:
            case 30:
                if (battle_char == 1)
                    return -170f;
                else if (battle_char == 2)
                    return -90f;
                else if (battle_char == 3)
                    return -250f;
                else
                    return -170f;
            default:
                return pos_y;
        }
    }

    (GameObject, float, float, bool) Movement(GameObject Obj, float ori_x, float ori_y, float now_x, float now_y, float pos_x, float pos_y, float speed, float speedz, int move_mode, bool move_end)
    {
        float delx = pos_x - ori_x;
        float dely = pos_y - ori_y;
        float dist = Mathf.Pow(Mathf.Pow(delx, 2f) + Mathf.Pow(dely, 2f), 0.5f);
        float frm = dist / speed;
        float now_delx = now_x - ori_x;
        float now_dely = now_y - ori_y;
        float now_dist = Mathf.Pow(Mathf.Pow(now_delx, 2f) + Mathf.Pow(now_dely, 2f), 0.5f);
        float now_frm = now_dist / speed;

        switch (move_mode)
        {
            case 0: //人物指數移動至定點
                if (!Field.frame_unchange && !move_end && (Obj.transform.position.x > pos_x + 1))
                    Obj.transform.position -= new Vector3(Mathf.Pow(1.2f,(Obj.transform.position.x - pos_x)/50f)*speed-speed+1, 0, 0);
                else if (!Field.frame_unchange && !move_end && (Obj.transform.position.x < pos_x - 1))
                    Obj.transform.position += new Vector3(Mathf.Pow(1.2f,-(Obj.transform.position.x - pos_x)/50f)*speed-speed+1, 0, 0);
                if (!Field.frame_unchange && !move_end && (Obj.transform.position.y > pos_y + 1))
                    Obj.transform.position -= new Vector3(0, Mathf.Pow(1.2f,(Obj.transform.position.y - pos_y)/50f)*speed-speed+1, 0);
                else if (!Field.frame_unchange && !move_end && (Obj.transform.position.y < pos_y - 1))
                    Obj.transform.position += new Vector3(0, Mathf.Pow(1.2f,-(Obj.transform.position.y - pos_y)/50f)*speed-speed+1, 0);
                if (!Field.frame_unchange && !move_end && (Obj.transform.position.x <= pos_x + 1) && (Obj.transform.position.x >= pos_x - 1) && (Obj.transform.position.y <= pos_y + 1) && (Obj.transform.position.y >= pos_y - 1) )
                    move_end = true;
                break;

            case 1: //人物線性移動至定點
                if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x < pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x > pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x > pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x < pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y < pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y > pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y > pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y < pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end)
                    Obj.transform.position = new Vector3(now_x, now_y, Obj.transform.position.z);
                if (!Field.frame_unchange && !move_end && (now_x == pos_x) && (now_y == pos_y))
                    move_end = true;
                break;

            case 2: //人物拋物線移動至定點
                if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x < pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x > pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x > pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x < pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y < pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y > pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y > pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y < pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end)
                    Obj.transform.position = new Vector3(now_x, now_y + ( -1 * Mathf.Pow(now_frm, 2f) + now_frm * frm )*Mathf.Pow(20f/frm, 2f)*(speedz/3f), Obj.transform.position.z);
                if (!Field.frame_unchange && !move_end && (now_x == pos_x) && (now_y == pos_y))
                    move_end = true;
                break;

            case 3: //人物飄移(sine)移動至定點
                if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x < pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x > pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x > pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x < pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y < pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y > pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y > pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y < pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end)
                    Obj.transform.position = new Vector3(now_x, now_y + Mathf.Sin(now_frm/frm*4*Mathf.PI)*(speedz*10f), Obj.transform.position.z);
                if (!Field.frame_unchange && !move_end && (now_x == pos_x) && (now_y == pos_y))
                    move_end = true;
                break;

            case 4: //人物亂數振動移動至定點
                if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x < pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x < pos_x ) && (now_x > pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x > pos_x))
                    now_x += speed * delx / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_x > pos_x ) && (now_x < pos_x))
                    now_x = pos_x;
                if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y < pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y < pos_y ) && (now_y > pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y > pos_y))
                    now_y += speed * dely / dist;
                else if (!Field.frame_unchange && !move_end && ( ori_y > pos_y ) && (now_y < pos_y))
                    now_y = pos_y;
                if (!Field.frame_unchange && !move_end)
                    Obj.transform.position = new Vector3(now_x + ((rand.Next(0,1000)-500)/200f)*speedz, now_y + ((rand.Next(0,1000)-500)/200f)*speedz, Obj.transform.position.z);
                if (!Field.frame_unchange && !move_end && (now_x == pos_x) && (now_y == pos_y))
                    move_end = true;
                break;

            case -1: //產生普通打擊效果
                if (!Field.frame_unchange && !move_end)
                {
                    MinusHP();
                    GameObject ObjEff;
                    ObjEff = (GameObject)Instantiate(ObjEffect, new Vector3(pos_x, pos_y, 0), new Quaternion(), Canvas.transform);
                    ObjEff.GetComponent<Animator>().Play("Eff_Hit");
                    move_end = true;
                }
                break;

            case -2: //產生毒打擊效果
                if (!Field.frame_unchange && !move_end)
                {
                    MinusHP();
                    AddStatus("psn", 3);
                    GameObject ObjEff;
                    ObjEff = (GameObject)Instantiate(ObjEffect, new Vector3(pos_x, pos_y, 0), new Quaternion(), Canvas.transform);
                    ObjEff.GetComponent<Animator>().Play("Eff_Psn");
                    move_end = true;
                }
                break;
        }

        return (Obj, now_x, now_y, move_end);
    }

    void MinusHP()
    {
        switch(battle_foe1)
        {
            case 1:
                op1.hp -= Math.Max(0, oe.atk - op1.def) + rand.Next(1,5);
                break;
            case 2:
                op2.hp -= Math.Max(0, oe.atk - op2.def) + rand.Next(1,5);
                break;
            case 3:
                op3.hp -= Math.Max(0, oe.atk - op3.def) + rand.Next(1,5);
                break;
            case -1:
                if (battle_char1 == 1)
                    oe.hp -= Math.Max(0, op1.atk - oe.def) + rand.Next(1,5);
                if (battle_char1 == 2)
                    oe.hp -= Math.Max(0, op2.atk - oe.def) + rand.Next(1,5);
                if (battle_char1 == 3)
                    oe.hp -= Math.Max(0, op3.atk - oe.def) + rand.Next(1,5);
                break;
            default:
                break;
        }
    }

    void AddStatus(string str, int rounds)
    {
        if (str == "psn")
        {
            switch(battle_foe1)
            {
                case 1:
                    op1.status_psn = true;
                    op1.status_psn_tm = Field.SysFrameCount;
                    op1.status_psn_round = rounds;
                    break;
                case 2:
                    op2.status_psn = true;
                    op2.status_psn_tm = Field.SysFrameCount;
                    op2.status_psn_round = rounds;
                    break;
                case 3:
                    op3.status_psn = true;
                    op3.status_psn_tm = Field.SysFrameCount;
                    op3.status_psn_round = rounds;
                    break;
                case -1:
                    oe.status_psn = true;
                    oe.status_psn_tm = Field.SysFrameCount;
                    oe.status_psn_round = rounds;
                    break;
                default:
                    break;
            }
        }

    }

    private static void Sort<T>(T[][] data, int col)
    {
        Comparer<T> comparer = Comparer<T>.Default;
        Array.Sort<T[]>(data, (x,y) => comparer.Compare(x[col],y[col]));
    }
}
