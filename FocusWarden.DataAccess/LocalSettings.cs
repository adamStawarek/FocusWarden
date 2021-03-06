using FocusWarden.DataAccess.Interfaces;
using FocusWarden.DataAccess.Models;
using System;
using System.Configuration;

namespace FocusWarden.DataAccess
{
    public sealed class LocalSettings : ApplicationSettingsBase, IDataSettings
    {
        [UserScopedSetting()]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        public DataSet<FocusSession> FocusSessions
        {
            get
            {
                if (!(this["FocusSessions"] is DataSet<FocusSession>)) this["FocusSessions"] = new DataSet<FocusSession>();

                return (DataSet<FocusSession>)this["FocusSessions"];
            }
            set
            {
                this["FocusSessions"] = value;
            }
        }

        [UserScopedSetting()]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        public DataSet<TodoItem> TodoItems
        {
            get
            {
                if (!(this["TodoItems"] is DataSet<TodoItem>)) this["TodoItems"] = new DataSet<TodoItem>();

                return (DataSet<TodoItem>)this["TodoItems"];
            }
            set
            {
                this["TodoItems"] = value;
            }
        }

        [UserScopedSetting()]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        public TimeSpan SessionDuration 
        {
            get
            {
                if (!(this["SessionDuration"] is TimeSpan)) this["SessionDuration"] = TimeSpan.FromMinutes(25);

                return (TimeSpan)this["SessionDuration"];
            }
            set
            {
                this["SessionDuration"] = value;
            }
        }

        FilterSettings settings;
        public FilterSettings Settings
        {
            get
            {
                if (settings == null) settings = new FilterSettings();
                return settings;
            }
            set
            {
                if (value == null || value == settings) return;
                settings = value;
            }
        }

        public new void Save() => ((ApplicationSettingsBase)this).Save();
    }
}
