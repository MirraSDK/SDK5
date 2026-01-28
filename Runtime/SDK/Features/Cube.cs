using System.Collections;
using UnityEngine;

namespace MirraGames.SDK
{
    public class Cube : MonoBehaviour
    {
        private readonly WaitForSeconds WaitForOneSecond = new(1.0f);

        private Quaternion targetRotation;

        private IEnumerator Start()
        {
            while (true)
            {
                float randomX = Random.Range(0f, 360f);
                float randomY = Random.Range(0f, 360f);
                float randomZ = Random.Range(0f, 360f);
                targetRotation = Quaternion.Euler(randomX, randomY, randomZ);
                yield return WaitForOneSecond;
            }
        }

        public void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }
}