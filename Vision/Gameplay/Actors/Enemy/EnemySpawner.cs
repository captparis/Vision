using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vision.Gameplay
{
	[ExecuteInEditMode]
	public class EnemySpawner : MonoBehaviour
	{
		interface IWeightedArray
		{
			int GetWeight();
		}
		[Serializable]
		class ClassProbability : IWeightedArray
		{
			[HideInInspector]
			public string className;
			//public Type classType;
			public int weight = 1;

			int IWeightedArray.GetWeight()
			{
				return weight;
			}

		}
		[Serializable]
		class AttackArray : IWeightedArray
		{
			[HideInInspector]
			public string className;
			//public Type classType;
			public int weight = 1;
			public GameObject weapon = null;
			public GameObject projectile = null;
			public RuntimeAnimatorController anim = null;

			int IWeightedArray.GetWeight()
			{
				return weight;
			}

		}
		
		[Serializable]
		class MeshArray : IWeightedArray
		{
			public GameObject prefab = null;
			public int weight = 1;

			int IWeightedArray.GetWeight()
			{
				return weight;
			}
		}
		[SerializeField]
		MeshArray[] meshes;
		[SerializeField]
		private ClassProbability[] aims;
		[SerializeField]
		private ClassProbability[] moves;
		[SerializeField]
		private AttackArray[] attacks;
		[SerializeField]
		private ClassProbability[] specials;

		[SerializeField]
		private bool forceRepopulate;
		[SerializeField]
		private GameObject dizzyEffect;

		void Start()
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				PopulateModules();
			}
			else
			{
#endif
				EnemySpawn[] spawns = UnityEngine.Object.FindObjectsOfType<EnemySpawn>();
				for (int i = 0; i < spawns.Length; i++)
				{
					int index = GetWeightedRandomIndex(meshes);
					GameObject enemy = Instantiate(meshes[index].prefab, spawns[i].transform.position, spawns[i].transform.rotation) as GameObject;
					//enemy.transform.localScale = new Vector3(2,2,2);
					if (enemy == null)
					{
						continue;
					}
					index = GetWeightedRandomIndex(aims);
					enemy.AddComponent(Type.GetType(aims[index].className));

					index = GetWeightedRandomIndex(attacks);
					AttackModule attackmodule = enemy.AddComponent(Type.GetType(attacks[index].className)) as AttackModule;
					attackmodule.weapon = attacks[index].weapon;
					attackmodule.projectile = attacks[index].projectile;
					enemy.GetComponent<Animator>().runtimeAnimatorController = attacks[index].anim;

					index = GetWeightedRandomIndex(moves);
					enemy.AddComponent(Type.GetType(moves[index].className));

					index = GetWeightedRandomIndex(specials);
					enemy.AddComponent(Type.GetType(specials[index].className));

					Enemy E = enemy.AddComponent<Enemy>();
					if (dizzyEffect != null)
					{
						E.deathEffect = dizzyEffect;
					}
				}
#if UNITY_EDITOR
			}
#endif
		}
#if UNITY_EDITOR
		void PopulateModules()
		{
			var assembly = typeof(BaseModule).Assembly;

			var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AimModule)));
			int count = types.Count<Type>();
			if (aims.Length != count || forceRepopulate)
			{
				aims = new ClassProbability[count];
				int i = 0;
				foreach (Type module in types)
				{
					aims[i] = new ClassProbability();
					aims[i].className = module.ToString();
					//aims[i].classType = module;
					//print(aims[i].classType);
					i++;
				}
				//print(aims[0].classType);
			}
			types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(MovementModule)));
			count = types.Count<Type>();
			if (moves.Length != count || forceRepopulate)
			{
				moves = new ClassProbability[count];
				int i = 0;
				foreach (Type module in types)
				{
					moves[i] = new ClassProbability();
					moves[i].className = module.ToString();
					//moves[i].classType = module;
					i++;
				}
			}
			types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(SpecialModule)));
			count = types.Count<Type>();
			if (specials.Length != count || forceRepopulate)
			{
				specials = new ClassProbability[count];
				int i = 0;
				foreach (Type module in types)
				{
					specials[i] = new ClassProbability();
					specials[i].className = module.ToString();
					//moves[i].classType = module;
					i++;
				}
			}
			types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AttackModule)));
			count = types.Count<Type>();
			if (attacks.Length != count || forceRepopulate)
			{
				attacks = new AttackArray[count];
				int i = 0;
				foreach (Type module in types)
				{
					attacks[i] = new AttackArray();
					attacks[i].className = module.ToString();
					//attacks[i].classType = module;
					i++;
				}
			}
			forceRepopulate = false;
			EditorUtility.SetDirty(this);
		}
#endif

		int GetWeightedRandomIndex(IWeightedArray[] array)
		{
			int totalSum = 0;

			for (int i = 0; i < array.Length; i++)
			{
				totalSum += array[i].GetWeight();
			}
			int rand = UnityEngine.Random.Range(0, totalSum) + 1;
			int sum = 0;
			for (int i = 0; i < array.Length; i++)
			{
				sum += array[i].GetWeight();
				if (rand <= sum)
				{
					return i;
				}
			}
			return 0;
		}
	}
}
