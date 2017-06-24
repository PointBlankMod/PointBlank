using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.Framework.Objects
{
    internal class RuntimeObject : MonoBehaviour
    {
        #region Properties
        public GameObject GameObject { get; private set; } // The instance of the gameobject
        public Dictionary<string, MonoBehaviour> CodeObjects { get; private set; } // The instances of the code objects/monobehaviour
        #endregion

        #region Entry Points
        public RuntimeObject() // Default everything
        {
            // Setup the variables
            GameObject = new GameObject();
            CodeObjects = new Dictionary<string, MonoBehaviour>();

            // Run the methods
            DontDestroyOnLoad(GameObject);
        }

        public RuntimeObject(GameObject gameObject) // Set the gameobject
        {
            // Setup the variables
            this.GameObject = gameObject;
            CodeObjects = new Dictionary<string, MonoBehaviour>();

            // Run the methods
            DontDestroyOnLoad(gameObject);
        }

        public RuntimeObject(Dictionary<string, MonoBehaviour> codeObjects) // Set the codeobjects
        {
            // Setup the variables
            GameObject = new GameObject();
            this.CodeObjects = codeObjects;

            // Run the methods
            DontDestroyOnLoad(GameObject);
        }

        public RuntimeObject(MonoBehaviour[] codeObjects) // Insert the codeobjects
        {
            // Setup the variables
            GameObject = new GameObject();
            this.CodeObjects = new Dictionary<string, MonoBehaviour>();

            // Run important code
            for (int i = 0; i < codeObjects.Length; i++)
                this.CodeObjects.Add(codeObjects[i].GetType().Name, codeObjects[i]);

            // Run the methods
            DontDestroyOnLoad(GameObject);
        }

        public RuntimeObject(GameObject gameObject, Dictionary<string, MonoBehaviour> codeObjects) // Set the gameobject and the codeobject
        {
            // Setup the variables
            this.GameObject = gameObject;
            this.CodeObjects = codeObjects;

            // Run the methods
            DontDestroyOnLoad(gameObject);
        }
        
        public RuntimeObject(GameObject gameObject, MonoBehaviour[] codeObjects) // Set the gameobject and insert the code objects
        {
            // Setup the variables
            this.GameObject = new GameObject();
            this.CodeObjects = new Dictionary<string, MonoBehaviour>();

            // Run important code
            for (int i = 0; i < codeObjects.Length; i++)
                this.CodeObjects.Add(codeObjects[i].GetType().Name, codeObjects[i]);

            // Run the methods
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region Functions
        public MonoBehaviour AddCodeObject<T>() where T : MonoBehaviour // For adding the code object
        {
            MonoBehaviour comp = (MonoBehaviour)GameObject.AddComponent<T>();

            CodeObjects.Add(comp.GetType().Name, comp);
            return comp;
        }

        public MonoBehaviour AddCodeObject(Type t) // For adding the code object
        {
            MonoBehaviour comp = (MonoBehaviour)GameObject.AddComponent(t);

            CodeObjects.Add(comp.GetType().Name, comp);
            return comp;
        }

        public void RemoveCodeObject(string name) // For removing code objects
        {
            GameObject.Destroy(CodeObjects[name]);
            CodeObjects.Remove(name);
        }

        public void RemoveCodeObject<T>() where T : MonoBehaviour // For removing code objects
        {
            MonoBehaviour comp = GetCodeObject<T>();

            GameObject.Destroy(comp);
            CodeObjects.Remove(comp.GetType().Name);
        }

        public T GetCodeObject<T>() where T : MonoBehaviour => (T)CodeObjects.First(a => a.Value.GetType() == typeof(T)).Value; // Returns code object

        public MonoBehaviour GetCodeObject(string name) => CodeObjects[name]; // Returns code object
        #endregion
    }
}
