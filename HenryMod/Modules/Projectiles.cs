using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayableTemplar.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject templarGrenadePrefab;
        internal static GameObject templarRocketPrefab;


        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            CreateBomb();
            CreateGrenade();

            AddProjectile(bombPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void CreateGrenade()
        {
            templarGrenadePrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile"), "TemplarGrenadeProjectile", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\cs", "RegisterTemplar", 527);
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = Modules.Config.clayGrenadeDamageCoefficient.Value;
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = Modules.Config.clayGrenadeProcCoefficient.Value;
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().blastRadius = Modules.Config.clayGrenadeRadius.Value;
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().falloffModel = BlastAttack.FalloffModel.Linear;
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = Modules.Config.clayGrenadeDetonationTime.Value;
            templarGrenadePrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosivePotExplosion");
            templarGrenadePrefab.GetComponent<ProjectileDamage>().damageType = DamageType.ClayGoo;
            GameObject gameObject = PrefabAPI.InstantiateClone(Assets.clayBombModel, "TemplarBombModel");
            gameObject.AddComponent<NetworkIdentity>();
            gameObject.AddComponent<ProjectileGhostController>();
            gameObject.transform.GetChild(0).localScale *= 0.5f;
            templarGrenadePrefab.GetComponent<ProjectileController>().ghostPrefab = gameObject;
        }

        private static void Create()
        {

        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            //projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            //projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        public class TemplarMissileController : MonoBehaviour
        {
            // Token: 0x060000F8 RID: 248 RVA: 0x0001092C File Offset: 0x0000EB2C
            private void Awake()
            {
                this.rb = base.GetComponentInChildren<Rigidbody>();
                this.speed = TemplarMissileController.startSpeed;
            }

            // Token: 0x060000F9 RID: 249 RVA: 0x00010946 File Offset: 0x0000EB46
            private void FixedUpdate()
            {
                this.speed += TemplarMissileController.acceleration;
                this.rb.velocity = base.transform.forward * this.speed;
            }

            // Token: 0x060000FA RID: 250 RVA: 0x0001097D File Offset: 0x0000EB7D
            public TemplarMissileController()
            {
            }

            // Token: 0x060000FB RID: 251 RVA: 0x00010986 File Offset: 0x0000EB86
            // Note: this type is marked as 'beforefieldinit'.
            static TemplarMissileController()
            {
            }

            // Token: 0x04000150 RID: 336
            public static float acceleration = 2.5f;

            // Token: 0x04000151 RID: 337
            public static float startSpeed = 25f;

            // Token: 0x04000152 RID: 338
            private float speed;

            // Token: 0x04000153 RID: 339
            private Rigidbody rb;
        }

        public class TemplarExplosionForce : MonoBehaviour
        {
            // Token: 0x060000FE RID: 254 RVA: 0x000109B8 File Offset: 0x0000EBB8
            private void Awake()
            {
                bool flag = true;
                bool flag2 = flag;
                if (flag2)
                {
                    Collider[] array = Physics.OverlapSphere(base.transform.position, this.radius);
                    foreach (Collider collider in array)
                    {
                        CharacterBody componentInChildren = collider.transform.root.GetComponentInChildren<CharacterBody>();
                        bool flag3 = componentInChildren != null;
                        if (flag3)
                        {
                            bool flag4 = componentInChildren.baseNameToken == "TEMPLAR_NAME";
                            if (flag4)
                            {
                                bool flag5 = componentInChildren.characterMotor != null;
                                if (flag5)
                                {
                                    float num = 16f / Vector3.Distance(componentInChildren.transform.position, base.transform.position);
                                    Vector3 vector = new Vector3(0f, Mathf.Clamp(num * this.force, 0f, this.maxForce), 0f);
                                    bool flag6 = !componentInChildren.characterMotor.isGrounded;
                                    if (flag6)
                                    {
                                        componentInChildren.characterMotor.ApplyForce(vector, false, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Token: 0x060000FF RID: 255 RVA: 0x00010AD7 File Offset: 0x0000ECD7
            public TemplarExplosionForce()
            {
            }

            // Token: 0x04000154 RID: 340
            public float force = 750f;

            // Token: 0x04000155 RID: 341
            public float radius = 16f;

            // Token: 0x04000156 RID: 342
            public float maxForce = 4000f;
        }

        // Base Stuff //

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }


    }
}