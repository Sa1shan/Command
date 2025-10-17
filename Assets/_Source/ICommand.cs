using UnityEngine;

namespace _Source
{
    public interface ICommand
    {
        void Invoke(Vector2 position);
        void Undo();
    }
}