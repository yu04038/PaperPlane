﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class plane : MonoBehaviour
{
    public float JUMP_AMOUNT = 100f;
    public float AIR = 10f;
    public static int life = 3;
    public AudioSource planeforjump;
    public AudioClip jumpingsound;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D planeRigidbody2D;
    private float timer = 0;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void Awake()
    {
        planeRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame

    void Update()
    {
        timer += Time.deltaTime;
        if(timer<63)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1f;
            }

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //캐릭터의 월드 좌표를 뷰포트 좌표계로 변환해준다.
            if (viewPos.y < 0f)
            {
                if(gameObject.layer == 10)
                {
                    viewPos.y = 0.5f;
                    planeRigidbody2D.velocity = Vector2.down;
                }
                else
                {
                    life--;
                    if (life >= 1)
                    {
                        gameObject.layer = 10;
                        Debug.Log("Hit");
                        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                        planeRigidbody2D.velocity = Vector2.down;
                        Invoke("offDamaged", 2);
                    }
                    if (life == 0)
                    {
                        Debug.Log("Die");
                        Time.timeScale = 0.1F;
                        Invoke("gameover", 0.11f);
                    }
                }
                viewPos.y = 0.5f;
            }

            if (viewPos.y > 1f)
            {
                if (gameObject.layer == 10)
                {
                    viewPos.y = 0.5f;
                    planeRigidbody2D.velocity = Vector2.up;
                }
                else
                {
                    life--;
                    if (life >= 1)
                    {
                        gameObject.layer = 10;
                        Debug.Log("Hit");
                        planeRigidbody2D.velocity = Vector2.up;
                        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                        Invoke("offDamaged", 2);
                    }
                    if (life == 0)
                    {
                        Debug.Log("Die");
                        Time.timeScale = 0.1F;
                        Invoke("gameover", 0.11f);
                    }
                }
                viewPos.y = 0.5f;
            }
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //다시 월드 좌표로 변환한다.
            transform.position = worldPos; //좌표를 적용한다.
            if ((Input.GetMouseButtonDown(0)))  //버튼을 누름.
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {  //UI이 위가 아니면.
                    if (Input.GetMouseButtonDown(0))
                    {
                        Jump();
                    }
                }
            }
        }
        else
        {
            SceneManager.LoadScene("level clear");
        }
    }

    public void Jump()
    {
        planeRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
        planeforjump.PlayOneShot(jumpingsound);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "spawner")
        {
            life--;
            if (life >= 1)
            {
                gameObject.layer = 10;
                Debug.Log("Hit");
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                Invoke("offDamaged", 2);
            }

            if (life == 0)
            {
                Debug.Log("Die");
                Time.timeScale = 0.1F;
                Invoke("gameover", 0.11f);
            }
        }
    }
    void offDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    void gameover()
    {
        SceneManager.LoadScene("Game Over");
    }
}
