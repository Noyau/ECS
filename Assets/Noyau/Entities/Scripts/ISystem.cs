using System.Collections.Generic;

namespace Assets.Noyau.Entities.Scripts
{
    public interface ISystem
    {
        List<T> GetEntities<T>() where T : IGroup;

        void OnUpdate();
    } // interface: ISystem
} // namespace