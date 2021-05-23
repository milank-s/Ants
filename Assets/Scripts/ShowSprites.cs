using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSprites : MonoBehaviour
{
    public SpriteRenderer[] sprites;

    int count;
    void Start(){
        StageManager.i.OnHit += TryShowSprite;
    }

    int index;
    public void TryShowSprite(){
        count ++;
        if(index < sprites.Length){
            if(count > 1){
                count = 0;
                sprites[index].enabled = true;
                index ++;
            }
        }
    }
}
