using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.AI
{
    public class NavigationModule : MonoBehaviour
    {
        [Header("Параметры")]
        [Tooltip("The maximum speed at which the enemy is moving (in world units per second).")]
        public float MoveSpeed = 0f;

        [Tooltip("The maximum speed at which the enemy is rotating (degrees per second).")]
        public float AngularSpeed = 0f;

        [Tooltip("The acceleration to reach the maximum speed (in world units per second squared).")]
        public float Acceleration = 0f;
    }
}
