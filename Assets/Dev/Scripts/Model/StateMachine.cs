namespace Model
{
    public class StateMachine
    {
        private IState _currentState;

        public void Initialize(IState startState)
        {
            _currentState = startState;
            _currentState.Enter();
        }

        public void ChangeState(IState newState)
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