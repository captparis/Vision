using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public class EnemySpawn : MonoBehaviour
	{
		public Mesh mesh;
		void OnDrawGizmos()
		{
			Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation);
			//Gizmos.DrawIcon(transform.position, "spawner.tga", true);
		}
	}
}
