namespace UD.Core.Helper.Database
{
    using Microsoft.EntityFrameworkCore;
    using UD.Core.Extensions;
    public class ChangeEntry
    {
        public object entity { get; set; }
        public string entityname { get; set; }
        public string entitystate { get; set; }
        public Dictionary<string, ChangePropertyInfo> changeproperties { get; set; }
        public ChangeEntry() : this(default, default, default, default) { }
        public ChangeEntry(object entity, Type entityType, EntityState entitystate, Dictionary<string, ChangePropertyInfo> changeproperties)
        {
            this.entity = entity;
            this.entityname = entityType.GetTableName(true);
            this.entitystate = entitystate.ToString("g");
            this.changeproperties = changeproperties ?? [];
        }
    }
}