using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Soccer : MonoBehaviour
{
    public UnityEvent GameStarted;
    public UnityEvent GameEnded;

    private List<Collider> players = new List<Collider>();
    [SerializeField] private Transform ball;
    [SerializeField] private Transform ballStartPoint;

    private int[] scores = new int[2];
    [SerializeField] private SoccerGoal[] goals = new SoccerGoal[2];

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !players.Contains(other))
        {
            players.Add(other);
            StartGame();
        }

        if (other.CompareTag("Ball"))
            StopCoroutine("ResetBallDelay");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && players.Contains(other))
        {
            players.Remove(other);
            StopGame();
        }

        if (other.CompareTag("Ball"))
            StartCoroutine("ResetBallDelay", 0.4f);
    }

    private void StopGame()
    {
        if (players.Count < 1)
        {
            GameEnded.Invoke();

            StopAllCoroutines();
            ball.gameObject.SetActive(false);

            scores[0] = 0; scores[1] = 0;
        }
    }

    private void StartGame()
    {
        if (players.Count >= 1 && ball.gameObject.activeSelf == false)
        {
            GameStarted.Invoke();
            ball.gameObject.SetActive(true);
            ResetBall();
        }
    }

    public void PointScored(int pSide)
    {
        scores[pSide]++;
        goals[pSide].UpdateText(scores[pSide]);
        ResetBall();
    }

    private void ResetBall()
    {
        if (ball != null)
        {
            ball.position = ballStartPoint.position;
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private IEnumerator ResetBallDelay(float pDelay)
    {
        yield return new WaitForSeconds(pDelay);
        ResetBall();
    }
}
