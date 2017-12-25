﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{

    [System.Serializable]
    public class EnemyStats
    {

        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

		public int damage = 20;

        public void Init()
        {
            curHealth = maxHealth;
        }

    }

	public int scoreValue = 10; 

    public EnemyStats stats = new EnemyStats();

    [Header("Optional:")]
    [SerializeField]
    private StatusIndicator statusIndicator;


    void Start()
    {
        stats.Init();

        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    public void DamageEnemy(int damage)
    {
        stats.curHealth -= damage;

        if (stats.curHealth <= 0)
        {
			ScoreManager.score += scoreValue;
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

	void OnCollisionEnter2D(Collision2D _colInfo)
	{
		Player _player = _colInfo.collider.GetComponent<Player>();
		if(_player != null)
		{
			_player.DamagePlayer(stats.damage);
		}
	}
}
