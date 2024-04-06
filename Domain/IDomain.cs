using System;

namespace Domain
{
    public interface IDomain<T> where T : class
    {
        Guid UniqueId { get; set; }
        void ChangeValues(T newElem);
        bool AreEqual(T newElem);
    }
}
