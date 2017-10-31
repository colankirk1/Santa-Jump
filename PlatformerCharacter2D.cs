using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlatformerCharacter2D : MonoBehaviour
{
    private float maxVerticalSpeed = 10f;
    private float jumpForce = 550f;
    private bool playerAirControl = true;

    private bool isGrounded;  
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;
    private bool facingRight = true;
    private bool hasJumped;
    public GameObject backgroundPrefab;
    private float backgroundOffsetValue = 8.92f;
    private int backgroundCount = 2;
    public int score;
    private bool canCollide = true;
    public Text scoreText;
    private int scoreModifier = 1;
    public GameObject gameOverMenu;
    private bool canJump = true;
    public ParticleSystem ps;
    private int psStart = 2;    //number of presents to hit before changing the particle system

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        hasJumped = false;
        isGrounded = true;
    }

    //Get player input
    private void Update()
    {
        if (!hasJumped)
        {
            hasJumped = Input.GetButtonDown("Jump");
        }
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        Move(h, hasJumped);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        scoreText.text = "Score: " + score.ToString();
    }


    public void Move(float move, bool jump)
    {
        if (isGrounded || playerAirControl)
        {
            m_Anim.SetFloat("Speed", Mathf.Abs(move));
            m_Rigidbody2D.velocity = new Vector2(move*maxVerticalSpeed, m_Rigidbody2D.velocity.y);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }
        // If the player should jump...
        if (canJump && isGrounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            isGrounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    //Adjust jump height based on which part of the present was hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                hasJumped = false;
                isGrounded = true;
                m_Anim.SetBool("Ground", true);
                break;
            case "Present":
                if (canCollide)
                {
                    canCollide = false;
                    StartCoroutine(canCollideTrue(.1f));
                    psStart--;
                    if(psStart == 0)
                    {
                        ps.emissionRate = 15;
                    }
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                    if(m_Rigidbody2D.velocity.y < 0)
                    {
                        m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce*1.1f));
                    }
                    else
                    {
                        m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                    }
                    collision.gameObject.GetComponent<PresentScript>().destroy(scoreModifier);
                    score += scoreModifier;
                    scoreModifier++;
                }
                break;
            case "PresentBottom":
                if (canCollide)
                {
                    canCollide = false;
                    StartCoroutine(canCollideTrue(.1f));
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                    m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce * 1.15f));
                    collision.transform.parent.gameObject.GetComponent<PresentScript>().destroy(scoreModifier);
                    score += scoreModifier;
                    scoreModifier++;
                }
                break;
            case "EndGame":
                canCollide = false;
                canJump = false;
                ps.emissionRate = 5;
                gameOverMenu.SetActive(true);
                scoreText.rectTransform.anchorMin = new Vector2(.5f, .5f);
                scoreText.rectTransform.anchorMax = new Vector2(.5f, .5f);
                scoreText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                scoreText.alignment = TextAnchor.MiddleCenter;
                scoreText.rectTransform.anchoredPosition = new Vector2(0, 37);
                break;
        }
    }

    public void spawnBackground()
    {
        backgroundCount++;
        GameObject x = Instantiate(backgroundPrefab, new Vector3(0, transform.position.y), Quaternion.identity);
        x.GetComponent<BackgroundScript>().changeOffset(backgroundCount * backgroundOffsetValue);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator canCollideTrue(float x)
    {
        yield return new WaitForSeconds(x);
        canCollide = true;
    }
}
