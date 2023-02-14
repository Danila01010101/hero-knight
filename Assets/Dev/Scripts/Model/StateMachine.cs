namespace Model
{
    public class StateMachine
    {
        private State _currentState;

        public State CurrentState { get; }

        public void Initialize(State startState)
        {
            _currentState = startState;
            _currentState.Enter();
        }

        public void ChangeState(State newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}