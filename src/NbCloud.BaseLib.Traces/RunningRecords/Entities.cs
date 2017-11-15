using System;
using FluentNHibernate.Mapping;
using NbCloud.Common.Data.Model;

namespace NbCloud.BaseLib.Traces.RunningRecords
{
    public class RunningRecordEntity : NbEntity<RunningRecordEntity>
    {
        public virtual string ApplicationName { get; set; }
        public virtual DateTime? StartAt { get; set; }
        public virtual DateTime? StopAt { get; set; }
    }

    public class RunningRecordEntityMap : ClassMap<RunningRecordEntity>
    {
        public RunningRecordEntityMap()
        {
            Table("Lib_Trace_RunningRecord");
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.ApplicationName);
            Map(x => x.StartAt);
            Map(x => x.StopAt);
        }
    }
}
