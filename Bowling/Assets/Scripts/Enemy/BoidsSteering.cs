using System.Collections.Generic;
using UnityEngine;

public class BoidsSteering
{
    private EnemyAI enemy;
    private float neighborRadius;
    private float separationWeight;
    private float alignmentWeight;
    private float cohesionWeight;
    private float maxForce;
    private int updateInterval;
    private int frameCounter;
    private Vector3 lastForce;

    public BoidsSteering(EnemyAI enemy, float neighborRadius, float separationWeight, float alignmentWeight, float cohesionWeight, float maxForce, int updateInterval)
    {
        this.enemy = enemy;
        this.neighborRadius = neighborRadius;
        this.separationWeight = separationWeight;
        this.alignmentWeight = alignmentWeight;
        this.cohesionWeight = cohesionWeight;
        this.maxForce = maxForce;
        this.updateInterval = Mathf.Max(1, updateInterval);
        lastForce = Vector3.zero;
    }

    public Vector3 GetBoidsForceOptimized()
    {
        frameCounter++;
        if (frameCounter >= updateInterval)
        {
            lastForce = CalculateBoidsForce();
            frameCounter = 0;
        }
        return lastForce;
    }

    //BoidsŒvŽZ
    private Vector3 CalculateBoidsForce()
    {
        var neighbors = enemy.Manager.GetNearbyEnemies(enemy, neighborRadius);
        if (neighbors.Count == 0) return Vector3.zero;

        Vector3 separation = Vector3.zero; //(‹——£•ÛŽ)
        Vector3 alignment = Vector3.zero;  //(‘¬“x‡‚í‚¹)
        Vector3 cohesion = Vector3.zero;   //(ŒQ‚ê’†S)

        foreach (var other in neighbors)
        {
            Vector3 diff = enemy.transform.position - other.transform.position;
            float dist = diff.magnitude;
            if (dist > 0)
            {
                float t = Mathf.InverseLerp(neighborRadius, 0.3f, dist);
                float strength = Mathf.Lerp(0.5f, 3.0f, t);
                separation += diff.normalized * strength;
            }
            alignment += other.Agent.velocity;
            cohesion += other.transform.position;
        }

        separation /= neighbors.Count;
        alignment /= neighbors.Count;
        cohesion = (cohesion / neighbors.Count) - enemy.transform.position;

        Vector3 boidsForce = separation * separationWeight +
                             (alignment.normalized) * alignmentWeight +
                             (cohesion.normalized) * cohesionWeight;

        return Vector3.ClampMagnitude(boidsForce, maxForce);
    }
}