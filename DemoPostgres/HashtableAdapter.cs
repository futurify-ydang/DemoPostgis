using System.Collections;

namespace DemoPostgres
{
    public class HashtableAdapter
    {
        public T HashtableToObject<T>(Hashtable hashtable) where T : new()
        {
            var objectT = new T();
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                prop.SetValue(objectT, hashtable[prop.Name]);
            }

            return objectT;
        }

        public Hashtable ObjectToHashtable<T>(T objectT)
        {
            var hashtable = new Hashtable();
            // map all data to hash table
            ObjectToHashtable(objectT, ref hashtable);
            return hashtable;
        }

        public void ObjectToHashtable<T>(T objectT, ref Hashtable hashtable)
        {
            // map all data to hash table
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                hashtable[prop.Name] = prop.PropertyType.IsEnum ? (int)prop.GetValue(objectT, null) : prop.GetValue(objectT, null);
            }
        }
    }
}