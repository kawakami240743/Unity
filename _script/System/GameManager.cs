using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class GameManager : MonoBehaviour
{
    //名前
    private GameManager gameManager;
    public PlayerControl playercontrol;
    public EnemySpawner enemySpawner;
    public Enemy_01 enemies_01;
    public Enemy_02 enemies_02;
    public BossEnemy bossenemy;

    public GameObject Player;　//プレイヤー本体

    //SE等
    private AudioSource audioSource;
    public AudioClip SetItem;
    public AudioClip UseItem_00;
    public AudioClip UseItem_01;
    public AudioClip JoinsBoss;
    public AudioClip Hit;

    //シーン移動・ダイアログ
    //public GameOverController gameOverController;
    //public GameClearController gameClearController;
    public MethodOfOperationController methodOf;

    //アイテム関連（未実装あり）
    private int Heal = 3;
    //private int BombDamege = 1;
    //private int ThunderStop = 3;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playercontrol = Player.GetComponent<PlayerControl>();
        audioSource = GetComponent<AudioSource>();
    }

    //シーン移動・ダイアログ
    public void OnPlayerDestroyed()
    {
        SwitchGameOverScene();
        //gameOverController = GetComponent<GameOverController>();
        //gameOverController.GameOver();
    }

    public void SwitchGameOverScene()
    {
        SceneManager.LoadScene("GameOver_Scene");
    }

    public void OnBossEnemyDestroyed()
    {
        SwitchGameClearScene();
        //gameClearController = GetComponent<GameClearController>();
        //gameClearController.GameClear();
    }

    public void SwitchGameClearScene()
    {
        SceneManager.LoadScene("GameClear_Scene");
    }

    public void OnGameStartDestroyed()
    {
        methodOf = GetComponent<MethodOfOperationController>();
        methodOf.MethodOfOprerationPause();
    }

    //BGM・SE
    public void SetItemSound()
    {
        audioSource.PlayOneShot(SetItem);
    }

    public void JoinsBoss_SE()
    {
        audioSource.PlayOneShot(JoinsBoss);
    }

    public void Hit_SE()
    {
        audioSource.PlayOneShot(Hit);
    }

    //アイテム
    public void ProcessItem(int itemId)
    {
        switch (itemId)
        {
            case 0:
                if(playercontrol.playerInfo.hp <= 10)
                {
                    audioSource.PlayOneShot(UseItem_00);
                    Debug.Log("Heal");
                    playercontrol.playerInfo.hp += Heal;
                    Debug.Log("Player HP:" + playercontrol.playerInfo.hp);
                }
                else
                {
                    Debug.Log("HPが満タン");
                }
                break;

            /*case 2:
                Debug.Log("Invincible");
                break;*/

            case 1:
                Debug.Log("Auto_Shot");
                Shooting shootingComponent = playercontrol.GetComponent<Shooting>();
                if (shootingComponent != null)
                {
                    audioSource.PlayOneShot(UseItem_01);
                    shootingComponent.EnableHoming(5f);
                }
                else
                {
                    Debug.LogError("Shooting component not found on PlayerController!");
                }
                break;
                /*case 4:
                    List<Enemy_01> enemies_01_to_damage = enemySpawner.GetEnemies_01();
                    List<Enemy_02> enemies_02_to_damage = enemySpawner.GetEnemies_02();
                    foreach (Enemy_01 enemy in enemies_01_to_damage)
                    {
                        enemy.enemy_01.hp -= (int)BombDamege;
                        Debug.Log("Enemy_01 HP:" + enemies_01.enemy_01.hp);
                    }

                    foreach (Enemy_02 enemy in enemies_02_to_damage)
                    {
                        enemy.enemy_02.hp -= (int)BombDamege;
                        Debug.Log("Enemy_02 HP:" + enemies_02.enemy_02.hp);
                    }

                    if (bossenemy != null)
                    {
                        bossenemy.BossInfo.hp -= (int)BombDamege;
                        Debug.Log("Boss HP:" + bossenemy.BossInfo.hp);
                    }
                    Debug.Log("Bomb!");
                    break;

                case 5:
                    Debug.Log("Tornado!");
                    break;

                case 6:
                    Debug.Log("Thunder!");
                    break;

                default:
                    Debug.Log("Unknown item ID");
                    break;*/
        }
    }
}