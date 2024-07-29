using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Player;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Stages.Scriptable;
using _3._Scripts.Wallet;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using Random = System.Random;

namespace _3._Scripts.Stages
{
    [RequireComponent(typeof(BoxCollider))]
    public class GrassField : MonoBehaviour
    {
        [SerializeField] private Vector2 fieldSize;
        [SerializeField] private Grass prefab;
        [SerializeField] private Bolt boltPrefab;
        [SerializeField] private TMP_Text recommendationText;
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;


        private readonly List<Grass> _grasses = new();
        private List<Bolt> _bolts = new();
        private float _resistance;

        private void OnValidate()
        {
            GetComponent<BoxCollider>().size = new Vector3(fieldSize.x, 1, fieldSize.y);
        }

        private void Awake()
        {
            GetComponent<BoxCollider>().size = new Vector3(fieldSize.x, 1, fieldSize.y);
        }

        public IEnumerable<Grass> Initialize(GrassData data)
        {
            ClearField();
            DestroyBolts();
            FillTheField(prefab, data);
            StopAllCoroutines();
            StartCoroutine(SpawnBoltsByTime());
            _resistance = data.Resistance;
            recommendationText.text = $"<sprite index=1>{data.Resistance}";
            return _grasses;
        }

        public void Respawn()
        {
            foreach (var grass in _grasses)
            {
                grass.Respawn();
            }

            DestroyBolts();
        }

        private void ClearField()
        {
            foreach (var grass in _grasses)
            {
                Destroy(grass.gameObject);
            }

            _grasses.Clear();
        }

        private void FillTheField(Grass obj, GrassData data)
        {
            var objectSize = obj.transform.localScale;
            var columns = Mathf.FloorToInt(fieldSize.x / objectSize.x);
            var rows = Mathf.FloorToInt(fieldSize.y / objectSize.y);

            var centerPosition = transform.position;
            var startPosition = new Vector3(
                centerPosition.x - (columns * objectSize.x / 2) + (objectSize.x / 2),
                centerPosition.y,
                centerPosition.z - (rows * objectSize.y / 2) + (objectSize.y / 2)
            );
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var position = new Vector3(
                        startPosition.x + j * objectSize.x + UnityEngine.Random.Range(-0.25f, 0.25f) * objectSize.x,
                        startPosition.y,
                        startPosition.z + i * objectSize.y + UnityEngine.Random.Range(-0.25f, 0.25f) * objectSize.y
                    );
                    var rotation = new Vector3(0, UnityEngine.Random.Range(-360f, 360f), 0);
                    var grass = Instantiate(obj, position, Quaternion.identity, transform);
                    grass.transform.eulerAngles = rotation;
                    grass.Initialize(data);
                    _grasses.Add(grass);
                }
            }
        }

        private IEnumerator SpawnBoltsByTime()
        {
            while (true)
            {
                var time = UnityEngine.Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(time);
                var rand = UnityEngine.Random.Range(0, 2);
                if (rand > 0 && WalletManager.FirstCurrency >= _resistance * 0.75f && _bolts.Count < 2)
                    SpawnBolt();
            }
        }

        private void SpawnBolt()
        {
            var xPosition = UnityEngine.Random.Range(-fieldSize.x / 2, fieldSize.x / 2);
            var yPosition = UnityEngine.Random.Range(-fieldSize.y / 2, fieldSize.y / 2);
            var spawnPosition = new Vector3(xPosition, 1, yPosition);

            var bolt = Instantiate(boltPrefab, transform);

            bolt.transform.localPosition = spawnPosition;
            bolt.Initialize(GetRandomEvenNumber(2, StageController.Instance.CurrentStage.MaxBoltCount));
            _bolts.Add(bolt);
        }

        private void DestroyBolts()
        {
            foreach (var bolt in _bolts.Where(bolt => bolt != null))
            {
                Destroy(bolt.gameObject);
            }

            _bolts.Clear();
        }

        private int GetRandomEvenNumber(int min, int max)
        {
            var randomValue = UnityEngine.Random.Range(min, max);

            if (randomValue % 2 != 0)
            {
                randomValue++;
            }

            if (randomValue > max)
            {
                randomValue -= 2;
            }

            return randomValue;
        }

        private void OnDrawGizmos()
        {
            var color = Color.red;
            color.a = 0.5f;

            UnityEngine.Gizmos.color = color;
            UnityEngine.Gizmos.DrawCube(transform.position, new Vector3(fieldSize.x, 1, fieldSize.y));
        }
    }
}