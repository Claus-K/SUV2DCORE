using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class DOTHandler : MonoBehaviour
    {
        [SerializeField] private Transform _abEffect;
        private List<Transform> _listAb = new();
        private int defaultInfectFactor;
        private List<int> antiBodyTicks = new();
        private EnemyAgent virus;
        // private CombatComponentV2 _cb;

        private bool isAlive;

        // Start is called before the first frame update
        private void Start()
        {
            virus = GetComponent<EnemyAgent>();
            defaultInfectFactor = virus.InfectFactor;
            for (int i = 0; i < _abEffect.childCount; i++)
            {
                _listAb.Add(_abEffect.GetChild(i));
            }

            isAlive = true;
        }

        private void OnDestroy()
        {
            isAlive = false;
            // antiBodyTicks.Add(1);
            antiBodyTicks.Clear();
            _listAb.Clear();
        }


        public void ApplyAntiBody(int ticks)
        {
            if (antiBodyTicks.Count == 0)
            {
                if (isAlive)
                {
                    antiBodyTicks.Add(ticks);
                    _abEffect.gameObject.SetActive(true);
                    StartCoroutine(AntiBody());
                }
            }
            else if (antiBodyTicks.Count < 5)
            {
                antiBodyTicks.Add(ticks);
            }
        }


        private IEnumerator AntiBody()
        {
            var cb = GetComponent<CombatComponentV2>();
            if (!cb) yield break;
            while (antiBodyTicks.Count > 0)
            {
                HandleAntiBodyObject();
                for (var i = 0; i < antiBodyTicks.Count; i++)
                {
                    antiBodyTicks[i] -= 1;
                }

                cb.TakeDamage(3);
                virus.InfectFactor = Mathf.Max(virus.InfectFactor - 10, 0);
                antiBodyTicks.RemoveAll(i => i == 0);
                yield return new WaitForSeconds(0.75f);
            }

            yield return new WaitForSeconds(2f);
            StartCoroutine(AntiBodyReverse());
        }

        private IEnumerator AntiBodyReverse()
        {
            while (virus.InfectFactor != defaultInfectFactor && antiBodyTicks.Count == 0)
            {
                HandleAntiBodyObject(true);
                virus.InfectFactor = Mathf.Min(virus.InfectFactor + 10, defaultInfectFactor);
                yield return new WaitForSeconds(1f);
            }

            if (antiBodyTicks.Count == 0)
            {
                _abEffect.gameObject.SetActive(false);
            }
        }

        private void HandleAntiBodyObject(bool disaable = false)
        {
            if (!disaable)
            {
                EnableAntiBody();
            }

            DisableAntiBody();
        }

        private void EnableAntiBody()
        {
            if (virus.InfectFactor < defaultInfectFactor * 0.2f)
            {
                _listAb[0].gameObject.SetActive(true);
            }
            else if (virus.InfectFactor < defaultInfectFactor * 0.4f)
            {
                _listAb[1].gameObject.SetActive(true);
            }
            else if (virus.InfectFactor < defaultInfectFactor * 0.6f)
            {
                _listAb[2].gameObject.SetActive(true);
            }
            else if (virus.InfectFactor < defaultInfectFactor * 0.8f)
            {
                _listAb[3].gameObject.SetActive(true);
            }
        }

        private void DisableAntiBody()
        {
            if (virus.InfectFactor > defaultInfectFactor * 0.8f)
            {
                _listAb[0].gameObject.SetActive(false);
            }
            else if (virus.InfectFactor > defaultInfectFactor * 0.6f)
            {
                _listAb[1].gameObject.SetActive(false);
            }
            else if (virus.InfectFactor > defaultInfectFactor * 0.4f)
            {
                _listAb[2].gameObject.SetActive(false);
            }
            else if (virus.InfectFactor > defaultInfectFactor * 0.2f)
            {
                _listAb[3].gameObject.SetActive(false);
            }
        }
    }
}