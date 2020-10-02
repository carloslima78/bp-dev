using System;

namespace BeePlace.Infra.DataBaseUtils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataBaseAttributes : Attribute
    {
        public bool PrimaryKey { get; set; }

        public bool Pagging { get; set; }

        public DataBaseAttributes()
        {
            PrimaryKey = false;
            Pagging = false;
        }

        public DataBaseAttributes(bool primaryKey)
        {
            this.PrimaryKey = primaryKey;
        }

        public DataBaseAttributes( bool primaryKey, bool pagging)
        {
            this.PrimaryKey = primaryKey;
            this.Pagging = pagging;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DapperKey : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DapperIgnore : Attribute
    {

    }

}
