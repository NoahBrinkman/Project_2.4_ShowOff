
    using UnityEngine;

    public static class MonoBehaviourExtensions
    {
        public static T GetComponentInSiblings<T>(this Component mb, bool includeChildren = false)
        {
            if (mb.transform.parent != null)
            {
                foreach (Transform t in mb.transform.parent)
                {
                    T component = t.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }

                    if (includeChildren)
                    {
                        foreach (Transform t1 in t)
                        {
                            T c = t1.GetComponent<T>();
                            if (c != null)
                            {
                                return c;
                            }
                        }
                    }
                }
            }
            
            
            return default;
        }
    }
