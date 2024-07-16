using System;
using System.Collections.Generic;
using _3._Scripts.Player;
using _3._Scripts.Player.Scriptables;
using _3._Scripts.Stages.Scriptable;
using _3._Scripts.Wallet;
using UnityEngine;

namespace _3._Scripts.Stages
{
    public class GrassField : MonoBehaviour
    {
        [SerializeField] private Vector2 fieldSize;
        [SerializeField] private GrassData test;

        public GrassData Data { get; private set; }
        private readonly List<Grass> _grasses = new();

        private void OnValidate()
        {
            GetComponent<BoxCollider>().size = new Vector3(fieldSize.x, 1, fieldSize.y);
        }

        private void Awake()
        {
            GetComponent<BoxCollider>().size = new Vector3(fieldSize.x, 1, fieldSize.y);
        }

        private void Start()
        {
            Initialize(test);
        }

        public void Initialize(GrassData data)
        {
            ClearField();
            FillTheField(data.Prefab);
            Data = data;
        }

        public void Respawn()
        {
            foreach (var grass in _grasses)
            {
                grass.Respawn();
            }
        }

        private void ClearField()
        {
            foreach (var grass in _grasses)
            {
                Destroy(grass.gameObject);
            }

            _grasses.Clear();
        }

        private void FillTheField(Grass obj)
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
                        startPosition.x + j * objectSize.x,
                        startPosition.y,
                        startPosition.z + i * objectSize.y
                    );

                    var grass = Instantiate(obj, position, Quaternion.identity, transform);
                    _grasses.Add(grass);
                }
            }
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