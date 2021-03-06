using System.Collections.Generic;

namespace FocusWarden.DataAccess.Interfaces
{
    public interface ISetCollection<T> : IList<T>
    {
        void Update(T item);
    }
}