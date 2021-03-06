using FocusWarden.DataAccess.Interfaces;
using System;

namespace FocusWarden.DataAccess
{
    public class DataSet<T> where T : ISetEntity
    {
        public LocalSet<T> LocalSet { get; set; }

        public DateTime DateTime { get; set; }

        public DataSet()
        {
            LocalSet = new LocalSet<T>();
        }
    }
}
