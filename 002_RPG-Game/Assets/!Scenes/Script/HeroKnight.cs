using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class HeroKnight : MonoBehaviour
{

    public float m_HP = 100.0f;
    public float m_PreHP = 100.0f;
    public int m_Money = 0;
    public int m_Potion = 0;
    [SerializeField] float m_speed = 90.0f;
    [SerializeField] float m_jumpForce = 180.0f;
    [SerializeField] float m_rollForce = 75.0f;
    [SerializeField] bool m_noBlood = false;
    public GameObject slideDust;
    public GameObject HeroKnight1;
    Dialog _Dialog;
    public Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    public bool m_grounded = false;
    public bool m_rolling = false;
    public int m_facingDirection = 1;
    public int m_currentAttack = 0;
    public float m_timeSinceAttack = 0.0f;
    public float m_delayToIdle = 0.0f;
    public bool m_attack;
    public bool m_defense;
    public float invincibleTime = 1.0f;
    public GameObject m_ImgHP;
    public GameObject TxtCoinPotion;


    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        _Dialog = GameObject.Find("DialogPlatform").GetComponent<Dialog>();
    }

    // Update is called once per frame
    void Update()
    {
        //開頭預設非攻擊狀態
        m_attack = false;

        //讓HP血條隨時更新
        m_ImgHP.GetComponent<Transform>().position += new Vector3(m_HP - m_PreHP, 0, 0);
        m_PreHP = m_HP;

        //更新目前持有金幣與藥水
        TxtCoinPotion.GetComponent<Text>().text = "金幣: "+m_Money+"元\n藥水: "+m_Potion+"瓶";

        //HP小於零時進入死亡且不能動
        if (m_HP < 0.0f)
        {
            m_HP = 0.0f;
            m_speed = 0.0f;
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }

        //Death
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            HeroKnight1.SetActive(false);
            invincibleTime = 1.0f;
        }

        if (invincibleTime <= 0.0f && m_HP <= 0.0f)
        {
            EditorSceneManager.LoadScene("GameoverScene");
        }

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            //-- Handle Animations --
            //Wall Slide
            m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()) );

        //Use Potion
        if (Input.GetKeyDown("e") && !m_rolling && invincibleTime <= 0.0f && m_Potion > 0)
        {
            m_Potion -= 1;
            m_HP = 100;
            invincibleTime = 1.0f;
        }

        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && m_HP > 0.0f)
        {
            m_attack = true;
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
            {
                m_currentAttack = 1;
            }

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling && m_HP > 0.0f)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            m_defense = true;
        }

        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
            m_defense = false;
        }

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && m_HP > 0.0f)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            Instantiate(slideDust, HeroKnight1.transform.position-new Vector3(m_facingDirection*20.0f,0,100), HeroKnight1.transform.rotation, HeroKnight1.transform);
        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling && m_HP > 0.0f && _Dialog.speak == false)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && m_HP > 0.0f)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
            {
                m_animator.SetInteger("AnimState", 0);
            }
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        m_rolling = false;
    }

    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
