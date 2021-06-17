namespace FocusWarden.DataAccess.Interfaces
{
    using System.Collections.Generic;

    public interface ISetCollection<T> : IList<T>
    {
        void Update(T item);
    }
}