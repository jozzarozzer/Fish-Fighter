using UnityEditor;
using System.Collections;
using UnityEngine;

public class GunWindow : EditorWindow
{

    Texture2D headerSectionTexture;
    Texture2D gunSectionTexture;
    Texture2D bulletSectionTexture;
    Texture2D behaviourSectionTexture;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    Rect headerSection;
    Rect gunSection;
    Rect bulletSection;
    Rect behaviourSection;

    static GunSO gunData;
    static BulletSO bulletData;
    static BulletBehaviourSO behaviourData;

    
    public static GunSO gunInfo { get { return gunData; } }
    public static BulletSO bulletInfo { get { return bulletData; } }
    public static BulletBehaviourSO behaviourInfo { get { return behaviourData; } }
    

    float timeBetweenBursts;
    string newGunName = "New Name";
    string newBulletName = "New Name";
    string newBehaviourName = "New Name";

    GUIStyle titleStyle;
    GUIStyle subtitleStyle;
    GUIStyle smallStyle;

    bool creatingNewGun;
    bool creatingNewBullet;
    bool creatingNewBehaviour;

    [MenuItem("Window/Gun Designer")]
    static void OpenWindow()
    {
        GunWindow window = (GunWindow)GetWindow(typeof(GunWindow));
        window.minSize = new Vector2(600, 320);
        window.Show();
    }

    void OnEnable()
    {
        InitLabelStyles();
        InitTextures();
        //InitData();
        InitNullData();
    }

    public static void InitData()
    {
        gunData = (GunSO)ScriptableObject.CreateInstance(typeof(GunSO));
        bulletData = (BulletSO)ScriptableObject.CreateInstance(typeof(BulletSO));
        behaviourData = (BulletBehaviourSO)ScriptableObject.CreateInstance(typeof(BulletBehaviourSO));
    }

    public static void InitNullData()
    {
        gunData = null;
        bulletData = null;
        behaviourData = null;
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        //gunSectionTexture = Resources.Load<Texture2D>("icons/editor_gun_gradient");
        gunSectionTexture = new Texture2D(1, 1);
        gunSectionTexture.SetPixel(0, 0, new Color (1,0.5f,0.2f,1));
        gunSectionTexture.Apply();

        bulletSectionTexture = new Texture2D(1, 1);
        bulletSectionTexture.SetPixel(0, 0, new Color(1, 0f, 0.3f, 1));
        bulletSectionTexture.Apply();

        behaviourSectionTexture = new Texture2D(1, 1);
        behaviourSectionTexture.SetPixel(0, 0, new Color(0.2f, 0.7f, 1f, 1));
        behaviourSectionTexture.Apply();
    }

    void InitLabelStyles()
    {
        titleStyle = new GUIStyle();
        subtitleStyle = new GUIStyle();
        smallStyle = new GUIStyle();

        titleStyle.fontSize = 40;
        titleStyle.normal.textColor = new Color(1, 1, 1, 1);
        titleStyle.alignment = TextAnchor.UpperCenter;

        subtitleStyle.fontSize = 25;
        subtitleStyle.normal.textColor = new Color(1, 1, 1, 1);
        subtitleStyle.alignment = TextAnchor.UpperCenter;

        smallStyle.fontSize = 8;
        smallStyle.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1);
    }

    void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawGunSettings();
        if (gunData != null)
        {
            DrawBulletSettings();

            if (bulletData != null)
            {
                DrawBehaviourSettings();

                DrawGunStats();
            }
        }


        


    }

    void DrawGunStats()
    {
        if (!gunData.burstFire)
        {
            GUI.Label(new Rect(0, 0, 40, 10), "DPS: " + (bulletData.damage / gunData.fireRate), smallStyle);
            if (bulletData.piercesEnemies)
            {
                GUI.Label(new Rect(40, 0, 40, 10), "DPS MAX: " + ((bulletData.damage / gunData.fireRate) * (bulletData.enemyPierceAmount + 1)), smallStyle);
            }
            GUI.Label(new Rect(0, 10, 40, 10), "DPS(R): " + (bulletData.damage * gunData.ammoMax / (gunData.fireRate * gunData.ammoMax + gunData.reloadTime)), smallStyle);
            GUI.Label(new Rect(0, 20, 40, 10), "RPS(R): " + (gunData.ammoMax / (gunData.fireRate * gunData.ammoMax + gunData.reloadTime)), smallStyle);

        }
        else
        {
            GUI.Label(new Rect(0, 0, 40, 10), "DPS: " + (bulletData.damage * gunData.burstFireAmount / gunData.fireRate), smallStyle);
            if (bulletData.piercesEnemies)
            {
                GUI.Label(new Rect(40, 0, 40, 10), "DPS MAX: " + ((bulletData.damage * gunData.burstFireAmount / gunData.fireRate) * (bulletData.enemyPierceAmount + 1)), smallStyle);
            }
            GUI.Label(new Rect(0, 10, 40, 10), "DPS(R): " + (bulletData.damage * gunData.ammoMax / (gunData.fireRate * (gunData.ammoMax / gunData.burstFireAmount) + gunData.reloadTime)), smallStyle);
            GUI.Label(new Rect(0, 20, 40, 10), "RPS(R): " + (gunData.ammoMax / (gunData.fireRate * (gunData.ammoMax / gunData.burstFireAmount) + gunData.reloadTime)), smallStyle);
        }
    }

    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        gunSection.x = 0;
        gunSection.y = 50;
        gunSection.width = Screen.width / 3f;
        gunSection.height = Screen.height - 50;

        bulletSection.x = Screen.width / 3f;
        bulletSection.y = 50;
        bulletSection.width = Screen.width / 3f;
        bulletSection.height = Screen.height - 50;

        behaviourSection.x = 2 * Screen.width / 3f;
        behaviourSection.y = 50;
        behaviourSection.width = Screen.width / 3f;
        behaviourSection.height = Screen.height - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(gunSection, gunSectionTexture);
        GUI.DrawTexture(bulletSection, bulletSectionTexture);
        GUI.DrawTexture(behaviourSection, behaviourSectionTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Gun Designer", titleStyle);
        GUILayout.EndArea();
    }

    void DrawGunSettings()
    {
        GUILayout.BeginArea(gunSection);

        GUILayout.Label("Gun", subtitleStyle);

        if (!creatingNewGun)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Gun", GUILayout.Width(gunSection.width / 2));
            gunData = (GunSO)EditorGUILayout.ObjectField(gunData, typeof(GunSO), false);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (gunData != null)
        {

            GUILayout.Label("Name", GUILayout.Width(gunSection.width / 2));
            newGunName = EditorGUILayout.TextField(newGunName);
            if (!creatingNewGun)
            {
                if (GUILayout.Button("S"))
                {
                    string path = AssetDatabase.GetAssetPath(gunData);
                    AssetDatabase.RenameAsset(path, newGunName);
                }
                if (GUILayout.Button("R"))
                {
                    newGunName = gunData.name;
                }
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Hold to Fire?", GUILayout.Width(gunSection.width / 2));
            gunData.canHold = EditorGUILayout.Toggle(gunData.canHold);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Burst fire?", GUILayout.Width(gunSection.width / 2));
            gunData.burstFire = EditorGUILayout.Toggle(gunData.burstFire);

            EditorGUILayout.EndHorizontal();

            if (gunData.burstFire)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Burst Fire Rate", GUILayout.Width(gunSection.width / 2));
                gunData.burstFireRate = EditorGUILayout.FloatField(gunData.burstFireRate);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Burst Fire Amount", GUILayout.Width(gunSection.width / 2));
                gunData.burstFireAmount = EditorGUILayout.IntField(gunData.burstFireAmount);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Time between bursts", GUILayout.Width(gunSection.width / 2));
                timeBetweenBursts = EditorGUILayout.FloatField(timeBetweenBursts);
                gunData.fireRate = gunData.burstFireAmount * gunData.burstFireRate + timeBetweenBursts;

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Fire Rate", GUILayout.Width(gunSection.width / 2));
                gunData.fireRate = EditorGUILayout.FloatField(gunData.fireRate);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Ammo", GUILayout.Width(gunSection.width / 2));
            gunData.ammoMax = EditorGUILayout.IntField(gunData.ammoMax);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Reload Time", GUILayout.Width(gunSection.width / 2));
            gunData.reloadTime = EditorGUILayout.FloatField(gunData.reloadTime);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Recoil", GUILayout.Width(gunSection.width / 2));
            gunData.recoil = EditorGUILayout.FloatField(gunData.recoil);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Price", GUILayout.Width(gunSection.width / 2));
            gunData.price = EditorGUILayout.IntField(gunData.price);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Level", GUILayout.Width(gunSection.width / 2));
            gunData.level = EditorGUILayout.IntField(gunData.level);

            EditorGUILayout.EndHorizontal();
        }
        if (!creatingNewGun)
        {
            if (GUILayout.Button("New Gun"))
            {
                creatingNewGun = true;

                gunData = new GunSO();
                newGunName = "";
            }
        }
        else
        {
            if (GUILayout.Button("Cancel New Gun"))
            {
                creatingNewGun = false;
            }
            if (GUILayout.Button("Create Gun"))
            {
                string dataPath = "Assets/Scripts/Action/";
                dataPath += "Guns/" + newGunName + ".asset";
                AssetDatabase.CreateAsset(gunData, dataPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                creatingNewGun = false;
            }
        }
        GUILayout.EndArea();
    }

    void DrawBulletSettings()
    {
        GUILayout.BeginArea(bulletSection);

        bulletData = gunData.bulletType;

        GUILayout.Label("Bullet", subtitleStyle);

        if (!creatingNewBullet)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Bullet", GUILayout.Width(bulletSection.width / 2));
            bulletData = (BulletSO)EditorGUILayout.ObjectField(bulletData, typeof(BulletSO), false);

            EditorGUILayout.EndHorizontal();
        }

        if (bulletData != null)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Name", GUILayout.Width(bulletSection.width / 2));
            newBulletName = EditorGUILayout.TextField(newBulletName);
            if (!creatingNewBullet)
            {
                if (GUILayout.Button("S"))
                {
                    string path = AssetDatabase.GetAssetPath(bulletData);
                    AssetDatabase.RenameAsset(path, newBulletName);
                }
                if (GUILayout.Button("R"))
                {
                    newBulletName = bulletData.name;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Multi Bullet?", GUILayout.Width(bulletSection.width / 2));
            bulletData.multiBullet = EditorGUILayout.Toggle(bulletData.multiBullet);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Damage", GUILayout.Width(bulletSection.width / 2));
            bulletData.damage = EditorGUILayout.FloatField(bulletData.damage);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Velocity", GUILayout.Width(bulletSection.width / 2));
            bulletData.velocity = EditorGUILayout.FloatField(bulletData.velocity);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Pierces Enemies?", GUILayout.Width(bulletSection.width / 2));
            bulletData.piercesEnemies = EditorGUILayout.Toggle(bulletData.piercesEnemies);

            EditorGUILayout.EndHorizontal();

            if (bulletData.piercesEnemies)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Pierce Amount", GUILayout.Width(bulletSection.width / 2));
                bulletData.enemyPierceAmount = EditorGUILayout.IntField(bulletData.enemyPierceAmount);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Bullet Object", GUILayout.Width(bulletSection.width / 2));
            bulletData.bulletObj = (GameObject)EditorGUILayout.ObjectField(bulletData.bulletObj, typeof(GameObject), false);

            EditorGUILayout.EndHorizontal();

            if (!bulletData.multiBullet)
            {
                if(bulletData.bulletObj != null)
                {
                    int childCount = bulletData.bulletObj.transform.childCount;
                    //get the trail and particles
                }
            }

        }
        if (!creatingNewBullet)
        {
            if (GUILayout.Button("New Bullet"))
            {
                creatingNewBullet = true;

                bulletData = new BulletSO();
                newBulletName = "";
            }
        }
        else
        {
            if (GUILayout.Button("Cancel New Bullet"))
            {
                creatingNewBullet = false;
            }
            if (GUILayout.Button("Create Bullet"))
            {
                string dataPath = "Assets/Scripts/Action/";
                dataPath += "Bullets/" + newBulletName + ".asset";
                AssetDatabase.CreateAsset(bulletData, dataPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                creatingNewBullet = false;
            }
        }

        gunData.bulletType = bulletData;
        GUILayout.EndArea();
    }

    void DrawBehaviourSettings()
    {
        GUILayout.BeginArea(behaviourSection);

        GUILayout.Label("Behaviour", subtitleStyle);

        if (bulletData.bulletObj != null)
        {
            behaviourData = bulletData.bulletObj.GetComponent<ProjectileScript>().bulletBehaviour;
        }
        if (!creatingNewBehaviour)
        {
            if (!bulletData.multiBullet)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Behaviour", GUILayout.Width(behaviourSection.width / 2));
                behaviourData = (BulletBehaviourSO)EditorGUILayout.ObjectField(behaviourData, typeof(BulletBehaviourSO), false);

                EditorGUILayout.EndHorizontal();
            }
        }

        if (behaviourData != null && !bulletData.multiBullet)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Name", GUILayout.Width(behaviourSection.width / 2));
            newBehaviourName = EditorGUILayout.TextField(newBehaviourName);
            if (!creatingNewBehaviour)
            {
                if (GUILayout.Button("S"))
                {
                    string path = AssetDatabase.GetAssetPath(behaviourData);
                    AssetDatabase.RenameAsset(path, newBehaviourName);
                }
                if (GUILayout.Button("R"))
                {
                    newBehaviourName = behaviourData.name;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Drag?", GUILayout.Width(behaviourSection.width / 2));
            behaviourData.drag = EditorGUILayout.Toggle(behaviourData.drag);

            EditorGUILayout.EndHorizontal();

            if (behaviourData.drag)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Drag Amount", GUILayout.Width(behaviourSection.width / 2));
                behaviourData.dragAmount = EditorGUILayout.FloatField(behaviourData.dragAmount);

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Wave?", GUILayout.Width(behaviourSection.width / 2));
            behaviourData.wave = EditorGUILayout.Toggle(behaviourData.wave);

            EditorGUILayout.EndHorizontal();

            if (behaviourData.wave)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("Wave Mult", GUILayout.Width(behaviourSection.width / 2));
                behaviourData.waveMult = EditorGUILayout.FloatField(behaviourData.waveMult);

                EditorGUILayout.EndHorizontal();
            }


        }

        if (!creatingNewBehaviour)
        {
            if (GUILayout.Button("New Behaviour"))
            {
                creatingNewBehaviour = true;

                behaviourData = new BulletBehaviourSO();
                newBehaviourName = "";
            }
        }
        else
        {
            if (GUILayout.Button("Cancel New Behaviour"))
            {
                creatingNewBehaviour = false;
            }
            if (GUILayout.Button("Create Behaviour"))
            {
                string dataPath = "Assets/Scripts/Action/";
                dataPath += "Bullets/Bullet Behaviours/" + newBehaviourName + ".asset";
                AssetDatabase.CreateAsset(behaviourData, dataPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                creatingNewBehaviour = false;
            }
        }

        if (bulletData.bulletObj != null)
        {
            bulletData.bulletObj.GetComponent<ProjectileScript>().bulletBehaviour = behaviourData;
        }
        GUILayout.EndArea();
    }

}
