using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace MithrixSurprise
{
	[BepInPlugin("com.Nuxlar.MithrixSurprise", "MithrixSurprise", "1.0.4")]
	public class MithrixSurprise : BaseUnityPlugin
	{
		private ConfigFile RoRConfig { get; set; }
		
		public void Awake()
		{
			RoRConfig = new ConfigFile(Paths.ConfigPath + "\\MithrixSurprise.cfg", true);
			probability = RoRConfig.Bind<float>("General", "Spawn Chance", 0.005f, "Mithrix spawn chance.");
			ModSettingsManager.AddOption(new StepSliderOption(this.probability, new StepSliderConfig
			{
				min = 0.005f,
				max = 1f,
				increment = 0.005f
			}));
			ModSettingsManager.SetModDescription("Goodbye Moon Man");
			On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseInteraction_OnInteractionBegin;
		}
		
		private void PurchaseInteraction_OnInteractionBegin(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
		{
			if (self.CanBeAffordedByInteractor(activator))
			{
				if (Random.value < probability.Value)
				{
					SpawnTheBoi(activator.GetComponent<CharacterBody>());
				}
				orig(self, activator);
				return;
			}
			orig(self, activator);
		}
		
		private void SpawnTheBoi(CharacterBody activatorBody)
		{
			SpawnCard spawnCard = theBoi;
			Transform coreTransform = activatorBody.coreTransform;
			DirectorCore.MonsterSpawnDistance input = DirectorCore.MonsterSpawnDistance.Far;
			DirectorPlacementRule directorPlacementRule = new DirectorPlacementRule
			{
				spawnOnTarget = coreTransform,
				placementMode = DirectorPlacementRule.PlacementMode.NearestNode
			};
			DirectorCore.GetMonsterSpawnDistance(input, out directorPlacementRule.minDistance, out directorPlacementRule.maxDistance);
			GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, directorPlacementRule, RoR2Application.rng)
			{
				teamIndexOverride = TeamIndex.Monster
			});
			gameObject.GetComponent<CharacterMaster>().isBoss = true;
			if (!gameObject)
			{
				return;
			}
			NetworkServer.Spawn(gameObject);
		}
		
		private ConfigEntry<float> probability;
		
		private SpawnCard theBoi = Addressables.LoadAssetAsync<SpawnCard>("RoR2/Base/Brother/cscBrother.asset").WaitForCompletion();
	}
}