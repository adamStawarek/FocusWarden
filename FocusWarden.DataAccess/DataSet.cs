namespace FocusWarden.DataAccess
{
    using Interfaces;
    using System;

    public class DataSet<T> where T : ISetEntity
    {
        public DataSet()
        {
            LocalSet = new LocalSet<T>();
        }

        public LocalSet<T> LocalSet { get; set; }

        public DateTime DateTime { get; set; }
    }
}