using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace MithrixSurprise
{
	[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
	[BepInPlugin("com.Nuxlar.MithrixSurprise", "MithrixSurprise", "1.0.6")]
	public class MithrixSurprise : BaseUnityPlugin
	{
		private static ConfigFile RoRConfig { get; set; }
		internal static ConfigEntry<float> probability;
		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		private static SpawnCard theBoi = 
			Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Brother/cscBrother.asset").WaitForCompletion();
		
		internal static ConfigEntry<float> probabilityFalse;
		private static readonly RoR2.ExpansionManagement.ExpansionDef dlc2 =
			Addressables.LoadAssetAsync<RoR2.ExpansionManagement.ExpansionDef>(
					"RoR2/DLC2/Common/DLC2.asset").WaitForCompletion();
		private static readonly SpawnCard theFalseBoi = 
			Addressables.LoadAssetAsync<SpawnCard>("RoR2/DLC2/FalseSonBoss/cscFalseSonBoss.asset").WaitForCompletion();
		
		public void Awake()
		{
			RoRConfig = new ConfigFile(Paths.ConfigPath + "\\MithrixSurprise.cfg", true);
			probability = RoRConfig.Bind<float>("General", "Spawn Chance", 0.5f,
				"Mithrix spawn chance.");
			probabilityFalse = RoRConfig.Bind<float>("General", "Spawn Chance False Son", 0f,
				"False Son spawn chance. (Requires SotS)");
			
			if (!System.IO.File.Exists(Paths.ConfigPath + "\\MithrixSurpriseFirstRun.cfg")) {
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (probability.Value != (float)probability.DefaultValue) probability.Value *= 100f;
				System.IO.File.Create(Paths.ConfigPath + "\\MithrixSurpriseFirstRun.cfg");
			}
			
			System.Math.Clamp(probability.Value, 0f, 100f);
			System.Math.Clamp(probabilityFalse.Value, 0f, 100f);

			if (RoO.Enabled) {
				string iconPath = System.Reflection.Assembly
					.GetExecutingAssembly().Location.Replace("MithrixSurprise.dll", "icon.png");
				if (!System.IO.File.Exists(iconPath)) iconPath = "";
				RoO.AddOptions(iconPath);
			}
			
			On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseInteraction_OnInteractionBegin;
		}
		
		private static void PurchaseInteraction_OnInteractionBegin(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, 
			PurchaseInteraction self, Interactor activator)
		{
			if (self.CanBeAffordedByInteractor(activator))
			{
				if (RoR2Application.rng.RangeFloat(0f, 1f) < (probability.Value / 100f))
				{
					SpawnTheBoi(activator.GetComponent<CharacterBody>(), theBoi);
				}
				
				if (!Run.instance.IsExpansionEnabled(dlc2)) {
					orig(self, activator);
					return;
				}
				if (RoR2Application.rng.RangeFloat(0f, 1f) < (probabilityFalse.Value / 100f)) {
					SpawnTheBoi(activator.GetComponent<CharacterBody>(), theFalseBoi);
				}
				
				orig(self, activator);
				return;
			}
			orig(self, activator);
		}
		
		private static void SpawnTheBoi(CharacterBody activatorBody, SpawnCard spawnCard)
		{
			Transform coreTransform = activatorBody.coreTransform;
			DirectorCore.MonsterSpawnDistance input = DirectorCore.MonsterSpawnDistance.Far;
			DirectorPlacementRule directorPlacementRule = new DirectorPlacementRule
			{
				spawnOnTarget = coreTransform,
				placementMode = DirectorPlacementRule.PlacementMode.NearestNode
			};
			DirectorCore.GetMonsterSpawnDistance(input, out directorPlacementRule.minDistance, 
				out directorPlacementRule.maxDistance);
			GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, 
				directorPlacementRule, RoR2Application.rng)
			{
				teamIndexOverride = TeamIndex.Monster
			});
			gameObject.GetComponent<CharacterMaster>().isBoss = true;
			if (!gameObject) return;
			NetworkServer.Spawn(gameObject);
		}
	}
}