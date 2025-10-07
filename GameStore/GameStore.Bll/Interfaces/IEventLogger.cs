using System;using GameStore.Dal.Entities;

namespace GameStore.Bll.Interfaces
{
    public interface IEventLogger 
    {
        void LogCreate(BaseEntity entity);

        void LogDelete(BaseEntity entity);

        void LogUpdate(BaseEntity entityOld, BaseEntity entityNew);
    }
}
