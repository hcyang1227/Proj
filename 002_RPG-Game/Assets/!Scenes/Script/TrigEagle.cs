using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigEagle : MonoBehaviour
{
    public float m_HP = 50.0f;
    public float invincible = 0.0f;
    public float PosX;
    public float PosY;
    public float FloatTime;
    public float WaitTime = 7.0f;
    public float AttackTime = 0.0f;
    public float AttackTimeEnd = 0.0f;
    public Animator m_animator;
    public GameObject EnemyDeath;

    HeroKnight _HeroKnight;
    Dialog _Dialog;
    RandomPlatform _RandomPlatform;
    // Start is called before the first frame update
    void Start()
    {
        PosX = this.gameObject.transform.position.x;
        PosY = this.gameObject.transform.position.y;
        m_animator = GetComponent<Animator>();
        _HeroKnight = GameObject.Find("HeroKnight").GetComponent<HeroKnight>();
        _Dialog = GameObject.Find("DialogPlatform").GetComponent<Dialog>();
        _RandomPlatform = GameObject.Find("GroundGrid").GetComponent<RandomPlatform>();

    }
    void Update()
    {
        //無敵時間
        if (invincible > 0)
            invincible -= Time.deltaTime;

        //若非攻擊狀態下，加入老鷹浮空計時器
        if (AttackTime == 0.0f && AttackTimeEnd == 0.0f)
            FloatTime += Time.deltaTime;

        //非攻擊狀態下，老鷹會在空中飄來飄去
        if (AttackTime == 0.0f && AttackTimeEnd == 0.0f && !this.m_animator.GetNextAnimatorStateInfo(0).IsName("Injure") && !this.m_animator.GetNextAnimatorStateInfo(0).IsName("Attack"))
            this.gameObject.transform.position = new Vector3(PosX, PosY + Mathf.Sin(FloatTime * 7) * 10, 0);

        //判定主角走進老鷹警戒線範圍(+-100,+-250)時，扣WaitTime
        if ((this.transform.position.x < _HeroKnight.transform.position.x + 100) && (this.transform.position.x > _HeroKnight.transform.position.x - 100))
        {
            if ((this.transform.position.y < _HeroKnight.transform.position.y + 250) && (this.transform.position.y > _HeroKnight.transform.position.y - 250))
            {
                WaitTime -= Time.deltaTime;
            }
        }

        //當WaitTime小於零，老鷹進行攻擊
        if (WaitTime < 0)
        {
            WaitTime = 3.0f + UnityEngine.Random.Range(2.0f, 5.0f);
            this.m_animator.SetTrigger("Attack");
            AttackTimeEnd = 2.0f;
            AttackTime = 1.0f;
        }

        //第一段攻擊，老鷹直接朝向主角飛進
        if (AttackTime > 0)
        {
            AttackTime -= Time.deltaTime;
            PosX = _HeroKnight.gameObject.transform.position.x;
            PosY = _HeroKnight.gameObject.transform.position.y + 30;

            if ((this.gameObject.transform.position.x < PosX) && (Mathf.Abs(this.gameObject.transform.position.x - PosX) > 2))
            {
                this.gameObject.transform.position += new Vector3(1.0f, 0, 0);
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if ((this.gameObject.transform.position.x > PosX) && (Mathf.Abs(this.gameObject.transform.position.x - PosX) > 2))
            {
                this.gameObject.transform.position -= new Vector3(1.0f, 0, 0);
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (this.gameObject.transform.position.y < PosY)
                this.gameObject.transform.position += new Vector3(0, 1.0f, 0);
            else
                this.gameObject.transform.position -= new Vector3(0, 1.0f, 0);
        }

        //第二段攻擊，老鷹飛離主角並抵達空中，並設定為主角y值+100位置，讓主角仍可跳躍攻擊到
        if (AttackTime < 0 && AttackTimeEnd > 0)
        {
            PosX = _HeroKnight.gameObject.transform.position.x + UnityEngine.Random.Range(0.0f, 200.0f) - 100.0f;
            PosY = _HeroKnight.gameObject.transform.position.y + 100;
            AttackTime = 0.0f;
        }
        if (AttackTime == 0.0f && AttackTimeEnd > 0)
        {
            AttackTimeEnd -= Time.deltaTime;

            if ((this.gameObject.transform.position.x < PosX) && (Mathf.Abs(this.gameObject.transform.position.x - PosX) > 2))
            {
                this.gameObject.transform.position += new Vector3(1.0f, 0, 0);
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if ((this.gameObject.transform.position.x > PosX) && (Mathf.Abs(this.gameObject.transform.position.x - PosX) > 2))
            {
                this.gameObject.transform.position -= new Vector3(1.0f, 0, 0);
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (this.gameObject.transform.position.y < PosY)
                this.gameObject.transform.position += new Vector3(0, 1.0f, 0);
            else
                this.gameObject.transform.position -= new Vector3(0, 1.0f, 0);
        }
        else if (AttackTime == 0.0f && AttackTimeEnd < 0)
        {
            AttackTimeEnd = 0.0f;
            PosX = this.gameObject.transform.position.x;
            PosY = this.gameObject.transform.position.y;
        }

        //若老鷹被擊倒，則獲得金錢，產生敵方死亡後的爆發動畫
        if (m_HP < 0)
        {
            _HeroKnight.m_Money += UnityEngine.Random.Range(50, 60);
            Instantiate(EnemyDeath, this.gameObject.transform.position, this.gameObject.transform.rotation);
            _Dialog.Monster -= 1;
            int EnmNum = 0;
            foreach (GameObject Enemy in _RandomPlatform.EnemyObj)
            {
                if(Enemy == this.gameObject)
                    _RandomPlatform.EnemyObj[EnmNum] = null;
                EnmNum++;
            }

            Destroy(this.gameObject);
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        //老鷹攻擊主角
        if (other.tag == "Player" && !_HeroKnight.m_rolling && _HeroKnight.invincibleTime <= 0 && this.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _HeroKnight.m_defense == false && _HeroKnight.m_attack == false)
        {
            _HeroKnight.m_HP -= 15.0f + UnityEngine.Random.Range(0.0f, 5.0f);
            _HeroKnight.m_animator.SetTrigger("Hurt");
            _HeroKnight.invincibleTime = 1.0f;
        }
        //主角攻擊老鷹
        if (other.tag == "Player" && _HeroKnight.m_defense == false && _HeroKnight.m_attack == true && this.invincible <= 0 && _HeroKnight.m_facingDirection == 1)
        {
            this.gameObject.transform.position += new Vector3(30.0f, 0, 0);
            PosX = this.gameObject.transform.position.x;
            PosY = this.gameObject.transform.position.y;
            this.m_HP -= 20.0f + UnityEngine.Random.Range(0.0f, 5.0f);
            this.m_animator.SetTrigger("Injure");
            this.AttackTime = 0.0f;
            this.AttackTimeEnd = 0.0f;
            this.WaitTime = 3.0f + UnityEngine.Random.Range(2.0f, 5.0f);
            this.invincible = 1.0f;
            this.FloatTime = 0;
        }
        if (other.tag == "Player" && _HeroKnight.m_defense == false && _HeroKnight.m_attack == true && this.invincible <= 0 && _HeroKnight.m_facingDirection == -1)
        {
            this.gameObject.transform.position += new Vector3(-30.0f, 0, 0);
            PosX = this.gameObject.transform.position.x;
            PosY = this.gameObject.transform.position.y;
            this.m_HP -= 20.0f + UnityEngine.Random.Range(0.0f, 5.0f);
            this.m_animator.SetTrigger("Injure");
            this.AttackTime = 0.0f;
            this.AttackTimeEnd = 0.0f;
            this.WaitTime = 3.0f + UnityEngine.Random.Range(2.0f, 5.0f);
            this.invincible = 1.0f;
            this.FloatTime = 0;
        }
    }
}