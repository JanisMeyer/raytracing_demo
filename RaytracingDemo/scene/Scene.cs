using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytracingDemo
{
    public class Scene : ICollection<Model>
    {
        private List<Model> models;

        public List<Light> Lights { get; set; }

        public int Count => models.Count;

        public bool IsReadOnly => false;
        public Scene()
        {
            this.models = new List<Model>();
            this.Lights = new List<Light>();
        }

        public void Add(Model item)
        {
            models.Add(item);
        }

        public void Clear()
        {
            models.Clear();
        }

        public bool Contains(Model item)
        {
            return models.Contains(item);
        }

        public void CopyTo(Model[] array, int arrayIndex)
        {
            models.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Model> GetEnumerator()
        {
            return models.GetEnumerator();
        }

        public bool Remove(Model item)
        {
            return models.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return models.GetEnumerator();
        }
    }
}
