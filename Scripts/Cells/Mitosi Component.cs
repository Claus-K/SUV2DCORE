using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cells
{
    public class MitosiComponent : MonoBehaviour
    {
        [SerializeField] private CellDatabase _cellDatabase;
        [SerializeField] private int populationRequeried;
        [SerializeField] private int energyRequeired;

        public void Mitosi()
        {
            var infectionComponet = GetComponent<InfectionComponentV2>();
            if (infectionComponet && infectionComponet.Infected)
            {
                Notification.Instance.SetNotification("Cell Infected");
                return;
            }


            if (ResourceManagerV2.Instance.CanMitosi(energyRequeired, populationRequeried))
            {
                ResourceManagerV2.Instance.Energy -= energyRequeired;
                var type = transform.GetComponent<CellTypeIdentifier>().Type;
                var prefab = _cellDatabase.GetPrefab(type);
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
        }

        // ON ENABLE CONFLICT WITH AWAKE of RESOURCE MANAGER
        // private void OnEnable() 
        // {
        //     // StartCoroutine(AddPopulation(populationRequeried));
        //     // CONFLICT
        //     // ResourceManagerV2.Instance.Population -= populationRequeried;
        // }

        private void Start()
        {
            ResourceManagerV2.Instance.Population += populationRequeried;
        }

        private void OnDestroy()
        {
            ResourceManagerV2.Instance.Population -= populationRequeried;
        }

        // private IEnumerator AddPopulation(int value)
        // {
        //     yield return new WaitForSeconds(1);
        //     ResourceManagerV2.Instance.Population += value;
        // }
    }
}