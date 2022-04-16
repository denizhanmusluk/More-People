using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void hitPeople(GameObject human);
    void hitObstacle(GameObject obs);
}
