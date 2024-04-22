using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattler 
{
    public void Attack(IBattler target);
    public void Defense(float attackPower);
}
