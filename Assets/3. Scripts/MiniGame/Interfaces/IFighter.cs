using System;

namespace _3._Scripts.MiniGame.Interfaces
{
    public interface IFighter
    {
        public event Action OnSlap;
        public void StartFight();
        public void EndFight(bool win);
    }
}