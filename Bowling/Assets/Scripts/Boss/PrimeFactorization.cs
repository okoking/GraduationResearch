using System.Collections.Generic;
using UnityEngine;

public class PrimeFactorization : MonoBehaviour
{
    public int num;
    List<int> factors = new List<int>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            factors.Clear();
            int n = num;

            for (int i = 2; i <= n; i++)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                }
            }

            Debug.Log("‘fˆö”•ª‰ðŒ‹‰Ê:");
            foreach (int f in factors)
                Debug.Log(f);
        }
    }
}