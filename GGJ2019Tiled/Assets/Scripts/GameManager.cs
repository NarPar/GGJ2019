using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Fruitz;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Transform[] SpawnRegionTransforms;
        public Transform GoalRegionTransform;
        public GameObject OrangeGuardianPrefab;
        public GameObject OrangeInvaderPrefab;
        public GameObject StrawberryGuardianPrefab;
        public GameObject StrawberryInvaderPrefab;
        public GameObject BananaGuardianPrefab;
        public GameObject BananaInvaderPrefab;
        public GameObject LimeGuardianPrefab;
        public GameObject LimeInvaderPrefab;
        public GameObject ScoreObjectPrefab;
        public GameObject GuardianPlacementControllerPrefab;

        public GameObject NamePrefab;
        public ScoreBox ScoreboardDisplay;

        public List<Identity> Identities { get; private set; }
        public List<Player> Players { get; private set; }

        public UIManager UIManager;

        public List<Identity> Leaderboard
        {
            get { return Identities.OrderByDescending(i => i.Score).ToList(); }
        }

        public List<Transform> SpawnRegions
        {
            get { return SpawnRegionTransforms.ToList(); }
        }

        private void Awake()
        {
            Identities = new List<Identity>();

            var player1 = gameObject.AddComponent<Player>();
            player1.GameManager = this;
            player1.Init(this, 1);

            var player2 = gameObject.AddComponent<Player>();
            player2.Init(this, 2);

            Players = new List<Player>
            {
                player1,
                player2,
            };
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                TryEnd();
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public PlayerNameText SpawnNameText()
        {
            return Instantiate(NamePrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerNameText>();
        }

        public GuardianPlacementController SpawnGuardianPlacementController()
        {
            return Instantiate(GuardianPlacementControllerPrefab, Vector3.zero, Quaternion.identity).GetComponent<GuardianPlacementController>();
        }

        public Invader SpawnInvader()
        {
            var spawnPoint = SpawnRegionTransforms[UnityEngine.Random.Range(0, SpawnRegionTransforms.Length)]; // Random.Range() max is exclusive, FYI

            var identity = Identity.GenerateNewIdentity();
            Identities.Add(identity);

            GameObject prefab;
            switch (identity.Type)
            {
                case FruitType.Strawberry:
                    prefab = StrawberryInvaderPrefab;
                    break;
                case FruitType.Banana:
                    prefab = BananaInvaderPrefab;
                    break;
                case FruitType.Lime:
                    prefab = LimeInvaderPrefab;
                    break;
                case FruitType.Orange:
                default:
                    prefab = OrangeInvaderPrefab;
                    break;
            }

            var invader = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Invader>();
            invader.Identity = identity;

            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            return invader;
        }

        public Guardian SpawnGuardian(Identity identity)
        {
            identity.Score += CalculateInvaderScore();

            var spawnPoint = GoalRegionTransform;

            GameObject prefab;
            switch (identity.Type)
            {
                case FruitType.Strawberry:
                    prefab = StrawberryGuardianPrefab;
                    break;
                case FruitType.Banana:
                    prefab = BananaGuardianPrefab;
                    break;
                case FruitType.Lime:
                    prefab = LimeGuardianPrefab;
                    break;
                case FruitType.Orange:
                default:
                    prefab = OrangeGuardianPrefab;
                    break;
            }

            var guardian = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Guardian>();
            guardian.Identity = identity;

            return guardian;
        }

        public void RemoveIdentity(Identity identity)
        {
            Identities.Remove(identity);
        }

        private int CalculateInvaderScore()
        {
            return 1 + Identities.Count - Players.Count(p => p.State == Player.PlayerState.Invading);
        }

        bool confirming = false;
        public void TryEnd()
        {
            if (!confirming)
            {
                confirming = true;
                ShowConfirmEnd();
            }
            else
            {
                confirming = false;
                Score scoreObject = Instantiate<GameObject>(ScoreObjectPrefab, Vector2.zero, Quaternion.identity).GetComponent<Score>();
                scoreObject.SetScores(Leaderboard);

                SceneManager.LoadScene("EndScene");//, LoadSceneMode.Additive);
            }
        }
        private void ShowConfirmEnd()
        {
            if (UIManager == null)
            {
                UIManager = GetComponent<UIManager>();
            }

            UIManager.ShowConfirmEnd();
        }

        public void UpdateScoreBoard()
        {
            ScoreboardDisplay.SetScore(Leaderboard);
        }
    }
}
