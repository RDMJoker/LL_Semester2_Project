using System.Buffers.Text;

namespace KI
{
    public class IdleState : State
    {
        float idleTime;
        
        public IdleState(float _idleTime) : base()
        {
            idleTime = _idleTime;
        }

        protected override void StateEnter()
        {
            base.StateEnter();
        }

        protected override void StateExit()
        {
            base.StateExit();
        }

        protected override void Tick()
        {
            base.Tick(); 
        }
    }
}