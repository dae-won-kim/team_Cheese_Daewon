using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new();
    [SerializeField] List<Enemy> enemyList = new();
    [SerializeField] List<SpriteRenderer> enemySprites = new();
    [SerializeField] List<Animator> enemyEffects = new();  

    private Bullet bullet;
    private bool isCoroutining = false; 

    public void takeDamage(string enemyName) 
    {
        (GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
            = GetEnemyInformation(enemyName);

        enemy.hp -= 1;
        Debug.Log(enemy.hp);

        // For Box
        if (!isCoroutining && enemy.hp > 0
            && objEnemy.layer == LayerMask.NameToLayer("attackable object") && objEnemy.tag == "Push_Object")
        {
            StartCoroutine(changeColor(enemySprite));
        }

        if (!isCoroutining && enemy.hp > 0)
        {
            Debug.Log("hp --");
            if((enemy.name + "Hit") != null)
            {
                enemyAni.Play(enemy.name + "Hit");
            }

            StartCoroutine(changeColor(enemySprite));
            StartCoroutine(ResetToDefaultState(enemyAni,enemy));
        }

        // For Box
        if (!isCoroutining && enemy.hp == 0 
            && objEnemy.layer == LayerMask.NameToLayer("attackable object")
            && objEnemy.tag =="Push_Object")
        {
            destroyEnemy(objEnemy, enemy, enemySprite, enemyAni); 
        }

        if (!isCoroutining && enemy.hp == 0)
        {
            // enemyAni.Play(enemy.tag + "Die");
            destroyEnemy(objEnemy, enemy, enemySprite, enemyAni);  
        }
    }

    void addEnemyInformationInLists()
    {
        // enemies.AddRange(transform.GetChild());
        enemyList.AddRange(GetComponentsInChildren<Enemy>());
        enemySprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
        enemyEffects.AddRange(GetComponentsInChildren<Animator>());
    }

    (GameObject, Enemy, SpriteRenderer, Animator) GetEnemyInformation(string enemyName)
    {
        int objIndex = enemies.FindIndex(x => x.name.Equals(enemyName));
        GameObject obj = enemies[objIndex];

        int enemyIndex = enemyList.FindIndex(x => x.name.Equals(enemyName));
        Enemy enemy = enemyList[enemyIndex];

        int spriteIndex = enemySprites.FindIndex(x => x.name.Equals(enemyName));
        SpriteRenderer enemySprite = enemySprites[spriteIndex];

        int aniIndex = enemySprites.FindIndex(x => x.name.Equals(enemyName));
        Animator enemyAni = enemyEffects[aniIndex];


        return (obj, enemy, enemySprite, enemyAni);
    }

    void destroyEnemy(GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
    {
        string enemyTag = enemy.tag;
        if (enemy.hp <= 0)
        {
            switch (enemyTag)
            {
                case "pinkDollEnemy":
                    {
                        deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                        break;
                    }
                case "rabbitDollEnemy":
                    {
                        deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                        break;
                    }
                case "NPC Enemy":
                    {
                        deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                        break;
                    }
                case "yellowBearEnemy":
                    {
                        deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                        break;
                    }
            }
        }

        if(enemy == null)
            deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
    }

    void deleteEnemyInLists(GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
    {
        enemies.Remove(objEnemy);
        enemyList.Remove(enemy);
        enemySprites.Remove(enemySprite);
        enemyEffects.Remove(enemyAni);
    }

    IEnumerator changeColor(SpriteRenderer enemySprite)
    {
        isCoroutining = true;
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        enemySprite.color = Color.white;
        isCoroutining = false;
    }

    IEnumerator ResetToDefaultState(Animator enemyAni, Enemy enemy)   
    {
        yield return new WaitForSeconds(0.4f);
        if (enemy.gameObject.layer != LayerMask.NameToLayer("enemy"))
        {
            enemyAni.Play("None");
        }
    }

    void removeNullEnemiesFromLists()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                enemyList.RemoveAt(i);
                enemySprites.RemoveAt(i);
                enemyEffects.RemoveAt(i);
            }
        }
    }


    // ===============================================================================


    void Start()
    {
        bullet = FindObjectOfType<Bullet>();
        addEnemyInformationInLists();
    }

    void Update()
    {
        removeNullEnemiesFromLists();
    }
}
