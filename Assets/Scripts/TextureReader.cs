﻿using UnityEngine;
using System;

namespace NONE
{
    [ExecuteInEditMode]
    public class TextureReader : MonoBehaviour
    {
        public bool reload;

        public Texture2D inputTexture;

        public ColorAndPrefabDictionary dict;

		public Vector2 GetSize() {
			return new Vector2(inputTexture.width, inputTexture.height * 0.5f);
		}

		public Vector2 GetPos() {
			return new Vector2(-0.5f, -0.25f);
		}

        void Awake()
        {
			//Debug.Log(inputTexture.width + "x" + inputTexture.height);
            LoadFromTexture();
        }

        void Update()
        {
            if (reload)
            {
                LoadFromTexture();
                reload = false;
            }
        }

        void LoadFromTexture()
        {
            while (transform.childCount != 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            for (int x = 0; x < inputTexture.width; x++)
            {
                for (int y = 0; y < inputTexture.height; y++)
                {
                    GameObject prefab = dict.Find(inputTexture.GetPixel(x, y));

                    if (prefab != null)
                        Instantiate(prefab, new Vector3(x, y - (0.5f * y)), new Quaternion(), transform);
                }
            }
        }


        [Serializable]
        public class ColorAndPrefabDictionary
        {
            [SerializeField]
            CaP[] dictionary;

            public GameObject Find(Color32 color)
            {
                foreach (var item in dictionary)
                {
                    if (item.color.Equals(color))
                        return item.prefab;
                }

                throw new Exception(color + " not in dictionary");
            }

            public Color32 Find(GameObject prefab)
            {
                foreach (var item in dictionary)
                {
                    if (item.prefab.Equals(prefab))
                        return item.color;
                }

                throw new Exception(prefab + " not in dictionary");
            }

            [Serializable]
            private class CaP
            {
                public Color32 color = new Color32();
                public GameObject prefab = null;
            }
        }
    }
}
